<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="CustomerDashboard.aspx.vb" Inherits="CRM_CustomerDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">

    <div class="dashboard">
        <div class="row">
            <div class="col-md-6">
                <div class="panel panel-info widget">
                    <div class="panel-heading anchorheader">
                        <asp:HyperLink ID="lnkBasicInfoHeader" Text="Basic Info" runat="server">
                            <asp:Image ID="lnkEdit" CssClass="pull-right" runat="server" ToolTip="Edit" ImageUrl="/Images/24x24/document-edit.png" />
                        </asp:HyperLink>

                    </div>
                    <div class="panel-body action-bottom">
                        <div class="contentarea ">
                            <asp:HyperLink ID="lnkCustomerName" Width="100%" runat="server" />
                            <asp:Label ID="lblAddress" Width="100%" runat="server" />
                            <asp:HyperLink ID="lnkCustomerPhone" Width="100%" runat="server" />
                            <asp:HyperLink ID="lnkWebSite" Width="100%" Target="_blank" runat="server" />
                            <asp:Label ID="lblStatus" Width="100%" runat="server" />
                        </div>
                        <div class="action">
                            <telerik:RadToolBar ID="rdActions" runat="server">
                                <Items>
                                    <telerik:RadToolBarDropDown DropDownWidth="175px" Text="Actions">
                                        <Buttons>
                                            <telerik:RadToolBarButton Text="Mark As Active" Value="Active" CommandName="Active" runat="server" />
                                            <telerik:RadToolBarButton Text="Mark As Pending" Value="Pending" CommandName="Pending" runat="server" />
                                            <telerik:RadToolBarButton Text="Mark As Lost" Value="Lost" CommandName="Lost" runat="server" />
                                            <telerik:RadToolBarButton Text="Mark As Inactive" Value="Inactive" CommandName="Inactive" runat="server" />
                                            <telerik:RadToolBarButton IsSeparator="true" runat="server" />
                                            <telerik:RadToolBarButton Text="Locate Address" Target="_blank" Value="ViewMap" ImageUrl="/Images/16x16/place_blue.png" CommandName="ViewMap" runat="server" />
                                        </Buttons>
                                    </telerik:RadToolBarDropDown>
                                </Items>
                            </telerik:RadToolBar>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="panel panel-default widget">
                    <div class="panel-heading anchorheader">
                        <asp:HyperLink ID="lnkContactsHeader" Text="Contacts" runat="server" />
                    </div>
                    <div class="panel-body action-right">
                        <div class="contentarea">
                            <asp:Label ID="lblNoPrimaryContact" Text="No primary contact assigned for this customer" runat="server" Width="100%" Font-Italic="true" Visible="false" />
                            <asp:HyperLink ID="lnkContactName" runat="server" /><br />
                            <asp:Label ID="lblContactTitle" runat="server" /><br />
                            <asp:HyperLink ID="lnkContactEmail" runat="server" /><br />
                            <asp:HyperLink ID="lnkContactPhone" runat="server" />
                        </div>
                        <div class="action">
                            <asp:ImageButton ID="btnAddNewContacts" CommandName="AddNew" runat="server" ImageUrl="~/Images/24x24/document-add.png"
                                ToolTip="Add New Contact" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <miles:MilesNotesDashboard ID="ucNotesDashboard" NumberofDisplayItems="2" runat="server" />
            </div>
            <div class="col-md-6">
                <miles:MilesDocumentsDashboard ID="ucDocumentsDashboard" runat="server" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <miles:MilesAuditLogDashboard ID="ucAuditLogDashboard"  Collapsible="true" ExpandOnLoad="false" runat="server" />
            </div>
        </div>
    </div>
</asp:Content>

