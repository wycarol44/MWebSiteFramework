Imports CommonLibrary

Public Class CustomerAddressManager
#Region "Standard"

    Public Shared Function GetById(CustomerAddressID As Integer) As DataLibrary.CustomerAddress
        Using ctx As New DataLibrary.ModelEntities
            Dim obj = ctx.CustomerAddresses.Where(Function(x) x.CustomerAddressID = CustomerAddressID).SingleOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.CustomerAddress
            End If
            Return obj
        End Using
    End Function

    Public Shared Function GetList(Optional PageIndex As Integer = 0, Optional PageSize As Integer = 0, Optional CustomerID As Integer = Nothing, Optional CustomerAddressID As Integer = Nothing, Optional AddressTypeID As Integer = Nothing, Optional Archived As Boolean? = Nothing) As DataLibrary.CustomerAddress_GetList_Result()
        Using ctx As New DataLibrary.ModelEntities
            Return ctx.CustomerAddress_GetList(PageIndex,
                                         PageSize,
                                         ToNullableInteger(CustomerID),
                                         ToNull(CustomerAddressID),
                                         ToNullableInteger(AddressTypeID),
                                         Archived).ToArray()
        End Using
    End Function



    Public Shared Function Save(obj As DataLibrary.CustomerAddress) As Integer
        Using ctx As New DataLibrary.ModelEntities()


            'if the id is 0, its new, so add it
            If obj.CustomerAddressID = 0 Then

                ctx.CustomerAddresses.Add(obj)
                obj.DateCreated = Now
                If Not UserAuthentication.User Is Nothing Then
                    obj.CreatedBy = UserAuthentication.User.UserID
                End If
            Else
                'its an update. attach the object to the context
                ctx.CustomerAddresses.Attach(obj)
                'tell the context that the entry is in the modified state
                ctx.Entry(obj).State = EntityState.Modified
                obj.DateModified = Now

                If Not UserAuthentication.User Is Nothing Then
                    obj.ModifiedBy = UserAuthentication.User.UserID
                End If

            End If

            'save the record
            ctx.SaveChanges()

            Return obj.CustomerAddressID
        End Using
    End Function


    Public Shared Function SaveAdd(obj As DataLibrary.Address) As Integer
        Using ctx As New DataLibrary.ModelEntities()


            'if the id is 0, its new, so add it
            If obj.AddressID = 0 Then

                ctx.Addresses.Add(obj)
                obj.DateCreated = Now
                obj.CreatedBy = UserAuthentication.User.UserID

            Else
                'its an update. attach the object to the context
                ctx.Addresses.Attach(obj)
                'tell the context that the entry is in the modified state
                ctx.Entry(obj).State = EntityState.Modified
                obj.DateModified = Now
                obj.ModifiedBy = UserAuthentication.User.UserID
            End If

            'save the record
            ctx.SaveChanges()

            Return obj.AddressID
        End Using
    End Function




    Public Shared Sub ToggleArchived(CustomerAddressID As Integer)
        Dim obj = GetById(CustomerAddressID)
        If obj IsNot Nothing AndAlso obj.CustomerAddressID > 0 Then
            obj.Archived = Not obj.Archived
            Save(obj)
        End If
    End Sub

#End Region




End Class
