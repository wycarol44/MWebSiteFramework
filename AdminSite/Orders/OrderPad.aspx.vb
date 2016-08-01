Imports Telerik.Web.UI
Imports System.Web.Services
Imports System.Data

Partial Class Orders_OrderPad
    Inherits BasePage


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

    Public Property catList As List(Of Category_GetList_Result)
        Get
            If ViewState("catList") Is Nothing Then
                ViewState("catList") = CategoryManager.GetList().ToList
            End If
            Return ViewState("catList")
        End Get
        Set(value As List(Of Category_GetList_Result))
            ViewState("catList") = value
        End Set
    End Property

    Public Property ProList As List(Of Product_GetList_Result)
        Get
            If ViewState("ProList") Is Nothing Then
                ViewState("ProList") = ProductManager.GetList().ToList
            End If
            Return ViewState("ProList")
        End Get
        Set(value As List(Of Product_GetList_Result))
            ViewState("ProList") = value
        End Set
    End Property

#End Region

    Protected Sub btnSaveCon_Click(sender As Object, e As EventArgs) Handles btnSaveCon.Click
        orderheader.Save()
        clOrderPad.Visible = True
        Label1.Visible = True
        lbloTotal.Visible = True
        BindGrid()
    End Sub

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Response.Redirect("~/Orders/ManageOrders.aspx")
    End Sub



#Region "Methods"

    Private Sub BindGrid()
        clOrderPad.DataSource = Nothing
        clOrderPad.AutoBind = True
        clOrderPad.Rebind()
    End Sub

#End Region

#Region "CardList"




    Protected Sub clOrderPad_NeedDataSource(sender As Object, e As RadListViewNeedDataSourceEventArgs) Handles clOrderPad.NeedDataSource
        Dim objList = OrderDetailManager.GetList(PageIndex:=clOrderPad.CurrentPageIndex,
                                                 PageSize:=clOrderPad.PageSize,
                                                 OrderID:=OrderID)
        clOrderPad.DataSource = objList
        If objList.Any Then
            clOrderPad.VirtualItemCount = objList.FirstOrDefault.TotalCount
        End If

        If objList.Count > 0 Then
            Dim totalMoney As String = String.Format("{0:c}", objList.FirstOrDefault().OrderTotal)
            lbloTotal.Text = totalMoney
        End If

    End Sub

    Protected Sub rdajaxmanager_ajaxrequest(sender As Object, e As AjaxRequestEventArgs) Handles rdAjaxManager.AjaxRequest
        BindGrid()
    End Sub
