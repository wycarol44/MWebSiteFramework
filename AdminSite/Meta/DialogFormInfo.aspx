<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Dialog.master" AutoEventWireup="false" CodeFile="DialogFormInfo.aspx.vb" Inherits="Meta_DialogFormInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">
    <telerik:RadAjaxPanel ID="rdAjaxPanel" runat="server" EnableAJAX="true">
        <asp:Panel ID="pnlContent" runat="server" DefaultButton="btnSaveClose" Style="padding-top: 10px">

            <%--dialog content here--%>
            <div class="row">
            <div class="col-xs-8">
                <asp:Label ID="Label1" runat="server" Text="Form Name" AssociatedControlID="txtFormName" />
                <asp:Label ID="txtFormName" runat="server" Width="100%" />
                
                <asp:Label ID="Label2" runat="server" Text="Form Path" AssociatedControlID="txtFormPath" />
                <asp:Label ID="txtFormPath" runat="server" Width="100%" />
                
                <asp:Label ID="Label3" runat="server" Text="Module" AssociatedControlID="ddlModule" />
                 <miles:MilesDropDownTree ID="ddlModule" runat="server" CheckBoxes="None" AutoPostBack="false" AutoCollapseDropDown="true"  />

                <asp:CheckBox ID="chkCanBeFavorite" runat="server" Text="Can Be Favorite" Width="100%" />
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

