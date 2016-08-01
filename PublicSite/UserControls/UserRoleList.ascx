<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UserRoleList.ascx.vb" Inherits="UserControls_UserRoleList" %>


<telerik:RadScriptBlock ID="rdScriptBlock" runat="server">
    <script type="text/javascript">
        function dglist_validate(sender, args) {
            var grid = $find("<%=dgSelectList.ClientID%>");
            var MasterTable = grid.get_masterTableView();
            var Rows = MasterTable.get_dataItems();
            if (Rows.length > 0) {
                args.IsValid = true;
            }
            else {
                args.IsValid = false;
            }
        }
    </script>
</telerik:RadScriptBlock>



    <asp:Label ID="label" Text="User Role"  AssociatedControlID="ddlList" runat="server" />
    <asp:Label ID="requiredLabel" runat="server" SkinID="Required" />
    <miles:MilesToolTipDisplay ID="ucToolTip" runat="server" />

<telerik:RadComboBox ID="ddlList" runat="server" AutoPostBack="true" CausesValidation="false" />

<miles:MilesGrid ID="dgSelectList" runat="server" Visible="true"
    ShowHeader="false" AllowSorting="True" AutoBind="False">
    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="True" ReorderColumnsOnClient="True" />
    <PagerStyle Visible="false" />
    <MasterTableView CommandItemDisplay="None" DataKeyNames="UserRoleID">
        <Columns>
            <telerik:GridBoundColumn DataField="UserRoleName" />

            <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="~/images/24x24/trash_bin.png"
                CommandName="Remove" Text="Remove" HeaderStyle-Width="16px" UniqueName="RemoveCol" />

        </Columns>
    </MasterTableView>

</miles:MilesGrid>

<asp:CustomValidator ID="cvUserRole" runat="server" Display="None" ErrorMessage="User Role is required" ControlToValidate="ddlList"
    ClientValidationFunction="dglist_validate" Enabled="false" />
