<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="DialogManageCustomerAddress.aspx.vb" Inherits="CRM_DialogManageCustomerAddress" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
  <script>
            function OnClientCommandExecuting(editor, args) {
                var name = args.get_name();
                var val = args.get_value();

                if (name == "MergeFields") {
                    editor.pasteHtml("[[" + val + "]]");
                    //Cancel the further execution of the command as such a command does not exist in the editor command list
                    args.set_cancel(true);
                }
            }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" Runat="Server">
    <telerik:RadAjaxPanel ID="rdAjaxPanel" runat="server" EnableAJAX="false">
        <asp:Panel ID="pnlContent" runat="server" DefaultButton="btnSaveClose" Style="padding-top: 10px">

            <%--dialog content here--%>
       <asp:Panel ID="pnlDataEntry" runat="server" CssClass="detail">
	        <asp:Panel ID="pnlHeader" runat="server" CssClass="detail-header">
	        </asp:Panel>
	        <div class="detail-body">
		        <div class="row">
			        <div class="col-md-6">
			        <div class="panel panel-info">
				        <div class="panel-body">
                             <miles:MilesAddress runat="server" ID ="milesAdd" Required="true"  />
				        </div>
			        </div>
			        </div>
		        </div>
	        </div>
        </asp:Panel>

           

            <%--end dialog content--%>

            <%--buttons--%>
           <div class="dialog-actions">
                <div>
                    <miles:MilesButton ID="btnSaveClose" runat="server" Text="Save and Close" ActionType="Primary" />
                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-default" CausesValidation="false" />
                </div>
            </div>

        </asp:Panel>

    </telerik:RadAjaxPanel>

</asp:Content>

