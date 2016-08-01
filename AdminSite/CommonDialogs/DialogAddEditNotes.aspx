<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Dialog.master" AutoEventWireup="false" CodeFile="DialogAddEditNotes.aspx.vb" Inherits="CommonDialogs_DialogAddEditNotes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">
    <asp:Panel ID="pnlContent" runat="server" DefaultButton="btnSaveClose">


        <%--dialog content here--%>
     
      
                <div class="row">

                    <div class="col-sm-8">

                        <asp:Label ID="lblTitle" runat="server" Text="Title" AssociatedControlID="txtTitle" />
                        <asp:Label ID="lblTitleRequired" runat="server" SkinID="Required" />
                        <asp:TextBox ID="txtTitle" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvtxtTitle" runat="server" ControlToValidate="txtTitle"
                            ErrorMessage="Title is required" />
                        <telerik:RadAjaxPanel ID="rdAjax" runat="server">
                            <asp:Label ID="lblType" runat="server" Text="Type" AssociatedControlID="ddlType" />
                            <asp:Label ID="lblTypeRequired" runat="server" SkinID="Required" />
                            <miles:MilesTypeComboBox AutoPostBack="true" CausesValidation="false" ID="ddlType" runat="server" StatusType="NoteType" />
                            <asp:RequiredFieldValidator ID="rfvddlType" runat="server" ControlToValidate="ddlType"
                                ErrorMessage="Type is required" InitialValue="< Choose >" />
                            <asp:Panel ID="pnlLinkURL" Visible="false" runat="server">
                                <asp:Label ID="lblLinkURL" runat="server" Text="Link URL" AssociatedControlID="txtLinkURL" />
                                <asp:Label ID="lblLinkURLRequired" runat="server" SkinID="Required" />
                                <asp:TextBox ID="txtLinkURL" runat="server" />
                                <asp:RequiredFieldValidator ID="rfvLinkURL" runat="server" ControlToValidate="txtLinkURL"
                                    ErrorMessage="Link URL is required" />
                                <asp:RegularExpressionValidator ID="revtxtLinkURL" runat="server" ControlToValidate="txtLinkURL"
                                    ValidationExpression="<%$ AppSettings: Miles.URLValidationExpression %>"
                                    ErrorMessage="<%$ AppSettings: Miles.URLValidationMessage %>" />
                            </asp:Panel>
                        </telerik:RadAjaxPanel>
                        <asp:CheckBox ID="chkPinNote" Text="Pin this note" runat="server" />
                        <br />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-10">
                        <asp:Label ID="Label13" runat="server" Text="Notes" AssociatedControlID="txtNotes" />
                        <miles:MilesEditor ID="txtNotes" runat="server" />
                        <br />
                    </div>
                </div>
           
     
        <%--end dialog content--%>

            <%--buttons--%>
            <div class="dialog-actions">
                <div>
                    <miles:MilesButton ID="btnSaveClose" runat="server" Text="Save and Close" ActionType="Primary" />
                  
                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-default" CausesValidation="false"/>
                </div>
            </div>
    </asp:Panel>

</asp:Content>

