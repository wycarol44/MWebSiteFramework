<%@ Page Title="" Language="VB" MasterPageFile="~/Master/System.master" AutoEventWireup="false" CodeFile="NoAccess.aspx.vb" Inherits="SystemPages_NoAccess" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" Runat="Server">
   
       <div class="panel panel-danger">
        <div class="panel-heading">No Access!</div>
        <div class="panel-body">

            <div class="row">
                <div class="col-sm-2">
                    <img src="/Images/64x64/warning.png" alt="Alert" />
                </div>
                <div class="col-sm-10">
                    Sorry, you do not have access to this page. Please contact your system administrator. Click
                    <a href='<%= If(Request.UrlReferrer IsNot Nothing, Request.UrlReferrer.ToString(), "/default.aspx" )%>'>here</a> to go back.
                </div>
            </div>

        </div>
    </div>
</asp:Content>

