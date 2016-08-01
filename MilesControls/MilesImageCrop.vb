Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Web
Imports System.Web.Script.Serialization
Imports System.IO

<ParseChildren(True)>
Public Class MilesImageCrop
    Inherits Panel
    Implements INamingContainer

#Region "Properties"
    ''' <summary>
    ''' Gets or sets the image url of the image to crop
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ImageUrl As String
        Get
            Return CropImage.ImageUrl
        End Get
        Set(value As String)
            CropImage.ImageUrl = value
            PreviewImage.ImageUrl = value

            ImagePath = Page.Server.MapPath(value)
        End Set
    End Property

    ''' <summary>
    ''' Get's or sets the locked aspect ratio of the selection
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AspectRatio As Decimal
        Get
            Return ViewState("AspectRatio")
        End Get
        Set(value As Decimal)
            ViewState("AspectRatio") = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets whether the preview pane is visible
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ShowPreview As Boolean
        Get
            Return PreviewPane.Visible
        End Get
        Set(value As Boolean)
            PreviewPane.Visible = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the server mapped image path
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ImagePath As String
        Get
            Return ViewState("ImagePath")
        End Get
        Protected Set(value As String)
            ViewState("ImagePath") = value
        End Set
    End Property

    'Public Property Width As Unit
    '    Get
    '        Return
    '    End Get
    '    Set(value As Unit)

    '    End Set
    'End Property

#End Region

#Region "Fields"

    Private ImagePane As Panel
    Private PreviewPane As Panel
    Private PreviewContainer As Panel

    Private CropImage As Image
    Private PreviewImage As Image

    Private ClientState As HiddenField

#End Region

    Public Sub New()

        ImagePane = New Panel
        PreviewPane = New Panel
        PreviewContainer = New Panel

        CropImage = New Image
        PreviewImage = New Image

        ClientState = New HiddenField

        AspectRatio = 1

    End Sub

#Region "Overrides"
    Protected Overrides Sub OnInit(e As EventArgs)

        Me.CssClass = "crop-container"

        'panes
        ImagePane.CssClass = "image-pane"
        ImagePane.Width = Unit.Percentage(65)

        PreviewPane.CssClass = "preview-pane"
        PreviewPane.Width = Unit.Percentage(30)


        ClientState.ID = "ClientState"

        'images
        PreviewImage.CssClass = "jcrop-preview"


        PreviewContainer.CssClass = "preview-container"

        If AspectRatio > 0 Then
            'PreviewContainer.Width =
            'PreviewContainer.Height = PreviewContainer.Height.Value / (AspectRatio)
        End If


        'add images to panels
        ImagePane.Controls.Add(CropImage)
        PreviewPane.Controls.Add(PreviewContainer)

        PreviewContainer.Controls.Add(PreviewImage)



        'add panels to parent
        Me.Controls.Add(ImagePane)
        Me.Controls.Add(PreviewPane)


        'add hidden fields to parent
        Me.Controls.Add(ClientState)


        MyBase.OnInit(e)
    End Sub

    Protected Overrides Sub CreateChildControls()



        MyBase.CreateChildControls()
    End Sub

    Protected Overrides Sub OnPreRender(e As EventArgs)

        'output some javascript
        Dim milesImageCropJs = Page.ClientScript.GetWebResourceUrl(Me.GetType(), "MilesControls.MilesImageCrop.js")

        'link javascript
        ScriptManager.RegisterClientScriptInclude(Page, Me.GetType(), "MilesImageCrop", milesImageCropJs)

        'get the css reference
        Dim cssUrl = Page.ClientScript.GetWebResourceUrl(Me.GetType(), "MilesControls.MilesImageCrop.css")

        'include css
        Dim link = "<link rel='stylesheet' text='text/css' href='{0}' />"
        Dim cssInclude As New LiteralControl(String.Format(link, cssUrl))

        Page.Header.Controls.Add(cssInclude)

        'check to see if our image is set, if not, do nothing
        If Not String.IsNullOrWhiteSpace(ImageUrl) Then
            'get some info from the image
            Dim sizeW As Integer
            Dim sizeH As Integer
            Using fs = File.OpenRead(ImagePath),
                origImage As System.Drawing.Image = System.Drawing.Image.FromStream(fs)
                sizeW = origImage.Width
                sizeH = origImage.Height
            End Using

            'use the script manager to startup the cropping
            Dim js = <script>
                     setTimeout(function(){MilesImageCrop_CropImage('<%= Me.ClientID %>', <%= AspectRatio %>, [<%= sizeW %>, <%= sizeH %>]);}, 0);
                 </script>

            'register the script
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "MilesImageCrop", HttpUtility.HtmlDecode(js.Value), True)
        End If

        MyBase.OnPreRender(e)
    End Sub


#End Region

#Region "Methods"


    ''' <summary>
    ''' Takes an image and crops it
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Crop() As Byte()

        'deserialize the client state
        Dim json As New JavaScriptSerializer

        Dim obj = json.DeserializeObject(ClientState.Value)

        If obj Is Nothing Then
            Return Nothing
        End If

        'create rectangle
        Dim rect = New System.Drawing.RectangleF(
            Convert.ToDecimal(obj("cX")),
            Convert.ToDecimal(obj("cY")),
            Convert.ToDecimal(obj("cW")),
            Convert.ToDecimal(obj("cH")))


        'invalid size
        If rect.Width = 0 Or rect.Height = 0 Then
            Return Nothing
        End If
        'if no file, return nothing
        If Not File.Exists(ImagePath) Then
            Return Nothing
        End If

        'get original image and create new image
        Using fs = File.OpenRead(ImagePath),
            origImage As System.Drawing.Image = System.Drawing.Image.FromStream(fs)

            'create a new blank bitmap of the scaled selection
            Using bmp As New System.Drawing.Bitmap(CInt(rect.Width), CInt(rect.Height))

                'set the resolution of the bitmap before we create the graphics object
                bmp.SetResolution(origImage.HorizontalResolution, origImage.VerticalResolution)

                'create a graphics object from our bitmap
                Using Graphic As System.Drawing.Graphics = System.Drawing.Graphics.FromImage(bmp)

                    'set properties of the graphics device
                    Graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias
                    Graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic
                    Graphic.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality

                    'draw the original image on the new image
                    Graphic.DrawImage(
                        origImage,
                        0, 0,
                        rect,
                        System.Drawing.GraphicsUnit.Pixel)

                    'save image as a stream of bytes
                    Using ms As New MemoryStream()
                        bmp.Save(ms, origImage.RawFormat)
                        Return ms.GetBuffer()
                    End Using

                End Using
            End Using
        End Using
    End Function

#End Region

End Class
