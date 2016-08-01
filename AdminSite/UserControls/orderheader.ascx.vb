Imports Telerik.Web.UI
Imports System.Web.Services
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Configuration



Partial Class UserControls_orderheader
    Inherits System.Web.UI.UserControl

#Region "Properties"
    Public Property OrderID As Integer
        Get
            Dim v As Object = ViewState("OrderID")
            If v Is Nothing Then
                v = ToInteger(Request("OrderID"))
                ViewState("OrderID") = v
            End If
            Return v
        End Get
        Set(value As Integer)
            ViewState("OrderID") = value
        End Set
    End Property

#End Region

    'Customer Name Dropdown List
    Protected Sub lblCustomerName_Load(sender As Object, e As EventArgs) Handles lblCustomerName.Load
        Dim ddlList = CustomerManager.GetList(Archived:=False)
        ddlCustomerName.DataSource = ddlList
        ddlCustomerName.DataTextField = "CustomerName"
        ddlCustomerName.DataValueField = "CustomerID"
        ddlCustomerName.DataBind()

        If OrderID <> 0 Then
            Dim obj = OrderManager.GetById(OrderID)
            Dim CusName = CustomerManager.GetById(obj.CustomerID).CustomerName
            If Not Page.IsPostBack Then
                ddlCustomerName.SelectedValue = obj.CustomerID
                ddlCustomerName.Text = CusName
            End If
        End If

    End Sub

    Private Const ItemsPerRequest As Integer = 10

    Protected Sub ddlCustomerName_ItemsRequested(ByVal sender As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)
        Dim data2 = CustomerManager.GetList()
        Dim data As New DataTable()
        data.LoadDataRow(data2, True) 'Change array to datatable

        Dim itemOffset As Integer = e.NumberOfItems
        Dim endOffset As Integer = Math.Min(itemOffset + ItemsPerRequest, data.Rows.Count)
        e.EndOfItems = endOffset = data.Rows.Count

        For i As Integer = itemOffset To endOffset - 1
            ddlCustomerName.Items.Add(New RadComboBoxItem(data.Rows(i)("CustomerName").ToString(), data.Rows(i)("CustomerName").ToString()))
        Next
    End Sub


    'Order Date and Contact Phone
    Protected Sub txtOrderDate_Load(sender As Object, e As EventArgs) Handles txtOrderDate.Load
        If Not Page.IsPostBack Then
            Dim obj = OrderManager.GetById(OrderID)
            txtOrderDate.DbSelectedDate = obj.DateOrdered
        End If
    End Sub

    Protected Sub mpPhone_Load(sender As Object, e As EventArgs) Handles mpPhone.Load
        If Not Page.IsPostBack Then
            Dim obj = OrderManager.GetById(OrderID)
            mpPhone.Text = obj.ContactPhone
        End If
    End Sub

    Protected Sub txtTrackingNumber_Load(sender As Object, e As EventArgs) Handles txtTrackingNumber.Load
        If Not Page.IsPostBack Then
            Dim obj = OrderManager.GetById(OrderID)
            txtTrackingNumber.Text = obj.TrackingNumber
        End If
    End Sub

    Protected Sub txtDateShipped_Load(sender As Object, e As EventArgs) Handles txtDateShipped.Load
        If Not Page.IsPostBack Then
            Dim obj = OrderManager.GetById(OrderID)
            txtDateShipped.DbSelectedDate = obj.DateShipped
        End If
    End Sub

    'Billing Dropdown List AddTypeID = 2
    Protected Sub ddlBillAdd_Load(sender As Object, e As EventArgs) Handles ddlBillAdd.Load
        If OrderID <> 0 Then
            Dim obj = OrderManager.GetById(OrderID)
            If Not Page.IsPostBack Then
                PopulateBillAdd(obj.CustomerID)
            End If
        End If
    End Sub


    Protected Sub ddlBillAdd_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlBillAdd.SelectedIndexChanged
        Dim AddID = ddlBillAdd.SelectedValue
        Dim Address = AddressManager.GetById(AddID)

        txtAddress1.Text = Address.Address1
        txtAddress2.Text = Address.Address2
        txtCity.Text = Address.City
        txtState.Text = Address.State
        txtZip.Text = Address.PostalCode

    End Sub

    'Shipping Dropdown List AddTypeID = 1
    Protected Sub ddlShipAdd_Load(sender As Object, e As EventArgs) Handles ddlShipAdd.Load
        If OrderID <> 0 Then
            Dim obj = OrderManager.GetById(OrderID)
            If Not Page.IsPostBack Then
                PopulateShipAdd(obj.CustomerID)
            End If
        End If
    End Sub


    Protected Sub ddlShipAdd_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlShipAdd.SelectedIndexChanged
        Dim AddID = ddlShipAdd.SelectedValue
        Dim Address = AddressManager.GetById(AddID)

        txtSAddress1.Text = Address.Address1
        txtSAddress2.Text = Address.Address2
        txtSCity.Text = Address.City
        txtSState.Text = Address.State
        txtSZip.Text = Address.PostalCode

    End Sub


    Protected Sub ddlCustomerName_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlCustomerName.SelectedIndexChanged
        Dim newCusID = ddlCustomerName.SelectedValue
        Dim newCusName = ddlCustomerName.Text

        'disable the required field validator
        PopulateBillAdd(newCusID)
        PopulateShipAdd(newCusID)
    End Sub

    Protected Sub PopulateBillAdd(newCusID As Integer)
        Dim BillFullAdd = CustomerAddressManager.GetList(CustomerID:=newCusID, AddressTypeID:=2)

        ddlBillAdd.DataSource = BillFullAdd
        ddlBillAdd.DataTextField = "FullAddress"
        ddlBillAdd.DataValueField = "AddressID"
        ddlBillAdd.DataBind()


        Dim AddID = ddlBillAdd.SelectedValue

        If AddID <> "" Then

            Dim Address = AddressManager.GetById(AddID)

            txtAddress1.Text = Address.Address1
            txtAddress2.Text = Address.Address2
            txtCity.Text = Address.City
            txtState.Text = Address.State
            txtZip.Text = Address.PostalCode

        End If
    End Sub

    Protected Sub PopulateShipAdd(newCusID As Integer)

        Dim ShipFullAdd = CustomerAddressManager.GetList(CustomerID:=newCusID, AddressTypeID:=1)

        ddlShipAdd.DataSource = ShipFullAdd
        ddlShipAdd.DataTextField = "FullAddress"
        ddlShipAdd.DataValueField = "AddressID"
        ddlShipAdd.DataBind()

        Dim AddID2 = ddlShipAdd.SelectedValue
        Dim Address2 = AddressManager.GetById(AddID2)

        txtSAddress1.Text = Address2.Address1
        txtSAddress2.Text = Address2.Address2
        txtSCity.Text = Address2.City
        txtSState.Text = Address2.State
        txtSZip.Text = Address2.PostalCode
    End Sub



    'Public Sub LoadByOrderID(oID As Integer)
    '    Dim obj = OrderManager.GetById(oID)

    '    'Populate Customer dropdown list
    '    Dim Cus = CustomerManager.GetById(obj.CustomerID)
    '    Dim CusID = Cus.CustomerID
    '    Dim CusName = Cus.CustomerName
    '    ddlCustomerName.SelectedValue = CusID
    '    ddlCustomerName.Text = CusName

    '    PopulateBillAdd(CusID)
    '    PopulateShipAdd(CusID)
    'End Sub


    Public Function Save() As Integer

        Dim obj = OrderManager.GetById(OrderID)

        obj.CustomerID = ddlCustomerName.SelectedValue
        obj.DateOrdered = txtOrderDate.DbSelectedDate
        obj.ContactPhone = mpPhone.Text

        obj.BillingAddress1 = txtAddress1.Text
        obj.BillingAddress2 = txtAddress2.Text
        obj.BillingCity = txtCity.Text
        obj.BillingState = txtState.Text
        obj.BillingZip = txtZip.Text

        obj.ShippingAddress1 = txtSAddress1.Text
        obj.ShippingAddress2 = txtSAddress2.Text
        obj.ShippingCity = txtSCity.Text
        obj.ShippingState = txtSState.Text
        obj.ShippingZip = txtSZip.Text

        OrderManager.Save(obj)
        Return OrderID

    End Function


    Protected Sub CheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox.CheckedChanged
        If CheckBox.Checked Then
            Dim AddID = ddlBillAdd.SelectedValue
            Dim Address = AddressManager.GetById(AddID)

            txtSAddress1.Text = Address.Address1
            txtSAddress2.Text = Address.Address2
            txtSCity.Text = Address.City
            txtSState.Text = Address.State
            txtSZip.Text = Address.PostalCode
        End If

    End Sub
End Class
