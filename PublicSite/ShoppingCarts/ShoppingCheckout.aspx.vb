
Imports Telerik.Web.UI
Imports System.Web.Services
Imports System.Data

Partial Class ShoppingCarts_ShoppingCheckout
    Inherits BasePage

    Private Sub BindGrid()
        clCarts.DataSource = Nothing
        clCarts.AutoBind = True
        clCarts.Rebind()
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            BindGrid()
            populatestate()
        End If
    End Sub

    Protected Sub clCarts_ItemDataBound(sender As Object, e As RadListViewItemEventArgs) Handles clCarts.ItemDataBound
        Dim lblCartID As Label = e.Item.FindControl("lblCartID")
        Dim lblExtendedPrice As Label = e.Item.FindControl("lblExtendedPrice")
        Dim obj = CartsManager.GetById(lblCartID.Text)
        Dim pro = ProductManager.GetById(obj.ProductID)

        'Total price
        lblExtendedPrice.Text = FormatCurrency((obj.Qty * pro.Price), 2)
    End Sub

    Protected Sub clCarts_NeedDataSource(sender As Object, e As RadListViewNeedDataSourceEventArgs) Handles clCarts.NeedDataSource
        Dim objList = CartsManager.GetList(PageIndex:=clCarts.CurrentPageIndex,
                                                      PageSize:=clCarts.PageSize)

        clCarts.DataSource = objList

        If objList.Any Then
            clCarts.VirtualItemCount = objList.FirstOrDefault.TotalCount
        End If
    End Sub

    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Response.Redirect("ShoppingCarts.aspx")
    End Sub

    Private Sub populatestate()
        cbState.Country = MilesMetaCountry.UnitedStates
        cbState.StateDisplayMode = MilesStateComboBox.DisplayMode.Code

        cbState2.Country = MilesMetaCountry.UnitedStates
        cbState2.StateDisplayMode = MilesStateComboBox.DisplayMode.Code

    End Sub

    Protected Sub CheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox.CheckedChanged
        If CheckBox.Checked Then
            txtSAddress1.Text = txtAddress1.Text
            txtSAddress2.Text = txtAddress2.Text
            txtSCity.Text = txtCity.Text
            cbState2.SelectedValue = cbState.SelectedValue
            txtSZip.Text = txtZip.Text
        End If
    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click

            'Create new customer, Table:Address, customer, customeraddress

            'Add Address Table
            Dim newAddB As New Address With {.Address1 = txtAddress1.Text, .Address2 = txtAddress2.Text, .City = txtCity.Text, .Archived = False}
            Dim newAddS As New Address With {.Address1 = txtSAddress1.Text, .Address2 = txtSAddress2.Text, .City = txtSCity.Text, .Archived = False}
            AddressManager.Save(newAddB)
            AddressManager.Save(newAddS)

            'Add Customer
            Dim newCus As New Customer With {.CustomerName = txtFirstName.Text + txtLastName.Text, .AddressID = newAddB.AddressID, .Archived = False}
            CustomerManager.Save(newCus)

            'Add customeraddress
            Dim newCusAddB As New CustomerAddress With {.CustomerID = newCus.CustomerID, .AddressID = newAddB.AddressID, .Archived = False}
            Dim newCusAddS As New CustomerAddress With {.CustomerID = newCus.CustomerID, .AddressID = newAddS.AddressID, .Archived = False}
            CustomerAddressManager.Save(newCusAddB)
            CustomerAddressManager.Save(newCusAddS)

        'Add neworder, pending is 25
        Dim newOrder As New Order With {.CustomerID = newCus.CustomerID, .DateOrdered = Now(), .BillingAddress1 = newAddB.Address1, .BillingAddress2 = newAddB.Address2, .BillingCity = newAddB.City, .ShippingAddress1 = newAddS.Address1, .ShippingAddress2 = newAddS.Address2, .ShippingCity = newAddS.City, .OrderStatusID = 25}
        OrderManager.Save(newOrder)

        'Need Add to OrdeDetail Table:(((((, Need a FOR LOOP, Product Info is in Shopping Carts Table
        Using ctx As New DataLibrary.ModelEntities()
            Dim cartsInfo = (From ci In ctx.ShoppingCarts
                             Where ci.SessionID = Session.SessionID
                             Select ci)

            For Each cInfo In cartsInfo
                Dim pro = ProductManager.GetById(cInfo.ProductID)
                Dim newOrderDetail As New OrderDetail With {.OrderID = newOrder.OrderID, .ProductID = cInfo.ProductID, .Qty = cInfo.Qty, .Price = pro.Price, .Archived = False}
                OrderDetailManager.Save(newOrderDetail)
            Next

        End Using

        'Save the Session
        Dim InfoArray As New ArrayList
        InfoArray.Add(txtFirstName.Text)
        InfoArray.Add(txtLastName.Text)
        InfoArray.Add(txtSAddress1.Text)
        InfoArray.Add(txtSAddress2.Text)
        InfoArray.Add(txtSCity.Text)
        InfoArray.Add(cbState2.SelectedItem.Text)
        InfoArray.Add(txtSZip.Text)
        InfoArray.Add(newOrder.OrderID)
        Session("InfoArray") = InfoArray



        Dim Str1 As String = String.Format("<p>Dear {0} {1}: </p>", txtFirstName.Text, txtLastName.Text)
        Dim Str2 As String = "<p>Thank you for submitting your order with us. Your order will ship shortly.</p>"
        Dim Str3 As String = "<p>The following are the details of your order: </p>"
        Dim Str4 As String = "<p>First Name: " & txtFirstName.Text & "</p><p>Last Name: " & txtLastName.Text & "</p><p>Shipping Address: " & txtSAddress1.Text & ", " _
                             & txtSAddress2.Text & ", " & txtSCity.Text & "</p>"

        'OrderDetail
        Dim objLists = CartsManager.GetList(PageIndex:=clCarts.CurrentPageIndex,
                                              PageSize:=clCarts.PageSize).ToList()

        Dim Str As String = Str1 + Str2 + Str3 + Str4
        Dim Strb As String = ""

        For Each objList In objLists
            Strb += "<p> Product Name: " & objList.ProductName & ", Qty: " & objList.Qty & ", Price: " & objList.Price & "</p>"
        Next

        Dim Strc As String = "THANK YOU!!!!"


        Functions.SendEmail(emailFrom:="luquan@milestechnologies.com", emailTo:="luquan@milestechnologies.com", emailSubject:="Order Accepted", emailBody:=Str + Strb + Strc)

        'redirect tp confirmation page
        Response.Redirect("ShoppingConfirmation.aspx")

    End Sub


End Class
