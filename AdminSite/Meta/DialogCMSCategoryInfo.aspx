<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Dialog.master" AutoEventWireup="false" CodeFile="DialogCMSCategoryInfo.aspx.vb" Inherits="Meta_DialogCMSCategoryInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">

    <telerik:RadAjaxPanel ID="rdAjaxPanel" runat="server" RenderMode="Inline" EnableAJAX="true">
        <asp:Panel ID="pnlContent" runat="server" DefaultButton="btnSaveClose" Style="padding-top: 10px">


            <%--dialog content here--%>
            <div class="row">
                <div class="col-xs-8">

                    <asp:Label ID="lblCategoryName" runat="server" Text="Category Name" AssociatedControlID="txtCategoryName" />
                    <asp:Label ID="lblCategoryNameRequired" runat="server" SkinID="Required" />
                    <asp:TextBox ID="txtCategoryName" runat="server" MaxLength="500" />
                    <asp:RequiredFieldValidator ID="rfvCategoryName" runat="server" ControlToValidate="txtCategoryName"
                        ErrorMessage="Category Name is required" Display="None" />

                    <asp:Label ID="lblContentType" runat="server" Text="Content Type" AssociatedControlID="ddlContentType" />
                    <asp:Label ID="Label2" runat="server" SkinID="Required" />
                    <miles:MilesTypeComboBox ID="ddlContentType" runat="server" StatusType="CMSContentType" AutoPostBack="true" />
                    <asp:RequiredFieldValidator ID="rfvddlContentType" runat="server" ControlToValidate="ddlContentType"
                        ErrorMessage="Content Type is required" InitialValue="< Choose >" />

                    <asp:Panel ID="pnlEmailFromType" runat="server" Visible="false">
                        <asp:Label ID="lblEmailFromType" runat="server" Text="Email From" AssociatedControlID="ddlEmailFromType" />
                        <asp:Label ID="Label3" runat="server" SkinID="Required" />
                        <miles:MilesTypeComboBox ID="ddlEmailFromType" runat="server" StatusType="CMSEmailFrom" />
                        <asp:RequiredFieldValidator ID="rfvddlEmailFromType" runat="server" ControlToValidate="ddlEmailFromType"
                            ErrorMessage="Email From is required" InitialValue="< Choose >" />
                    </asp:Panel>

                    <miles:CMSMergeFieldsList ID="milesCMSMergeFields" Required="false" runat="server"></miles:CMSMergeFieldsList>
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

