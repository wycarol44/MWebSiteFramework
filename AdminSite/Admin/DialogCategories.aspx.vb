
Partial Class Admin_DialogCategories
    Inherits BasePage

#Region "Properties"
    Public Property CategoryID As Integer
        Get
            Dim v As Object = ViewState("CategoryID")
            If v Is Nothing Then
                v = ToInteger(Request("CategoryID"))
                ViewState("CategoryID") = v
            End If
            Return v
        End Get
        Set(value As Integer)
            ViewState("CategoryID") = value
        End Set
    End Property
#End Region
#Region "Page Events"
    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If CategoryID = 0 Then
                Page.SetFocus(txtCategoryName)
                pnlSearch.Visible = False
                clCategories.Visible = False
            End If
            If CategoryID > 0 Then
                Populate()
                BindGrid()
            End If
        End If
    End Sub

#End Region







#Region "Methods"

    Private Sub Populate()
        Dim obj = CategoryManager.GetById(CategoryID)
        txtCategoryName.Text = obj.CategoryName
        txtDescription.Content = obj.Description
        'SetEntityFooter(obj.CreatedBy, obj.DateCreated, obj.ModifiedBy, obj.DateModified)
    End Sub

    Private Function Save(Optional close As Boolean = False) As Boolean
        Dim newRecord As Boolean = (CategoryID = 0)

        'prevent duplicate name
        Dim dup As Boolean = False
        Using ctx As New DataLibrary.ModelEntities()
            Dim count As Integer = (From c In ctx.Categories
                         Where c.ParentID = 0 And c.CategoryName = txtCategoryName.Text).Count()
            If count > 0 Then
                dup = True
            End If
        End Using

        If newRecord Then
            If dup Then
                JGrowl.ShowMessage(JGrowlMessageType.Error, objectName:="Duplicate Name")
            Else
                Using ctx As New DataLibrary.ModelEntities()

                    Dim NewID As Integer = ctx.Categories.Count() + 2
                    Dim newCa = ctx.Categories.Find(NewID)
                    If newCa Is Nothing Then
                        newCa = New DataLibrary.Category()
                        newCa.CategoryID = NewID
                        newCa.ParentID = 0
                        newCa.CategoryName = txtCategoryName.Text
                        newCa.Description = txtDescription.Text
                        newCa.DateCreated = Now
                        newCa.Archived = False
                        newCa.CreatedBy = UserAuthentication.User.UserID
                        ctx.Categories.Add(newCa)
                        ctx.SaveChanges()
                    End If

                    'Display Card List
                    pnlSearch.Visible = True
                    clCategories.Visible = True

                End Using

                txtCategoryName.Text = ""
                txtDescription.Content = ""

                JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="Record", isDelayed:=close)
                clCategories.Rebind()

            End If

        Else
            Dim obj = CategoryManager.GetById(CategoryID)
            obj.CategoryName = txtCategoryName.Text
            obj.Description = txtDescription.Text
            CategoryID = CategoryManager.Save(obj)

            JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="Record", isDelayed:=close)
            clCategories.Rebind()
        End If


    End Function

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Response.Redirect("~//Admin/ManageCategories.aspx")
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Save()
    End Sub

    Protected Sub btnSaveClose_Click(sender As Object, e As EventArgs) Handles btnSaveClose.Click
        Save()
        Response.Redirect("~//Admin/ManageCategories.aspx")
    End Sub

#End Region



    Protected Sub pnlSearch_Search(sender As Object, e As EventArgs) Handles pnlSearch.Search
        BindGrid()
    End Sub





