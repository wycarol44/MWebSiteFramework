Imports CommonLibrary
Public Class CustomerContactManager
#Region "Standard"
    ''' <summary>
    ''' Gets a record by its ID. If no record is found, returns a new, blank record
    ''' </summary>
    ''' <param name="contactid"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetById(contactid As Integer) As DataLibrary.CustomerContact
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = ctx.CustomerContacts.Where(Function(x) x.ContactID = contactid).SingleOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.CustomerContact
            End If

            Return obj
        End Using
    End Function

    ''' <summary>
    ''' Gets a record by its with details
    ''' </summary>
    ''' <param name="contactid"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetDetailsById(contactid As Integer) As DataLibrary.CustomerContacts_GetDetailsByID_Result
        Using ctx As New DataLibrary.ModelEntities()
            Return ctx.CustomerContacts_GetDetailsByID(contactid).FirstOrDefault()
        End Using
    End Function

    Public Shared Function GetList(Optional CustomerID As Integer? = Nothing, Optional ContactName As String = Nothing, Optional Email As String = Nothing, Optional phone As String = Nothing, Optional Archived As Boolean? = Nothing) As DataLibrary.CustomerContacts_GetList_Result()
        Using ctx As New DataLibrary.ModelEntities()
            Return ctx.CustomerContacts_GetList(CustomerID,
                                         ToNull(ContactName),
                                         ToNull(Email),
                                         ToNull(phone),
                                         Archived).ToArray()
        End Using
    End Function

    ''' <summary>
    ''' Inserts or updates the record
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Save(obj As DataLibrary.CustomerContact) As Integer
        Using ctx As New DataLibrary.ModelEntities()
            'If this obj is set to be primary then we need to reset another one if exists
            If obj.IsPrimary Then
                'check if there is any other primary contact
                Dim primarycontact = ctx.CustomerContacts.Where(Function(x) x.CustomerID = obj.CustomerID And x.ContactID <> obj.ContactID And x.Archived = 0 And x.IsPrimary = True).SingleOrDefault()
                'if any primarycontact exists then reset it
                If primarycontact IsNot Nothing Then
                    primarycontact.IsPrimary = False
                End If
            End If
            'if the id is 0, its new, so add it
            If obj.ContactID = 0 Then
                ctx.CustomerContacts.Add(obj)
                obj.DateCreated = Now
                obj.CreatedBy = UserAuthentication.User.UserID
            Else
                'its an update. attach the object to the context
                ctx.CustomerContacts.Attach(obj)
                'tell the context that the entry is in the modified state
                ctx.Entry(obj).State = EntityState.Modified

                obj.DateModified = Now
                obj.ModifiedBy = UserAuthentication.User.UserID
            End If

            'save the record
            ctx.SaveChanges()

            Return obj.ContactID
        End Using
    End Function

    ''' <summary>
    ''' Archives the specified record
    ''' </summary>
    ''' <param name="contactid"></param>
    ''' <remarks></remarks>
    Public Shared Sub ToggleArchived(contactid As Integer)
        Dim obj = GetById(contactid)
        If obj IsNot Nothing AndAlso obj.ContactID > 0 Then
            obj.Archived = Not obj.Archived

            Save(obj)
        End If
    End Sub

#End Region

#Region "Custom"
    ''' <summary>
    ''' Checks if any unarchived contact exists for the customer
    ''' </summary>
    ''' <param name="CustomerID"></param>
    ''' <remarks></remarks>
    Public Shared Function IsFirstContact(CustomerID As Integer)
        Using ctx As New DataLibrary.ModelEntities
            Dim obj = ctx.CustomerContacts.Where(Function(x) x.CustomerID = CustomerID And x.Archived = 0).FirstOrDefault()
            If obj Is Nothing Then
                Return True
            Else
                Return False
            End If
        End Using
    End Function

    ''' <summary>
    ''' Gets count of unarchived notes
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetCount(ByVal CustomerID As Integer)
        Using ctx As New DataLibrary.ModelEntities
            Return ctx.CustomerContacts.Count(Function(x) x.CustomerID = CustomerID AndAlso x.Archived = False)
        End Using
    End Function

#End Region


End Class
