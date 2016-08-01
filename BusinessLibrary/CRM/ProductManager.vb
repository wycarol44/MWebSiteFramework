Imports CommonLibrary


Public Class ProductManager

#Region "Standard"

    Public Shared Function GetById(ProductID As Integer) As DataLibrary.Product
        Using ctx As New DataLibrary.ModelEntities
            Dim obj = ctx.Products.Where(Function(x) x.ProductID = ProductID).SingleOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.Product
            End If
            Return obj
        End Using
    End Function

    Public Shared Function GetList(Optional PageIndex As Integer = 0, Optional PageSize As Integer = 0, Optional ProductID As Integer = Nothing, Optional ProductName As String = Nothing, Optional Description As String = Nothing, Optional CategoryID As Integer = Nothing, Optional SubCategoryID As Integer = Nothing,
                                   Optional Cost As Double = Nothing, Optional Price As Double = Nothing, Optional Archived As Boolean? = Nothing, Optional DateCreatedFrom As String = Nothing, Optional DateCreatedTo As String = Nothing, Optional SortExpression As String = Nothing, Optional SortOrder As Integer? = Nothing) As DataLibrary.Product_GetList_Result()
        Using ctx As New DataLibrary.ModelEntities
            Return ctx.Product_GetList(PageIndex,
                                         PageSize,
                                         ToNull(ProductID),
                                         ToNull(ProductName),
                                         ToNull(Description),
                                         ToNull(CategoryID),
                                         ToNull(SubCategoryID),
                                         ToNull(Cost),
                                         ToNull(Price),
                                         Archived,
                                         ToNull(DateCreatedFrom),
                                         ToNull(DateCreatedTo),
                                         ToNull(SortExpression),
                                         SortOrder).ToArray()
        End Using
    End Function



    Public Shared Function Save(obj As DataLibrary.Product) As Integer
        Using ctx As New DataLibrary.ModelEntities()


            'if the id is 0, its new, so add it
            If obj.ProductID = 0 Then

                ctx.Products.Add(obj)
                obj.DateCreated = Now
                obj.CreatedBy = UserAuthentication.User.UserID

            Else
                'its an update. attach the object to the context
                ctx.Products.Attach(obj)
                'tell the context that the entry is in the modified state
                ctx.Entry(obj).State = EntityState.Modified
                obj.DateModified = Now
                obj.ModifiedBy = UserAuthentication.User.UserID
            End If

            'save the record
            ctx.SaveChanges()

            Return obj.ProductID
        End Using
    End Function

    Public Shared Sub ToggleArchived(ProductID As Integer)
        Dim obj = GetById(ProductID)
        If obj IsNot Nothing AndAlso obj.ProductID > 0 Then
            obj.Archived = Not obj.Archived
            Save(obj)
        End If
    End Sub

#End Region



End Class
