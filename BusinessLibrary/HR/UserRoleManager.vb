Public Class UserRoleManager
#Region "Standard"


    ''' <summary>
    ''' Gets a record by its ID. If no record is found, returns a new, blank record
    ''' </summary>
    ''' <param name="userroleid"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetById(userroleid As Integer) As DataLibrary.UserRole
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = (From t In ctx.UserRoles
                       Where t.UserRoleID = userroleid).SingleOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.UserRole
            End If

            Return obj
        End Using
    End Function

    ''' <summary>
    ''' Gets a simple list of records
    ''' </summary>
    ''' <param name="userrole"></param>
    ''' <param name="archived"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetList(Optional userrole As String = Nothing, Optional archived As Boolean? = False) As DataLibrary.UserRole()
        Using ctx As New DataLibrary.ModelEntities

            'get list of objects
            Dim objs = (From t In ctx.UserRoles
                        Order By t.UserRoleName
                        Where
                            (userrole Is Nothing OrElse t.UserRoleName.Contains(userrole)) _
                        And (archived Is Nothing OrElse t.Archived = archived.Value)
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
    Public Shared Function Save(obj As DataLibrary.UserRole) As Integer
        Using ctx As New DataLibrary.ModelEntities()

            'if the id is 0, its new, so add it
            If obj.UserRoleID = 0 Then
                ctx.UserRoles.Add(obj)

                'set default values
                obj.DateCreated = Now
                obj.CreatedBy = UserAuthentication.User.UserID
            Else
                'set default values
                obj.DateModified = Now
                obj.ModifiedBy = UserAuthentication.User.UserID

                'its an update. attach the object to the context
                ctx.UserRoles.Attach(obj)
                'tell the context that the entry is in the modified state
                ctx.Entry(obj).State = EntityState.Modified
            End If

            'save the record
            ctx.SaveChanges()


            Return obj.UserRoleID
        End Using
    End Function

    ''' <summary>
    ''' Archives or unarchives the specified record
    ''' </summary>
    ''' <param name="userroleid"></param>
    ''' <remarks></remarks>
    Public Shared Sub ToggleArchived(userroleid As Integer)
        Dim obj = GetById(userroleid)

        If obj IsNot Nothing Then
            obj.Archived = Not obj.Archived

            Save(obj)
        End If
    End Sub

#End Region

#Region "Other"
    ''' <summary>
    ''' Checks to see if the job title is duplicate based on name
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsDuplicate(obj As DataLibrary.UserRole) As Boolean
        Using ctx As New DataLibrary.ModelEntities
            Dim dupe = (From t In ctx.UserRoles
                        Where t.UserRoleID <> obj.UserRoleID _
                        And t.UserRoleName = obj.UserRoleName).SingleOrDefault()
            Return dupe IsNot Nothing
        End Using
    End Function

    ''' <summary>
    ''' save multiple functions for a given userrole at once
    ''' </summary>
    ''' <param name="userroleid"></param>
    ''' <param name="functionIds"></param>
    ''' <remarks></remarks>
    Public Shared Sub SaveFunctions(userroleid As Integer, functionIds() As Integer)

        Using ctx As New DataLibrary.ModelEntities
            Dim ur = (From t In ctx.UserRoles
                       Where t.UserRoleID = userroleid).SingleOrDefault()

            'add all the functions with passed ids to meta functions
            For Each fr In functionIds
                ur.MetaFunctions.Add((From f In ctx.MetaFunctions Where f.FunctionID = fr).Single())
            Next
            'remove any functions that existed before
            Dim functionstoremove = (From f In ctx.MetaFunctions Where Not functionIds.Contains(f.FunctionID) Select f).ToArray
            For Each fr In functionstoremove
                ur.MetaFunctions.Remove(fr)
            Next

            ctx.SaveChanges()

        End Using
    End Sub

    ''' <summary>
    ''' Gets a Userroles List by functionID
    ''' </summary>
    ''' <param name="FunctionId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetByFunctionID(FunctionId As Integer) As DataLibrary.UserRoles_GetByFunctionID_Result()
        Using ctx As New DataLibrary.ModelEntities
            Return ctx.UserRoles_GetByFunctionID(FunctionId).ToArray()
        End Using
    End Function




#End Region
End Class
