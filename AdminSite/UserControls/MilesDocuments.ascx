<%@ Control Language="VB" AutoEventWireup="false" CodeFile="MilesDocuments.ascx.vb" Inherits="UserControls_MilesDocuments" ClientIDMode="AutoID" %>

<telerik:RadAjaxPanel ID="pnlAjax" EnableAJAX="True" runat="server" RenderMode="Inline">

    <asp:Panel ID="pnlUploadDocument" ActionType="Primary" runat="server">
        <span class="help-block">Select files to upload</span>
        <telerik:RadAsyncUpload ID="documentUploader" runat="server"
            MultipleFileSelection="Automatic" UploadedFilesRendering="AboveFileInput" PostbackTriggers="btnUpload"
            OnClientFileUploadFailed="onUploadFailed" OnClientFileUploaded="onFileUploaded" OnClientFileSelected="onFileSelected" RenderMode="Lightweight"
            AllowedFileExtensions=".doc,.docx,.xls,.xlsx,.ppt,.pptx,.pdf,.tif,.jpeg,.jpg,.gif,.bmp,.txt,.csv,.png,.msg,.wav,.mp3,.mp4,.zip" />
        <miles:MilesButton ID="btnUpload" runat="server" Text="Upload" CssClass="Buttons" />
        <br />
        <em>
            <asp:Label ID="lblFileExtensions" CssClass="text-muted" runat="server" /></em><br />
        <em>
            <asp:Label ID="lblMaxFileSize" CssClass="text-muted" runat="server" /></em>

    </asp:Panel>

    <miles:MilesCardList ID="clDocuments" AutoBind="true" runat="server" AllowPaging="true" ShowCommandRow="false" DataKeyNames="DocumentID" >
        <CardTemplate HeaderLinkType="LinkButton">

            <CardHeaderTemplate>
                <h4><%# Eval("DocumentName")%> </h4>
            </CardHeaderTemplate>
            <CardItemTemplate>
                <div class="row">
                    <div class="col-xs-4">
                        <asp:HyperLink ID="hypimgThumbNail" Target="_blank" runat="server" Enabled="false">
                            <asp:Image ID="imgDoc" CssClass="miles-documentsImageIcon" runat="server" />
                        </asp:HyperLink>
                    </div>

                    <div class="col-xs-8">
                        <asp:HiddenField ID="hdFilePath" Value='<%# Eval("FilePath")%>' runat="server" />
                        <asp:HiddenField ID="hdMimeType" Value='<%# Eval("MimeType")%>' runat="server" />
                        <asp:HiddenField ID="hdDocumentName" Value='<%# Eval("DocumentName")%>' runat="server" />

                        <em>
                            <div class="text-muted">Uploaded By   <%# Eval("UserFullName")%> On:<%# ToFormattedShortDateString(Eval("DateCreated"))%></div>
                        </em>
                    </div>
                </div>
            </CardItemTemplate>

            <CardActionTemplate>
                <asp:ImageButton ID="btnDownload" runat="server" ToolTip="Download" OnClick="btnDownload_Click"
                    CommandName="DownloadFile" ImageUrl="/images/24x24/arrow_download.png" />
                <asp:ImageButton ID="btnDelete" runat="server" CommandName="DeleteFile" ToolTip="Delete"
                    OnClientClick="javascript: confirm('Are you sure you want to delete this file?');" ImageUrl="/images/24x24/trash_bin.png" />

            </CardActionTemplate>
        </CardTemplate>
    </miles:MilesCardList>
</telerik:RadAjaxPanel>
