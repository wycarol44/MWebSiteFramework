Imports System.IO

Partial Class UserControls_MilesDocuments
    Inherits System.Web.UI.UserControl


#Region "Properties"

    Public Property ObjectID As MilesMetaObjects
        Get
            Return ToInteger(ViewState("ObjectID"))
        End Get
        Set(ByVal value As MilesMetaObjects)
            ViewState("ObjectID") = value
        End Set
    End Property

    Public Property KeyID As Integer
        Get
            Return ToInteger(ViewState("KeyID"))
        End Get
        Set(ByVal value As Integer)
            ViewState("KeyID") = value
        End Set
    End Property

    Public Property MaxFileSize As Integer
        Get
            If documentUploader.MaxFileSize = 0 Then
                Return AppSettings.DocumentsMaxFileSize
            Else
                Return documentUploader.MaxFileSize
            End If

        End Get
        Set(value As Integer)
            documentUploader.MaxFileSize = value
        End Set
    End Property

#End Region

#Region "Page Events"

    Protected Sub UserControls_MilesDocuments_Load(sender As Object, e As EventArgs) Handles Me.Load
        AddClientSideScript()

        If Not Page.IsPostBack Then
            RadUploadControlSetting()
        End If
    End Sub

#End Region

#Region "Document Upload Control"

    Public Sub AddClientSideScript()

        Dim js = <script>
                    var uploadsInProgress = 0;

                    function onFileSelected(sender, args) {
                        uploadsInProgress++;

                    }

                    function onFileUploaded(sender, args) {
                        decrementUploadsInProgress();

                    }

                    function onUploadFailed(sender, args) {
                        decrementUploadsInProgress();
                    }

                    function decrementUploadsInProgress() {
                        uploadsInProgress--;
                    }

                    function  realPostBack(eventTarget, eventArgument) {
                        $find("<%= pnlAjax.ClientID %>").__doPostBack(eventTarget, eventArgument);
                    }


                 </script>

        'register the scripts with the script manager
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "MilesDocumentsScriptKey", DecodeJS(js), True)

    End Sub

    Private Sub RadUploadControlSetting()

        Me.lblFileExtensions.Text = "Allowed File Extensions: "
        Me.lblFileExtensions.Text += String.Join(", ", documentUploader.AllowedFileExtensions)

        Me.lblMaxFileSize.Text = "Max File Size: " & (MaxFileSize / 1024 / 1024).ToString & " MB"
        DocumentUploadFolderSettings()

    End Sub

    Private Sub DocumentUploadFolderSettings()

        Dim TempUploadedDocumentsFolder As String = AppSettings.TempDocumentFolder
        Dim UploadedDocumentsFolder As String = AppSettings.UploadedDocumentsFolder
        If (Not System.IO.Directory.Exists(TempUploadedDocumentsFolder)) Then
            System.IO.Directory.CreateDirectory(TempUploadedDocumentsFolder)
        End If

        If (Not System.IO.Directory.Exists(UploadedDocumentsFolder)) Then
            System.IO.Directory.CreateDirectory(UploadedDocumentsFolder)
        End If

        documentUploader.TemporaryFolder = TempUploadedDocumentsFolder
        documentUploader.TargetFolder = UploadedDocumentsFolder

    End Sub

    Protected Sub documentUploader_FileUploaded(sender As Object, e As FileUploadedEventArgs) Handles documentUploader.FileUploaded

        Dim tempFileInfo As New TempFileInfo(documentUploader.TargetFolder, e.File.FileName)

        Dim documentFileName As String = System.IO.Path.Combine(documentUploader.TargetFolder, tempFileInfo.FileName)

        e.File.SaveAs(documentFileName)
        Dim OriginalFileName As String = e.File.GetName()
        Dim FileName As String = e.File.GetNameWithoutExtension()

        SaveUploadedFiles(OriginalFileName, tempFileInfo.FileName, e.File.ContentType)

    End Sub


    Protected Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click

        clDocuments.Rebind()

    End Sub

#End Region

#Region "Methods"

    Private Sub SaveUploadedFiles(ByVal OriginalFileName As String, ByVal FilePath As String, ByVal MimeType As String)
        Dim d As New DataLibrary.Document
        d.ObjectID = ObjectID
        d.KeyID = KeyID
        d.DocumentName = OriginalFileName
        d.MimeType = MimeType
        d.FilePath = FilePath

        DocumentManager.Save(d)

    End Sub

