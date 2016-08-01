Imports CommonLibrary

Public Class OrderManager

    Public Shared Function GetById(OrderID As Integer) As DataLibrary.Order
        Using ctx As New DataLibrary.ModelEntities
            Dim obj = ctx.Orders.Where(Function(x) x.OrderID = OrderID).SingleOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.Order
            End If
            Return obj
        End Using
    End Function

    Public Shared Function GetList(Optional PageIndex As Integer = 0, Optional PageSize As Integer = 0, Optional OrderID As Integer = Nothing, Optional CustomerID As Integer = Nothing, Optional CustomerName As String = Nothing, Optional PaymentTypeID As Integer = Nothing,
                                   Optional DateFrom As String = Nothing, Optional DateTo As String = Nothing, Optional Archived As Boolean? = Nothing) As DataLibrary.Orders_GetList_Result()
        Using ctx As New DataLibrary.ModelEntities
            Return ctx.Orders_GetList(PageIndex,
                                         PageSize,
                                         ToNull(OrderID),
                                         ToNull(CustomerID),
                                         ToNull(CustomerName),
                                         ToNull(PaymentTypeID),
                                         ToNull(DateFrom),
                                         ToNull(DateTo),
                                         Archived).ToArray()
        End Using
    End Function



    Public Shared Function Save(obj As DataLibrary.Order) As Integer
        Using ctx As New DataLibrary.ModelEntities()


            'if the id is 0, its new, so add it
            If obj.OrderID = 0 Then

                ctx.Orders.Add(obj)
                obj.DateCreated = Now
                If Not UserAuthentication.User Is Nothing Then
                    obj.CreatedBy = UserAuthentication.User.UserID
                End If
            Else
                'its an update. attach the object to the context
                ctx.Orders.Attach(obj)
                'tell the context that the entry is in the modified state
                ctx.Entry(obj).State = EntityState.Modified
                obj.DateModified = Now
                If Not UserAuthentication.User Is Nothing Then

                    obj.ModifiedBy = UserAuthentication.User.UserID
                End If
            End If
            'save the record
            ctx.SaveChanges()

            Return obj.OrderID
        End Using
    End Function



    Public Shared Sub ToggleArchived(OrderID As Integer)
        Dim obj = GetById(OrderID)
        If obj IsNot Nothing AndAlso obj.OrderID > 0 Then
            obj.Archived = Not obj.Archived
            Save(obj)
        End If
    End Sub






End Class
