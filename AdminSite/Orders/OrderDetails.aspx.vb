
Partial Class Orders_OrderDetails
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


    Public Property OrderDetailID As Integer
        Get
            Dim v As Object = ViewState("OrderDetailID")
            If v Is Nothing Then
                v = ToInteger(Request("OrderDetailID"))
                ViewState("OrderDetailID") = v
            End If
            Return v
        End Get
        Set(value As Integer)
            ViewState("OrderDetailID") = value
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


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim obj = OrderManager.GetById(OrderID)
            If obj.OrderStatusID = BusinessLibrary.OrderStatus.Shipped Then
                btnSaveCon.Enabled = False
                btnClose.Enabled = False
            End If
            If OrderDetailID = 0 Then
                populateCategory(0)
                populateSubCategory(0, 0)
                populateProduct(0, 0, 0)
            Else
                Populate()
            End If
        End If
    End Sub

    Private Sub populateCategory(ByVal CategoryID As Integer)
        Dim cList = (From x In catList
                     Where (x.Archived = False Or x.CategoryID = CategoryID) And x.ParentID = 0)
        ControlBinding.BindListControl(ddlCategory, cList, "CategoryName", "CategoryID", True, , "0")
        ddlCategory.SelectedValue = CategoryID
    End Sub

    Private Sub populateSubCategory(ByVal CategoryID As Integer, ByVal SubCategoryID As Integer)
        Dim cList = (From x In catList
                     Where (x.Archived = False And x.CategoryID = SubCategoryID) And x.ParentID = CategoryID)
        ControlBinding.BindListControl(ddlSubCategory, cList, "CategoryName", "CategoryID", True, , "0")
        ddlSubCategory.SelectedValue = SubCategoryID
    End Sub

    Private Sub populateProduct(ByVal CategoryID As Integer, ByVal SubCategoryID As Integer, ByVal ProductID As Integer)

        Dim cList = (From x In ProList)

        If CategoryID = 0 And SubCategoryID = 0 And ProductID = 0 Then
            cList = (From x In ProList
                         Where (x.Archived = False))
        Else
            cList = (From x In ProList
                         Where (x.Archived = False) And x.CategoryID = CategoryID And x.SubCategoryID = SubCategoryID)
        End If

        ControlBinding.BindListControl(ddlProduct, cList, "ProductName", "ProductID", True, , "0")
        ddlProduct.SelectedValue = ProductID
    End Sub


    Protected Sub ddlCategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCategory.SelectedIndexChanged
        Dim selected As String = ddlCategory.SelectedValue
        Dim cList = (From x In catList
                   Where (x.Archived = False And x.ParentID = selected))
        ControlBinding.BindListControl(ddlSubCategory, cList, "CategoryName", "CategoryID", True, , "0")

        ddlSubCategory.Enabled = True
    End Sub


    Protected Sub ddlSubCategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSubCategory.SelectedIndexChanged
        Dim selectedCateID As String = ddlCategory.SelectedValue
        Dim selectedSubCateID As String = ddlSubCategory.SelectedValue

        Dim cList = (From x In ProList
                    Where (x.Archived = False And x.CategoryID = selectedCateID And x.SubCategoryID = selectedSubCateID))
        ControlBinding.BindListControl(ddlProduct, cList, "ProductName", "ProductID", True, , "0")

        ddlProduct.Enabled = True
    End Sub


    Private Sub Populate()
        Dim obj1 = OrderDetailManager.GetById(OrderDetailID)
        Dim obj2 = ProductManager.GetById(obj1.ProductID)

        'Drop Down List 
        populateCategory(obj2.CategoryID)
        populateSubCategory(obj2.CategoryID, obj2.SubCategoryID)
        populateProduct(obj2.CategoryID, obj2.SubCategoryID, obj2.ProductID)

        'Populate Qty
        rtbQty.Value = obj1.Qty
    End Sub

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        CloseDialogWindow()
    End Sub

    Protected Sub btnSaveCon_Click(sender As Object, e As EventArgs) Handles btnSaveCon.Click
        If Save() Then
            CloseDialogWindow("1")
        End If
    End Sub


    Private Function Save() As Boolean

        If OrderDetailID > 0 Then

            Dim odobj = OrderDetailManager.GetById(OrderDetailID)

            odobj.ProductID = ddlProduct.SelectedValue
            odobj.Qty = CInt(rtbQty.Text)
            Dim pobj = ProductManager.GetById(odobj.ProductID)
            odobj.Price = pobj.Price

            OrderDetailID = OrderDetailManager.Save(odobj)


            'OrderID 对应的order Total 也得改
            Cal_OrderTotal(OrderID)
        ElseIf OrderDetailID = 0 Then
            'Add NEW Order
            Dim odobj = New DataLibrary.OrderDetail()

            odobj.OrderID = OrderID
            odobj.ProductID = ddlProduct.SelectedValue
            odobj.Qty = CInt(rtbQty.Text)

            Dim pobj = ProductManager.GetById(odobj.ProductID)
            odobj.Price = pobj.Price

            odobj.Archived = False
            OrderDetailID = OrderDetailManager.Save(odobj)

            Cal_OrderTotal(OrderID)
        End If


        JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="Record")
        Return True
    End Function

    Protected Sub Cal_OrderTotal(OrderID As Integer)
        Dim oobj = OrderManager.GetById(OrderID)
        Using ctx As New DataLibrary.ModelEntities
            oobj.OrderTotal = ctx.Calculate_OrderTotal(OrderID).SingleOrDefault()
        End Using
        OrderID = OrderManager.Save(oobj)
    End Sub


End Class
