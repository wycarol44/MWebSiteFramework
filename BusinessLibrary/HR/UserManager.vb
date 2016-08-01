Imports CommonLibrary
Imports System.DirectoryServices
Imports DataLibrary

Public Class UserManager

    ''' <summary>
    ''' Get a record by its id
    ''' </summary>
    ''' <param name="userId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetById(userId As Integer) As DataLibrary.User
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = (From t In ctx.Users
                       Where t.UserID = userId).SingleOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.User

            End If

            Return obj
        End Using
    End Function

    ''' <summary>
    ''' Gets User by ID with details
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetDetailsByID(userId As Integer?) As DataLibrary.Users_GetDetailsByID_Result
        Using ctx As New DataLibrary.ModelEntities

            Return ctx.Users_GetDetailsByID(
                userId
                ).SingleOrDefault()

        End Using
    End Function

    ''' <summary>
    ''' Gets a user based on username and password
    ''' </summary>
    ''' <param name="username"></param>
    ''' <param name="password"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ValidateLogin(username As String, password As String) As DataLibrary.Users_ValidateLogin_Result
        Using ctx As New DataLibrary.ModelEntities


            Dim res = ctx.Users_ValidateLogin(username, password, Nothing, Nothing).SingleOrDefault()
            Return res
        End Using
    End Function

    Public Shared Function ValidateADLogin(username As String, password As String) As DataLibrary.Users_ValidateLogin_Result

        Dim domainAndUsername As String = username


        Try
            Using entry As DirectoryEntry = New DirectoryEntry(AppSettings.ADPath, username, password)
                'Bind to the native AdsObject to force authentication.			
                'Dim obj As Object = entry.NativeObject
                Dim search As DirectorySearcher = New DirectorySearcher(entry)

                search.Filter = "(SAMAccountName=" & username & ")"
                search.PropertiesToLoad.Add("cn")
                Dim result As SearchResult = search.FindOne()

                If (result Is Nothing) Then
                    Return Nothing
                Else
                    Dim user = ValidateUserId(AppSettings.ADImpersonateUserID)
                    user.Fullname = CType(result.Properties("cn")(0), String)
                    Return user
                End If
            End Using
        Catch ex As Exception
            Throw New Exception("Error authenticating user. " & ex.Message)
        End Try
    End Function



    ''' <summary>
    ''' Gets a user based on reset key
    ''' </summary>
    ''' <param name="resetKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ValidateResetKey(resetKey As String) As DataLibrary.Users_ValidateLogin_Result
        Using ctx As New DataLibrary.ModelEntities

            'convert hex string to bytes
            Dim resetKeyBytes = HexToByteArray(resetKey)

            Dim res = ctx.Users_ValidateLogin(Nothing, Nothing, resetKeyBytes, Nothing).SingleOrDefault()
            Return res
        End Using
    End Function

    ''' <summary>
    ''' Gets a user based on reset key
    ''' </summary>
    ''' <param name="userId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ValidateUserId(userId As Integer) As DataLibrary.Users_ValidateLogin_Result
        Using ctx As New DataLibrary.ModelEntities
            Dim res = ctx.Users_ValidateLogin(Nothing, Nothing, Nothing, userId).SingleOrDefault()
            Return res
        End Using
    End Function

    ''' <summary>
    ''' Gets list of users
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>First user (Internal User) is filtered out</remarks>
    Public Shared Function GetList(Optional userId As Integer? = Nothing, Optional fullname As String = Nothing, Optional phone As String = Nothing, Optional archived As Boolean? = False, Optional email As String = Nothing, Optional jobTitleIds As XElement = Nothing, Optional statusIds As XElement = Nothing) As DataLibrary.Users_GetList_Result()
        Using ctx As New DataLibrary.ModelEntities

            Return ctx.Users_GetList(
                userId,
                archived,
                ToNull(fullname),
                ToNull(phone),
                ToNull(email),
                ToNull(jobTitleIds),
                ToNull(statusIds)
            ).ToArray()

        End Using
    End Function

    ''' <summary>
    ''' Gets list of records for combobox
    ''' </summary>
    ''' <param name="userId"></param>
    ''' <param name="fullname"></param>
    ''' <param name="skip"></param>
    ''' <param name="take"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetComboBoxList(Optional userId As Integer? = Nothing, Optional fullname As String = Nothing, Optional skip As Integer? = Nothing, Optional take As Integer? = Nothing) As DataLibrary.Users_GetComboBoxList_Result()
        Using ctx As New DataLibrary.ModelEntities

            Return ctx.Users_GetComboBoxList(userId, ToNull(fullname), skip, take).ToArray()

        End Using
    End Function

    ''' <summary>
    ''' Saves a record
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Save(obj As DataLibrary.User) As Integer
        Using ctx As New DataLibrary.ModelEntities()

            'if the id is 0, its new, so add it
            If obj.UserID = 0 Then
                ctx.Users.Add(obj)

                obj.DateCreated = Now
                obj.CreatedBy = UserAuthentication.User.UserID

            Else

                obj.DateModified = Now
                obj.ModifiedBy = UserAuthentication.User.UserID

                'its an update. attach the object to the context
                ctx.Users.Attach(obj)
                'tell the context that the entry is in the modified state
                ctx.Entry(obj).State = EntityState.Modified

            End If

            'save the record
            ctx.SaveChanges()

            Return obj.UserID
        End Using
    End Function

    ''' <summary>
    ''' Archives or unarchives the specified record
    ''' </summary>
    ''' <param name="userId"></param>
    ''' <remarks></remarks>
    Public Shared Sub ToggleArchived(userId As Integer)
        Using ctx As New DataLibrary.ModelEntities
            Dim obj = (From t In ctx.Users
                       Where t.UserID = userId).SingleOrDefault()

            If obj IsNot Nothing Then
                obj.Archived = Not obj.Archived

                ctx.SaveChanges()
            End If
        End Using
    End Sub

    ''' <summary>
    ''' Updates User Status Field
    ''' </summary>
    ''' <param name="UserID"></param>
    ''' <param name="StatusID"></param>
    ''' <remarks></remarks>
    Public Shared Sub UpdateStatus(UserID As Integer, StatusID As MilesMetaTypeItem)
        Using ctx As New DataLibrary.ModelEntities
            Dim obj = (From t In ctx.Users
                       Where t.UserID = UserID).SingleOrDefault()

            If obj IsNot Nothing Then
                Dim OldStatus As String = MetaTypeItemManager.GetById(obj.StatusID).ItemName
                Dim NewStatus As String = MetaTypeItemManager.GetById(StatusID).ItemName
                obj.StatusID = StatusID

                ctx.SaveChanges()
                'Save Audit Log
                AuditLogManager.SaveAuditLog(AuditLogType.Update, MilesMetaObjects.Users, UserID, StatusID, MilesMetaAuditLogAttributes.Status, NewStatus, OldStatus)


            End If
        End Using
    End Sub
    ''' <summary>
    ''' Updates the user's password
    ''' </summary>
    ''' <param name="userId"></param>
    ''' <param name="password"></param>
    ''' <remarks></remarks>
    Public Shared Sub UpdatePassword(userId As Integer, password As String, modifiedBy As Integer)
        Using ctx As New DataLibrary.ModelEntities
            ctx.Users_UpdatePassword(userId, password, modifiedBy)
        End Using
    End Sub

    Public Shared Function UpdateResetKey(username As String) As DataLibrary.Users_UpdateResetKey_Result
        Using ctx As New DataLibrary.ModelEntities
            Return ctx.Users_UpdateResetKey(ToNull(username)).SingleOrDefault
        End Using
    End Function

    ''' <summary>
    ''' Checks to see if the email address is unique
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsEmailDuplicate(obj As DataLibrary.User)
        Using ctx As New DataLibrary.ModelEntities

            Dim dupe = (From t In ctx.Users
                        Where (Not t.UserID = obj.UserID) _
                        And (t.Email = obj.Email)
                        Select True).FirstOrDefault()

            Return dupe

        End Using
    End Function

    ''' <summary>
    ''' Checks to see if the Username is unique
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsUsernameDuplicate(obj As DataLibrary.User)
        Using ctx As New DataLibrary.ModelEntities
            Dim dupe = (From t In ctx.Users
                        Where (Not t.UserID = obj.UserID) _
                        And (t.Username = obj.Username)
                        Select True).FirstOrDefault()

            Return dupe

        End Using
    End Function
    ''' <summary>
    ''' Get a record by its id
    ''' </summary>
    ''' <param name="userId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetUserRole(userId As Integer) As DataLibrary.UserRole()
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = ctx.Users.Where(Function(x) x.UserID = userId).SingleOrDefault()

            Dim objlist() As DataLibrary.UserRole
            If obj IsNot Nothing Then
                objlist = obj.UserRoles.ToArray()
            Else
                objlist = Nothing
            End If

            Return objlist
        End Using
    End Function

    Public Shared Sub SaveRole(userid As Integer, userroleid As Integer)

        Using ctx As New DataLibrary.ModelEntities

            Dim ur = (From t In ctx.Users
                       Where t.UserID = userid).SingleOrDefault()

            ur.UserRoles.Add((From u In ctx.UserRoles Where u.UserRoleID = userroleid).Single())

            ctx.SaveChanges()

            'clear function access chache for this user
            ClearUserFunctionAccessCache(userid)

        End Using
    End Sub

    ''' <summary>
    ''' save multiple userroles for a given user at once
    ''' used to save user role for new user
    ''' </summary>
    ''' <param name="userid"></param>
    ''' <param name="userroles"></param>
    ''' <remarks></remarks>
    Public Shared Sub SaveRoles(userid As Integer, userroles As List(Of DataLibrary.UserRole))

        Using ctx As New DataLibrary.ModelEntities

            Dim u = (From t In ctx.Users
                       Where t.UserID = userid).SingleOrDefault()
            For Each ur In userroles
                u.UserRoles.Add((From x In ctx.UserRoles Where x.UserRoleID = ur.UserRoleID).Single())
            Next

            ctx.SaveChanges()

        End Using
    End Sub

    Public Shared Sub RemoveRole(userid As Integer, userroleid As Integer)

        Using ctx As New DataLibrary.ModelEntities

            Dim ur = (From t In ctx.Users
                       Where t.UserID = userid).SingleOrDefault()

            ur.UserRoles.Remove((From u In ctx.UserRoles Where u.UserRoleID = userroleid).Single())

            ctx.SaveChanges()

            'clear function access chache for this user
            ClearUserFunctionAccessCache(userid)

        End Using
    End Sub

