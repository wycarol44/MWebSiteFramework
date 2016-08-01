
Partial Class Admin_ManageListItem
    Inherits BasePage

    Protected Sub Admin_ManageListItem_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Page.SetFocus(ddlItemType)
            PopulateDDL()
        End If
    End Sub

    Private Sub PopulateDDL()
        Dim types = ManagedTypeManager.GetList()
        ControlBinding.BindListControl(ddlItemType, types, "FriendlyName", "TypeID", True)

        'bind isHierarchy attribute to each dropdown item, so that it can be used when selected index is changed
        For Each item As RadComboBoxItem In ddlItemType.Items
            'skip the default item
            If Not String.IsNullOrEmpty(item.Value) Then
                Dim Hierarchy As Boolean = (From t In types Where t.TypeID = item.Value Select t.IsHierarchy).FirstOrDefault()
                item.Attributes.Add("IsHierarchy", Hierarchy.ToString)
            End If
        Next
    End Sub

    Private Sub BindTreeView()
        Dim listitems = ManagedTypeItemManager.GetList(ddlItemType.SelectedValue, Archived:=False)

        tvListItems.DataTextField = "ItemName"
        tvListItems.DataValueField = "ItemID"
        tvListItems.DataFieldID = "ItemID"
        tvListItems.DataFieldParentID = "ParentID"

        tvListItems.DataSource = listitems
        tvListItems.DataBind()
        tvListItems.ExpandAllNodes()
    End Sub

    Private Sub BindListView()
        cardListItems.DataSource = Nothing
        cardListItems.AutoBind = True
        cardListItems.Rebind()
    End Sub

    Private Function SaveListItem(ItemName As String, Optional ItemID As Integer = 0, Optional parentid As Integer = 0) As Integer
        Dim obj = ManagedTypeItemManager.GetByID(ItemID)
        obj.ItemName = ItemName
        obj.TypeID = ddlItemType.SelectedValue
        obj.ParentID = parentid
        Dim id As Integer = ManagedTypeItemManager.Save(obj)
        Return id

    End Function

    Private Sub startNodeInEditMode(ByVal nodeValue As String)
        'find the node by its Value and edit it when page loads
        Dim js As String = "Sys.Application.add_load(editNode); function editNode(){ "
        js += "var tree = $find(""" + tvListItems.ClientID + """);"
        js += "var node = tree.findNodeByValue('" + nodeValue + "');"
        js += "if (node) node.startEdit();"
        js += "Sys.Application.remove_load(editNode);};"

        ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "nodeEdit", js, True)
    End Sub

    Protected Sub ToggleChild(ByVal rdnode As RadTreeNode)
        ManagedTypeItemManager.ToggleArchived(rdnode.Value)
        If rdnode.Nodes.Count > 0 Then
            For Each childnode As RadTreeNode In rdnode.Nodes
                ToggleChild(childnode)
            Next
        End If
    End Sub

    Protected Sub CheckNode(ByVal rd As RadTreeNode)
        If rd.Nodes.Count > 0 Then
            rd.ContextMenuID = "Subcategory"
            For Each rdchild As RadTreeNode In rd.Nodes
                CheckNode(rdchild)
            Next
        Else
            rd.ContextMenuID = "SubcategoryEmpty"
        End If
    End Sub

#Region "Events"
    Protected Sub ddlItemType_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlItemType.SelectedIndexChanged
        If String.IsNullOrEmpty(ddlItemType.SelectedValue) Then
            pnlHierarchyType.Visible = False
            pnlNormalType.Visible = False
        ElseIf CBool(ddlItemType.SelectedItem.Attributes("IsHierarchy")) = True Then
            pnlHierarchyType.Visible = True
            pnlNormalType.Visible = False
            BindTreeView()

            rdAjaxPanel.FocusControl(txtNewItemName)
        ElseIf CBool(ddlItemType.SelectedItem.Attributes("IsHierarchy")) = False Then
            pnlHierarchyType.Visible = False
            pnlNormalType.Visible = True
            'BindGrid()
            BindListView()
        End If
    End Sub

#Region "TreeView"
    Protected Sub tvListItems_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles tvListItems.DataBound
        For Each rd As RadTreeNode In tvListItems.Nodes
            If rd.Nodes.Count > 0 Then
                rd.ContextMenuID = "MainCategory"
                For Each rdchild As RadTreeNode In rd.Nodes
                    CheckNode(rdchild)
                Next
            Else
                rd.ContextMenuID = "MainCategoryEmpty"
            End If
        Next
    End Sub

    Protected Sub tvListItems_NodeDrop(sender As Object, e As RadTreeNodeDragDropEventArgs) Handles tvListItems.NodeDrop
        If e.DestDragNode IsNot Nothing Then
            If e.SourceDragNode.ParentNode IsNot Nothing Then
                e.SourceDragNode.ParentNode.Nodes.Remove(e.SourceDragNode)
            End If

            Select Case e.DropPosition
                Case RadTreeViewDropPosition.Above
                    e.DestDragNode.InsertBefore(e.SourceDragNode)
                Case RadTreeViewDropPosition.Below
                    e.DestDragNode.InsertAfter(e.SourceDragNode)
                Case RadTreeViewDropPosition.Over
                    e.DestDragNode.Nodes.Add(e.SourceDragNode)
            End Select

            'Dim cClient As New CoreServiceClient()
            Dim c = ManagedTypeItemManager.GetByID(e.SourceDragNode.Value)

            If e.SourceDragNode.ParentNode IsNot Nothing Then
                c.ParentID = e.SourceDragNode.ParentNode.Value
            Else
                c.ParentID = 0
            End If
            ManagedTypeItemManager.Save(c)
            JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="List Item")
            BindTreeView()
            e.DestDragNode.Expanded = True
            e.DestDragNode.TreeView.UnselectAllNodes()
        End If
    End Sub

    Protected Sub tvListItems_NodeEdit(sender As Object, e As RadTreeNodeEditEventArgs) Handles tvListItems.NodeEdit
        Dim c = ManagedTypeItemManager.GetByID(e.Node.Value)
        Dim parentid As Integer = If(e.Node.ParentNode IsNot Nothing, e.Node.ParentNode.Value.ToInteger, 0)
        SaveListItem(e.Text, c.ItemID, parentid)
        e.Node.Text = e.Text
        JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="List Item")

    End Sub

    Protected Sub tvListItems_ContextMenuItemClick(sender As Object, e As RadTreeViewContextMenuEventArgs) Handles tvListItems.ContextMenuItemClick
        Dim clickedNode As RadTreeNode = e.Node
        Try
            Select Case e.MenuItem.Value
                Case "NewSubcategory"
                    Dim NewSubCategory As New RadTreeNode(String.Format("New Subcategory {0}", clickedNode.Nodes.Count + 1))
                    NewSubCategory.Selected = True
                    clickedNode.Nodes.Add(NewSubCategory)
                    clickedNode.Expanded = True
                    clickedNode.Font.Bold = True

                    Dim EditID As Integer = 0
                    EditID = SaveListItem(NewSubCategory.Text, 0, clickedNode.Value)

                    'set node's value so we can find it in startNodeInEditMode
                    NewSubCategory.Value = EditID
                    NewSubCategory.ContextMenuID = "SubcategoryEmpty"
                    startNodeInEditMode(NewSubCategory.Value)

                    Exit Select
                Case "Delete"
                    'Deactivate selected node 
                    ManagedTypeItemManager.ToggleArchived(clickedNode.Value)

                    'reassign appropriate ParentID to other categories who are child of deactivated category( If exists)
                    For Each childnode As RadTreeNode In clickedNode.Nodes
                        Dim parentid As Integer = If(e.Node.ParentNode IsNot Nothing, e.Node.ParentNode.Value.ToInteger, 0)
                        SaveListItem(childnode.Text, childnode.Value, parentid)
                    Next

                    BindTreeView()
                    Exit Select

                Case "DeleteTree"
                    ManagedTypeItemManager.ToggleArchived(clickedNode.Value)
                    If clickedNode.Nodes.Count > 0 Then
                        For Each childnode As RadTreeNode In clickedNode.Nodes
                            ToggleChild(childnode)
                        Next
                    End If
                    clickedNode.Remove()
                    Exit Select
            End Select
        Catch ex As Exception
            JGrowl.ShowMessage(JGrowlMessageType.Error, ex.Message)
        End Try
    End Sub

#End Region

    Protected Sub btnAddItem_Click(sender As Object, e As EventArgs) Handles btnAddItem.Click
        SaveListItem(txtNewItemName.Text)
        JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="List Item")
        txtNewItemName.Text = ""
        BindTreeView()
    End Sub

#Region "Grid"
    Protected Sub cardListItems_ItemCommand(sender As Object, e As RadListViewCommandEventArgs) Handles cardListItems.ItemCommand
        If e.CommandName = "PerformInsert" Then 'RadListView.PerformInsertCommandName
            If TypeOf e.ListViewItem Is RadListViewInsertItem Then
                InsertUpdate(e, 0)
            ElseIf TypeOf e.ListViewItem Is RadListViewEditableItem Then
                Dim item As RadListViewDataItem = e.ListViewItem
                Dim keyID = item.GetDataKeyValue("ItemID")
                InsertUpdate(e, keyID)
            End If
        End If
    End Sub

    Protected Sub cardListItems_ItemCreated(sender As Object, e As RadListViewItemEventArgs) Handles cardListItems.ItemCreated
        If e.Item.IsInEditMode Then
            Dim txtitemname As TextBox = e.Item.FindControl("txtItemName")
            If txtitemname IsNot Nothing Then
                txtitemname.Focus()
            End If
        End If
    End Sub

    Protected Sub cardListItems_ItemDataBound(sender As Object, e As RadListViewItemEventArgs) Handles cardListItems.ItemDataBound
        Dim item As RadListViewDataItem = e.Item
        Dim keyID = item.GetDataKeyValue("ItemID")
        Dim obj = ManagedTypeItemManager.GetByID(keyID)

        If obj.ReadOnly Then
            Dim myControl As ImageButton = e.Item.FindControl("imgbtnEdit")
            myControl.Visible = False
            Dim myControl2 As MilesToggleArchiveImageButton = e.Item.FindControl("btnDelete")
            myControl2.Visible = False
        End If
    End Sub

    Protected Sub cardListItems_NeedDataSource(sender As Object, e As RadListViewNeedDataSourceEventArgs) Handles cardListItems.NeedDataSource
        Dim iList = ManagedTypeItemManager.GetList(ddlItemType.SelectedValue, Archived:=False)
        cardListItems.DataSource = iList
    End Sub

    Protected Sub cardListItems_ToggleArchive(sender As Object, e As RadListViewCommandEventArgs) Handles cardListItems.ToggleArchive
        Dim item As RadListViewDataItem = e.ListViewItem
        Dim keyId As Integer = item.GetDataKeyValue("ItemID")
      
        ManagedTypeItemManager.ToggleArchived(keyId)
        BindListView()

    End Sub

    Protected Sub InsertUpdate(ByVal e As Telerik.Web.UI.RadListViewCommandEventArgs, ByVal KeyID As Integer)
        If TypeOf e.ListViewItem Is RadListViewEditableItem Or TypeOf e.ListViewItem Is RadListViewInsertItem Then
            'get the edit item
            Dim editedItem As Telerik.Web.UI.RadListViewEditableItem = CType(e.ListViewItem, Telerik.Web.UI.RadListViewEditableItem)
            'Get the edit fields
            Dim txtItemName As TextBox
            txtItemName = editedItem.FindControl("txtItemName")
            ''save the object
            SaveListItem(txtItemName.Text, KeyID, 0)

            JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="List Item")

            cardListItems.Rebind()
        End If
    End Sub

#End Region
#End Region

End Class
