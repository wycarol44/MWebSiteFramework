<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MilesAddress.ascx.vb" Inherits="UserControls_MilesAddress" %>

<div>

    <asp:Label runat="server" Text="Address Line 1" AssociatedControlID="txtAddressLine1" />
    <asp:Label ID="lblAddressLine1Required" runat="server" SkinID="Required" Visible="false" />
    <asp:TextBox ID="txtAddressLine1" runat="server" MaxLength="100" />
    <asp:RequiredFieldValidator ID="rfvtxtAddressLine1" runat="server" ControlToValidate="txtAddressLine1"
        ErrorMessage="Address Line 1 is required" Enabled="false" />

    <asp:Label ID="Label1" runat="server" Text="Address Line 2" AssociatedControlID="txtAddressLine2" />
    <asp:TextBox ID="txtAddressLine2" runat="server" MaxLength="100" />

    <asp:Label ID="Label2" runat="server" Text="City" AssociatedControlID="txtCity" />
    <asp:Label ID="lblCityRequired" runat="server" SkinID="Required" Visible="false" />
    <asp:TextBox ID="txtCity" runat="server" MaxLength="50" />
    <asp:RequiredFieldValidator ID="rfvtxtCity" runat="server" ControlToValidate="txtCity"
        ErrorMessage="City is required" Enabled="false"/>

    <asp:Label ID="Label3" runat="server" Text="Country" AssociatedControlID="ddlCountry" />
    <asp:Label ID="lblCountryRequired" runat="server" SkinID="Required" Visible="false"/>
    <miles:MilesCountryComboBox ID="ddlCountry" runat="server" AutoPostBack="true" CausesValidation="false" />
    <asp:RequiredFieldValidator ID="rfvddlCountry" runat="server" ControlToValidate="ddlCountry"
        ErrorMessage="Country is required" InitialValue="< Choose >" Enabled="false"/>

    <div class="row">
        <div class="col-sm-8">
            <asp:Label ID="Label4" runat="server" Text="State" AssociatedControlID="ddlState" />
            <asp:Label ID="lblStateRequired" runat="server" SkinID="Required" Visible="false" />

            <asp:MultiView ID="mvState" runat="server" ActiveViewIndex="0">

                <asp:View ID="vwStateComboxBox" runat="server">
                    <%--Dropdown for canada and US--%>
                    <miles:MilesStateComboBox ID="ddlState" runat="server" />
                    <asp:RequiredFieldValidator ID="rfvddlState" runat="server" ControlToValidate="ddlState"
                        ErrorMessage="State is required" InitialValue="< Choose >" Enabled="false"/>
                </asp:View>

                <asp:View ID="vwStateTextBox" runat="server">
                    <%--Textbox for everyone else--%>
                    <asp:TextBox ID="txtState" runat="server" MaxLength="50" />
                    <asp:RequiredFieldValidator ID="rfvtxtState" runat="server" ControlToValidate="txtState"
                        ErrorMessage="State is required" Enabled="false"/>

                </asp:View>
            </asp:MultiView>
        </div>

        <div class="col-sm-4">
            <asp:Label ID="Label5" runat="server" Text="Postal Code" AssociatedControlID="txtPostalCode" />
            <asp:Label ID="lblPostalCodeRequired" runat="server" SkinID="Required" Visible="false" />
            <miles:MilesPostalCode ID="txtPostalCode" runat="server" MaxLength="50" />
             <asp:RequiredFieldValidator ID="rfvtxtPostalCode" runat="server" ControlToValidate="txtPostalCode"
                ErrorMessage="Postal Code is required" Enabled="false"/>
        </div>
    </div>

</div>
