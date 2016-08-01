<%@ Page Title="" Language="VB" MasterPageFile="~/Master/System.master" AutoEventWireup="false" CodeFile="SessionExpired.aspx.vb" Inherits="SystemPages_SessionExpired" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" Runat="Server">
       <div class="panel panel-warning">
        <div class="panel-heading">Session has expired!</div>
        <div class="panel-body">

            <div class="row">
                <div class="col-sm-2">
                    <img src="/Images/64x64/warning.png" alt="Alert" />
                </div>
                <div class="col-sm-10">
                    Sorry your session has expired!
                    Click <asp:HyperLink id="lnkGoBack" runat="server">here</asp:HyperLink> to go back.
                </div>
            </div>

        </div>
    </div>
</asp:Content>

