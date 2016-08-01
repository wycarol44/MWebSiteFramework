<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Dialog.master" AutoEventWireup="false" CodeFile="DialogManageFavorites.aspx.vb" Inherits="Admin_DialogManageFavorites" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">
    <telerik:RadCodeBlock ID="rdCodeBlock" runat="server">
        <script>


            function ValidateColorPicker(sender, args) {
                var colorPicker = $find('<%= rdIconColorPicker.ClientID%>');
                var selectedColor = colorPicker.get_selectedColor();
                if (null != selectedColor) {
                    args.IsValid = true;
                }
                else {
                    args.IsValid = false;
                }
            }

            function isChildOf(parentId, element) {
                while (element) {
                    if (element.id && element.id.indexOf(parentId) > -1) {
                        return true;
                    }
                    element = element.parentNode;
                }
                return false;
            }

        </script>
    </telerik:RadCodeBlock>

    <telerik:RadAjaxPanel ID="rdAjaxPanel" runat="server" EnableAJAX="true">
        <asp:Panel ID="pnlContent" runat="server" DefaultButton="btnSaveClose" Style="padding-top: 10px">

            <%--dialog content here--%>
            <asp:Panel ID="pnlManageFavorites" runat="server">

                <div class="col-md-12">
                    <div class="row">
                        <miles:MilesCardList ID="clFavorites" runat="server" AllowPaging="false" NumberOfColumns="One" AllowItemsDragDrop="true"
                            CardMinHeight="50px" ShowCommandRow="false" DataKeyNames="FavoriteID" AutoBind="true" >
                            <CardTemplate>
                                <CardItemTemplate>
                                    <h4>
                                        <div class="rlvI">
                                            <telerik:RadListViewItemDragHandle ToolTip="Drag to reorder" ID="draghandle" runat="server" />
                                            <asp:LinkButton ID="btnDisplayName" runat="server" Text='<%# Eval("DisplayName")%>' CommandName="ViewFavorite"></asp:LinkButton>
                                        </div>
                                    </h4>
                                    <asp:Label ID="Label4" Text="Landing Page" Visible='<%# IIf(Eval("IsLandingPage"), True, False)%>' runat="server" CssClass="label label-warning"></asp:Label>
                                    <asp:Label ID="lblVisibility" runat="server" CssClass="label label-info"></asp:Label>
                                </CardItemTemplate>
                                <CardActionTemplate>
                                    <asp:ImageButton ID="btnRemove" CommandName="Remove" runat="server" OnClientClick="if(!confirm('Are you sure you want to remove the favorite?')){return false;}" 
                                        ToolTip="Delete" ImageUrl="/images/24x24/trash_bin.png" />
                                </CardActionTemplate>
                            </CardTemplate>
                        </miles:MilesCardList>
                    </div>
                </div>
            </asp:Panel>


            <asp:Panel ID="pnlFavoriteInfo" runat="server" Visible="true">
                <div class="col-md-12">

                    <div class="row">
                        <asp:Label ID="Label4" runat="server" Text="Favorite Name" AssociatedControlID="txtFavorite" />
                        <asp:Label ID="Label3" runat="server" SkinID="Required" />
                        <asp:TextBox ID="txtFavorite" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvtxtFavorite" runat="server" ControlToValidate="txtFavorite" Display="None"
                            ErrorMessage="Favorite Name is required" />
                    </div>

                    <div class="row">
                        <asp:Label ID="Label1" runat="server" Text="Display Setting" AssociatedControlID="chkDisplaySettings"></asp:Label>
                        <asp:Label ID="Label2" runat="server" SkinID="Required"></asp:Label>
                        <br />
                        <asp:CheckBoxList ID="chkDisplaySettings" AutoPostBack="true" runat="server" RepeatColumns="2"
                            RepeatDirection="Horizontal">
                            <asp:ListItem Text="Show in Icon Bar" Value="ShowInIconBar" />
                            <asp:ListItem Text="Show in Favorites Menu" Value="ShowInFavoriteMenu" style="padding-left: 10px" />
                        </asp:CheckBoxList>
                        <miles:CheckBoxListValidator ID="clvchkDisplaySettings" runat="server" ControlToValidate="chkDisplaySettings"
                            Display="None" ErrorMessage="Display Setting is required" />

                    </div>

                    <div class="row">
                        <asp:Panel ID="pnlIconColor" Visible="false" runat="server">
                            <asp:Label ID="lblPickColor" Text="Icon Color" runat="server" AssociatedControlID="rdIconColorPicker"></asp:Label>
                            <asp:Label ID="lblColorPickerRequired" runat="server" SkinID="Required" />
                            <telerik:RadColorPicker runat="server" ID="rdIconColorPicker" ShowRecentColors="true"
                                KeepInScreenBounds="true" PaletteModes="WebPalette" ShowIcon="true">
                            </telerik:RadColorPicker>
                            <asp:CustomValidator ID="cvColorPicker" ClientValidationFunction="ValidateColorPicker"
                                Display="None" runat="server" ErrorMessage="Icon Color is required"></asp:CustomValidator>
                        </asp:Panel>
                    </div>

                    <div class="row">
                        <br />
                        <asp:CheckBox ID="chkLandingPage" runat="server" Text="Is Landing Page" />
                    </div>
                </div>
            </asp:Panel>

            <%--end dialog content--%>

            <%--buttons--%>
            <div class="dialog-actions">
                <div>
                    <miles:MilesButton ID="btnSaveClose" runat="server" Text="Save and Close" ActionType="Primary" />
                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-default" CausesValidation="false" />
                </div>
            </div>

        </asp:Panel>

    </telerik:RadAjaxPanel>
</asp:Content>

