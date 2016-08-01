<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="CompanyInfo.aspx.vb" Inherits="Admin_CompanyInfo" %>

<%@ MasterType virtualPath="~/Master/Main.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" Runat="Server">
    <asp:Panel ID="pnlDataEntry" runat="server" CssClass="detail" DefaultButton="btnSave">
        <telerik:RadAjaxPanel ID="rdAjaxPanel" runat="server" EnableAJAX="false">

            <%--data-entry--%>
            <div class="detail-body">
                <div class="row">

                    <div class="col-sm-4">

                        <div class="panel panel-info">
                            <div class="panel-heading">Basic Info</div>
                            <div class="panel-body">

                                <asp:Label ID="lblCompanyName" runat="server" Text="Company Name" AssociatedControlID="txtCompanyName" />
                                <asp:Label ID="lblCompanyNameRequired" runat="server" SkinID="Required" />
                                <asp:TextBox ID="txtCompanyName" runat="server" MaxLength="500" />
                                <asp:RequiredFieldValidator ID="rfvCompanyName" runat="server" ControlToValidate="txtCompanyName"
                                    ErrorMessage="Company Name is required" />

                                <asp:Label ID="lblApplicationName" runat="server" Text="Application Name" AssociatedControlID="txtApplicationName" />
                                <asp:Label ID="lblApplicationNameRequired" runat="server" SkinID="Required" />
                                <asp:TextBox ID="txtApplicationName" runat="server" MaxLength="200" />
                                <asp:RequiredFieldValidator ID="rfvApplicationName" runat="server" ControlToValidate="txtApplicationName"
                                    ErrorMessage="Application Name is required" />

                            </div>
                        </div>

                    </div>

                    <div class="col-sm-4">

                        <div class="panel panel-default">
                            <div class="panel-heading">Contact Info</div>
                            <div class="panel-body">

                                <asp:Label ID="lblEmail" runat="server" Text="Email" AssociatedControlID="txtEmail:EmailTextBox" />
                                <asp:Label ID="lblEmailRequired" runat="server" SkinID="Required" />
                                <miles:MilesEmail ID="txtEmail" runat="server" />

                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail:EmailTextBox"
                                    ErrorMessage="Email is required" />

                                <asp:Label ID="lblPhone" runat="server" Text="Phone" AssociatedControlID="txtPhone" />
                                <miles:MilesPhone ID="txtPhone" runat="server" ShowExt="false" />
                                
                                <miles:MilesAddress ID="milesAddr" runat="server" />

                            </div>
                        </div>

                    </div>

                </div>
            </div>
            
            <%--footer--%>
            <div class="detail-footer">
                <miles:MilesButton ID="btnSave" runat="server" Text="Save" ActionType="Primary" />
            </div>
        </telerik:RadAjaxPanel>
    </asp:Panel>
</asp:Content>

