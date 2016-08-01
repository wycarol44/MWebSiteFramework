<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="ManageMenu.aspx.vb" Inherits="Admin_ManageMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">

        function openEditDialog(mid, pid) {
            openWindow('/Meta/DialogMenuInfo.aspx?MenuID=' + mid + '&ParentID=' + pid, 'Menu Info', WINDOW_MEDIUM, dialogClosed);
        }

        function dialogClosed(sender, args) {
            var arg = args.get_argument();

            if (arg != null) {
                $find('<%= rdAjaxPanel.ClientID%>').ajaxRequest(arg);
                }
            }
        //<!--
        function onClientContextMenuShowing(sender, args) {
            var treeNode = args.get_node();
            treeNode.set_selected(true);
            //enable/disable menu items
            setMenuItemsState(args.get_menu().get_items(), treeNode);
        
        }

        //this method disables the appropriate context menu items
        function setMenuItemsState(menuItems, treeNode) {
            for (var i = 0; i < menuItems.get_count() ; i++) {
                var menuItem = menuItems.getItem(i);
                switch (menuItem.get_value()) {
                    case "Delete":
                        var childnodecount = treeNode.get_nodes().get_count();
                        if (childnodecount > 0)
                        {
                            formatMenuItem(menuItem, treeNode, 'Delete <strong>\'{0}\'</strong> and its SubItem(s)');
                        }
                        else
                        {
                            formatMenuItem(menuItem, treeNode, 'Delete <strong>\'{0}\'</strong>');
                        }
                        break;
                    case "Edit":
                        formatMenuItem(menuItem, treeNode, 'Edit <strong>\'{0}\'</strong>');
                        break;
                }
            }
        }

        //formats the Text of the menu item
        function formatMenuItem(menuItem, treeNode, formatString) {
            var nodeValue = treeNode.get_value();
            if (nodeValue && nodeValue.indexOf("_Private_") == 0) {
                menuItem.set_enabled(false);
            }
            else {
                menuItem.set_enabled(true);
            }
            var newText = String.format(formatString, treeNode.get_text());
            menuItem.set_text(newText);
        }


        function onClientContextMenuItemClicking(sender, args) {
            var menuItem = args.get_menuItem();
            var treeNode = args.get_node();
            menuItem.get_menu().hide();
            
            switch (menuItem.get_value()) {
                case "Delete":
                    var childnodecount = treeNode.get_nodes().get_count();
                    if (childnodecount > 0) {
                        var result = confirm("Are you sure you want to delete '" + treeNode.get_text() + "'\n and all the items underneath?");
                    }
                    else {
                        var result = confirm("Are you sure you want to delete the item: " + treeNode.get_text() + "?");
                    }

                    args.set_cancel(!result);
                    break;
                case "Edit":
                    openEditDialog(treeNode.get_value());
                    break;
                case "New":
                    openEditDialog(0, treeNode.get_value());
                    break;
            }
        }

    </script>
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">
        <telerik:RadAjaxLoadingPanel ID="rdLoadingPanel" runat="server" />
        <telerik:RadAjaxPanel ID="rdAjaxPanel" runat="server" LoadingPanelID="rdLoadingPanel">
            <div class="row">
                <div class="col-sm-5 menu-tree-wrapper">
                    <div class="menu-tree">
                        <telerik:RadTreeView ID="rdMenuTree" runat="server" EnableDragAndDrop="true"
                            EnableDragAndDropBetweenNodes="true" OnClientContextMenuShowing="onClientContextMenuShowing" OnClientContextMenuItemClicking="onClientContextMenuItemClicking" >
                            <ContextMenus>
                                <telerik:RadTreeViewContextMenu ID="ctxmenu" runat="server" CausesValidation="false">
                                    <Items>
                                        <telerik:RadMenuItem Text="Edit Item" Value="Edit"></telerik:RadMenuItem>
                                        <telerik:RadMenuItem Text="Add New Item" Value="New"></telerik:RadMenuItem>
                                        <telerik:RadMenuItem Text="Delete Item"  Value="Delete"></telerik:RadMenuItem>
                                    </Items>
                                </telerik:RadTreeViewContextMenu>
                            </ContextMenus>

                        </telerik:RadTreeView>
                        <br />
                        <br />
                        <div class="menu-tree-footer">
                            <div>
                                <asp:Button ID="btnAddNewItem" runat="server" Text="Add New Item" CssClass="btn btn-secondary" />
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </telerik:RadAjaxPanel>
</asp:Content>

