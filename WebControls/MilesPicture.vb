Imports System.Web.UI.WebControls
Imports BusinessLibrary
Imports CommonLibrary

Public Class MilesPicture
    Inherits Image

#Region "Properties"

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

    Public Property PictureID As Integer
        Get
            Return ToInteger(ViewState("PictureID"))
        End Get
        Set(value As Integer)
            ViewState("PictureID") = value
        End Set
    End Property


    ''' <summary>
    ''' Gets or sets the default image url
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DefaultImageUrl As String
        Get
            Return ViewState("DefaultImageUrl")
        End Get
        Set(value As String)
            ViewState("DefaultImageUrl") = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the thumnail (display) image
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ImageName As String
        Get
            Return ViewState("ImageName")
        End Get
        Set(value As String)
            ViewState("ImageName") = value

            If String.IsNullOrWhiteSpace(value) Then
                MyBase.ImageUrl = DefaultImageUrl
            Else
                ImageUrl = AppSettings.PicturesPath + value
            End If
        End Set
    End Property


    ''' <summary>
    ''' Gets or sets the image for the full picture
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FullSizeImageName As String
        Get
            Return ViewState("FullSizeImageName")
        End Get
        Set(value As String)
            ViewState("FullSizeImageName") = value
        End Set
    End Property

#End Region

#Region "Overrides"
    Protected Overrides Sub OnInit(e As EventArgs)

        'hide / show image depending on picture value
        Me.ImageUrl = DefaultImageUrl

        MyBase.OnInit(e)
    End Sub


    Protected Overrides Sub OnPreRender(e As EventArgs)

        If Not Page.IsPostBack Then
            LoadPicture()
        End If

        MyBase.OnPreRender(e)
    End Sub

    Public Sub LoadPicture()
        'if there is no imagename look for objectid and keyid
        If String.IsNullOrEmpty(ImageName) Then
            Dim p = PictureManager.GetByObjectAndKey(ObjectID, KeyID)

            'if we have a picture ID, use it to load the images
            If p.PictureID <> 0 Then


                If Not String.IsNullOrWhiteSpace(p.ThumbnailPath) Then
                    Me.ImageUrl = AppSettings.PicturesPath + p.ThumbnailPath
                End If
            Else
                Me.ImageUrl = DefaultImageUrl
            End If
        End If

    End Sub

#End Region


End Class
