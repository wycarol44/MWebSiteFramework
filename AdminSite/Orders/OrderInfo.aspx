<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="OrderInfo.aspx.vb" Inherits="Orders_OrderInfo" %>

<%@ Register TagPrefix="miles" TagName="orderheader" Src="~/UserControls/orderheader.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
        function openEditDialog(id, id2) {
            openWindow('/Orders/OrderDetails.aspx?OrderDetailID=' + id + '&OrderID=' + id2, 'Order Details', WINDOW_LARGE, dialogClosed);
            return false;
        }

        function dialogClosed(sender, args) {
           
            var arg = args.get_argument();
            if (arg != null) {
                $find('<%= rdAjaxManager.ClientID%>').ajaxRequest();
                   }
               }

    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">

    <miles:orderheader ID="orderheader" runat="server" />

    <div class="detail-footer">
        <miles:MilesButton ID="btnSaveCon" runat="server" Text="Save and Continue" ActionType="Primary" />
        <asp:Button ID="btnClose" runat="server" Text="Cancel" SkinID="Cancel" CausesValidation="false" />
    </div>


    <hr />
    <asp:Panel ID="pnlHeader" runat="server" CssClass="detail-header">
        <miles:MilesTabStrip ID="rdTabs" runat="server" ShowInvisible="true" XmlFileName="~/App_Data/Tabs/OrderTabs.xml" />
    </asp:Panel>

    <%--Order Detail--%>
    <telerik:RadAjaxManager ID="rdAjaxManager" runat="server" DefaultLoadingPanelID="rdLoadingPanel">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="clOrderDetails">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="clOrderDetails" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rdAjaxManager">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="clOrderDetails" />
                    <telerik:AjaxUpdatedControl ControlID="lbloTotal" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="clOrderDetails">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lbloTotal" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>


    <telerik:RadAjaxPanel runat="server" ID="pnlAjax" LoadingPanelID="rdLoadingPanel" EnableAJAX="true">
        <miles:MilesCardList ID="clOrderDetails" runat="server" AllowCustomPaging="true" DataKeyNames="OrderDetailID" AddNewRecordText="Add New OrderDetail" CardMinHeight="40px" NumberOfColumns="One" Visible="false">
            <SortExpressionList>
            </SortExpressionList>



            <CardInfoTemplate>
                <div class="row hidden-xs hidden-sm panel-heading">
                    <div class="col-md-1">OrderDetailID</div>
                    <div class="col-md-2">Product Name</div>
                    <div class="col-md-2">Category</div>
                    <div class="col-md-2">Subcategory</div>
                    <div class="col-md-1">Qty</div>
                    <div class="col-md-1">Price</div>
                    <div class="col-md-1">Total</div>
                    <div class="col-md-2">Edit</div>
                </div>
            </CardInfoTemplate>

            <CardTemplate HeaderLinkType="HyperLink">
                <CardItemTemplate>
                    <div class="row panel-heading">
                        <div class="col-md-1">
                            <miles:CardListDataBoundProperty runat="server" DataField="OrderDetailID" />
                        </div>
                        <div class="col-md-2">
                            <miles:CardListDataBoundProperty runat="server" DataField="ProductName" />
                        </div>
                        <div class="col-md-2">
                            <miles:CardListDataBoundProperty runat="server" DataField="CategoryName" />
                        </div>
                        <div class="col-md-2">
                            <miles:CardListDataBoundProperty runat="server" DataField="SubcategoryName" />
                        </div>
                        <div class="col-md-1">
                            <miles:CardListDataBoundProperty runat="server" DataField="Qty" />
                        </div>
                        <div class="col-md-1">
                            <miles:CardListDataBoundProperty runat="server" DataField="Price" DataFormatString="{0:C}"/>
                        </div>
                        <div class="col-md-1">
                            <miles:CardListDataBoundProperty runat="server" DataField="Total" DataFormatString="{0:C}"/>
                        </div>
                        <div class="col-md-2">
                            <asp:LinkButton ID="hlEdit" runat="server" OnClientClick='<%#String.Format("return openEditDialog({0},{1});", Eval("OrderDetailID"), Eval("OrderID"))%>'>Edit</asp:LinkButton>
                        </div>

                    </div>
                </CardItemTemplate>
                <CardActionTemplate>
                    <miles:MilesToggleArchiveImageButton ID="btnArchiveRestore" runat="server" Archived='<%# CBool(Eval("Archived")) %>' />
                </CardActionTemplate>


            </CardTemplate>
        </miles:MilesCardList>
    </telerik:RadAjaxPanel>

    <div class="pull-right">
        <asp:Label ID="Label1" runat="server" Text="Order Total: " Visible="false"></asp:Label>
        <asp:Label ID="lbloTotal" runat="server" Text="" Visible="false"></asp:Label>
    </div>
</asp:Content>

