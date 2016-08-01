<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MilesPictureUpload.ascx.vb" Inherits="UserControls_MilesPictureUpload" %>

<script>
    function uploadWindow(objId, keyId, callback) {
        openWindow("/CommonDialogs/DialogUploadImage.aspx?ObjectID=" + objId + "&KeyID=" + keyId , "Upload Image", WINDOW_MEDIUM, pictureUploadClosed);
    }

    function pictureUploadClosed(sender, args) {

        var arg = args.get_argument();
        if (arg) {
            var clientState = $get('<%=ClientState.ClientID%>');

            clientState.value = arg;

            //trigger change event
            __doPostBack(clientState.name, '');
        }

    }
</script>



<asp:Panel ID="pnlImage" runat="server" >
    <asp:HiddenField ID="ClientState" runat="server" />

    <asp:HyperLink ID="lnkPicture" runat="server" Target="_blank" CssClass="thumbnail" >
        <asp:Image ID="imgPicture" runat="server" Width="100%" ImageUrl="~/Images/NoImage.png" />
    </asp:HyperLink>
    <br />

    <asp:HyperLink ID="lnkUpload" runat="server" NavigateUrl="#">
        <span class="glyphicon glyphicon-cloud-upload"></span> Upload Image
    </asp:HyperLink>

    <asp:LinkButton ID="lnkDelete" runat="server" ToolTip="Remove Image" CausesValidation="false"
        OnClientClick="return confirm('Are you sure you want to remove this image?');">
        <span class="close">&times;</span>
    </asp:LinkButton>

</asp:Panel>

