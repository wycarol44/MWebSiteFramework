<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="Utility.aspx.vb" Inherits="Admin_Utility" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">

    <div class="panel panel-warning">

        <div class="panel-heading">Clean Up</div>
        <div class="panel-body">

            <div class="row">
                <div class="col-sm-3">
                    <asp:Button ID="btnCleanDeletedForms" runat="server" Text="Clean Deleted Forms" CssClass="btn btn-warning" />


                </div>
            </div>
        </div>

    </div>
    <div class="panel panel-warning">

        <div class="panel-heading">Cache</div>
        <div class="panel-body">

            <div class="row">
                <div class="col-sm-3">

                    <asp:Button ID="btnClearCache" runat="server" Text="Clear Cache" CssClass="btn btn-warning" />

                </div>
            </div>
        </div>

    </div>

</asp:Content>

