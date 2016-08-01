Imports CommonLibrary

Public Class AddressManager
#Region "Standard"
    ''' <summary>
    ''' Gets a record by its id
    ''' </summary>
    ''' <param name="addressId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetById(addressId As Integer) As DataLibrary.Address
        Using ctx As New DataLibrary.ModelEntities
            Dim obj = (From t In ctx.Addresses
                       Where t.AddressID = addressId
                       Select t).SingleOrDefault()

            If obj Is Nothing Then
                obj = New DataLibrary.Address
            End If

            Return obj

        End Using
    End Function

    ''' <summary>
    ''' Saves a record
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Save(obj As DataLibrary.Address) As Integer
        Using ctx As New DataLibrary.ModelEntities()

            'if the id is 0, its new, so add it
            If obj.AddressID = 0 Then
                ctx.Addresses.Add(obj)

                obj.DateCreated = Now

                If Not UserAuthentication.User Is Nothing Then
                    obj.CreatedBy = UserAuthentication.User.UserID
                End If

            Else

                obj.DateModified = Now
                obj.ModifiedBy = UserAuthentication.User.UserID

                'its an update. attach the object to the context
                ctx.Addresses.Attach(obj)
                'tell the context that the entry is in the modified state
                ctx.Entry(obj).State = EntityState.Modified

            End If

            'save the record
            ctx.SaveChanges()

            Return obj.AddressID
        End Using
    End Function
#End Region
End Class
