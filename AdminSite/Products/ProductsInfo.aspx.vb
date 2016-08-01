
Partial Class Products_ProductsInfo
    Inherits BasePage

#Region "Properties"
    Public Property ProductID As Integer
        Get
            Dim v As Object = ViewState("ProductID")
            If v Is Nothing Then
                v = ToInteger(Request("ProductID"))
                ViewState("ProductID") = v
            End If
            Return v
        End Get
        Set(value As Integer)
            ViewState("ProductID") = value
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
#End Region

#Region "Page Events"
    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If ProductID = 0 Then
                Page.SetFocus(txtProductName)
                populateCategory(0)
                populateSubCategory(0, 0)
            Else
                Populate()
            End If
        End If
    End Sub

#End Region


#Region "Methods"

    Private Sub Populate()
        Dim obj = ProductManager.GetById(ProductID)

        txtProductName.Text = obj.ProductName
        txtShortDescription.Text = obj.ShortDescription
        txtLongDescription.Content = obj.LongDescription
        txtCost.Text = obj.Cost
        txtPrice.Text = obj.Price

        'Drop Down List 
        populateCategory(obj.CategoryID)
        populateSubCategory(obj.CategoryID, obj.SubCategoryID)

    End Sub

    Private Sub populateCategory(ByVal CategoryID As Integer)
        Dim cList = (From x In catList
                     Where (x.Archived = False Or x.CategoryID = CategoryID) And x.ParentID = 0)
        ControlBinding.BindListControl(ddlCategoryName, cList, "CategoryName", "CategoryID", True, , "0")
        ddlCategoryName.SelectedValue = CategoryID
    End Sub

    Private Sub populateSubCategory(ByVal CategoryID As Integer, ByVal SubCategoryID As Integer)
        Dim cList = (From x In catList
                     Where (x.Archived = False Or x.CategoryID = SubCategoryID) And x.ParentID = CategoryID)
        ControlBinding.BindListControl(ddlSubCategoryName, cList, "CategoryName", "CategoryID", True, , "0")
        ddlSubCategoryName.SelectedValue = SubCategoryID
    End Sub

    Private Function Save(Optional close As Boolean = False) As Boolean
        Dim newRecord As Boolean = (ProductID = 0)

        'prevent duplicate name
        Dim dup As Boolean = False
        Using ctx As New DataLibrary.ModelEntities()
            Dim count As Integer = (From c In ctx.Products
                         Where c.ProductName = txtProductName.Text).Count()
            If count > 0 Then
                dup = True
            End If
        End Using

        If newRecord Then
            If dup Then
                JGrowl.ShowMessage(JGrowlMessageType.Error, objectName:="Duplicate Name", message:="Duplicate Product Name")
            Else
                Using ctx As New DataLibrary.ModelEntities()

                    Dim NewID As Integer = ctx.Products.Count() + 2
                    Dim newCa = ctx.Products.Find(NewID)
                    If newCa Is Nothing Then
                        newCa = New DataLibrary.Product()
                        newCa.ProductID = NewID
                        newCa.ProductName = txtProductName.Text
                        newCa.ShortDescription = txtShortDescription.Text
                        newCa.LongDescription = txtLongDescription.Content
                        newCa.DateCreated = Now
                        newCa.Archived = False
                        newCa.CreatedBy = UserAuthentication.User.UserID
                        newCa.Cost = txtCost.Value
                        newCa.Price = txtPrice.Value

                        Dim Cate As String = ddlCategoryName.SelectedValue
                        Dim SubCate As String = ddlSubCategoryName.SelectedItem.Text
                        Dim CateID As Integer = (From c In ctx.Categories
                                                 Where c.CategoryName = Cate
                                                 Select c.CategoryID).FirstOrDefault()

                        Dim CateID2 As Integer = (From c In ctx.Categories
                                                 Where c.CategoryName = SubCate
                                                 Select c.CategoryID).FirstOrDefault()

                        newCa.CategoryID = CateID
                        newCa.SubCategoryID = CateID2

                        ctx.Products.Add(newCa)
                        ctx.SaveChanges()
                    End If

                End Using

                JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="Record", isDelayed:=close)
                txtProductName.Text = ""
                txtShortDescription.Text = ""
                txtLongDescription.Content = ""
                txtCost.Text = ""
                txtPrice.Text = ""
            End If

        Else
            Dim obj = ProductManager.GetById(ProductID)
            obj.ProductName = txtProductName.Text
            obj.ShortDescription = txtShortDescription.Text
            obj.LongDescription = txtLongDescription.Content

            'Save - get cate name
            'Dim Cate As String = ddlCategoryName.SelectedValue
            'Dim SubCate As String = ddlSubCategoryName.SelectedItem.Text

            'Using ctx As New DataLibrary.ModelEntities()
            '    Dim CateID As Integer = (From c In ctx.Categories
            '             Where c.CategoryName = Cate
            '             Select c.CategoryID).FirstOrDefault()

            '    Dim CateID2 As Integer = (From c In ctx.Categories
            '           Where c.CategoryName = SubCate
            '           Select c.CategoryID).FirstOrDefault()

            '    obj.CategoryID = CateID
            '    obj.SubCategoryID = CateID2
            'End Using

            obj.CategoryID = ddlCategoryName.SelectedValue
            obj.SubCategoryID = ddlSubCategoryName.SelectedValue
            obj.Cost = txtCost.Text
            obj.Price = txtPrice.Text
            ProductID = ProductManager.Save(obj)
            JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="Record", isDelayed:=close)
        End If


        

    End Function

#End Region

#Region "DropDownList"

    Protected Sub ddlCategoryName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCategoryName.SelectedIndexChanged
        Dim selected As String = ddlCategoryName.SelectedValue
            
        Using ctx As New DataLibrary.ModelEntities()
            'Dim CateID2 As Integer = (From c In ctx.Categories
            '                     Where c.CategoryName = selected
            '                     Select c.CategoryID).FirstOrDefault()

            Dim SubCate = (From c In ctx.Categories
                           Where c.ParentID = selected
                           Select c.CategoryName, c.CategoryID).ToArray()


            ddlSubCategoryName.DataSource = SubCate
            ddlSubCategoryName.DataTextField = "CategoryName"
            ddlSubCategoryName.DataBind()

        End Using

    End Sub

#End Region

#Region "Buttons"
    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Response.Redirect("~/Products/ManageProducts.aspx")
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Save()
    End Sub

    Protected Sub btnSaveClose_Click(sender As Object, e As EventArgs) Handles btnSaveClose.Click
        Save()
        Response.Redirect("~/Products/ManageProducts.aspx")
    End Sub
#End Region


End Class
