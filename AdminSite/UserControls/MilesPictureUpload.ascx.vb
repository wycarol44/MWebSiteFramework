Imports System.IO

Partial Class UserControls_MilesPictureUpload
    Inherits System.Web.UI.UserControl

#Region "Events Handlers"
    Public Event PictureChanged As EventHandler

#End Region

    Public Sub New()

    End Sub

#Region "Properties"


    ''' <summary>
    ''' Gets or sets the filename of the cropped image
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CroppedImageName As String
        Get
            Return ViewState("CroppedImageName")
        End Get
        Protected Set(value As String)
            ViewState("CroppedImageName") = value

            'when the image url is specified, check to see if the cropped image is set
            'if it is, create a navigate url for it
            If Not String.IsNullOrWhiteSpace(value) Then
                lnkPicture.NavigateUrl = GetPicturePath() + value
            Else
                lnkPicture.NavigateUrl = ""
            End If

        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the filename of the thumbnail image
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ThumbImageName As String
        Get
            Return ViewState("ThumbImageName")
        End Get
        Protected Set(value As String)
            ViewState("ThumbImageName") = value

            'when the image url is specified, check to see if the cropped image is set
            'if it is, create a navigate url for it
            If Not String.IsNullOrWhiteSpace(value) Then
                imgPicture.ImageUrl = GetPicturePath() + value
            Else
                imgPicture.ImageUrl = "~/Images/NoImage.png"
            End If
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the width of the image container
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Width As Unit
        Get
            Return pnlImage.Width
        End Get
        Set(value As Unit)
            pnlImage.Width = value
        End Set
    End Property

    Public Property PictureID As Integer
        Get
            Return ToInteger(ViewState("PictureID"))
        End Get
        Set(value As Integer)
            ViewState("PictureID") = value
        End Set
    End Property

    Public Property ObjectID As Integer
        Get
            Return ToInteger(ViewState("ObjectID"))
        End Get
        Set(value As Integer)
            ViewState("ObjectID") = value
        End Set
    End Property

    Public Property KeyID As Integer
        Get
            Return ToInteger(ViewState("KeyID"))
        End Get
        Set(value As Integer)
            ViewState("KeyID") = value
        End Set
    End Property

#End Region

#Region "Overrides"

    Protected Overrides Sub OnLoad(e As EventArgs)
        If Not Page.IsPostBack Then
            LoadPicture()
        End If
        MyBase.OnLoad(e)
    End Sub


    Protected Overrides Sub OnPreRender(e As EventArgs)

        'output js
        Dim onclickJs = <script>
                            uploadWindow('<%= ObjectID %>','<%= KeyID %>'); return false;
                        </script>


        'set link
        lnkUpload.Attributes.Add("onclick", DecodeJS(onclickJs))


        MyBase.OnPreRender(e)
    End Sub

#End Region

#Region "Methods"

    Private Sub LoadPicture()

        If KeyID = 0 Then
            pnlImage.CssClass = "hidden-sm hidden-xs"
        End If

        Dim p = PictureManager.GetByObjectAndKey(ObjectID, KeyID)
        PictureID = p.PictureID
        'if we have a picture ID, use it to load the images
        If p.PictureID <> 0 Then

            CroppedImageName = p.PicturePath
            ThumbImageName = p.ThumbnailPath
        Else
            ThumbImageName = Nothing
        End If

    End Sub

    ''' <summary>
    ''' Gets the picture path
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetPicturePath() As String
        'if temp, use temp folder, otherwise use picture folder
        Return If(KeyID = 0, AppSettings.TempDocumentPath, AppSettings.PicturesPath)

    End Function

    ''' <summary>
    ''' Gets the picture folder
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetPictureFolder() As String
        'if temp, use temp folder, otherwise use picture folder
        Return If(KeyID = 0, AppSettings.TempDocumentFolder, AppSettings.PicturesFolder)

    End Function

    Private Sub MoveFiles()
        'move the files
        File.Move(Path.Combine(AppSettings.TempDocumentFolder, CroppedImageName),
                  Path.Combine(AppSettings.PicturesFolder, CroppedImageName))

        File.Move(Path.Combine(AppSettings.TempDocumentFolder, ThumbImageName),
                  Path.Combine(AppSettings.PicturesFolder, ThumbImageName))
    End Sub


    ''' <summary>
    ''' Saves the images to the pictures folder
    ''' </summary>
    ''' <remarks></remarks>
    Public Function Save() As Integer?
        If String.IsNullOrWhiteSpace(ThumbImageName) Then Return Nothing

        'update the picture
        Dim p = PictureManager.GetById(PictureID)

        p.ObjectID = ObjectID
        p.KeyID = KeyID

        'check to see if the thumb path has changed
        If p.PictureID > 0 AndAlso Not p.ThumbnailPath = ThumbImageName Then
            'delete the current thumbnail
            File.Delete(Path.Combine(AppSettings.PicturesFolder, p.PicturePath))
            File.Delete(Path.Combine(AppSettings.PicturesFolder, p.ThumbnailPath))

            'move the temp files to the picture folder
            MoveFiles()

        ElseIf p.PictureID = 0 Then
            'move the temp files to the picture folder
            MoveFiles()
        End If

        'set the names
        p.PicturePath = CroppedImageName
        p.ThumbnailPath = ThumbImageName

        'refresh the images to use the pictures folder
        CroppedImageName = CroppedImageName
        ThumbImageName = ThumbImageName

        imgPicture.ImageUrl = "~/Images/NoImage.png"

        Return PictureManager.Save(p)

    End Function

#End Region

#Region "Events"

    Protected Sub ClientState_ValueChanged(sender As Object, e As EventArgs) Handles ClientState.ValueChanged
        Dim paths = ClientState.Value.Split("|")
        If paths.Count = 2 Then

            'set the images
            CroppedImageName = paths(0)
            ThumbImageName = paths(1)

        End If
    End Sub

    Protected Sub lnkDelete_Click(sender As Object, e As EventArgs) Handles lnkDelete.Click

        'delete picture
        If PictureID <> 0 Then
            'delete pics and set picid null on the tables
            PictureManager.DeleteByObjectAndKey(ObjectID, KeyID)
            PictureID = Nothing
            'saved automatically, notify user
            JGrowl.ShowMessage(JGrowlMessageType.Notification, "Picture was removed successfully")
        Else
            If Not String.IsNullOrWhiteSpace(ThumbImageName) Then
                'delete the temp images
                File.Delete(Path.Combine(AppSettings.TempDocumentFolder, CroppedImageName))
                File.Delete(Path.Combine(AppSettings.TempDocumentFolder, ThumbImageName))
            End If
        End If

        'nullify images
        CroppedImageName = Nothing
        ThumbImageName = Nothing

    End Sub
#End Region


End Class
