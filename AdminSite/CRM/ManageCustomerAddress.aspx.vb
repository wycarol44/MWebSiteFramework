
Partial Class CRM_ManageCustomerAddress
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

    Public Property AddressTypeID As Integer
        Get
            Dim v As Object = ViewState("AddressTypeID")
            If v Is Nothing Then
                v = ToInteger(Request("AddressTypeID"))
                ViewState("AddressTypeID") = v
            End If
            Return v
        End Get
        Set(value As Integer)
            ViewState("AddressTypeID") = value
        End Set
    End Property

#End Region


    'Search Panel
    Protected Sub pnlSearch_Search(sender As Object, e As EventArgs) Handles pnlSearch.Search
        BindGrid()
    End Sub


#Region "CardList"

    Private Sub BindGrid()
        clAddress.DataSource = Nothing
        clAddress.AutoBind = True
        clAddress.Rebind()
    End Sub

    Private Sub clAddress_NeedDataSource(sender As Object, e As RadListViewNeedDataSourceEventArgs) Handles clAddress.NeedDataSource
        Dim objList = CustomerAddressManager.GetList(
            CustomerID:=CustomerID,
            AddressTypeID:=AddressTypeID,
            Archived:=ToNegBool(chkArchived.Checked)
            )
        clAddress.DataSource = objList
    End Sub

    Protected Sub clAddress_ToggleArchive(sender As Object, e As RadListViewCommandEventArgs) Handles clAddress.ToggleArchive
        Dim item As RadListViewDataItem = e.ListViewItem
        Dim keyId As Integer = item.GetDataKeyValue("CustomerAddressID")
        Dim camobj = CustomerAddressManager.GetById(keyId)
        Dim AddressID = camobj.AddressID

        Using ctx As New DataLibrary.ModelEntities
            ctx.Customer_ToggleArchive(keyId)
        End Using

        BindGrid()
    End Sub

#End Region


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            clAddress.OnAddNewRecordClientClick = "return openEditDialog(0," & CustomerID & "," & AddressTypeID & ");"
        End If
    End Sub
End Class
