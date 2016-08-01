
Partial Class Admin_ManageMenu
    Inherits BasePage

    Protected Sub Admin_ManageMenu_Load(sender As Object, e As EventArgs) Handles Me.Load
        btnAddNewItem.Attributes.Add("onclick", "return openEditDialog(0,0);")
        If Not IsPostBack Then
            BindMenuTree()
        End If
    End Sub

#Region "Methods"

    Private Sub DeleteItem(ItemID As Integer, Optional IsParent As Boolean = False)
        If IsParent Then
            MetaMenuManager.DeleteTree(ItemID)
        Else
            MetaMenuManager.Delete(ItemID)
        End If

        'clear cache
        MetaMenuManager.ClearMenuCache()

        BindMenuTree()

        'show message
        JGrowl.ShowMessage(JGrowlMessageType.Notification, "Menu Item was deleted successfully")
    End Sub

    Private Sub BindMenuTree()

        Dim menuList = MetaMenuManager.GetList()

        rdMenuTree.DataSource = menuList


        rdMenuTree.DataTextField = "Title"
        rdMenuTree.DataFieldID = "MenuID"
        rdMenuTree.DataValueField = "MenuID"
        rdMenuTree.DataFieldParentID = "ParentID"

        rdMenuTree.DataBind()


    End Sub

    Private Sub ExpandCurrentNodeParents(mId As Integer)
        'expand the parent node
        Dim node = rdMenuTree.FindNodeByValue(mId.ToString())
        If node IsNot Nothing Then
            node.ExpandParentNodes()
        End If
    End Sub

    Private Sub SelectCurrentNode(mId As Integer)
        'expand the parent node
        Dim node = rdMenuTree.FindNodeByValue(mId.ToString())
        If node IsNot Nothing Then
            node.Selected = True
        End If
    End Sub
#End Region

#Region "Events"
    Protected Sub rdMenuTree_ContextMenuItemClick(sender As Object, e As RadTreeViewContextMenuEventArgs) Handles rdMenuTree.ContextMenuItemClick
        Dim clickedNode As RadTreeNode = e.Node
        Try
            Select Case e.MenuItem.Value
                Case "Delete"
                    'check if it has child nodes
                    If clickedNode.Nodes.Count > 0 Then
                        DeleteItem(clickedNode.Value, True)
                    Else
                        DeleteItem(clickedNode.Value)
                    End If
            End Select
        Catch ex As Exception
            JGrowl.ShowMessage(JGrowlMessageType.Error, ex.Message)
        End Try
    End Sub

    Protected Sub rdMenuTree_NodeDrop(sender As Object, e As RadTreeNodeDragDropEventArgs) Handles rdMenuTree.NodeDrop
        If e.DestDragNode Is Nothing Then Return

        'move the item to the dest node's parent
        Dim parentId As Integer? = Nothing

        If e.DropPosition = RadTreeViewDropPosition.Over Then
            'dropped on top of another element
            parentId = ToInteger(e.DestDragNode.Value)
        ElseIf e.DestDragNode.ParentNode IsNot Nothing Then
            'item was dropped under a node, on the same level as a sibling node
            parentId = ToInteger(e.DestDragNode.ParentNode.Value)

        End If


        'get the source node id
        Dim srcNodeId = ToInteger(e.SourceDragNode.Value)

        Dim m = MetaMenuManager.GetByID(srcNodeId)
        m.ParentID = parentId
        MetaMenuManager.Save(m)


        'Reorder the items

        'get all nodes that are under the same parent as our destination
        Dim nodes = (From n In rdMenuTree.GetAllNodes()
                     Where n.ParentNode Is e.DestDragNode.ParentNode
                     Select n).ToList()

        'remove the source node
        nodes.Remove(e.SourceDragNode)

        'get the index of the destination node
        Dim index = nodes.IndexOf(e.DestDragNode)

        'determine where to put the node
        If e.DropPosition = RadTreeViewDropPosition.Above Then
            nodes.Insert(index, e.SourceDragNode)
        ElseIf e.DropPosition = RadTreeViewDropPosition.Below Then
            nodes.Insert(index + 1, e.SourceDragNode)
        End If

        'save the new order
        For Each node In nodes
            Dim nid = Val(node.Value)
            Dim n = MetaMenuManager.GetByID(nid)

            n.SortOrder = nodes.IndexOf(node) + 1
            MetaMenuManager.Save(n)
        Next

        BindMenuTree()

        'expand the parent node
        ExpandCurrentNodeParents(m.MenuID)

        'clear cache
        MetaMenuManager.ClearMenuCache()

        JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="Menu Item")
    End Sub
#End Region

    Protected Sub rdAjaxPanel_AjaxRequest(sender As Object, e As AjaxRequestEventArgs) Handles rdAjaxPanel.AjaxRequest
        BindMenuTree()
        Dim mid As Integer = ToInteger(e.Argument)
        ExpandCurrentNodeParents(mid)
        SelectCurrentNode(mid)
    End Sub
End Class
