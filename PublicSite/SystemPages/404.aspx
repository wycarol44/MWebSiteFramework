<%@ Page Title="" Language="VB" MasterPageFile="~/Master/System.master" AutoEventWireup="false" CodeFile="404.aspx.vb" Inherits="SystemPages_404" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" Runat="Server">
      <div class="panel panel-warning">
        <div class="panel-heading">Page not found!</div>
        <div class="panel-body">

            <div class="row">
                <div class="col-sm-2">
                    <img src="/Images/64x64/warning.png" alt="Alert" />
                </div>
                <div class="col-sm-10">
                    Sorry we could not find the page you were looking for.
                    Click <a href="/Default.aspx">here</a> to go to the Home Page
                </div>
            </div>

        </div>
    </div>
</asp:Content>

