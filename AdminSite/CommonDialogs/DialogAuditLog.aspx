<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Dialog.master" AutoEventWireup="false" CodeFile="DialogAuditLog.aspx.vb" Inherits="CommonDialogs_DialogAuditLog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">
    <div>
            <div class="row">
                <br />
                <div class="col-md-12">
                    <div class="contentarea">
                        <miles:MilesAuditLogDashboard ID="ucAuditLog" runat="server" Collapsible="false" ExpandOnLoad="true" ShowHeader="false"
                              NumberofDisplayItems="7"></miles:MilesAuditLogDashboard>
                    </div>
                </div>
            </div>
             <div class="dialog-actions">
                <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-default" CausesValidation="false"/>
            </div>
    </div>
</asp:Content>

