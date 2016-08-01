<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Dialog.master" AutoEventWireup="false" CodeFile="DialogFunctionInfo.aspx.vb" Inherits="Meta_DialogFunctionInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">
    <telerik:RadAjaxPanel ID="rdAjaxPanel" runat="server" EnableAJAX="true">
        <asp:Panel ID="pnlContent" runat="server" DefaultButton="btnSaveClose" Style="padding-top: 10px">

            <%--dialog content here--%>
            <div class="row">
                <div class="col-xs-8">
                    <asp:Label ID="Label1" runat="server" Text="FunctionName" AssociatedControlID="txtFunctionName" />
                    <asp:Label ID="Label2" runat="server" SkinID="Required" />
                    <asp:TextBox ID="txtFunctionName" runat="server" MaxLength="200" />
                    <asp:RequiredFieldValidator ID="rfvtxtFunctionName" runat="server" ControlToValidate="txtFunctionName"
                        ErrorMessage="Function Name is required" Display="None" />

                    <asp:Label ID="Label3" runat="server" Text="Module" AssociatedControlID="ddlModule" />
                    <asp:Label ID="Label6" runat="server" SkinID="Required" />
                    <miles:MilesDropDownTree ID="ddlModule" runat="server" CheckBoxes="None" AutoPostBack="false" AutoCollapseDropDown="true" />
                    <asp:RequiredFieldValidator ID="rfvddlModule" runat="server" InitialValue="" ControlToValidate="ddlModule"
                        ErrorMessage="Module is required" Display="None" />

                    <asp:CheckBox ID="chkViewOnly" runat="server" Text="View Only Function" Width="100%" />

                    <asp:Label ID="Label4" runat="server" Text="Related Forms" AssociatedControlID="ddlRelatedForms" />
                    <telerik:RadComboBox ID="ddlRelatedForms" runat="server" AutoPostBack="true" CausesValidation="false" />


                    <asp:Label ID="Label5" runat="server" Text="Assigned Forms" AssociatedControlID="dgSelectList" />
                    <miles:MilesGrid ID="dgSelectList" runat="server" Visible="true"
                        ShowHeader="false" CssClass="divborderthin" Skin="Silk" AllowSorting="True" AutoBind="true" AutoGenerateColumns="False" CellSpacing="0" DataArchivedCssClass="rgDataArchived" DataArchivedCssClassAlternating="rgDataArchivedAlt" DataArchivedField="Archived" GridLines="None" GroupingEnabled="False">
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="True" ReorderColumnsOnClient="True">
                        </ClientSettings>
                        <PagerStyle Visible="false" />
                        <MasterTableView CommandItemDisplay="None" DataKeyNames="FormID">
                            <Columns>
                                <telerik:GridBoundColumn DataField="FormName"></telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn UniqueName="RemoveCol">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnRemove" runat="server" CausesValidation="false" CommandName="Remove"
                                            ImageUrl="~/images/24x24/trash_bin.png" ToolTip="Remove" />
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                        <ExportSettings IgnorePaging="false">
                        </ExportSettings>
                    </miles:MilesGrid>
                </div>
            </div>
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

