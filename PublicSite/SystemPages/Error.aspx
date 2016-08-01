<%@ Page Title="" Language="VB" MasterPageFile="~/Master/System.master" AutoEventWireup="false" CodeFile="Error.aspx.vb" Inherits="SystemPages_Error" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">

    <div class="panel panel-danger">
        <div class="panel-heading">Something went wrong!</div>
        <div class="panel-body">

            <div class="row">
                <div class="col-sm-2">
                    <img src="/Images/64x64/warning.png" alt="Alert" />
                </div>
                <div class="col-sm-10">
                    Sorry an error has occured.
                    Click <a href="/">here</a> to go to the Home Page
                </div>
            </div>

        </div>
    </div>


</asp:Content>

