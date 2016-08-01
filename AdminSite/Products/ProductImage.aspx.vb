
Partial Class Products_ProductImage
    Inherits BasePage

#Region "Properties"
    Public Property ProductID As Integer
        Get
            Dim v As Object = ViewState("ProductID")
            If v Is Nothing Then
                v = ToInteger(Request("ProductID"))
                ViewState("ProductID") = v
            End If
            Return v
        End Get
        Set(value As Integer)
            ViewState("ProductID") = value
        End Set
    End Property

#End Region



    Protected Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click


        Using ctx As New DataLibrary.ModelEntities()
            picUpload.ObjectID = 4

            Dim PicID As Integer = picUpload.Save()

            Dim newPic As New DataLibrary.ProductImage

            newPic.ProductID = ProductID
            newPic.ImageName = txtImageName.Text
            newPic.IsPrimary = False
            newPic.PictureID = PicID
            newPic.Archived = False
            ctx.ProductImages.Add(newPic)
            ctx.SaveChanges()
        End Using

        BindData()
    End Sub

    Public Sub BindData()
        Dim Pictures = ProductImageManager.ImgGetList(
         ProductID:=ProductID,
         Archived:=False
         )
        repeater.DataSource = Pictures
        repeater.DataBind()
    End Sub



    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            BindData()
        End If

    End Sub



    Protected Sub repeater_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles repeater.ItemDataBound

        Dim img As Label = e.Item.FindControl("lblPicImgID")
        Dim ProductImageID As Integer = CInt(img.Text)
        Dim myControl2 As Button = e.Item.FindControl("lbtPri")

        'Dim priImg = ProductManager.ImgGetList(
        '    PictureID:=imgID
        '    ).Single()
        Dim priImg As ProductImage_GetList_Result = e.Item.DataItem

        Dim myControl As Label = e.Item.FindControl("lblPrimary")

        If priImg.IsPrimary Then
            myControl.Visible = True
            myControl2.Visible = False
        Else
            myControl.Visible = False
            myControl2.Visible = True
        End If
    End Sub

    Protected Sub repeater_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles repeater.ItemCommand

        Dim img As Label = e.Item.FindControl("lblPicImgID")
        Dim ProductImageID As Integer = CInt(img.Text)
        Dim obj = ProductImageManager.GetById(ProductImageID)

        'Dim deleteImg = ProductImageManager.ImgGetList(PictureID:=obj.PictureID, ProductID:=ProductID).SingleOrDefault
        If e.CommandName = "Delete" Then

            Using ctx As New DataLibrary.ModelEntities()
                Dim deleteImg = (From pi In ctx.ProductImages
                                 Where pi.ProductID = ProductID And pi.PictureID = obj.PictureID
                                 Select pi).ToArray()

                Dim deleteImg2 = (From p In ctx.Pictures
                               Where p.PictureID = obj.PictureID
                               Select p).ToArray()

                For Each deleteImg3 In deleteImg
                    deleteImg3.Archived = True
                Next
                For Each deleteImg4 In deleteImg2
                    System.IO.File.Delete("C:\Webapps\TrainingLLQ\Documents\Pictures\" + deleteImg4.PicturePath)
                    System.IO.File.Delete("C:\Webapps\TrainingLLQ\Documents\Pictures\" + deleteImg4.ThumbnailPath)
                Next
                ctx.SaveChanges()

                Response.Redirect("~/Products/ProductImage.aspx?ProductID=" & ProductID)

            End Using

        ElseIf e.CommandName = "Primary" Then
            Dim myControl As Label = e.Item.FindControl("lblPrimary")
            myControl.Visible = True
            Dim myControl2 As Button = e.Item.FindControl("lbtPri")
            myControl2.Visible = False

            'Set the previous primary to be false
            Using ctx As New DataLibrary.ModelEntities()
                Dim nonPriImg = (From pi In ctx.ProductImages
                                 Where pi.ProductID = ProductID And pi.IsPrimary = True
                                 Select pi).ToArray()

                For Each nonPriImg2 In nonPriImg
                    If nonPriImg2.IsPrimary = True Then
                        nonPriImg2.IsPrimary = False
                    End If
                    ctx.SaveChanges()
                Next
            End Using

            'Set this picture to be primary
            Dim PriImg = ProductImageManager.GetById(ProductImageID)
            PriImg.IsPrimary = True
            ProductImageManager.Save(PriImg)

            BindData()

        End If



    End Sub


End Class




