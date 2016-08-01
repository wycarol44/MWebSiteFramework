Imports System.ComponentModel
Imports CommonLibrary
Imports BusinessLibrary
Imports Telerik.Web.UI
Imports System.Data
Imports System.IO

''' <summary>
''' Author: Pashupati Shrestha
''' Control: Async Upload . It includes async uploader and an attachment grid to show all the uploaded files.
''' These controls can be controled by various properties listed on Properties section
''' Upload can be handled from parent page control or by using upload button of this control. This button is 
''' turned on or off by the property: UseParentPageButtonsForSave . If we want to use parent page buttons for save
''' then we have to set the property : PostbackTriggers.
''' 
''' </summary>
''' <remarks></remarks>
Partial Class UserControls_MilesAsyncUpload
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

    ''' <summary>
    ''' this property holds uploaded files on viewstate until they are  saved in database
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AttachmentList As List(Of DataLibrary.Attachment)
        Get
            If ViewState("AttachmentList") Is Nothing Then
                ViewState("AttachmentList") = New List(Of DataLibrary.Attachment)
            End If
            Return ViewState("AttachmentList")
        End Get
        Set(value As List(Of DataLibrary.Attachment))
            ViewState("AttachmentList") = value
        End Set
    End Property

    ''' <summary>
    ''' Type of the attachment. Default value is regular attachment
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AttachmentType As MilesMetaTypeItem
        Get
            If IsNothing(ViewState("AttachmentType")) Then
                Return MilesMetaTypeItem.RegularAttachment
            Else
                Return ToInteger(ViewState("AttachmentType"))
            End If

        End Get
        Set(ByVal value As MilesMetaTypeItem)
            ViewState("AttachmentType") = value
        End Set
    End Property


    Public WriteOnly Property AllowedFileExtensions As String

        Set(ByVal value As String)

            AsyncUploader.AllowedFileExtensions = value.Split(",")

        End Set
    End Property

    ''' <summary>
    ''' Maximum file size that can be uploaded in bytes
    ''' </summary>
    ''' <value></value>
    ''' <remarks></remarks>
    Public WriteOnly Property MaxFileSize As Decimal
        Set(ByVal value As Decimal)
            AsyncUploader.MaxFileSize = value
        End Set
    End Property

    Public Property AsyncUploaderVisible As Boolean
        Get
            Return pnlAsyncUpload.Visible
        End Get
        Set(ByVal value As Boolean)
            pnlAsyncUpload.Visible = value
        End Set
    End Property

    Public ReadOnly Property RadAsyncUploader As Telerik.Web.UI.RadAsyncUpload
        Get
            Return AsyncUploader
        End Get
    End Property

    Public Property AttachmentsListVisible As Boolean
        Get
            Return pnlAttachmentsList.Visible
        End Get

        Set(ByVal value As Boolean)
            pnlAttachmentsList.Visible = value

        End Set
    End Property

    Public WriteOnly Property AttachmentListTitle As String

        Set(ByVal value As String)
            lblAttachmentListTitle.Text = value
        End Set
    End Property

    Public Property AllowDelete As Boolean
        Get
            If IsNothing(ViewState("AllowDelete")) Then
                Return False
            Else
                Return ViewState("AllowDelete")
            End If
        End Get
        Set(value As Boolean)
            ViewState("AllowDelete") = value

        End Set
    End Property

    Public WriteOnly Property PostbackTriggers As String
        Set(ByVal value As String)
            Me.RadAsyncUploader.PostbackTriggers = value.Split(",")
        End Set
    End Property

    Public Property UseParentPageButtonsForSave As Boolean
        Get
            If IsNothing(ViewState("UseParentPageButtonsForSave")) Then
                Return False
            Else
                Return ViewState("UseParentPageButtonsForSave")
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("UseParentPageButtonsForSave") = value

            If value = True Then
                pnlUploadButton.Visible = False
            End If

        End Set
    End Property

    Public WriteOnly Property UploaderTitle As String
        Set(value As String)
            lblUploaderTitle.Text = value
        End Set
    End Property

#End Region

#Region "Page Events"

    Protected Sub AsyncUploader_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles AsyncUploader.Load

        AddClientSideScript()

        If Not Page.IsPostBack Then
            LoadAsyncUploadControl()
        End If

    End Sub

#End Region

#Region "Methods"

    Public Sub AddClientSideScript()

        'the reason we are creating function names dynamically is so that if same control is being used multiple times in same page then it points to the right one
        'since we are naming function name with Control.ClientID
        Dim realPostBackFunctionName As String = Me.ClientID & "realPostBack"
        Dim scriptID As String = Me.ClientID & "ScriptKey"

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

                    function <%= realPostBackFunctionName %>(eventTarget, eventArgument) {
                        $find("<%= pnlAjax.ClientID %>").__doPostBack(eventTarget, eventArgument);
                    }

                 </script>

        'register the scripts with the script manager
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), scriptID, DecodeJS(js), True)

    End Sub

    Public Sub LoadAsyncUploadControl()

        RadUploadControlSetting()

        If KeyID > 0 And AttachmentsListVisible Then

            BindAttachmentsList()

        End If

    End Sub

    ''' <summary>
    ''' Sets up allowed file extensions, max file size and temfolder
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RadUploadControlSetting()

        Me.lblFileExtensions.Text = "("
        For i As Integer = 0 To AsyncUploader.AllowedFileExtensions.Length - 1
            Me.lblFileExtensions.Text = Me.lblFileExtensions.Text & IIf(i = 0, "", ",") & AsyncUploader.AllowedFileExtensions(i)
        Next

        Me.lblFileExtensions.Text = Me.lblFileExtensions.Text + ")"
        Me.lblMaxFileSize.Text = "Max File Size: " & (AsyncUploader.MaxFileSize / 1024 / 1024).ToString & " MB"
        AsyncUploadFolderSettings()

    End Sub

    ''' <summary>
    ''' Define designated target folder, if doesnt exists, create one
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub AsyncUploadFolderSettings()

        Dim UploadedDocumentsFolder As String = AppSettings.UploadedDocumentsFolder

        If (Not System.IO.Directory.Exists(UploadedDocumentsFolder)) Then
            System.IO.Directory.CreateDirectory(UploadedDocumentsFolder)
        End If

        AsyncUploader.TargetFolder = UploadedDocumentsFolder

    End Sub

    ''' <summary>
    ''' Once a postback occurs the RadAsyncUpload fires the OnFileUploaded event for each file.
    ''' Generate unique file name for each of the files that are uploaded. Use that unique file name as FilePath
    ''' Author: Pashupati Shrestha
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub AsyncUpload1_FileUploaded(ByVal sender As Object, ByVal e As Telerik.Web.UI.FileUploadedEventArgs) Handles AsyncUploader.FileUploaded

        Dim tempFileInfo As New TempFileInfo(AsyncUploader.TargetFolder, e.File.FileName)

        Dim documentFileName As String = System.IO.Path.Combine(AsyncUploader.TargetFolder, tempFileInfo.FileName)

        e.File.SaveAs(documentFileName)
        Dim OriginalFileName As String = e.File.GetName()
        Dim FileName As String = e.File.GetNameWithoutExtension()

        If KeyID > 0 Then
            SaveUploadedFiles(OriginalFileName, FileName, tempFileInfo.FileName)
        Else
            SaveToViewState(OriginalFileName, FileName, tempFileInfo.FileName)
        End If

    End Sub

    Private Sub SaveToViewState(ByVal OriginalFileName As String, ByVal FileName As String, ByVal FilePath As String)

        Dim na As New DataLibrary.Attachment
        na.ObjectID = ObjectID
        na.KeyID = KeyID
        na.AttachmentTypeID = AttachmentType
        na.OriginalFileName = OriginalFileName
        na.FileName = FileName
        na.FilePath = FilePath

        AttachmentList.Add(na)

    End Sub

    ''' <summary>
    ''' Save Attachment in DB:
    ''' OriginalFileName is name of the file, FileName is OriginalFileName without ext and File path is the random file name created for the file
    ''' AUTHOR: PASHUPATI SHRESTHA
    ''' </summary>
    ''' <param name="FileName"></param>
    ''' <param name="FilePath"></param>
    ''' <remarks></remarks>
    Private Sub SaveUploadedFiles(ByVal OriginalFileName As String, ByVal FileName As String, ByVal FilePath As String)
        Dim na As New DataLibrary.Attachment
        na.ObjectID = ObjectID
        na.KeyID = KeyID
        na.AttachmentTypeID = AttachmentType
        na.OriginalFileName = OriginalFileName
        na.FileName = FileName
        na.FilePath = FilePath

        AttachmentManager.Save(na)

    End Sub


    ''' <summary>
    ''' Call this method from Parent Page if we are using parent page button for save
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub UploadFiles()

        For Each a In AttachmentList
            a.KeyID = KeyID
            AttachmentManager.Save(a)
        Next

        If AttachmentsListVisible Then
            BindAttachmentsList()
        End If

    End Sub

    Private Sub DeleteFileFromPhysicalLocation(filepath As String)
        Dim documentFolder As String = AppSettings.UploadedDocumentsFolder
        If File.Exists(documentFolder & filepath) Then
            File.Delete(documentFolder & filepath)
        End If
    End Sub

#End Region

#Region "Attachments List"

    Public Sub BindAttachmentsList()

        If AttachmentsListVisible Then
            pnlAttachmentsList.Visible = True
        Else
            pnlAttachmentsList.Visible = False
        End If

        clAttachments.DataSource = Nothing
        clAttachments.AutoBind = True
        clAttachments.Rebind()
    End Sub

    Protected Sub clAttachments_ItemCommand(sender As Object, e As RadListViewCommandEventArgs) Handles clAttachments.ItemCommand
        If e.CommandName = "DeleteFile" Then
            Dim item As RadListViewDataItem = e.ListViewItem
            Dim AttachmentID As Integer = item.GetDataKeyValue("AttachmentID")

            If AttachmentID = 0 Then

                Dim hdFilePath As HiddenField = item.FindControl("hdFilePath")
                AttachmentList.Remove(AttachmentList.Where(Function(x) x.FilePath = hdFilePath.Value).FirstOrDefault)
                DeleteFileFromPhysicalLocation(hdFilePath.Value)
                BindAttachmentsList()

            Else
                Dim hdFilePath As HiddenField = item.FindControl("hdFilePath")
                AttachmentManager.Delete(AttachmentID)
                DeleteFileFromPhysicalLocation(hdFilePath.Value)
            End If

            BindAttachmentsList()

        End If
    End Sub

    Protected Sub clAttachments_ItemDataBound(sender As Object, e As RadListViewItemEventArgs) Handles clAttachments.ItemDataBound
        If TypeOf e.Item Is RadListViewDataItem Then

            Dim item As RadListViewDataItem = e.Item
            Dim imgDoc As Image = e.Item.FindControl("imgDoc")
            Dim hdFilePath As HiddenField = e.Item.FindControl("hdFilePath")
            Dim sArray As Array = Split(hdFilePath.Value, ".")
            'If MimeType is image then show actual picture else show icon
            If Path.GetExtension(hdFilePath.Value) = ".png" Or Path.GetExtension(hdFilePath.Value) = ".jpg" Or Path.GetExtension(hdFilePath.Value) = ".jpeg" Then

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



            'Since the grid is ajaxified, need to add realpostback script
            Dim realPostBackFunctionName As String = Me.ClientID & "realPostBack"
            Dim lnkFileName As LinkButton = e.Item.FindControl("lnkFileName")
            lnkFileName.Attributes.Add("onclick", String.Format(realPostBackFunctionName & "('{0}', ''); return false;", lnkFileName.UniqueID))

            Dim btnDownload As ImageButton = item.FindControl("btnDownload")
            btnDownload.Attributes.Add("onclick", String.Format(realPostBackFunctionName & "('{0}', ''); return false;", btnDownload.UniqueID))

            Dim lblDocType As Label = DirectCast(e.Item.FindControl("lblDocType"), Label)
            Dim hdAttachmentTypeID As HiddenField = DirectCast(e.Item.FindControl("hdAttachmentTypeID"), HiddenField)

        End If
    End Sub

    Protected Sub clAttachments_NeedDataSource(sender As Object, e As RadListViewNeedDataSourceEventArgs) Handles clAttachments.NeedDataSource
        If KeyID > 0 Then

            Dim aList = AttachmentManager.GetList(CInt(ObjectID), KeyID)
            clAttachments.DataSource = aList
        Else
            'bind it from viewstate
            clAttachments.DataSource = AttachmentList.ToList()
        End If

    End Sub

    Protected Sub btnDownload_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim item As RadListViewDataItem = DirectCast(DirectCast(sender, ImageButton).NamingContainer, RadListViewDataItem)
        FlushDocument(item)

    End Sub
   
    Protected Sub btnFile_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim item As RadListViewDataItem = DirectCast(DirectCast(sender, LinkButton).NamingContainer, RadListViewDataItem)

        FlushDocument(item)

    End Sub

    Private Sub FlushDocument(item As RadListViewDataItem)
        'Flush File
        Dim hdFilePath As HiddenField = item.FindControl("hdFilePath")
        Dim hdOriginalFileName As HiddenField = item.FindControl("hdOriginalFileName")


        Dim FilePath As String = hdFilePath.Value
        If Not FilePath = String.Empty Then
            Dim documentFolder As String = AppSettings.UploadedDocumentsFolder
            Functions.FlushFile(hdOriginalFileName.Value, documentFolder & hdFilePath.Value)

        End If
    End Sub

#End Region

#Region "Buttons"

    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        BindAttachmentsList()
    End Sub

#End Region

End Class
