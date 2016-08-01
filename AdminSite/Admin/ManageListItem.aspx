<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="ManageListItem.aspx.vb" Inherits="Admin_ManageListItem" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">
    <script type="text/javascript">
        //<!--
        function onClientContextMenuShowing(sender, args) {
            var treeNode = args.get_node();
            treeNode.set_selected(true);
            //enable/disable menu items
            setMenuItemsState(args.get_menu().get_items(), treeNode);
        }

        function onClientContextMenuItemClicking(sender, args) {
            var menuItem = args.get_menuItem();
            var treeNode = args.get_node();
            menuItem.get_menu().hide();

            switch (menuItem.get_value()) {
                case "Rename":
                    treeNode.startEdit();
                    break;
                case "NewSubcategory":
                    break;
                case "Delete":
                    var result = confirm("Are you sure you want to delete the item: " + treeNode.get_text() + "?");
                    args.set_cancel(!result);
                    break;
                case "DeleteTree":
                    var result = confirm("Are you sure you want to delete '" + treeNode.get_text() + "'\n and all the items underneath?");
                    args.set_cancel(!result);
                    break;
            }
        }

        //this method disables the appropriate context menu items
        function setMenuItemsState(menuItems, treeNode) {
            for (var i = 0; i < menuItems.get_count() ; i++) {
                var menuItem = menuItems.getItem(i);
                switch (menuItem.get_value()) {
                    case "Rename":
                        formatMenuItem(menuItem, treeNode, 'Rename <strong>\'{0}\'</strong>');
                        break;
                    case "Delete":
                        formatMenuItem(menuItem, treeNode, 'Delete <strong>\'{0}\'</strong>');
                        break;
                    case "Edit":
                        formatMenuItem(menuItem, treeNode, 'Edit <strong>\'{0}\'</strong>');
                        break;
                    case "NewSubcategory":
                        break;
                    case "DeleteTree":
                        formatMenuItem(menuItem, treeNode, 'Delete <strong>\'{0}\'</strong> and its Subcategories');
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

        //-->

    </script>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel" runat="server"></telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxPanel ID="rdAjaxPanel" runat="server" LoadingPanelID="RadAjaxLoadingPanel">
        <div class="row">
            <div class="col-sm-3">
                <asp:Label ID="Label1" runat="server" Text="Select Item Type" AssociatedControlID="ddlItemType" />
                <telerik:RadComboBox ID="ddlItemType" runat="server" AutoPostBack="true" CausesValidation="false" />
            </div>
        </div>
        <hr />
        <div class="row">
            <div class="col-sm-12">
                <asp:Panel ID="pnlHierarchyType" runat="server" Visible="false">
                    <div class="row">
                        <asp:Panel ID="pnlNewItemName" runat="server" DefaultButton="btnAddItem" CssClass="col-md-3">

                            <asp:Label ID="lblNewCategoryName" runat="server" Text="New Item Name" AssociatedControlID="txtNewItemName" />
                            <asp:Label ID="Label2" runat="server" SkinID="Required" />

                            <div class="input-group">

                                <asp:TextBox ID="txtNewItemName" runat="server" MaxLength="500" />
                                <asp:RequiredFieldValidator ID="rfvtxtNewItemName" runat="server" Display="None"
                                    ControlToValidate="txtNewItemName" ErrorMessage="Item Name is required" />
                                <span class="input-group-btn">
                                    <miles:MilesButton ID="btnAddItem" runat="server" Text="Add" />
                                </span>
                            </div>

                        </asp:Panel>
                    </div>
                    <div class="">
                        <br />
                        <telerik:RadTreeView ID="tvListItems" runat="server" EnableDragAndDrop="true"
                            EnableDragAndDropBetweenNodes="true" AllowNodeEditing="true" OnClientContextMenuItemClicking="onClientContextMenuItemClicking"
                            OnClientContextMenuShowing="onClientContextMenuShowing" CausesValidation="false">
                            <ContextMenus>
                                <telerik:RadTreeViewContextMenu ID="MainCategory" runat="server" CausesValidation="false">
                                    <Items>
                                        <telerik:RadMenuItem Value="Rename" Text="Rename" PostBack="false">
                                        </telerik:RadMenuItem>
                                        <telerik:RadMenuItem Value="NewSubcategory" Text="Add New Subcategory">
                                        </telerik:RadMenuItem>
                                        <telerik:RadMenuItem Value="Delete" Text="Delete Category">
                                        </telerik:RadMenuItem>
                                        <telerik:RadMenuItem Value="DeleteTree" Text="Delete Category and Subcategories">
                                        </telerik:RadMenuItem>
                                    </Items>
                                </telerik:RadTreeViewContextMenu>
                                <telerik:RadTreeViewContextMenu ID="MainCategoryEmpty" runat="server" CausesValidation="false">
                                    <Items>
                                        <telerik:RadMenuItem Value="Rename" Text="Rename " PostBack="false">
                                        </telerik:RadMenuItem>
                                        <telerik:RadMenuItem Value="NewSubcategory" Text="Add New Subcategory">
                                        </telerik:RadMenuItem>
                                        <telerik:RadMenuItem Value="Delete" Text="Delete Category ">
                                        </telerik:RadMenuItem>
                                    </Items>
                                </telerik:RadTreeViewContextMenu>
                                <telerik:RadTreeViewContextMenu ID="Subcategory" runat="server" CausesValidation="false">
                                    <Items>
                                        <telerik:RadMenuItem Value="Rename" Text="Rename " Enabled="false" PostBack="false">
                                        </telerik:RadMenuItem>
                                        <telerik:RadMenuItem Value="NewSubcategory" Text="Add New Subcategory">
                                        </telerik:RadMenuItem>
                                        <telerik:RadMenuItem Value="Delete" Text="Delete Subcategory">
                                        </telerik:RadMenuItem>
                                        <telerik:RadMenuItem Value="DeleteTree" Text="Delete Subcategory and Subcategories Inside">
                                        </telerik:RadMenuItem>
                                    </Items>
                                </telerik:RadTreeViewContextMenu>
                                <telerik:RadTreeViewContextMenu ID="SubcategoryEmpty" runat="server" CausesValidation="false">
                                    <Items>
                                        <telerik:RadMenuItem Value="Rename" Text="Rename " PostBack="false">
                                        </telerik:RadMenuItem>
                                        <telerik:RadMenuItem Value="NewSubcategory" Text="Add New Subcategory">
                                        </telerik:RadMenuItem>
                                        <telerik:RadMenuItem Value="Delete" Text="Delete Subcategory ">
                                        </telerik:RadMenuItem>
                                    </Items>
                                </telerik:RadTreeViewContextMenu>
                            </ContextMenus>
                        </telerik:RadTreeView>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlNormalType" runat="server" Visible="false">
                    <miles:MilesCardList ID="cardListItems" runat="server" CardMinHeight="100px" AllowPaging="true" DataKeyNames="ItemID" NumberOfColumns="Three"
                        AddNewRecordText="Add New Item">
                        <CardTemplate ActionPosition="Right">
                            <CardItemTemplate>
                                <%# Eval("ItemName")%>
                            </CardItemTemplate>

                            <CardActionTemplate>
                                <asp:ImageButton ID="imgbtnEdit" runat="server" SkinID="CardListEdit" CommandName="Edit" />
                                <miles:MilesToggleArchiveImageButton ID="btnDelete" runat="server" Archived='<%# CBool(Eval("Archived")) %>' />
                            </CardActionTemplate>
                        </CardTemplate>

                        <CardEditTemplate ActionPosition="Bottom" DefaultButton="lnkSave">
                            <CardItemTemplate>

                                <asp:Label ID="lblItemName" Text="Item Name" runat="server" AssociatedControlID="txtItemName" />
                                <asp:TextBox ID="txtItemName" Text='<%# Eval("ItemName")%>' runat="server" MaxLength="500"/>
                                <asp:RequiredFieldValidator runat="server" ID="rfvListItemName" ControlToValidate="txtItemName"
                                    ErrorMessage="Item Name is required" />

                            </CardItemTemplate>
                            <CardActionTemplate>
                                <asp:LinkButton ID="lnkSave" runat="server" SkinID="CardListSave" CommandName="PerformInsert" />
                                <asp:LinkButton ID="lnkCancel" runat="server" SkinID="CardListCancel" CommandName="Cancel" CausesValidation="false" />
                            </CardActionTemplate>
                        </CardEditTemplate>
                    </miles:MilesCardList>
                </asp:Panel>
            </div>
        </div>
    </telerik:RadAjaxPanel>
</asp:Content>

