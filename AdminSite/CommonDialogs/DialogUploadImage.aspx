<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Dialog.master" AutoEventWireup="false" CodeFile="DialogUploadImage.aspx.vb" Inherits="CommonDialogs_DialogUploadImage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script>
        var imageUploaded = false;
        function rdUpload_OnClientFileUploaded(e, args) {
            imageUploaded = true;
            $find('<%= rdAjaxPanel.ClientID %>').ajaxRequest();
        }

        function validateFileUpload(sender, args) {
            if (!imageUploaded)
            {
                showMsg("Please choose a valid image upload", true, "Error", "errorMsg", "center", 3000);
                args.set_cancel(true);
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">

    <telerik:RadAjaxPanel ID="rdAjaxPanel" runat="server">

        <asp:Panel ID="pnlContent" runat="server" DefaultButton="btnSaveClose">


            <%--dialog content here--%>
            <div>
                <asp:Label runat="server" Text="Choose image to upload" AssociatedControlID="rdUpload" />
                <br />
                <telerik:RadAsyncUpload ID="rdUpload" runat="server" OnClientFileUploaded="rdUpload_OnClientFileUploaded" AllowedFileExtensions="jpg,jpeg,png,gif" MaxFileInputsCount="1" />
                <br />
                <br />
                <miles:MilesImageCrop ID="milescrop" runat="server" AspectRatio="1" ShowPreview="true" Visible="false" Width="100%" />

            </div>
            <%--end dialog content--%>

            <%--buttons--%>
            <div class="dialog-actions">
                <div>
                    <miles:MilesButton ID="btnSaveClose" OnClientClicking="validateFileUpload" runat="server" Text="Save and Close" ActionType="Primary" />
                    
                    <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="btn btn-default" CausesValidation="false"
                        OnClientClick="closeWindow(); return false;" />
                </div>
            </div>

        </asp:Panel>
    </telerik:RadAjaxPanel>

</asp:Content>

