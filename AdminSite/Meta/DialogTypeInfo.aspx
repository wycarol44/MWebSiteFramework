<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Dialog.master" AutoEventWireup="false" CodeFile="DialogTypeInfo.aspx.vb" Inherits="Meta_DialogTypeInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">
    <asp:Panel ID="pnlContent" runat="server" Style="padding-top: 10px">
        <%--dialog content here--%>
        <div class="row">
            <div class="col-xs-12">
                <asp:Label ID="Label1" runat="server" Text="Type Name" AssociatedControlID="lblTypeName" />
                <asp:Label ID="lblTypeName" runat="server" />

                <telerik:RadAjaxPanel ID="rdAjaxPanel" runat="server" RenderMode="Inline" EnableAJAX="true">
                    <asp:Panel ID="pnlNewItem" runat="server" DefaultButton="btnAddItem">
                        <asp:Label ID="lblNewCategoryName" runat="server" Text="New Item Name" AssociatedControlID="txtNewItemName" />
                        <asp:Label ID="LabelReq" runat="server" SkinID="Required"></asp:Label>
                        <div class="input-group">
                            <asp:TextBox ID="txtNewItemName" runat="server" MaxLength="500" />
                            <asp:RequiredFieldValidator id="rfvItemName" runat="server" Display="None" ControlToValidate="txtNewItemName"
                                ErrorMessage="Item Name is required"  />
                            <span class="input-group-btn">
                                <miles:MilesButton ID="btnAddItem" runat="server" Text="Add" />
                            </span>
                        </div>
                    </asp:Panel>
                    <miles:MilesGrid ID="dgSelectList" runat="server" Visible="true"
                        ShowHeader="false" AllowSorting="True" AutoBind="true">
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="True" ReorderColumnsOnClient="True" />
                        <PagerStyle Visible="false" />
                        <MasterTableView CommandItemDisplay="None" DataKeyNames="ItemID">
                            <Columns>
                                <telerik:GridTemplateColumn>
                                    <ItemTemplate>
                                        <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtItemName" runat="server" Text='<%# Eval("ItemName") %>' ValidationGroup="inline" />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn>
                                    <ItemStyle Width="150px" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEdit" runat="server" SkinID="CardListEdit" CausesValidation="false" CommandName="Edit" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <miles:MilesButton ID="lnkSave" runat="server" SkinID="CardListSave" ValidationGroup="inline" CommandName="Update" />
                                        <asp:Button ID="lnkCancel" runat="server" SkinID="CardListCancel" CommandName="Cancel" CausesValidation="false" />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="~/images/24x24/trash_bin.png"
                                    CommandName="Remove" Text="Remove" HeaderStyle-Width="16px" UniqueName="RemoveCol" />
                            </Columns>
                        </MasterTableView>
                    </miles:MilesGrid>
                </telerik:RadAjaxPanel>

            </div>
        </div>
        <%--end dialog content--%>

        <%--buttons--%>
        <div class="dialog-actions">
            <div>
                <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-default" CausesValidation="false" />
            </div>
        </div>
    </asp:Panel>

</asp:Content>

