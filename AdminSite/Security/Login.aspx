<%@ Page Title="Login" Language="VB" MasterPageFile="~/Master/Login.master" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="Login"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">
    <telerik:RadAjaxPanel ID="rdAjaxPanel" runat="server">

        <div id="login-content">

            <%--login--%>
            <asp:Panel ID="loginform" runat="server" CssClass="row" DefaultButton="btnLogin" ClientIDMode="Static">

                <div class="col-xs-12">


                    <div class="input-group">
                        <span class="input-group-addon">
                            <span class="glyphicon glyphicon-user"></span>
                        </span>
                        <asp:RequiredFieldValidator ID="rfvtxtUsername" runat="server" ControlToValidate="txtUsername"
                            ErrorMessage="Username is required" ValidationGroup="Login" />
                        <asp:TextBox ID="txtUsername" runat="server" placeholder="Username" />

                    </div>

                    <br />

                    <div class="input-group">
                        <span class="input-group-addon"><span class="glyphicon glyphicon-lock"></span></span>

                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" placeholder="Password" />
                        <asp:RequiredFieldValidator ID="rfvtxtPassword" runat="server" ControlToValidate="txtPassword"
                            ErrorMessage="Password is required" Display="None" ValidationGroup="Login" />
                        <span class="input-group-btn">
                            <button class="btn btn-info" onclick="login.showForm('#forgotpass'); return false;">
                                <span class="hidden-xs">Forgot Password</span>
                                <span class="visible-xs"><span class="glyphicon glyphicon-question-sign"></span></span>
                            </button>
                        </span>
                    </div>
                    <hr />
                    <div class="footer">
                        <div class="pull-left">
                            <asp:CheckBox ID="chkRememberMe" runat="server" Text="keep me logged in" />

                        </div>
                        <div class="pull-right">


                            <miles:MilesButton ID="btnLogin" runat="server" Text="Log In" CssClass="btn btn-primary"
                                ValidationGroup="Login" />


                        </div>
                        <br class="clear" />
                    </div>


                </div>
            </asp:Panel>

            <%--forgot password--%>
            <asp:Panel ID="forgotpass" runat="server" ClientIDMode="Static" DefaultButton="btnSendResetEmail">
                <div class="col-xs-12">

                    <p>
                        <asp:Label ID="lblInfo" runat="server">  Provide your username and click Submit. An email will be sent to the email address
                    associated to your account.</asp:Label>

                    </p>


                    <div class="input-group">
                        <span class="input-group-addon"><span class="glyphicon glyphicon-user"></span></span>
                        <asp:TextBox ID="txtForgotPass" runat="server" placeholder="Username"  />
                        <asp:RequiredFieldValidator ID="rfvtxtForgotPass" runat="server" ControlToValidate="txtForgotPass"
                            ErrorMessage="Username is required" Display="None" ValidationGroup="ForgotPass" />
                    </div>


                    <hr />


                    <div class="footer">


                        <telerik:RadButton ID="btnSendResetEmail" runat="server" Skin="Silk" Text="Submit" CssClass="btn btn-primary"
                            ValidationGroup="ForgotPass" />
                        <button class="btn btn-default" onclick="login.showForm('#loginform'); return false;">Cancel</button>

                    </div>


                </div>
            </asp:Panel>

            <%--reset password--%>
            <asp:Panel ID="resetpass" runat="server" DefaultButton="btnResetPassword" ClientIDMode="Static">
                <div class="col-xs-12">

                    <div class="input-group">
                        <span class="input-group-addon"><span class="glyphicon glyphicon-lock"></span></span>

                        
                        
                        
                        <asp:RequiredFieldValidator ID="rfvtxtJobTitle" runat="server" ControlToValidate="txtResetPassword"
                            ErrorMessage="Password is required" ValidationGroup="ResetPassword" />
                        <asp:RegularExpressionValidator ID="revtxtPassword1" runat="server" ControlToValidate="txtResetPassword" ValidationGroup="ResetPassword"
                            ValidationExpression="<%$ AppSettings: Miles.PasswordValidationExpression %>"
                            ErrorMessage="<%$ AppSettings: Miles.PasswordValidationMessage %>" />

                        <asp:TextBox ID="txtResetPassword" runat="server" TextMode="Password" placeholder="Password" />

                    </div>
                    <br />
                    <div class="input-group">
                        <span class="input-group-addon"><span class="glyphicon glyphicon-lock"></span></span>

                        
                        
                        
                        <asp:RequiredFieldValidator ID="rfvtxtPassword2" runat="server" ControlToValidate="txtResetConfirmPassword"
                            ErrorMessage="Confirm Password is required" Display="None" ValidationGroup="ResetPassword" />
                        <asp:CompareValidator ID="comvrfvtxtPassword2" runat="server" ControlToValidate="txtResetConfirmPassword" ValidationGroup="ResetPassword"
                            ControlToCompare="txtResetPassword" ErrorMessage="Confirm Password must match"
                            Operator="Equal" />

                        <asp:TextBox ID="txtResetConfirmPassword" runat="server" TextMode="Password" placeholder="Confirm Password" />
                    </div>


                    <hr />


                    <div class="footer">


                        <telerik:RadButton ID="btnResetPassword" runat="server" Skin="Silk" Text="Submit" CssClass="btn btn-primary"
                            ValidationGroup="ResetPassword" />
                        <button class="btn btn-default" onclick="login.showForm('#loginform'); return false;">Cancel</button>

                    </div>


                </div>
            </asp:Panel>

        </div>

    </telerik:RadAjaxPanel>

    <asp:ValidationSummary ID="vsLogin" runat="server" ShowMessageBox="false" DisplayMode="BulletList"
        ValidationGroup="Login" />
    <asp:ValidationSummary ID="vsForgotPass" runat="server" ShowMessageBox="false" DisplayMode="BulletList"
        ValidationGroup="ForgotPass" />
    <asp:ValidationSummary ID="vsResetPassword" runat="server" ShowMessageBox="false" DisplayMode="BulletList"
        ValidationGroup="ResetPassword" />


    <script>

        (function (global) {
            var login = global.login = global.login || {}

            login.showForm = function (formToShow, duration) {
                //hide the current form, so we find the sibling with :visible
                var $visibleForm = $("#login-content > div:visible");

                //if duration is undefined, use 300 as default
                if (typeof duration === 'undefined') {
                    duration = 600;
                }

                //run animation
                if ($visibleForm.length > 0) {

                    $visibleForm.animate(
                        { marginLeft: -800, marginRight: 800, opacity: 0 },
                        {
                            duration: duration, easing: "easeInBack",
                            complete: function () {
                                //make the other form not visible
                                $visibleForm.css({ display: "none" });

                                //bring our form back
                                login.reviveForm(formToShow, duration);
                            }
                        }
                    );
                }
                else {
                    //no other forms to hide, so we dont have to wait for an animation to compelte
                    //bring our form back
                    login.reviveForm(formToShow, duration);
                }

            }

            //brings a form back into view
            login.reviveForm = function (formToShow, duration) {
                //bring in the form we want
                var $formToShow = $(formToShow);
                //console.log($formToShow);
                //reset the margin
                $formToShow.css({ margin: "0 auto", opacity: 1 });
                //fade in
                $formToShow.fadeIn(duration);

                //focus on the first available input control
                $formToShow.find("input[type='text'],input[type='password']").eq(0).focus();
            }

        })(window);

    </script>

</asp:Content>