#Region "Function Access Functions"

    ''' <summary>
    ''' Pashupati Shrestha
    ''' </summary>
    ''' <param name="functionid"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function HasFunctionAccess(functionid As MilesMetaFunctions) As Boolean

        If UserAuthentication.User.UserID = AppSettings.ADImpersonateUserID Then
            Return True
        Else

            Using ctx As New ModelEntities

                Return ctx.Users_HasFunctionAccess(UserAuthentication.User.UserID, CInt(functionid)).SingleOrDefault

            End Using

        End If


    End Function

    ''' <summary>
    ''' Author: Pashupati Shrestha
    ''' Checks if the User has access to the form (UserID - FormID)
    ''' </summary>
    ''' <param name="FormID"></param>
    ''' <param name="UserID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function HasPageAccess(ByVal FormID As Integer, ByVal UserID As Integer) As AccessType

        If FormID = 0 Then Return AccessType.FullAccess

        Dim ffList = MetaFunctionManager.GetMetaFunctionsForFunctionAccess()

        ' If ffList.Any Then
        If ffList.Where(Function(x) x.MetaForms.Select(Function(n) n.FormID).Contains(FormID)).Any Then

            Dim accessList = GetFunctionAccessList(UserID)

            'if form exists then only we have to go through other checks else give full access
            Dim formExists = (From fa In accessList
                            Where fa.FormID = FormID).Any


            If Not formExists Then
                Return AccessType.NoAccess

            Else
                Dim fullAccessList = (From fa In accessList
                                     Where fa.FormID = FormID And fa.ViewOnly = False).ToArray()


                If fullAccessList.Any Then
                    Return AccessType.FullAccess
                Else
                    Return AccessType.ReadOnlyAccess
                End If

            End If
        Else
            Return AccessType.FullAccess

        End If

    End Function

    ''' <summary>
    ''' Author: Pashupati Shrestha
    ''' Gets List of FuctionAccess for this UserID from cache. if there is nothing, then it gets from the database and adds to cache
    ''' </summary>
    ''' <param name="UserID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetFunctionAccessList(UserID As Integer) As DataLibrary.Users_GetFunctionAccess_Result()


        Dim key = String.Format(AppSettings.FunctionAccessCacheKey, UserID)
        Dim accessList As DataLibrary.Users_GetFunctionAccess_Result()
        Dim usingCache = Not AppSettings.BypassCaching
        Dim isInCache = CachingFunctions.IsInCache(key)

        'cache it
        If Not isInCache Or Not usingCache Then
            Using ctx As New DataLibrary.ModelEntities
                'get list of access
                accessList = ctx.Users_GetFunctionAccess(UserID).ToArray()
            End Using

            'clear it from cache
            If (isInCache) And usingCache Then CachingFunctions.RemoveFromCache(key)

            'add it to the cache
            If usingCache Then CachingFunctions.AddToCache(key, accessList)

        Else
            accessList = CachingFunctions.LoadFromCache(key)
        End If

        Return accessList
    End Function

    Public Shared Sub ClearAllFunctionAccessCache()
        Dim keyList As New List(Of String)()
        Dim webCache = System.Web.HttpRuntime.Cache
        Dim CacheEnum As IDictionaryEnumerator = webCache.GetEnumerator()
        While CacheEnum.MoveNext()
            keyList.Add(CacheEnum.Key.ToString())
        End While
        For Each key As String In keyList
            If key.Contains("functionAccess") Then
                CachingFunctions.RemoveFromCache(key)
            End If
        Next
    End Sub

    Public Shared Sub ClearUserFunctionAccessCache(ByVal UserID As Integer)
        Dim keyList As New List(Of String)()
        Dim webCache = System.Web.HttpRuntime.Cache
        Dim CacheEnum As IDictionaryEnumerator = webCache.GetEnumerator()
        While CacheEnum.MoveNext()
            keyList.Add(CacheEnum.Key.ToString())
        End While
        For Each key As String In keyList
            If key.Contains(UserID & ".functionAccess") Then
                CachingFunctions.RemoveFromCache(key)
            End If
        Next
    End Sub

#End Region

End Class