#End Region

#Region "documents card list"

    Protected Sub clDocuments_ItemClicked(sender As Object, e As RadListViewCommandEventArgs) Handles clDocuments.ItemClicked
        Dim item As RadListViewDataItem = e.ListViewItem

        FlushDocument(item)
     
    End Sub

    Protected Sub clDocuments_ItemCommand(sender As Object, e As RadListViewCommandEventArgs) Handles clDocuments.ItemCommand
        If e.CommandName = "DeleteFile" Then
            Dim item As RadListViewDataItem = e.ListViewItem

            'delete file from database
            Dim d = DocumentManager.GetById(item.GetDataKeyValue("DocumentID"))
            DocumentManager.Delete(d)

            'delete file from physical location
            If d.FilePath <> String.Empty Then
                Try
                    Dim documentFolder As String = AppSettings.UploadedDocumentsFolder
                    If File.Exists(documentFolder & d.FilePath) Then
                        File.Delete(documentFolder & d.FilePath)
                    End If
                Catch ex As Exception

                End Try
            End If

            clDocuments.Rebind()

        End If
    End Sub

    Protected Sub clDocuments_ItemDataBound(sender As Object, e As RadListViewItemEventArgs) Handles clDocuments.ItemDataBound
        If TypeOf e.Item Is RadListViewDataItem Then

            Dim btnDownload As ImageButton = e.Item.FindControl("btnDownload")

            Dim lnkHeaderLink As LinkButton = e.Item.FindControl("HeaderLink")

            lnkHeaderLink.Attributes.Add("onclick", String.Format("realPostBack('{0}', ''); return false;", lnkHeaderLink.UniqueID))
            btnDownload.Attributes.Add("onclick", String.Format("realPostBack('{0}', ''); return false;", btnDownload.UniqueID))

            Dim hdFilePath As HiddenField = e.Item.FindControl("hdFilePath")
            Dim hdMimeType As HiddenField = e.Item.FindControl("hdMimeType")
            Dim imgDoc As Image = e.Item.FindControl("imgDoc")

            'If MimeType is image then show actual picture else show icon
            If hdMimeType.Value.Contains("image/") Then

                Dim documentFolder As String = AppSettings.UploadedDocumentsPath

                imgDoc.ImageUrl = Path.Combine(documentFolder, hdFilePath.Value)

                'enable the hyperlink and assign navigateurl if it is image so that image can be viewed in new window
                Dim hypimgThumbNail As HyperLink = e.Item.FindControl("hypimgThumbNail")
                If hypimgThumbNail IsNot Nothing Then
                    hypimgThumbNail.Enabled = True
                    hypimgThumbNail.NavigateUrl = Path.Combine(documentFolder, hdFilePath.Value)
                End If
            Else
                imgDoc.ImageUrl = Functions.GetDocIcon64(Path.GetExtension(hdFilePath.Value))
            End If
        End If
    End Sub

    Protected Sub clDocuments_NeedDataSource(sender As Object, e As RadListViewNeedDataSourceEventArgs) Handles clDocuments.NeedDataSource

        Dim dl = DocumentManager.GetList(CInt(ObjectID), KeyID)
        clDocuments.DataSource = dl

        If dl.Any Then
            clDocuments.Visible = True
        Else
            '  clDocuments.Visible = False
        End If


    End Sub

    Protected Sub btnDownload_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim item As RadListViewDataItem = DirectCast(DirectCast(sender, ImageButton).NamingContainer, RadListViewDataItem)
        FlushDocument(item)

    End Sub

    Private Sub FlushDocument(item As RadListViewDataItem)

        'Flush File
        Dim hdFilePath As HiddenField = item.FindControl("hdFilePath")
        Dim hdDocumentName As HiddenField = item.FindControl("hdDocumentName")


        Dim FilePath As String = hdFilePath.Value
        If Not FilePath = String.Empty Then
            Dim documentFolder As String = AppSettings.UploadedDocumentsFolder
            Functions.FlushFile(hdDocumentName.Value, documentFolder & hdFilePath.Value)

        End If

    End Sub

#End Region

 
End Class
