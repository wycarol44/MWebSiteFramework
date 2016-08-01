Imports CommonLibrary

''' <summary>
''' Manages CRUD operations on an object
''' </summary>
''' <remarks></remarks>
Public Class CustomerManager

#Region "Standard"
    ''' <summary>
    ''' Gets a record by its ID. If no record is found, returns a new, blank record
    ''' </summary>
    ''' <param name="customerId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetById(customerId As Integer) As DataLibrary.Customer
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = ctx.Customers.Where(Function(x) x.CustomerID = customerId).SingleOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.Customer
            End If

            Return obj
        End Using
    End Function

    ''' <summary>
    ''' Gets a record by its with details
    ''' </summary>
    ''' <param name="customerId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetDetailsById(customerId As Integer) As DataLibrary.Customers_GetDetailsByID_Result
        Using ctx As New DataLibrary.ModelEntities()
            Return ctx.Customers_GetDetailsByID(customerId).FirstOrDefault()
        End Using
    End Function
    Public Shared Function GetList(Optional PageIndex As Integer = 0, Optional PageSize As Integer = 0, Optional CustomerName As String = Nothing, Optional phone As String = Nothing, Optional StatusIDs As XElement = Nothing, Optional ContactName As String = Nothing, Optional ContactEmail As String = Nothing, Optional Archived As Boolean? = Nothing, Optional SortExpression As String = Nothing, Optional SortOrder As Integer? = Nothing) As DataLibrary.Customers_GetList_Result()
        Using ctx As New DataLibrary.ModelEntities()
            Return ctx.Customers_GetList(PageIndex,
                                         PageSize,
                                         ToNull(CustomerName),
                                         ToNull(phone),
                                         ToNull(StatusIDs),
                                         ToNull(ContactName),
                                         ToNull(ContactEmail),
                                         Archived,
                                         ToNull(SortExpression),
                                         SortOrder).ToArray()
        End Using
    End Function

    ' ''' <summary>
    ' ''' Gets a list of Customer
    ' ''' </summary>
    ' ''' <param name="pageIndex"></param>
    ' ''' <param name="pageSize"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Shared Function GetGridView(pageIndex As Integer, pageSize As Integer, sortExpression As String, sortOrder As Integer) As DataLibrary.Customer_GetList_Result()
    '    Using ctx As New DataLibrary.ModelEntities()

    '        'call stored proc
    '        Dim list = ctx.Customer_GetList(pageIndex, pageSize, sortExpression, sortOrder)
    '        'return collection
    '        Return list.ToArray()

    '    End Using
    'End Function

    ''' <summary>
    ''' Inserts or updates the record
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Save(obj As DataLibrary.Customer) As Integer
        Using ctx As New DataLibrary.ModelEntities()


            'if the id is 0, its new, so add it
            If obj.CustomerID = 0 Then

                ctx.Customers.Add(obj)
                obj.DateCreated = Now
                If Not UserAuthentication.User Is Nothing Then
                    obj.CreatedBy = UserAuthentication.User.UserID
                End If

            Else
                'its an update. attach the object to the context
                ctx.Customers.Attach(obj)
                'tell the context that the entry is in the modified state
                ctx.Entry(obj).State = EntityState.Modified
                obj.DateModified = Now
                obj.ModifiedBy = UserAuthentication.User.UserID
            End If

            'save the record
            ctx.SaveChanges()

            Return obj.CustomerID
        End Using
    End Function

    ''' <summary>
    ''' Archives the specified record
    ''' </summary>
    ''' <param name="customerId"></param>
    ''' <remarks></remarks>
    Public Shared Sub ToggleArchived(customerId As Integer)
        Dim obj = GetById(customerId)
        If obj IsNot Nothing AndAlso obj.CustomerID > 0 Then
            obj.Archived = Not obj.Archived
            Save(obj)
        End If
    End Sub

#End Region


#Region "Custom"
    ''' <summary>
    ''' Checks to see if the record is a duplicate based on a few criteria
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsDuplicate(obj As DataLibrary.Customer) As Boolean
        'check if there is already a customer with the same name
        Using ctx As New DataLibrary.ModelEntities

            Return ctx.Customers.Any(Function(x) x.CustomerID <> obj.CustomerID And x.CustomerName = obj.CustomerName)

        End Using
    End Function

    ''' <summary>
    ''' Gets count of unarchived records
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetCount()
        Using ctx As New DataLibrary.ModelEntities
            Return ctx.Customers.Count(Function(x) x.Archived = False)
        End Using
    End Function


    ''' <summary>
    ''' Updates Customer Status Field
    ''' </summary>
    ''' <param name="CustomerID"></param>
    ''' <param name="StatusID"></param>
    ''' <remarks></remarks>
    Public Shared Sub UpdateStatus(CustomerID As Integer, StatusID As MilesMetaTypeItem)
        Using ctx As New DataLibrary.ModelEntities
            Dim obj = (From t In ctx.Customers
                       Where t.CustomerID = CustomerID).SingleOrDefault()


            If obj IsNot Nothing Then

                Dim OldStatus As String = MetaTypeItemManager.GetById(obj.StatusID).ItemName
                Dim NewStatus As String = MetaTypeItemManager.GetById(StatusID).ItemName

                obj.StatusID = StatusID

                ctx.SaveChanges()

                'Save Audit Log
                AuditLogManager.SaveAuditLog(AuditLogType.Update, MilesMetaObjects.Customers, CustomerID, StatusID, MilesMetaAuditLogAttributes.Status, NewStatus, OldStatus)


            End If
        End Using
    End Sub


    ''' <summary>
    ''' Returns Primary Contact For the Customer
    ''' </summary>
    ''' <param name="CustomerID"></param>
    ''' <remarks></remarks>
    Public Shared Function GetPrimaryContact(ByVal CustomerID As Integer) As DataLibrary.CustomerContact
        Using ctx As New DataLibrary.ModelEntities
            Return ctx.CustomerContacts.Where(Function(x) x.CustomerID = CustomerID AndAlso x.IsPrimary = True AndAlso x.Archived = False).FirstOrDefault
        End Using
    End Function
#End Region


End Class
