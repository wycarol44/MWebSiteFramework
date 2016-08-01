Imports System.IO

Partial Class CommonDialogs_DialogUploadImage
    Inherits BasePage

#Region "Properties"

    Private Property OriginalFileName As String
        Get
            Return ViewState("OriginalFileName")
        End Get
        Set(value As String)
            ViewState("OriginalFileName") = value
        End Set
    End Property

    Public Property ObjectID As Integer
        Get
            Dim v As Object = ViewState("ObjectID")
            If v Is Nothing Then
                v = ToInteger(Request("ObjectID"))
                ViewState("ObjectID") = v
            End If

            Return v
        End Get
        Set(value As Integer)
            ViewState("ObjectID") = value
        End Set
    End Property

    Public Property KeyID As Integer
        Get
            Dim v As Object = ViewState("KeyID")
            If v Is Nothing Then
                v = ToInteger(Request("KeyID"))
                ViewState("KeyID") = v
            End If

            Return v
        End Get
        Set(value As Integer)
            ViewState("KeyID") = value
        End Set
    End Property

#End Region

#Region "Page Functions"

    Protected Sub CommonDialogs_DialogUploadImage_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then

        End If
    End Sub

#End Region

#Region "Methods"

    Private Function Save() As Boolean
        Dim imgBytes = milescrop.Crop()
        If imgBytes IsNot Nothing Then
            Using ms As New MemoryStream(imgBytes)
                Dim img = System.Drawing.Image.FromStream(ms)

                'get filenames
                Dim croppedFile As New TempFileInfo(AppSettings.TempDocumentFolder, "cropped-" + OriginalFileName)
                Dim thumbFile As New TempFileInfo(AppSettings.TempDocumentFolder, "thumb-" + OriginalFileName)

                'save cropped image
                img.Save(croppedFile.FilePath)

                'generate thumb image
                Dim thumbBytes = ResizeImage(ms, 200)
                Using fs = File.Create(thumbFile.FilePath)
                    fs.Write(thumbBytes, 0, thumbBytes.Length)
                End Using

                'delete original temp file
                File.Delete(milescrop.ImagePath)

                If KeyID <> 0 Then

                    'save the picture
                    Dim p = PictureManager.GetByObjectAndKey(ObjectID, KeyID)

                    p.ObjectID = ObjectID
                    p.KeyID = KeyID

                    'check to see if the thumb path has changed
                    If p.PictureID > 0 Then

                        'delete the current thumbnail
                        File.Delete(Path.Combine(AppSettings.PicturesFolder, p.PicturePath))
                        File.Delete(Path.Combine(AppSettings.PicturesFolder, p.ThumbnailPath))

                        'move the temp files to the picture folder
                        MoveFiles(croppedFile.FileName, thumbFile.FileName)

                    ElseIf p.PictureID = 0 Then
                        'move the temp files to the picture folder
                        MoveFiles(croppedFile.FileName, thumbFile.FileName)
                    End If

                    'set the names
                    p.PicturePath = croppedFile.FileName
                    p.ThumbnailPath = thumbFile.FileName

                    PictureManager.Save(p)
                    JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="Picture", useParent:=True, isDelayed:=True)

                End If

                'return to the parent page, the paths of the new images
                CloseDialogWindow(String.Format("{0}|{1}", croppedFile.FileName, thumbFile.FileName))
            End Using

            milescrop.Visible = False
        Else
            'nothing to do
            CloseDialogWindow()
        End If

        Return True
    End Function

    Private Sub MoveFiles(CroppedImageName As String, ThumbImageName As String)
        'move the files
        File.Move(Path.Combine(AppSettings.TempDocumentFolder, CroppedImageName),
                  Path.Combine(AppSettings.PicturesFolder, CroppedImageName))

        File.Move(Path.Combine(AppSettings.TempDocumentFolder, ThumbImageName),
                  Path.Combine(AppSettings.PicturesFolder, ThumbImageName))
    End Sub

#End Region
    
#Region "Events"

    Protected Sub btnSaveClose_Click(sender As Object, e As EventArgs) Handles btnSaveClose.Click
        Save()
    End Sub

    Protected Sub rdUpload_FileUploaded(sender As Object, e As FileUploadedEventArgs) Handles rdUpload.FileUploaded

        'save the new image
        Dim tfi As New TempFileInfo(AppSettings.TempDocumentFolder, e.File.GetName())

        'resize the image
        Dim newImage = ResizeImage(e.File.InputStream, 1200)
        Using fs = File.Create(tfi.FilePath)
            fs.Write(newImage, 0, newImage.Length)
        End Using

        'e.File.SaveAs(Path.Combine(AppSettings.TempDocumentFolder, filename))

        'use the virtual path to set the image crop
        milescrop.ImageUrl = AppSettings.TempDocumentPath + tfi.FileName

        milescrop.Visible = True

        OriginalFileName = tfi.OriginalFileName

    End Sub

#End Region
     
End Class
