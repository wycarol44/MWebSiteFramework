
Partial Class Admin_CategoryRadTree
    Inherits BasePage

    Protected Sub Admin_ManageMenu_Load(sender As Object, e As EventArgs) Handles Me.Load
        'btnAddNewItem.Attributes.Add("onclick", "return openEditDialog(0,0);")
        btnAddNewCategory.Attributes.Add("onclick", "return openEditDialog(0,0);")
        If Not IsPostBack Then
            BindMenuTree()
        End If
    End Sub

    '#Region "Methods"

    Private Sub DeleteItem(ItemID As Integer, Optional IsParent As Boolean = False)
        If IsParent Then
            CategoryMenuManager.DeleteTree(ItemID)
        Else
            CategoryMenuManager.Delete(ItemID)
        End If

        'clear cache
        'MetaMenuManager.ClearMenuCache()
        BindMenuTree()
        'show message
        JGrowl.ShowMessage(JGrowlMessageType.Notification, "Menu Item was archived successfully")
    End Sub

    Private Sub BindMenuTree()

        Dim menuList = CategoryMenuManager.GetList(Archived:=False)

        rdMenuTree.DataSource = menuList
        rdMenuTree.DataTextField = "CategoryName"
        rdMenuTree.DataFieldID = "CategoryID"
        rdMenuTree.DataValueField = "CategoryID"
        rdMenuTree.DataFieldParentID = "ParentID2"

        rdMenuTree.DataBind()
    End Sub

    Private Sub ExpandCurrentNodeParents(cId As Integer)
        'expand the parent node
        Dim node = rdMenuTree.FindNodeByValue(cId.ToString())
        If node IsNot Nothing Then
            node.ExpandParentNodes()
        End If
    End Sub

    Private Sub SelectCurrentNode(cId As Integer)
        'expand the parent node
        Dim node = rdMenuTree.FindNodeByValue(cId.ToString())
        If node IsNot Nothing Then
            node.Selected = True
        End If
    End Sub

    '#Region "Events"
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

    'What is this for????? Do not change the parentID to null!!!!
    '不能把cate变成别人的subcate
    'subcate变到cate里要改product
    Protected Sub rdMenuTree_NodeDrop(sender As Object, e As RadTreeNodeDragDropEventArgs) Handles rdMenuTree.NodeDrop
        If e.DestDragNode Is Nothing Then Return

        'Dim dragnodePid As Integer = ToInteger(e.DraggedNodes.Item(0).ParentNode.Value)
        'move the item to the dest node's parent
        Dim parentId As Integer = Nothing
        'level=0 is root
        If e.DraggedNodes.Item(0).Level = 0 Then

        Else
            If e.DropPosition = RadTreeViewDropPosition.Over Then

                '找自己本身的parentID, and ID
                Dim dragnodePid As Integer = ToInteger(e.DraggedNodes.Item(0).ParentNode.Value)
                Dim dragnodeid As Integer = ToInteger(e.DraggedNodes.Item(0).Value)

                'find the destdragnode de ID
                Dim destdragnodeid = ToInteger(e.DestDragNode.Value)

                'if self parent id != dest id
                If dragnodePid <> destdragnodeid Then
                    'find product whose subcateID = dragnodeid
                    'Change his cateID to destdragnodeid

                    'Change category table
                    parentId = ToInteger(e.DestDragNode.Value)

                    'Change product table
                    Using ctx As New DataLibrary.ModelEntities
                        Dim pros = (From p In ctx.Products
                                    Where p.SubCategoryID = dragnodeid
                                    Select p)
                        For Each pro In pros
                            pro.CategoryID = destdragnodeid
                        Next
                        ctx.SaveChanges()
                    End Using
                Else
                    'if self parent id == dest id
                    'Everything is good
                End If


            ElseIf e.DestDragNode.ParentNode IsNot Nothing Then
                parentId = ToInteger(e.DestDragNode.ParentNode.Value)
            End If
        End If

        'get the source node id
        Dim srcNodeId = ToInteger(e.SourceDragNode.Value)

        Dim m = CategoryMenuManager.GetById(srcNodeId)
        m.ParentID = parentId
        CategoryMenuManager.Save(m)


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
            Dim n = CategoryMenuManager.GetById(nid)

            n.SortOrder = nodes.IndexOf(node) + 1
            CategoryMenuManager.Save(n)
        Next

        BindMenuTree()

        'expand the parent node
        ExpandCurrentNodeParents(m.CategoryID)

        'clear cache
        'CategoryMenuManager.ClearMenuCache()

        JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="Menu Item", message:="Success")
    End Sub
    '#End Region

    Protected Sub rdAjaxPanel_AjaxRequest(sender As Object, e As AjaxRequestEventArgs) Handles rdAjaxPanel.AjaxRequest
        BindMenuTree()
        Dim cId As Integer = ToInteger(e.Argument)
        ExpandCurrentNodeParents(cId)
        SelectCurrentNode(cId)
    End Sub
End Class