#Region "Grid"
    Protected Sub clCategories_ItemCommand(sender As Object, e As RadListViewCommandEventArgs) Handles clCategories.ItemCommand
        If e.CommandName = "PerformInsert" Then

            If TypeOf e.ListViewItem Is RadListViewInsertItem Then
                InsertUpdate(e, 0)
            ElseIf TypeOf e.ListViewItem Is RadListViewEditableItem Then
                Dim item As RadListViewDataItem = e.ListViewItem
                Dim keyID = item.GetDataKeyValue("CategoryID")
                InsertUpdate(e, keyID)
            End If
        End If
    End Sub

    Protected Sub InsertUpdate(ByVal e As Telerik.Web.UI.RadListViewCommandEventArgs, ByVal KeyID As Integer)
        If TypeOf e.ListViewItem Is RadListViewEditableItem Or TypeOf e.ListViewItem Is RadListViewInsertItem Then
            'get the edit item
            Dim editedItem As Telerik.Web.UI.RadListViewEditableItem = CType(e.ListViewItem, Telerik.Web.UI.RadListViewEditableItem)
            'Get the edit fields
            Dim txtCategoryName3 As TextBox
            txtCategoryName3 = editedItem.FindControl("txtCategoryName3")
            Dim txtDesc As TextBox
            txtDesc = editedItem.FindControl("txtDesc")


            'prevent duplicate name
            Dim dup As Boolean = False
            Using ctx As New DataLibrary.ModelEntities()
                Dim count As Integer = (From c In ctx.Categories
                             Where c.ParentID = CategoryID And c.CategoryName = txtCategoryName3.Text).Count()

                If count > 0 Then
                    dup = True
                End If
            End Using

            If KeyID > 0 Then
                'Edit
                Dim ur As DataLibrary.Category = CategoryManager.GetById(KeyID)
                ur.CategoryName = txtCategoryName3.Text
                ur.Description = txtDesc.Text
                CategoryManager.Save(ur)

                JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="Category")
                clCategories.Rebind()

            ElseIf KeyID = 0 Then
                If dup Then
                    JGrowl.ShowMessage(JGrowlMessageType.Error, objectName:="Duplicate Name")
                Else
                    Using ctx As New DataLibrary.ModelEntities()

                        'Add new
                        Dim NewID As Integer = ctx.Categories.Count() + 2
                        Dim newCa = ctx.Categories.Find(NewID)
                        If newCa Is Nothing Then
                            newCa = New DataLibrary.Category()
                            newCa.CategoryID = NewID
                            newCa.ParentID = CategoryID
                            newCa.CategoryName = txtCategoryName3.Text
                            newCa.Description = txtDesc.Text
                            newCa.DateCreated = Now
                            newCa.Archived = False
                            newCa.CreatedBy = UserAuthentication.User.UserID
                            ctx.Categories.Add(newCa)
                            ctx.SaveChanges()
                        End If
                    End Using

                    JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="Category")
                    clCategories.Rebind()
                End If
            End If

        End If

    End Sub

    Protected Sub clCategories_ItemCreated(sender As Object, e As RadListViewItemEventArgs) Handles clCategories.ItemCreated
        If e.Item.IsInEditMode Then
            Dim txtCategoryName3 As TextBox = e.Item.FindControl("txtCategoryName3")
            If txtCategoryName3 IsNot Nothing Then
                txtCategoryName3.Focus()
            End If
        End If
    End Sub


#End Region


#Region "CardList"

    Private Sub BindGrid()
        clCategories.DataSource = Nothing
        clCategories.AutoBind = True
        clCategories.Rebind()
    End Sub

    Private Sub clCategories_NeedDataSource(sender As Object, e As RadListViewNeedDataSourceEventArgs) Handles clCategories.NeedDataSource
        Dim objList = CategoryManager.GetList(PageIndex:=clCategories.CurrentPageIndex,
                                            PageSize:=clCategories.PageSize,
                                            CategoryID:=CategoryID,
                                            ParentID:=CategoryID,
                                            CategoryName:=txtCategoryName2.Text,
                                            Description:=txtDescription2.Text,
                                            DateCreatedFrom:=txtFrom2.DbSelectedDate,
                                            DateCreatedTo:=txtTo2.DbSelectedDate,
                                            Archived:=ToNegBool(chkArchived.Checked),
                                            SortExpression:=clCategories.CurrentSortExpression.FieldName,
                                            SortOrder:=clCategories.CurrentSortExpression.SortOrder)
        clCategories.DataSource = objList
    End Sub

    Protected Sub clCategories_ToggleArchive(sender As Object, e As RadListViewCommandEventArgs) Handles clCategories.ToggleArchive
        Dim item As RadListViewDataItem = e.ListViewItem
        Dim keyId As Integer = item.GetDataKeyValue("CategoryID")

        Using ctx As New DataLibrary.ModelEntities
            ctx.Category_ToggleArchive(keyId)
        End Using

        BindGrid()
    End Sub



#End Region

End Class
