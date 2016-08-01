<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CMSMergeFieldsList.ascx.vb" Inherits="UserControls_CMSMergeFieldsList" %>

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

        function OpenAddMergeFieldWindow(id) {
            openWindow('/Meta/DialogCMSMergeFieldInfo.aspx?MergeFieldID=' + id, 'Add CMS Merge Field', WINDOW_SMALL, dialogClosed);
        }

        function dialogClosed(sender, args) {
            var arg = args.get_argument();

            if (arg != null) {
                $find('<%= rdAjaxPanel.ClientID%>').ajaxRequest();
            }
        }

    </script>
</telerik:RadScriptBlock>

<telerik:RadAjaxLoadingPanel ID="rdLoadingPanel" runat="server" />

<telerik:RadAjaxPanel ID="rdAjaxPanel" runat="server" RenderMode="Inline" LoadingPanelID="rdLoadingPanel">
    
    <asp:Label ID="lblMergeField" Text="Merge Fields" AssociatedControlID="ddlList" runat="server" />
    <asp:Label ID="requiredLabel" runat="server" SkinID="Required" />
    <asp:LinkButton ID="hypAddNewMergeField" runat="server" Text="Add New" OnClientClick="OpenAddMergeFieldWindow(0);" CssClass="pull-right"></asp:LinkButton>
    <telerik:RadComboBox ID="ddlList" runat="server" AutoPostBack="true" CausesValidation="false" />

    <miles:MilesGrid ID="dgSelectList" runat="server" Visible="true"
        ShowHeader="false" AllowSorting="True" AutoBind="False">
        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="True" ReorderColumnsOnClient="True" />
        <PagerStyle Visible="false" />
        <MasterTableView CommandItemDisplay="None" DataKeyNames="MergeFieldID">
            <Columns>
                <telerik:GridBoundColumn DataField="MergeField" />

                <telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="~/images/24x24/trash_bin.png"
                    CommandName="Remove" Text="Remove" HeaderStyle-Width="16px" UniqueName="RemoveCol" />

            </Columns>
        </MasterTableView>

    </miles:MilesGrid>

    <asp:CustomValidator ID="cvMergeField" runat="server" Display="None" ErrorMessage="Merge Field is required" ControlToValidate="ddlList"
        ClientValidationFunction="dglist_validate" Enabled="false" />
</telerik:RadAjaxPanel>
