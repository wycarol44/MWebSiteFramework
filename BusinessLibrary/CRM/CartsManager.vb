Imports CommonLibrary

Public Class CartsManager

    Public Shared Function GetById(CartID As Integer) As DataLibrary.ShoppingCart
        Using ctx As New DataLibrary.ModelEntities
            Dim obj = ctx.ShoppingCarts.Where(Function(x) x.CartID = CartID).SingleOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.ShoppingCart
            End If
            Return obj
        End Using
    End Function


    Public Shared Function GetList(Optional PageIndex As Integer = 0, Optional PageSize As Integer = 0, Optional CartID As Integer = Nothing,
                                   Optional SessionID As String = Nothing, Optional ProductID As Integer = Nothing, Optional Archived As Boolean? = Nothing) As DataLibrary.ShoppingCart_GetList_Result()
        Using ctx As New DataLibrary.ModelEntities
            Return ctx.ShoppingCart_GetList(PageIndex,
                                         PageSize,
                                         ToNull(CartID),
                                         ToNull(SessionID),
                                         ToNull(ProductID),
                                         Archived).ToArray()
        End Using
    End Function



    Public Shared Function Save(obj As DataLibrary.ShoppingCart) As Integer
        Using ctx As New DataLibrary.ModelEntities()

            'if the id is 0, its new, so add it
            If obj.CartID = 0 Then
                ctx.ShoppingCarts.Add(obj)
                obj.DateCreated = Now
                obj.CreatedBy = UserAuthentication.User.UserID
            Else
                'its an update. attach the object to the context
                ctx.ShoppingCarts.Attach(obj)
                'tell the context that the entry is in the modified state
                ctx.Entry(obj).State = EntityState.Modified
                obj.DateModified = Now
                ' obj.ModifiedBy = UserAuthentication.User.UserID
            End If

            'save the record
            ctx.SaveChanges()
            Return obj.CartID
        End Using
    End Function



    Public Shared Sub ToggleArchived(CartID As Integer)
        Dim obj = GetById(CartID)
        If obj IsNot Nothing AndAlso obj.CartID > 0 Then
            obj.Archived = Not obj.Archived
            Save(obj)
        End If
    End Sub

End Class
