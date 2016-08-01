Imports CommonLibrary

Public Class OrderDetailManager

    Public Shared Function GetById(OrderDetailID As Integer) As DataLibrary.OrderDetail
        Using ctx As New DataLibrary.ModelEntities
            Dim obj = ctx.OrderDetails.Where(Function(x) x.OrderDetailID = OrderDetailID).SingleOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.OrderDetail
            End If
            Return obj
        End Using
    End Function

    Public Shared Function GetList(Optional PageIndex As Integer = 0, Optional PageSize As Integer = 0, Optional OrderID As Integer = Nothing, Optional CustomerID As Integer = Nothing, Optional OrderDetailID As Integer = Nothing,
                                   Optional ProductID As Integer = Nothing, Optional Archived As Boolean? = Nothing) As DataLibrary.OrderDetails_GetList_Result()
        Using ctx As New DataLibrary.ModelEntities
            Return ctx.OrderDetails_GetList(PageIndex,
                                         PageSize,
                                         ToNullableInteger(OrderID),
                                         ToNull(CustomerID),
                                         ToNull(OrderDetailID),
                                         ToNull(ProductID),
                                         Archived).ToArray()
        End Using
    End Function



    Public Shared Function Save(obj As DataLibrary.OrderDetail) As Integer
        Using ctx As New DataLibrary.ModelEntities()


            'if the id is 0, its new, so add it
            If obj.OrderDetailID = 0 Then

                ctx.OrderDetails.Add(obj)
                obj.DateCreated = Now
                If Not UserAuthentication.User Is Nothing Then
                    obj.CreatedBy = UserAuthentication.User.UserID
                End If
            Else
                'its an update. attach the object to the context
                ctx.OrderDetails.Attach(obj)
                'tell the context that the entry is in the modified state
                ctx.Entry(obj).State = EntityState.Modified
                obj.DateModified = Now
                If Not UserAuthentication.User Is Nothing Then
                    obj.ModifiedBy = UserAuthentication.User.UserID
                End If
            End If
            'save the record
            ctx.SaveChanges()

            Return obj.OrderDetailID
        End Using
    End Function



    Public Shared Sub ToggleArchived(OrderDetailID As Integer)
        Dim obj = GetById(OrderDetailID)
        If obj IsNot Nothing AndAlso obj.OrderDetailID > 0 Then
            obj.Archived = Not obj.Archived
            Save(obj)
        End If
    End Sub


End Class
