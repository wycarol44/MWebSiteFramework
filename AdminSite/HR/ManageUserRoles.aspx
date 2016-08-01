<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="ManageUserRoles.aspx.vb" Inherits="HR_ManageUserRoles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">
    <telerik:RadCodeBlock ID="rdCodeBlock" runat="server">
        <script type="text/javascript">
            function openAccessDialog(id) {
                openWindow('DialogUserRoleFunctionAccess.aspx?UserRoleID=' + id, 'Manage Function Access', WINDOW_MEDIUM);
            }
        </script>
    </telerik:RadCodeBlock>
    <miles:MilesSearchPanel ID="pnlSearch" runat="server" SearchOnLoadSavedState="true">
        <ContentPanel>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label ID="Label1" runat="server" Text="User Role" AssociatedControlID="txtUserRoleName" />
                    <br />
                    <asp:TextBox ID="txtUserRoleName" runat="server" />
                </div>
                <div class="col-md-3">
                    <br />
                    <asp:CheckBox ID="chkArchived" runat="server" Text="Include archived" CssClass="chk-inline" />
                </div>
            </div>
        </ContentPanel>
    </miles:MilesSearchPanel>
    <hr />
    <telerik:RadAjaxPanel ID="rdAjaxPanel" runat="server">
        <miles:MilesCardList ID="clUserRole" runat="server" DataKeyNames="UserRoleID"
            AddNewRecordText="Add New User Role" >
            <SortExpressionList>
                <miles:MilesCardListSortExpression DisplayName="User Role" FieldName="UserRoleName" />
            </SortExpressionList>

            <CardTemplate ActionPosition="Right">
                <CardHeaderTemplate>
                    <h4>
                        <%# Eval("UserRoleName")%>
                    </h4>
                </CardHeaderTemplate>

                <CardItemTemplate>
                    
                    <miles:CardListDataBoundProperty runat="server" DataField="UserRoleDesc" ShowLabel="false" />
                </CardItemTemplate>

                <CardActionTemplate>
                    <asp:ImageButton ID="btnEdit" runat="server" SkinID="CardListEdit" CommandName="Edit" />
                    <asp:ImageButton ID="btnManageAccess" ToolTip="Manage Access"
                        CommandName="ManageAccess" runat="server" ImageUrl="/Images/24x24/security_lock-settings.png" />
                    <miles:MilesToggleArchiveImageButton ID="btnDelete" runat="server" Archived='<%# CBool(Eval("Archived")) %>' />
                </CardActionTemplate>
            </CardTemplate>

            <CardEditTemplate ActionPosition="Bottom" DefaultButton="lnkSave">
                <CardItemTemplate>

                    <asp:Label ID="lblUserRole" Text="User Role" runat="server" AssociatedControlID="txtUserRole" />
                    <asp:Label ID="Label2" runat="server" SkinID="Required" />
                    <asp:TextBox ID="txtUserRole" Text='<%# Eval("UserRoleName")%>' runat="server" MaxLength="100" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvUserRole" ControlToValidate="txtUserRole"
                        ErrorMessage="User Role is required" Display="None"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblDesc" Text="Description" runat="server" AssociatedControlID="txtUserRoleDesc" />
                    <asp:TextBox ID="txtUserRoleDesc" TextMode="MultiLine" Rows="4" Text='<%# Eval("UserRoleDesc")%>' runat="server" />

                </CardItemTemplate>

                <CardActionTemplate>
                    <asp:LinkButton ID="lnkSave" runat="server" SkinID="CardListSave" CommandName="PerformInsert" />
                    <asp:LinkButton ID="lnkCancel" runat="server" SkinID="CardListCancel" CausesValidation="false" CommandName="Cancel" />
                </CardActionTemplate>
            </CardEditTemplate>

        </miles:MilesCardList>
    </telerik:RadAjaxPanel>
</asp:Content>

