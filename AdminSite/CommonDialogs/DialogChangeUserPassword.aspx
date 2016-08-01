<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Dialog.master" AutoEventWireup="false" CodeFile="DialogChangeUserPassword.aspx.vb" Inherits="HR_DialogChangeUserPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">

    <telerik:RadAjaxPanel ID="rdAjaxPanel" runat="server" EnableAJAX="true">

        <asp:Panel ID="pnlContent" runat="server" DefaultButton="btnSaveClose">


            <%--dialog content here--%>
            <div class="row">
                <div class="col-xs-8">

                    <asp:Label ID="Label1" runat="server" Text="Password" AssociatedControlID="txtPassword1" />
                    <asp:Label ID="Label2" runat="server" SkinID="Required" />
                    <asp:TextBox ID="txtPassword1" runat="server" TextMode="Password" />
                    <asp:RequiredFieldValidator ID="rfvtxtJobTitle" runat="server" ControlToValidate="txtPassword1"
                        ErrorMessage="Password is required" />
                    <asp:RegularExpressionValidator ID="revtxtPassword1" runat="server" ControlToValidate="txtPassword1"
                        ValidationExpression="<%$ AppSettings: Miles.PasswordValidationExpression %>"
                        ErrorMessage="<%$ AppSettings: Miles.PasswordValidationMessage %>" />


                    <asp:Label ID="Label3" runat="server" Text="Confirm Password" AssociatedControlID="txtPassword2" />
                    <asp:Label ID="Label4" runat="server" SkinID="Required" />
                    <asp:TextBox ID="txtPassword2" runat="server" TextMode="Password" />
                    <asp:RequiredFieldValidator ID="rfvtxtPassword2" runat="server" ControlToValidate="txtPassword2"
                        ErrorMessage="Confirm Password is required" Display="None" />
                    <asp:CompareValidator ID="comvrfvtxtPassword2" runat="server" ControlToValidate="txtPassword2"
                        ControlToCompare="txtPassword1" ErrorMessage="Confirm Password must match"
                        Operator="Equal" />

                </div>
            </div>
            <%--end dialog content--%>

            <%--buttons--%>
            <div class="dialog-actions">
                <miles:MilesButton ID="btnSaveClose" runat="server" Text="Save and Close" ActionType="Primary" />
                <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-default" CausesValidation="false"
                    />
            </div>

        </asp:Panel>


    </telerik:RadAjaxPanel>

</asp:Content>

