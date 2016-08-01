
Partial Class CRM_CustomerDashboard
    Inherits BasePage

#Region "Properties"
    Public Property CustomerID As Integer
        Get
            Dim v As Object = ViewState("CustomerID")
            If v Is Nothing Then
                v = ToInteger(Request("CustomerID"))
                ViewState("CustomerID") = v
            End If

            Return v
        End Get
        Set(value As Integer)
            ViewState("CustomerID") = value
        End Set
    End Property
#End Region

#Region "Methods"
    Protected Sub CRM_CustomerDashboard_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If CustomerID > 0 Then
                Dim cus As Customers_GetDetailsByID_Result = CustomerManager.GetDetailsById(CustomerID)

                SetPageHeader(cus)
                SetEntityFooter(cus.CreatedBy, cus.DateCreated, cus.ModifiedBy, cus.DateModified)
                PopulateBasicInfo(cus)
                PopulateContactInfo()
                PopulateNotes()
                PopulateDocuments()
                PopulateAuditLog()
            Else
                Navigate("/CRM/ManageCustomers.aspx")
            End If
        End If
    End Sub

    Private Sub SetPageHeader(obj As DataLibrary.Customers_GetDetailsByID_Result)
        PageHeader = obj.CustomerName & " - Dashboard"
    End Sub

    Private Sub PopulateBasicInfo(cus As Customers_GetDetailsByID_Result)
        'populate the label's/ hide them if there is no info

        lnkBasicInfoHeader.NavigateUrl = "/CRM/CustomerInfo.aspx?CustomerID=" & CustomerID
        lnkCustomerName.Text = cus.CustomerName
        lnkCustomerName.NavigateUrl = "/CRM/CustomerInfo.aspx?CustomerID=" & CustomerID

        Dim btnViewMap As RadToolBarButton = rdActions.FindButtonByCommandName("ViewMap")
        If Not String.IsNullOrEmpty(cus.FullAddress) Then
            lblAddress.Text = cus.FullAddress
            btnViewMap.Visible = True
            btnViewMap.NavigateUrl = "http://maps.google.com/?q=" & Replace(cus.FullAddress, "<br />", " ")
        Else
            lblAddress.Visible = False
            btnViewMap.Visible = False
        End If

        If Not String.IsNullOrEmpty(cus.Website) Then
            lnkWebSite.Text = cus.Website
            lnkWebSite.NavigateUrl = cus.Website
        Else
            lnkWebSite.Visible = False
        End If

        If Not String.IsNullOrEmpty(cus.FullAddress) Then
            lnkCustomerPhone.Text = FormatPhone(PhoneNumberType.Work, cus.Phone)
            lnkCustomerPhone.NavigateUrl = "tel:" + cus.Phone
        Else
            lnkCustomerPhone.Visible = False
        End If

        lblStatus.Text = cus.Status

        ConfigureActionsToolBar(cus.StatusID)
    End Sub


    Private Sub ConfigureActionsToolBar(statusid As MilesMetaTypeItem)
        Dim btnActive As RadToolBarButton = rdActions.FindButtonByCommandName("Active")
        Dim btnPending As RadToolBarButton = rdActions.FindButtonByCommandName("Pending")
        Dim btnLost As RadToolBarButton = rdActions.FindButtonByCommandName("Lost")
        Dim btnInActive As RadToolBarButton = rdActions.FindButtonByCommandName("InActive")

        btnActive.Visible = True
        btnPending.Visible = True
        btnLost.Visible = True
        btnInActive.Visible = True

        btnActive.CommandArgument = MilesMetaTypeItem.CustomerStatusActive
        btnPending.CommandArgument = MilesMetaTypeItem.CustomerStatusPending
        btnLost.CommandArgument = MilesMetaTypeItem.CustomerStatusLost
        btnInActive.CommandArgument = MilesMetaTypeItem.CustomerStatusInactive

        Select Case statusid
            Case MilesMetaTypeItem.CustomerStatusActive
                btnActive.Visible = False
            Case MilesMetaTypeItem.CustomerStatusInactive
                btnInActive.Visible = False
            Case MilesMetaTypeItem.CustomerStatusLost
                btnLost.Visible = False
            Case MilesMetaTypeItem.CustomerStatusPending
                btnPending.Visible = False
        End Select
    End Sub

    Private Sub PopulateContactInfo()
        Dim contactscount = CustomerContactManager.GetCount(CustomerID)
        If contactscount > 0 Then
            Dim contactscounter = <span class="badge pull-right"><%= contactscount %></span>
            lnkContactsHeader.Text = lnkContactsHeader.Text & contactscounter.ToString()
        End If
        lnkContactsHeader.NavigateUrl = "/CRM/ManageContacts.aspx?CustomerID=" & CustomerID

        Dim contact As CustomerContact = CustomerManager.GetPrimaryContact(CustomerID)
        If contact IsNot Nothing Then
            lblNoPrimaryContact.Visible = False
            lnkContactName.Text = contact.FullName
            lnkContactName.NavigateUrl = "/CRM/ContactInfo.aspx?CustomerID=" & CustomerID & "&ContactID=" & contact.ContactID

            If Not String.IsNullOrEmpty(contact.Title) Then
                lblContactTitle.Text = contact.Title
            Else
                lblContactTitle.Visible = False
            End If

            If Not String.IsNullOrEmpty(contact.Email) Then
                lnkContactEmail.Text = contact.Email
                lnkContactEmail.NavigateUrl = "mailto:" & contact.Email
            Else
                lnkContactEmail.Visible = False
            End If

            If Not String.IsNullOrEmpty(contact.Email) Then
                lnkContactPhone.Text = FormatPhone(PhoneNumberType.Work, contact.WorkPhone, contact.WorkPhoneExt)
                lnkContactPhone.NavigateUrl = "tel:" + contact.WorkPhone
            Else
                lnkContactPhone.Visible = False
            End If
        Else
            lblNoPrimaryContact.Visible = True
        End If
    End Sub

    Private Sub PopulateNotes()
        With ucNotesDashboard
            .ObjectID = MilesMetaObjects.Customers
            .KeyID = CustomerID
            .ShowCounter = True
            .HeaderNavigationURL = "/CRM/CustomerNotes.aspx?CustomerID=" & CustomerID
            .InfoPage = "/CommonDialogs/DialogAddEditNotes.aspx"
            .Populate()
        End With
    End Sub

    Private Sub PopulateDocuments()
        With ucDocumentsDashboard
            .ObjectID = MilesMetaObjects.Customers
            .KeyID = CustomerID
            .ShowCounter = True
            .HeaderNavigationURL = "/CRM/CustomerDocuments.aspx?CustomerID=" & CustomerID
            .Populate()
        End With
    End Sub

    Private Sub PopulateAuditLog()
        With ucAuditLogDashboard
            .ObjectID = MilesMetaObjects.Customers
            .KeyID = CustomerID
            .ShowCounter = True
            .Populate()
        End With
    End Sub

    Private Sub PopulateStatusLabel(statusid As Integer)
        lblStatus.Text = MetaTypeItemManager.GetById(statusid).ItemName
        'reconfigure the toolbar after populating status
        ConfigureActionsToolBar(statusid)
    End Sub
