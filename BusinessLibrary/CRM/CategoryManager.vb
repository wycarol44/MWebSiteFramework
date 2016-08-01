Imports CommonLibrary

Public Class CategoryManager

#Region "Standard"

    Public Shared Function GetById(CategoryID As Integer) As DataLibrary.Category
        Using ctx As New DataLibrary.ModelEntities
            Dim obj = ctx.Categories.Where(Function(x) x.CategoryID = CategoryID).SingleOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.Category
            End If
            Return obj
        End Using
    End Function

    Public Shared Function GetList(Optional PageIndex As Integer = 0, Optional PageSize As Integer = 0, Optional CategoryName As String = Nothing, Optional Description As String = Nothing, Optional CategoryID As Integer = Nothing,
                                   Optional ParentID As Integer? = Nothing, Optional Archived As Boolean? = Nothing, Optional DateCreatedFrom As String = Nothing, Optional DateCreatedTo As String = Nothing, Optional SortExpression As String = Nothing, Optional SortOrder As Integer? = Nothing) As DataLibrary.Category_GetList_Result()
        Using ctx As New DataLibrary.ModelEntities
            Return ctx.Category_GetList(PageIndex,
                                         PageSize,
                                         ToNull(CategoryName),
                                         ToNull(Description),
                                         CategoryID,
                                         ParentID,
                                         Archived,
                                         ToNull(DateCreatedFrom),
                                         ToNull(DateCreatedTo),
                                         ToNull(SortExpression),
                                         SortOrder).ToArray()
        End Using
    End Function

    Public Shared Function Save(obj As DataLibrary.Category) As Integer
        Using ctx As New DataLibrary.ModelEntities()


            'if the id is 0, its new, so add it
            If obj.CategoryID = 0 Then

                ctx.Categories.Add(obj)
                obj.DateCreated = Now
                obj.CreatedBy = UserAuthentication.User.UserID

            Else
                'its an update. attach the object to the context
                ctx.Categories.Attach(obj)
                'tell the context that the entry is in the modified state
                ctx.Entry(obj).State = EntityState.Modified
                obj.DateModified = Now
                obj.ModifiedBy = UserAuthentication.User.UserID
            End If

            'save the record
            ctx.SaveChanges()

            Return obj.CategoryID
        End Using
    End Function

    Public Shared Sub ToggleArchived(CategoryID As Integer)
        Dim obj = GetById(CategoryID)
        If obj IsNot Nothing AndAlso obj.CategoryID > 0 Then
            obj.Archived = Not obj.Archived
            Save(obj)
        End If
    End Sub

#End Region



End Class
