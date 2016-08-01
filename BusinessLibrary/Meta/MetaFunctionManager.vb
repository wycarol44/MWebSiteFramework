Imports CommonLibrary

Public Class MetaFunctionManager
#Region "Standard"
    ''' <summary>
    ''' Gets a record by its ID. If no record is found, returns a new, blank record
    ''' </summary>
    ''' <param name="functionID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetById(functionID As Integer) As DataLibrary.MetaFunction
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = (From t In ctx.MetaFunctions.Include("MetaForms")
                       Where t.FunctionID = functionID).SingleOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.MetaFunction
            End If

            Return obj
        End Using
    End Function


    Public Shared Function GetList(Optional FunctionName As String = Nothing, Optional ModuleName As String = Nothing, Optional FormName As String = Nothing, Optional archived As Boolean? = False) As DataLibrary.MetaFunction()
        Using ctx As New DataLibrary.ModelEntities

            'get list of objects
            Dim objs = (From t In ctx.MetaFunctions.Include("MetaModule")
                        Where
                        (FunctionName Is Nothing Or t.FunctionName.Contains(FunctionName)) _
                        And (ModuleName Is Nothing Or t.MetaModule.ModuleName.Contains(ModuleName)) _
                        And (FormName Is Nothing Or (t.MetaForms.Where(Function(x) _
                                                                           x.FormName.Contains(FormName)).Any)) _
                        And (archived Is Nothing Or t.Archived = archived)
                        Select t).ToArray()

            Return objs

        End Using
    End Function

    ''' <summary>
    ''' Inserts or updates the record
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Save(obj As DataLibrary.MetaFunction) As Integer
        Using ctx As New DataLibrary.ModelEntities()

            'if the id is 0, its new, so add it
            If obj.FunctionID = 0 Then
                ctx.MetaFunctions.Add(obj)

                'set default values
                obj.DateCreated = Now
                obj.CreatedBy = UserAuthentication.User.UserID
            Else
                'its an update. attach the object to the context
                ctx.MetaFunctions.Attach(obj)
                'tell the context that the entry is in the modified state
                ctx.Entry(obj).State = EntityState.Modified

                obj.DateModified = Now
                obj.ModifiedBy = UserAuthentication.User.UserID

            End If

            'save the record
            ctx.SaveChanges()

            UserManager.ClearAllFunctionAccessCache()
            Return obj.FunctionID
        End Using
    End Function

    ''' <summary>
    ''' Archives or unarchives the specified record
    ''' </summary>
    ''' <param name="functionId"></param>
    ''' <remarks></remarks>
    Public Shared Sub ToggleArchived(functionId As Integer)
        Using ctx As New DataLibrary.ModelEntities
            Dim obj = (From t In ctx.MetaFunctions
                       Where t.FunctionID = functionId).SingleOrDefault()

            If obj IsNot Nothing Then
                obj.Archived = Not obj.Archived

                ctx.SaveChanges()
            End If
        End Using
    End Sub


  

#End Region

#Region "Custom"

    Public Shared Function GetAllFunctionWithModules(Optional UserRoleID As Integer? = Nothing, Optional FunctionName As String = Nothing) As DataLibrary.MetaFunctions_GetAllFunctionsWithModules_Result()
        Using ctx As New DataLibrary.ModelEntities

            Return FilterModulesWithoutFunction(ctx.MetaFunctions_GetAllFunctionsWithModules(
                                                                UserRoleID, ToNull(FunctionName)).ToArray())

        End Using
    End Function

    Shared Function FilterModulesWithoutFunction(ByVal obj As DataLibrary.MetaFunctions_GetAllFunctionsWithModules_Result()) As DataLibrary.MetaFunctions_GetAllFunctionsWithModules_Result()
        'get parentid's of functions
        Dim functionparentids = (From o In obj Where o.ID > 0 Select o.ParentID).ToArray()
        'return all functions or the records whose parentid is in the above list
        Return obj.Where(Function(x) x.ID > 0 Or functionparentids.Contains(x.ID)).ToArray()
    End Function

    Public Shared Sub SaveForm(functionid As Integer, formid As Integer)

        Using ctx As New DataLibrary.ModelEntities

            Dim f = (From t In ctx.MetaFunctions
                       Where t.FunctionID = functionid).SingleOrDefault()

            f.MetaForms.Add((From u In ctx.MetaForms Where u.FormID = formid).Single())

            ctx.SaveChanges()

            'meta function cache needs to be clear so that updated list wil be used for function access purpose
            ClearMetaFunctionCache()

        End Using
    End Sub

    ''' <summary>
    ''' save multiple forms for a given user at once
    ''' used to save related forms for new function
    ''' </summary>
    ''' <param name="functionid"></param>
    ''' <param name="forms"></param>
    ''' <remarks></remarks>
    Public Shared Sub SaveForms(functionid As Integer, forms As List(Of DataLibrary.MetaForm))

        Using ctx As New DataLibrary.ModelEntities


            Dim f = (From t In ctx.MetaFunctions
                       Where t.FunctionID = functionid).SingleOrDefault()
            For Each fr In forms
                f.MetaForms.Add((From u In ctx.MetaForms Where u.FormID = fr.FormID).Single())
            Next

            ctx.SaveChanges()
            ClearMetaFunctionCache()

        End Using
    End Sub

    Public Shared Sub RemoveForm(functionid As Integer, formid As Integer)

        Using ctx As New DataLibrary.ModelEntities

            Dim f = (From t In ctx.MetaFunctions
                       Where t.FunctionID = functionid).SingleOrDefault()

            f.MetaForms.Remove((From u In ctx.MetaForms Where u.FormID = formid).Single())

            ctx.SaveChanges()

            'meta function cache needs to be clear so that updated list wil be used for function access purpose
            ClearMetaFunctionCache()

        End Using
    End Sub

    ''' <summary>
    ''' save multiple Userroles for a given function at once
    ''' </summary>
    ''' <param name="functionid"></param>
    ''' <param name="userroleIds"></param>
    ''' <remarks></remarks>
    Public Shared Sub SaveUserRoles(functionid As Integer, userroleIds() As Integer)

        Using ctx As New DataLibrary.ModelEntities
            Dim f = (From t In ctx.MetaFunctions
                       Where t.FunctionID = functionid).SingleOrDefault()

            'add all the userroles with passed ids to userroles navigation property
            For Each ur In userroleIds
                f.UserRoles.Add((From u In ctx.UserRoles Where u.UserRoleID = ur).Single())
            Next
            'remove any userroles that existed before
            Dim userrolestoremove = (From u In ctx.UserRoles Where Not userroleIds.Contains(u.UserRoleID) Select u).ToArray

            For Each ur In userrolestoremove
                f.UserRoles.Remove(ur)
            Next

            ctx.SaveChanges()

        End Using
    End Sub


    Public Shared Function GetMetaFunctionsForFunctionAccess() As DataLibrary.MetaFunction()

        Dim key = String.Format(AppSettings.MetaFunctionsCacheKey)
        Dim list As DataLibrary.MetaFunction()
        Dim usingCache = Not AppSettings.BypassCaching
        Dim isInCache = CachingFunctions.IsInCache(key)

        'cache it
        If Not isInCache Or Not usingCache Then
            Using ctx As New DataLibrary.ModelEntities
                'get list of access
                list = (From ff In ctx.MetaFunctions.Include("MetaForms")
                       Select ff).ToArray()

            End Using

            'clear it from cache
            If (isInCache) And usingCache Then CachingFunctions.RemoveFromCache(key)

            'add it to the cache
            If usingCache Then CachingFunctions.AddToCache(key, list)

        Else
            list = CachingFunctions.LoadFromCache(key)
        End If

        Return list
    End Function

    Public Shared Sub ClearMetaFunctionCache()
        Dim keyList As New List(Of String)()
        Dim webCache = System.Web.HttpRuntime.Cache
        Dim CacheEnum As IDictionaryEnumerator = webCache.GetEnumerator()
        While CacheEnum.MoveNext()
            keyList.Add(CacheEnum.Key.ToString())
        End While
        For Each key As String In keyList
            If key.Contains("metafunctions") Then
                CachingFunctions.RemoveFromCache(key)
            End If
        Next
    End Sub

#End Region
End Class
