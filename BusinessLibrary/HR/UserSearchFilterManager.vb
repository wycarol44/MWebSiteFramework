Public Class UserSearchFilterManager
#Region "Standard"
    ''' <summary>
    ''' Gets a record by its ID. If no record is found, returns a new, blank record
    ''' </summary>
    ''' <param name="filterId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetById(filterId As Integer) As DataLibrary.UserSearchFilter
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = (From t In ctx.UserSearchFilters
                       Where t.FilterID = filterId).SingleOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.UserSearchFilter
            End If

            Return obj
        End Using
    End Function

    ''' <summary>
    ''' Gets a record by its ID. If no record is found, returns a new, blank record
    ''' </summary>
    ''' <param name="userId"></param>
    ''' <param name="formId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetById(userId As Integer, formId As Integer, searchName As String) As DataLibrary.UserSearchFilter
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = (From t In ctx.UserSearchFilters
                       Where t.UserID = userId _
                       And t.FormID = formId _
                       And t.SearchName = searchName).SingleOrDefault()

            If obj Is Nothing Then
                obj = New DataLibrary.UserSearchFilter
            End If

            Return obj
        End Using
    End Function

    ''' <summary>
    ''' Inserts or updates the record
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Save(obj As DataLibrary.UserSearchFilter) As Integer
        Using ctx As New DataLibrary.ModelEntities()

            'if the id is 0, its new, so add it
            If obj.FilterID = 0 Then
                ctx.UserSearchFilters.Add(obj)

                'set default values
                obj.DateCreated = Now

            Else
                'its an update. attach the object to the context
                ctx.UserSearchFilters.Attach(obj)
                'tell the context that the entry is in the modified state
                ctx.Entry(obj).State = EntityState.Modified
            End If

            'save the record
            ctx.SaveChanges()

            Return obj.FilterID
        End Using
    End Function

    ''' <summary>
    ''' Deletes a record from the database
    ''' </summary>
    ''' <param name="userId"></param>
    ''' <param name="formId"></param>
    ''' <remarks></remarks>
    Public Shared Sub Delete(userId As Integer, formId As Integer, searchName As String)
        Using ctx As New DataLibrary.ModelEntities
            Dim obj = (From t In ctx.UserSearchFilters
                       Where t.UserID = userId _
                       And t.FormID = formId _
                       And t.SearchName = searchName).SingleOrDefault()

            If obj IsNot Nothing Then
                'mark the entry as deleted
                ctx.Entry(obj).State = EntityState.Deleted

                'save the record
                ctx.SaveChanges()
            End If
        End Using

    End Sub

#End Region
End Class
