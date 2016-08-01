<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="ManageCustomers.aspx.vb" Inherits="CRM_ManageCustomers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">
    <telerik:RadAjaxLoadingPanel ID="rdLoadingPanel" runat="server" />
    <telerik:RadAjaxManager ID="rdAjaxManager" runat="server" DefaultLoadingPanelID="rdLoadingPanel">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="clCustomerd:CardListPanel">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="clCustomers:CardListPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
    </telerik:RadAjaxManager>

    
    <miles:MilesSearchPanel ID="pnlSearch" runat="server" ListControlID="clCustomers">

        <ContentPanel>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label ID="lblCustomerName" runat="server" Text="Customer Name" AssociatedControlID="txtCustomerName" />
                    <asp:TextBox ID="txtCustomerName" runat="server" />
                </div>

                <div class="col-md-2">
                    <asp:Label ID="lblPhone" runat="server" Text="Phone" AssociatedControlID="txtPhone:PhoneTextBox" />
                    <miles:MilesPhone ID="txtPhone" runat="server" ShowExt="false" />
                </div>

                <div class="col-md-2">
                    <asp:Label ID="lblStatus" runat="server" Text="Status" AssociatedControlID="ddlStatusTypes" />
                    <miles:MilesTypeComboBox ID="ddlStatusTypes" runat="server" StatusType="CustomerStatus" IncludeDefaultItem="false"
                        CheckBoxes="true" EnableCheckAllItemsCheckBox="true" EmptyMessage="All" />
                </div>

                <div class="col-md-2">
                    <asp:Label ID="lblContactName" runat="server" Text="Contact Name" AssociatedControlID="txtContactName" />
                    <asp:TextBox ID="txtContactName" runat="server" />
                </div>

                <div class="col-md-2">
                    <asp:Label ID="Label4" runat="server" Text="Email" AssociatedControlID="txtEmail" />
                    <asp:TextBox ID="txtEmail" runat="server" />
                </div>

                <div class="col-md-2">
                    <br />
                    <asp:CheckBox ID="chkArchived" runat="server" Text="Include archived" CssClass="chk-inline" />
                </div>

            </div>
        </ContentPanel>
        <ToolBarCommands>
            <telerik:RadToolBarButton CommandName="ExportCustomers" Text="Export Customers" runat="server" />
        </ToolBarCommands>
    </miles:MilesSearchPanel>

    <hr />
    
    <miles:MilesCardList ID="clCustomers" runat="server" AllowCustomPaging="true"
        DataKeyNames="CustomerID" AddNewRecordText="Add New Customer" >
        <SortExpressionList>
            <miles:MilesCardListSortExpression DisplayName="Customer Name" FieldName="CustomerName" />
            <miles:MilesCardListSortExpression DisplayName="City" FieldName="City" />
            <miles:MilesCardListSortExpression DisplayName="State" FieldName="StateName" />
            <miles:MilesCardListSortExpression DisplayName="Phone" FieldName="Phone" />
            <miles:MilesCardListSortExpression DisplayName="Primary Contact" FieldName="PrimaryContact" />
            <miles:MilesCardListSortExpression DisplayName="Status" FieldName="Status" />
        </SortExpressionList>

        <CardTemplate HeaderLinkType="HyperLink" HeaderNavigateUrlFormatString="~/CRM/CustomerDashboard.aspx?CustomerID={0}" HeaderNavigateUrlFields="CustomerID">
            <CardHeaderTemplate>
                <h4>
                    <%# Eval("CustomerName") %>
                    <div class="pull-right">
                        <asp:Image ID="btnEditCustomerInfo" runat="server"  ImageUrl="/Images/24x24/nav_blue_right.png" />
                    </div>
                </h4>

            </CardHeaderTemplate>
            <CardItemTemplate>
                <div><a href='<%# "tel:" + Eval("Phone")%>'><%# FormatPhone(PhoneNumberType.Work, Eval("Phone"))%></a></div>

                <miles:CardListDataBoundProperty runat="server" DataField="City" LabelText="City:" />
                <miles:CardListDataBoundProperty runat="server" DataField="StateName" LabelText="State:" />
                <miles:CardListDataBoundProperty runat="server" DataField="PrimaryContact" LabelText="Primary Contact:" />
                <miles:CardListDataBoundProperty runat="server" DataField="Status" LabelText="Status:" />

            </CardItemTemplate>
            <CardActionTemplate>
                <miles:MilesToggleArchiveImageButton ID="btnDelete" runat="server" Archived='<%# CBool(Eval("Archived")) %>' />
            </CardActionTemplate>
        </CardTemplate>
    </miles:MilesCardList>
    
    <telerik:ReportViewer runat ="server" ID="rpt" ShowNavigationGroup="false"  ShowHistoryButtons="false"  ></telerik:ReportViewer>
</asp:Content>

