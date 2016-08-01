
Partial Class CRM_ManageContacts
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

#Region "Page Functions"

    Protected Sub CRM_ManageContacts_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If CustomerID > 0 Then

                Page.SetFocus(txtContactName)

                PageHeader = CustomerManager.GetById(CustomerID).CustomerName + " - Contacts"

                'For Each t As RadTab In rdTabs.SelectedTab.Tabs
                '    t.Visible = False
                'Next
            Else
                Navigate("/CRM/ManageCustomers.aspx")
            End If
        End If
    End Sub

#End Region

#Region "Methods"

    Private Sub BindGrid()
        clContacts.DataSource = Nothing
        clContacts.Rebind()
    End Sub

#End Region

#Region "Events"

    Protected Sub pnlSearch_Search(sender As Object, e As EventArgs) Handles pnlSearch.Search
        BindGrid()
    End Sub

#End Region

#Region "CardList"

    Protected Sub clContacts_ItemCommand(sender As Object, e As RadListViewCommandEventArgs) Handles clContacts.ItemCommand
        If e.CommandName = "InitInsert" Then
            Navigate("~/CRM/ContactInfo.aspx?CustomerID=" & CustomerID & "&ContactID=0")
        End If
    End Sub

    Protected Sub clContacts_NeedDataSource(sender As Object, e As RadListViewNeedDataSourceEventArgs) Handles clContacts.NeedDataSource
        Dim ccList = CustomerContactManager.GetList(CustomerID:=CustomerID,
                                            ContactName:=txtContactName.Text,
                                           Email:=txtEmail.Text,
                                           phone:=txtPhone.Text,
                                           Archived:=ToNegBool(chkArchived.Checked))
        clContacts.DataSource = ccList
    End Sub

    Protected Sub clContacts_ToggleArchive(sender As Object, e As RadListViewCommandEventArgs) Handles clContacts.ToggleArchive
        Dim item As RadListViewDataItem = e.ListViewItem

        Dim keyId As Integer = item.GetDataKeyValue("ContactID")

        'archive the Customer
        CustomerContactManager.ToggleArchived(keyId)

        clContacts.Rebind()
    End Sub
#End Region
End Class
