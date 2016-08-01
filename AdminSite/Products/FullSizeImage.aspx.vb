
Partial Class Products_FullSizeImage
    Inherits BasePage

    Public Property PictureID As Integer
        Get
            Dim v As Object = ViewState("PictureID")
            If v Is Nothing Then
                v = ToInteger(Request("PictureID"))
                ViewState("PictureID") = v
            End If
            Return v
        End Get
        Set(value As Integer)
            ViewState("PictureID") = value
        End Set
    End Property

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Using ctx As New DataLibrary.ModelEntities()
            Dim imgURL As String = (From p In ctx.Pictures
                                    Where p.PictureID = PictureID
                                    Select p.PicturePath).SingleOrDefault()

            Image1.ImageUrl = "~/Documents/Pictures/" + imgURL
        End Using
    End Sub

End Class
