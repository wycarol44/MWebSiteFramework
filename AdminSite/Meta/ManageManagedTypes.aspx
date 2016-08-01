<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="ManageManagedTypes.aspx.vb" Inherits="Meta_ManageManagedTypes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">
    <telerik:RadAjaxLoadingPanel ID="rdLoadingPanel" runat="server" />
    <telerik:RadAjaxManager ID="rdAjaxManager" runat="server" DefaultLoadingPanelID="rdLoadingPanel">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="clTypes:CardListPanel">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="clTypes:CardListPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>

            <telerik:AjaxSetting AjaxControlID="rdAjaxManager">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="clTypes:CardListPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <miles:MilesSearchPanel ID="pnlSearch" runat="server" ListControlID="clTypes">

        <ContentPanel>
            <div class="row">
                <div class="col-md-2">
                    <asp:Label ID="lblTypeName" runat="server" Text="Type Name" AssociatedControlID="txtType" />
                    <asp:TextBox ID="txtType" runat="server" />
                </div>
            </div>
        </ContentPanel>

    </miles:MilesSearchPanel>
    <hr />
    <miles:MilesCardList ID="clTypes" runat="server" DataKeyNames="TypeID" AddNewRecordText="Add New Type">
        <SortExpressionList>
            <miles:MilesCardListSortExpression DisplayName="Type Name" FieldName="TypeName" />
        </SortExpressionList>

        <CardTemplate ActionPosition="Right">
            <CardHeaderTemplate>
                <h4>
                    <%# Eval("FriendlyName")%>
                </h4>
            </CardHeaderTemplate>
            <CardItemTemplate>
                    <div><%# Eval("TypeName")%></div>
                    <div><%# If(Eval("IsHierarchy"),"Is Hierarchy","")%></div>
                    
            </CardItemTemplate>
            <CardActionTemplate>
                <asp:ImageButton ID="btnEdit" runat="server" SkinID="CardListEdit" CommandName="Edit" />
            </CardActionTemplate>
        </CardTemplate>

        <CardEditTemplate ActionPosition="Bottom" DefaultButton="lnkSave">
            <CardItemTemplate>
                <asp:Label ID="lblFriendlyName" Text="Friendly Name" runat="server" AssociatedControlID="txtFriendlyName" />
                <asp:Label ID="Label3" runat="server" SkinID="Required" />
                <asp:TextBox ID="txtFriendlyName" Text='<%# Eval("FriendlyName")%>' runat="server" MaxLength="500" />
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtFriendlyName"
                    ErrorMessage="Friendly Name is required" Display="None"></asp:RequiredFieldValidator>

                <asp:Label ID="lblTypeName" Text="Type Name" runat="server" AssociatedControlID="txtTypeName" />
                <asp:Label ID="Label1" runat="server" SkinID="Required" />
                <asp:TextBox ID="txtTypeName" Text='<%# Eval("TypeName")%>' runat="server" MaxLength="500" />
                <asp:RequiredFieldValidator runat="server" ID="rfvTypeName" ControlToValidate="txtTypeName"
                    ErrorMessage="Type Name is required" Display="None"></asp:RequiredFieldValidator>

                <asp:CheckBox ID="chkIsHierarchy" runat="server" Text="Is Hierarchy" Checked='<%# CBool(Eval("IsHierarchy")) %>' />
            </CardItemTemplate>

            <CardActionTemplate>
                <asp:LinkButton ID="lnkSave" runat="server" SkinID="CardListSave" CommandName="PerformInsert" />
                <asp:LinkButton ID="lnkCancel" runat="server" SkinID="CardListCancel" CausesValidation="false" CommandName="Cancel" />
            </CardActionTemplate>
        </CardEditTemplate>
    </miles:MilesCardList>
</asp:Content>

