<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="OrderPad.aspx.vb" Inherits="Orders_OrderPad" %>
<%@ Register TagPrefix="miles" TagName="orderheader" Src="~/UserControls/orderheader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" Runat="Server">

     <miles:orderheader ID="orderheader" runat="server" />

    <div class="detail-footer">
        <miles:MilesButton ID="btnSaveCon" runat="server" Text="Save and Continue" ActionType="Primary" />
        <asp:Button ID="btnClose" runat="server" Text="Cancel" SkinID="Cancel" CausesValidation="false" />
    </div>


    <hr />
    <asp:Panel ID="pnlHeader" runat="server" CssClass="detail-header">
        <miles:MilesTabStrip ID="rdTabs" runat="server" ShowInvisible="true" XmlFileName="~/App_Data/Tabs/OrderTabs.xml" />
    </asp:Panel>

    
<%--    Order Pad--%>
      <telerik:RadAjaxManager ID="rdAjaxManager" runat="server" DefaultLoadingPanelID="rdLoadingPanel">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="clOrderPad">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lbloTotal" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="clOrderPad">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="lbloTotal" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>



    <telerik:RadAjaxPanel runat="server" ID="pnlAjax" LoadingPanelID="rdLoadingPanel" EnableAJAX="true">
        <miles:MilesCardList ID="clOrderPad" runat="server" AllowCustomPaging="true" DataKeyNames="OrderDetailID" AddNewRecordText="Add New OrderDetail" CardMinHeight="40px" NumberOfColumns="One">
            <SortExpressionList>
            </SortExpressionList>



            <CardInfoTemplate>
                <div class="row hidden-xs hidden-sm panel-heading">
                    <div class="col-md-1">OrderDetailID</div>
                    <div class="col-md-2">Category</div>
                    <div class="col-md-2">Subcategory</div>
                    <div class="col-md-2">Product Name</div>
                    <div class="col-md-1">Qty</div>
                    <div class="col-md-2">Price</div>
                    <div class="col-md-2">Total</div>
                </div>
            </CardInfoTemplate>

            <CardTemplate HeaderLinkType="HyperLink">
                <CardItemTemplate>
                    <div class="row panel-heading">
                        <div class="col-md-1">
                            <miles:CardListDataBoundProperty runat="server" DataField="OrderDetailID" />
                        </div>
                        <div class="col-md-2">
                            <asp:DropDownList ID="ddlCategory" runat="server" Width ="200px" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                             <asp:DropDownList ID="ddlSubCategory" runat="server" Width ="200px" Enabled ="false" AutoPostBack="true" OnSelectedIndexChanged="ddlSubCategory_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                             <asp:DropDownList ID="ddlProduct" runat="server" Width ="200px" Enabled ="false"></asp:DropDownList>
                        </div>
                        <div class="col-md-1">
                            <telerik:RadNumericTextBox ID="txtQty" runat="server" AutoPostBack ="true" OnTextChanged ="txtQty_OnTextChanged"  Width ="50">
                                <NumberFormat GroupSeparator="" DecimalDigits="0" />
                            </telerik:RadNumericTextBox>
                        </div>
                        <div class="col-md-2">
                            <miles:CardListDataBoundProperty runat="server" DataField="Price" DataFormatString="{0:C}"/>
                        </div>
                        <div class="col-md-2">
                            <miles:CardListDataBoundProperty runat="server" DataField="Total" DataFormatString="{0:C}"/>
                            <asp:Label ID="lbltot" runat="server" Text="Label"></asp:Label>
                        </div>
                    </div>
                    <asp:Label ID="HideOrderDetailID" runat="server" Text="Label" Visible ="false" ></asp:Label>
                </CardItemTemplate>

                


                <CardActionTemplate>
                    <miles:MilesToggleArchiveImageButton ID="btnArchiveRestore" runat="server" Archived='<%# CBool(Eval("Archived")) %>' />
                </CardActionTemplate>


            </CardTemplate>

            <CardEditTemplate >

                <CardItemTemplate >
                <div class="row panel-heading">
                        <div class="col-md-1">
                            <miles:CardListDataBoundProperty runat="server" DataField="OrderDetailID" />
                        </div>
                        <div class="col-md-2">
                            <asp:DropDownList ID="ddlCategory2" runat="server" Width ="200px" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory2_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                             <asp:DropDownList ID="ddlSubCategory2" runat="server" Width ="200px" Enabled ="false" AutoPostBack="true" OnSelectedIndexChanged="ddlSubCategory2_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                             <asp:DropDownList ID="ddlProduct2" runat="server" Width ="200px" Enabled ="false" AutoPostBack="true" OnSelectedIndexChanged="ddlProduct2_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                        <div class="col-md-1">
                            <telerik:RadNumericTextBox ID="txtQty2" runat="server" AutoPostBack ="true" OnTextChanged ="txtQty2_OnTextChanged"  Width ="50">
                                <NumberFormat GroupSeparator="" DecimalDigits="0" />
                            </telerik:RadNumericTextBox>
                        </div>
                        <div class="col-md-2">
                            $<asp:Label ID="Price2" runat="server" Text="0"></asp:Label>
                        </div>
                        <div class="col-md-2">
                             $<asp:Label ID="Total2" runat="server" Text="0"></asp:Label>
                        </div>
                    </div>
                    <asp:Label ID="HideOrderDetailID" runat="server" Text="Label" Visible ="false" ></asp:Label>

                </CardItemTemplate>

            </CardEditTemplate>

        </miles:MilesCardList>
    </telerik:RadAjaxPanel>

    <div class="pull-right">
        <asp:Label ID="Label1" runat="server" Text="Order Total: " Visible="false"></asp:Label>
        <asp:Label ID="lbloTotal" runat="server" Text="" Visible="false"></asp:Label>
        &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
         <miles:MilesButton ID="btnSave" runat="server" Text="Save" ActionType="Secondary"/>
    </div>
        

</asp:Content>

