<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="ContactInfo.aspx.vb" Inherits="CRM_ContactInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">
    <asp:Panel ID="pnlHeader" runat="server" CssClass="detail-header">
        <miles:MilesTabStrip ID="rdTabs" runat="server" ShowInvisible="true"   XmlFileName="~/App_Data/Tabs/CustomerTabs.xml" />
    </asp:Panel>
    <asp:Panel ID="pnlDataEntry" runat="server" CssClass="detail">
        <telerik:RadAjaxPanel ID="rdAjaxPanel" runat="server">
            <%--data-entry--%>
            <div class="detail-body">
                <div class="row">

                    <div class="col-md-4">

                        <div class="panel panel-info">
                            <div class="panel-heading">Basic Info</div>
                            <div class="panel-body">

                                <asp:Label ID="Label1" runat="server" Text="First Name" AssociatedControlID="txtFirstName" />
                                <asp:Label ID="Label2" runat="server" SkinID="Required" />
                                <asp:TextBox ID="txtFirstName" runat="server" MaxLength="50" />
                                <asp:RequiredFieldValidator ID="rfvtxtFName" runat="server" ControlToValidate="txtFirstName"
                                    ErrorMessage="First Name is required" />


                                <asp:Label ID="Label3" runat="server" Text="Last Name" AssociatedControlID="txtLastName" />
                                <asp:Label ID="Label4" runat="server" SkinID="Required" />
                                <asp:TextBox ID="txtLastName" runat="server" MaxLength="50" />
                                <asp:RequiredFieldValidator ID="rfvtxtLName" runat="server" ControlToValidate="txtLastName"
                                    ErrorMessage="Last Name is required" />

                                <asp:Label ID="Label11" runat="server" Text="Title" AssociatedControlID="txtTitle" />
                                <asp:TextBox ID="txtTitle" runat="server" MaxLength="50" />

                                <asp:Label ID="Label13" runat="server" Text="Notes" AssociatedControlID="txtNotes" />
                                <miles:MilesEditor ID="txtNotes" runat="server" />
                                <br />
                                <asp:CheckBox ID="chkPrimaryContact" Text="Is Primary Contact" runat="server" />

                            </div>
                        </div>

                    </div>

                    <div class="col-md-4">
                        <div class="panel panel-default">
                            <div class="panel-heading">Contact Info</div>
                            <div class="panel-body">
                                <asp:Label ID="Label10" runat="server" Text="Email" AssociatedControlID="txtEmail:EmailTextBox" />
                                <miles:MilesEmail ID="txtEmail" runat="server" />
                                <%--<asp:RequiredFieldValidator ID="rfvtxtEmail" runat="server" ControlToValidate="txtEmail:EmailTextBox"
                                    ErrorMessage="txtEmail is required" Enabled="true" />--%>


                                <asp:Label ID="Label7" runat="server" Text="Home Phone" AssociatedControlID="txtHomePhone" />
                                <miles:MilesPhone ID="txtHomePhone" runat="server" ShowExt="false" />


                                <asp:Label ID="Label8" runat="server" Text="Mobile Phone" AssociatedControlID="txtMobilePhone" />
                                <miles:MilesPhone ID="txtMobilePhone" runat="server" ShowExt="false" />

                                <asp:Label ID="Label9" runat="server" Text="Work Phone" AssociatedControlID="txtWorkPhone" />
                                <miles:MilesPhone ID="txtWorkPhone" runat="server" ShowExt="true" />


                                <miles:MilesAddress ID="milesAddr" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%--footer--%>
            <div class="detail-footer">
                <miles:MilesButton ID="btnSaveClose" runat="server" Text="Save and Close" ActionType="Primary" />
                <miles:MilesButton ID="btnSave" runat="server" Text="Save" ActionType="Primary" />
                <asp:Button ID="btnClose" runat="server" Text="Close" SkinID="Cancel" CausesValidation="false" />
            </div>
        </telerik:RadAjaxPanel>
    </asp:Panel>
</asp:Content>

