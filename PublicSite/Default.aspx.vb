Imports Telerik.Web.UI

Partial Class _Default
    Inherits BasePage

    Protected Sub _Default_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            BindData()
        End If
    End Sub

    Protected Sub Page_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        'RadRotatorSizeConfigurator.ConfigureSize(RadRotator1, 4, PageUtility.SelectedSkin)
    End Sub


    Public Sub BindData()
        Dim Pictures = FeaturedProductsImageManager.GetList(IsFeatured:=True, PicIsPrimary:=True)
        RadRotator.DataSource = Pictures
        RadRotator.DataBind()

        RadListView1.DataSource = Pictures
        RadListView1.DataBind()
    End Sub


    Protected Sub RadListView1_ItemCommand(sender As Object, e As RadListViewCommandEventArgs) Handles RadListView1.ItemCommand
        If e.CommandName = "AddToCart" Then
            Dim ProductID As Label = e.ListViewItem.FindControl("lblProductID")
            Dim proID As Integer = CInt(ProductID.Text)
            Dim SessionID As String = Session.SessionID

            Using ctx As New DataLibrary.ModelEntities
                'Find the profudct in cart
                'if contains
                Dim product = (From sc In ctx.ShoppingCarts
                               Where sc.ProductID = proID
                               Select sc).SingleOrDefault()

                If product Is Nothing Then
                    'Add new
                    Dim obj As New ShoppingCart With {.ProductID = proID, .Qty = 1, .Archived = False, .SessionID = SessionID}
                    ctx.ShoppingCarts.Add(obj)
                    ctx.SaveChanges()
                Else
                    product.Qty += 1
                    ctx.SaveChanges()
                End If


       
            End Using

            JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="Add", message:="This product add to shopping cart successfully!")
        End If
    End Sub
End Class
