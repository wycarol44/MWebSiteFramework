<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="ManageAccess.aspx.vb" Inherits="HR_ManageAccess" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">
    <script type="text/javascript">
            function openAccessDialog(id, name) {
                openWindow('DialogFunctionUserRoles.aspx?FunctionID=' + id + '&FunctionName=' + name, 'Select User Roles', WINDOW_SMALL);
            }
    </script>
    <miles:MilesSearchPanel ID="pnlSearch" runat="server" SearchOnLoadSavedState="true">
        <ContentPanel>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label ID="Label1" runat="server" Text="Function Name" AssociatedControlID="txtFunctionName" />
                    <br />
                    <asp:TextBox ID="txtFunctionName" runat="server" />
                </div>
            </div>
        </ContentPanel>
    </miles:MilesSearchPanel>
    <hr />
    <telerik:RadAjaxLoadingPanel ID="rdAjaxLoadingPanel" runat="server" />
    <telerik:RadAjaxPanel ID="rdAjaxPanel" runat="server" LoadingPanelID="rdAjaxLoadingPanel">
        <telerik:RadTreeList ID="tlFunctions" runat="server" DataKeyNames="ID" ParentDataKeyNames="ParentID" AutoGenerateColumns="false">
            <Columns>
                <telerik:TreeListBoundColumn HeaderText="Functions" DataField="Name"></telerik:TreeListBoundColumn>
                <telerik:TreeListHyperLinkColumn UniqueName="ManageAccess" Text="Manage Access" DataNavigateUrlFields="ID, Name"
                            DataNavigateUrlFormatString="javascript:openAccessDialog({0}, '{1}');" />
            </Columns>
        </telerik:RadTreeList>
    </telerik:RadAjaxPanel>
    <hr />
</asp:Content>