#End Region

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            BindGrid()
            If OrderID > 0 Then
                Label1.Visible = True
                lbloTotal.Visible = True
                BindGrid()
            End If
        End If
        Cal_OrderTotal(OrderID)
    End Sub


    Protected Sub clOrderPad_ItemDataBound(sender As Object, e As RadListViewItemEventArgs) Handles clOrderPad.ItemDataBound

        'If Not Page.IsPostBack Then
        'Populate the Category
        Dim ddlCategory As DropDownList = e.Item.FindControl("ddlCategory")
        Dim item As RadListViewDataItem = e.Item
        Dim keyId As Integer = item.GetDataKeyValue("OrderDetailID")
        Dim oDetail = OrderDetailManager.GetById(keyId)
        Dim Pro = ProductManager.GetById(oDetail.ProductID)

        Dim cList = (From x In catList
               Where (x.Archived = False Or x.CategoryID = Pro.CategoryID) And x.ParentID = 0)
        ControlBinding.BindListControl(ddlCategory, cList, "CategoryName", "CategoryID", True, , "0")
        ddlCategory.SelectedValue = Pro.CategoryID

        'Populate the SubCategory
        Dim ddlSubCategory As DropDownList = e.Item.FindControl("ddlSubCategory")

        Dim cList2 = (From x In catList
               Where (x.Archived = False And x.CategoryID = Pro.SubCategoryID) And x.ParentID = Pro.CategoryID)
        ControlBinding.BindListControl(ddlSubCategory, cList2, "CategoryName", "CategoryID", True, , "0")
        ddlSubCategory.SelectedValue = Pro.SubCategoryID

        'Populate the Product
        Dim cList3 = (From x In ProList)

        If Pro.CategoryID = 0 And Pro.SubCategoryID = 0 And Pro.ProductID = 0 Then
            cList3 = (From x In ProList
                         Where (x.Archived = False))
        Else
            cList3 = (From x In ProList
                         Where (x.Archived = False) And x.CategoryID = Pro.CategoryID And x.SubCategoryID = Pro.SubCategoryID)
        End If

        Dim ddlProduct As DropDownList = e.Item.FindControl("ddlProduct")
        ControlBinding.BindListControl(ddlProduct, cList3, "ProductName", "ProductID", True, , "0")
        ddlProduct.SelectedValue = Pro.ProductID

        'Populate Qty
        Dim txtQty As RadNumericTextBox = e.Item.FindControl("txtQty")
        txtQty.Text = oDetail.Qty


        ''Populate Total
        'Dim Total As Label = e.Item.FindControl("Total")
        'Total.Text = txtQty.Text * oDetail.Price

        'Label OrderDetailID
        Dim HideOrderDetailID As Label = e.Item.FindControl("HideOrderDetailID")
        HideOrderDetailID.Text = oDetail.OrderDetailID
        ' End If
        Cal_OrderTotal(OrderID)
        'BindGrid()

    End Sub



    Protected Sub ddlCategory_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddlCategory As DropDownList = sender
        Dim ddlSubCategory As DropDownList = ddlCategory.Parent.FindControl("ddlSubCategory")

        Dim selected As String = ddlCategory.SelectedValue
        Dim cList = (From x In catList
                   Where (x.Archived = False And x.ParentID = selected))
        ControlBinding.BindListControl(ddlSubCategory, cList, "CategoryName", "CategoryID", True, , "0")

        ddlSubCategory.Enabled = True
    End Sub


    Protected Sub ddlSubCategory_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddlSubCategory As DropDownList = sender
        Dim ddlCategory As DropDownList = ddlSubCategory.Parent.FindControl("ddlCategory")
        Dim ddlProduct As DropDownList = ddlSubCategory.Parent.FindControl("ddlProduct")

        Dim selectedCateID As String = ddlCategory.SelectedValue
        Dim selectedSubCateID As String = ddlSubCategory.SelectedValue

        Dim cList = (From x In ProList
                    Where (x.Archived = False And x.CategoryID = selectedCateID And x.SubCategoryID = selectedSubCateID))
        ControlBinding.BindListControl(ddlProduct, cList, "ProductName", "ProductID", True, , "0")
        ddlProduct.Enabled = True
    End Sub

    Protected Sub txtQty_OnTextChanged(sender As Object, e As EventArgs)
        'Find Price - base on Product
        Dim txtQty As RadNumericTextBox = sender
        Dim ddlProduct As DropDownList = txtQty.Parent.FindControl("ddlProduct")
        Dim lbltot As Label = txtQty.Parent.FindControl("lbltot")
        Dim selectedProID As String = ddlProduct.SelectedValue
        Dim HideOrderDetailLabel As Label = txtQty.Parent.FindControl("HideOrderDetailID")

        Dim OrderDetailID As Integer = HideOrderDetailLabel.Text


        Dim Product = ProductManager.GetById(selectedProID)
        Dim total As String = txtQty.Text * Product.Price
        'ViewState("total") = total

        'lbltot.text = total
        ''How to get orderdetailID
        Dim orderdetail = OrderDetailManager.GetById(OrderDetailID)
        orderdetail.ProductID = selectedProID
        orderdetail.Price = Product.Price
        orderdetail.Qty = txtQty.Text
        OrderDetailManager.Save(orderdetail)

        Cal_OrderTotal(OrderID)
        BindGrid()
    End Sub

    Protected Sub Cal_OrderTotal(OrderID As Integer)
        Dim oobj = OrderManager.GetById(OrderID)
        Using ctx As New DataLibrary.ModelEntities
            oobj.OrderTotal = ctx.Calculate_OrderTotal(OrderID).SingleOrDefault()
        End Using
        OrderID = OrderManager.Save(oobj)
    End Sub

    Public Shared addnew As Boolean = False
    'Add NEW
    Protected Sub clOrderPad_ItemCommand(sender As Object, e As RadListViewCommandEventArgs) Handles clOrderPad.ItemCommand
        If e.CommandName = "InitInsert" Then
            addnew = True
        End If
    End Sub

    Protected Sub clOrderPad_ItemCreated(sender As Object, e As RadListViewItemEventArgs) Handles clOrderPad.ItemCreated

        If e.Item.ItemType = RadListViewItemType.InsertItem Then
            ''populate the category
            Dim ddlcategory2 As DropDownList = e.Item.FindControl("ddlCategory2")
            Dim clist = (From x In catList
                  Where (x.Archived = False And x.ParentID = 0))
            ControlBinding.BindListControl(ddlcategory2, clist, "CategoryName", "CategoryID", True, , "0")

            ''populate the subcategory
            Dim ddlsubcategory2 As DropDownList = e.Item.FindControl("ddlsubcategory2")
            Dim clist2 = (From x In catList
                   Where (x.Archived = False And x.ParentID = ddlcategory2.SelectedValue))
            ControlBinding.BindListControl(ddlsubcategory2, clist2, "categoryname", "categoryid", True, , "0")

            ''populate the product
            Dim ddlproduct2 As DropDownList = e.Item.FindControl("ddlproduct2")
            Dim clist3 = (From x In ProList
                        Where (x.Archived = False) And x.CategoryID = ddlcategory2.SelectedValue And x.SubCategoryID = ddlsubcategory2.SelectedValue)
            ControlBinding.BindListControl(ddlproduct2, clist3, "productname", "productid", True, , "0")
        End If

    End Sub

    Protected Sub ddlCategory2_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddlCategory2 As DropDownList = sender
        Dim ddlSubCategory2 As DropDownList = ddlCategory2.Parent.FindControl("ddlSubCategory2")

        Dim selected As String = ddlCategory2.SelectedValue
        Dim cList = (From x In catList
                   Where (x.Archived = False And x.ParentID = selected))
        ControlBinding.BindListControl(ddlSubCategory2, cList, "CategoryName", "CategoryID", True, , "0")

        ddlSubCategory2.Enabled = True
    End Sub


    Protected Sub ddlSubCategory2_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddlSubCategory2 As DropDownList = sender
        Dim ddlCategory2 As DropDownList = ddlSubCategory2.Parent.FindControl("ddlCategory2")
        Dim ddlProduct2 As DropDownList = ddlSubCategory2.Parent.FindControl("ddlProduct2")

        Dim selectedCateID As String = ddlCategory2.SelectedValue
        Dim selectedSubCateID As String = ddlSubCategory2.SelectedValue

        Dim cList = (From x In ProList
                    Where (x.Archived = False And x.CategoryID = selectedCateID And x.SubCategoryID = selectedSubCateID))
        ControlBinding.BindListControl(ddlProduct2, cList, "ProductName", "ProductID", True, , "0")
        ddlProduct2.Enabled = True


    End Sub

    Protected Sub ddlProduct2_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddlProduct2 As DropDownList = sender
        Dim Price2 As Label = ddlProduct2.Parent.FindControl("Price2")
        Dim Pro = ProductManager.GetById(ddlProduct2.SelectedValue)
        Price2.Text = Pro.Price

        'ViewState("ProductID") = ddlProduct2.SelectedValue
    End Sub

    Protected Sub txtQty2_OnTextChanged(sender As Object, e As EventArgs)
        'Find Price - base on Product
        Dim txtQty2 As RadNumericTextBox = sender
        Dim ddlProduct2 As DropDownList = txtQty2.Parent.FindControl("ddlProduct2")
        Dim Total2 As Label = txtQty2.Parent.FindControl("Total2")
        Dim selectedProID As String = ddlProduct2.SelectedValue

        Dim Product = ProductManager.GetById(selectedProID)
        'Total2.Text = txtQty2.Text * Product.Price
        'ViewState("Qty") = txtQty2.Text

    End Sub



    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        If addnew Then

            Dim ddlproduct2 As DropDownList = clOrderPad.InsertItem.FindControl("ddlproduct2")
            Dim txtqty2 As RadNumericTextBox = clOrderPad.InsertItem.FindControl("txtqty2")
            Dim price2 As Label = clOrderPad.InsertItem.FindControl("price2")


            If Not ddlproduct2 Is Nothing And Not txtqty2 Is Nothing And Not price2 Is Nothing Then

                ''    'Check Duplicate
                '' take all product ID that in @OrderID, check wheter ddlproduct2 included
                Dim contains As Boolean
                Using ctx As New DataLibrary.ModelEntities
                    Dim ProductIDs = (From od In ctx.OrderDetails
                                      Where od.OrderID = OrderID
                                      Select od.ProductID)
                    contains = ProductIDs.Contains(ddlproduct2.SelectedValue)
                End Using

                If contains Then
                    JGrowl.ShowMessage(JGrowlMessageType.Alert, objectName:="Alert", message:="There is duplicate record")
                Else
                    Dim obj = New DataLibrary.OrderDetail
                    obj.OrderDetailID = 0
                    obj.OrderID = OrderID
                    obj.ProductID = ddlproduct2.SelectedValue
                    obj.Qty = txtqty2.Text
                    obj.Price = price2.Text
                    obj.Archived = False

                    OrderDetailManager.Save(obj)

                    JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="Success", message:="New Record Saved Successfully")
                End If

            End If
        End If
     
    End Sub

End Class