#End Region

#Region "Events"
    Protected Sub btnAddNewContacts_Click(sender As Object, e As ImageClickEventArgs) Handles btnAddNewContacts.Click
        Navigate("/CRM/ContactInfo.aspx?CustomerID=" & CustomerID & "&ContactID=0")
    End Sub

    Protected Sub rdActions_ButtonClick(sender As Object, e As RadToolBarEventArgs) Handles rdActions.ButtonClick
        If TypeOf e.Item Is RadToolBarButton Then
            Dim button As RadToolBarButton = e.Item
            If button.Value = "Active" Or _
                button.Value = "Pending" Or _
                button.Value = "Lost" Or _
                button.Value = "Inactive" Then

                Try
                    'update the status
                    CustomerManager.UpdateStatus(CustomerID, button.CommandArgument)

                    'change status shown in status label and reconfigure toolbar
                    PopulateStatusLabel(button.CommandArgument)
                    PopulateAuditLog()
                    JGrowl.ShowMessage(JGrowlMessageType.Success, "Customer Status Changed")
                Catch wex As WarningException
                    JGrowl.ShowMessage(JGrowlMessageType.Error, wex.Message)

                Catch ex As Exception
                    Throw ex
                End Try


            End If
        End If
    End Sub
#End Region

End Class
