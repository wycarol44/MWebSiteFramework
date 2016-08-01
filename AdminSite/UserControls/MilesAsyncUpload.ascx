<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MilesAsyncUpload.ascx.vb" Inherits="UserControls_MilesAsyncUpload" %>

<telerik:RadAjaxPanel ID="pnlAjax" EnableAJAX="true" runat="server">
    <asp:HiddenField ID="hdAttachmentCount" runat="server" />
    <asp:Panel ID="pnlAsyncUpload" runat="server">
        <asp:Label ID="lblUploaderTitle" Text="Select files to Upload" runat="server"></asp:Label>
        <br class="clear" />
        <telerik:RadAsyncUpload OnClientFileUploaded="onFileUploaded" MultipleFileSelection="Automatic"
            UploadedFilesRendering="AboveFileInput" PostbackTriggers="btnUpload" runat="server"
            ID="AsyncUploader" Skin="Silk" OnClientFileUploadFailed="onUploadFailed"
            OnClientFileSelected="onFileSelected" MaxFileSize="4194304" AllowedFileExtensions=".doc,.docx,.xls,.xlsx,.ppt,.pptx,.pdf,.tif,.jpeg,.jpg,.gif,.bmp,.txt,.csv,.png,.msg,.wav,.mp3,.mp4,.zip">
        </telerik:RadAsyncUpload>
        <asp:Panel ID="pnlLabels" runat="server">
            <asp:Label ID="lblFileExtensions" CssClass="text-muted" Font-Size="Smaller" runat="server"></asp:Label><br />
            <asp:Label ID="lblMaxFileSize" CssClass="text-muted" Font-Size="Smaller" runat="server"></asp:Label>
            <br />
        </asp:Panel>
        <asp:Panel ID="pnlUploadButton" runat="server">
            <div class="HseparatorSec">
            </div>
            <miles:MilesButton ID="btnUpload" runat="server" Text="Upload" CssClass="Buttons" />
        </asp:Panel>
    </asp:Panel>
    <asp:Panel ID="pnlAttachmentsList" runat="server">
        <asp:Label ID="lblAttachmentListTitle" runat="server" Text="Attachments List"></asp:Label>

        <miles:MilesCardList ID="clAttachments" runat="server" AllowPaging="false" ShowNoRecordText="false" ShowCommandRow="false" NumberOfColumns="one" CardMinHeight="75px" DataKeyNames="AttachmentID">

            <CardTemplate HeaderLinkType="LinkButton" ActionPosition="Right">

                <CardItemTemplate>
                    <div class="row">

                        <div class="col-xs-4">
                            <asp:HyperLink ID="hypimgThumbNail" Target="_blank" runat="server" Enabled="false">
                                <asp:Image ID="imgDoc" CssClass="miles-documentsImageIcon" runat="server" />
                            </asp:HyperLink>
                        </div>

                        <div class="col-xs-8">

                            <asp:LinkButton ID="lnkFileName" Text='<%# Eval("FileName")%>' OnClick="btnFile_Click" runat="server"></asp:LinkButton></h4>
                                    
                                    <em>
                                        <div id="divUploadedBy" class="text-muted" runat="server">Uploaded By  <%# Eval("User.FullName")%> On:<%# ToFormattedShortDateString(Eval("DateCreated"))%></div>
                                    </em>
                        </div>

                    </div>

                    <asp:HiddenField ID="hdFilePath" Value='<%# Eval("FilePath")%>' runat="server" />
                    <asp:HiddenField ID="hdDocumentName" Value='<%# Eval("FileName")%>' runat="server" />
                    <asp:HiddenField ID="hdOriginalFileName" Value='<%# Eval("OriginalFileName") %>' runat="server" />

                </CardItemTemplate>

                <CardActionTemplate>

                    <asp:ImageButton ID="btnDownload" runat="server" ToolTip="Download" OnClick="btnDownload_Click"
                        CommandName="DownloadFile" ImageUrl="/images/24x24/arrow_download.png" />
                    <asp:ImageButton ID="btnDelete" runat="server" CommandName="DeleteFile" ToolTip="Delete"
                        ImageUrl="/images/24x24/trash_bin.png" />

                </CardActionTemplate>
            </CardTemplate>
        </miles:MilesCardList>
    </asp:Panel>

</telerik:RadAjaxPanel>
