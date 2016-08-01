Imports Telerik.Web.UI
Imports System.Web.Services
Imports System.Data

Partial Class ShoppingCarts_ShoppingCarts
    Inherits BasePage

    Private Sub BindGrid()
        clCarts.DataSource = Nothing
        clCarts.AutoBind = True
        clCarts.Rebind()
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            BindGrid()
        End If
    End Sub

    Protected Sub clCarts_ItemDataBound(sender As Object, e As RadListViewItemEventArgs) Handles clCarts.ItemDataBound
        Dim txtQty As RadNumericTextBox = e.Item.FindControl("txtQty")
        Dim lblCartID As Label = e.Item.FindControl("lblCartID")
        Dim lblExtendedPrice As Label = e.Item.FindControl("lblExtendedPrice")
        Dim obj = CartsManager.GetById(lblCartID.Text)
        Dim pro = ProductManager.GetById(obj.ProductID)

        txtQty.Text = obj.Qty

        'Total price
        lblExtendedPrice.Text = FormatCurrency((obj.Qty * pro.Price), 2)
    End Sub


    Protected Sub clCarts_NeedDataSource(sender As Object, e As RadListViewNeedDataSourceEventArgs) Handles clCarts.NeedDataSource
        Dim objList = CartsManager.GetList(PageIndex:=clCarts.CurrentPageIndex,
                                                      PageSize:=clCarts.PageSize)

        clCarts.DataSource = objList

        If objList.Any Then
            clCarts.VirtualItemCount = objList.FirstOrDefault.TotalCount
        End If
    End Sub

    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        For Each i In clCarts.Items
            Dim txtQty As RadNumericTextBox = i.FindControl("txtQty")
            Dim lblCartID As Label = i.FindControl("lblCartID")
            Dim lblExtendedPrice As Label = i.FindControl("lblExtendedPrice")
            Dim obj = CartsManager.GetById(lblCartID.Text)
            Dim pro = ProductManager.GetById(obj.ProductID)

            obj.Qty = txtQty.Text
            lblExtendedPrice.Text = FormatCurrency((obj.Qty * pro.Price), 2)
            CartsManager.Save(obj)
        Next
        JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="Update", message:="Shop cart update successfully!")
    End Sub


    Protected Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        If clCarts.SelectedCardValues.Count = 0 Then
            JGrowl.ShowMessage(JGrowlMessageType.Alert, objectName:="Record", message:="No product is selected")
        Else
            For Each i As SelectedCard In clCarts.SelectedCardValues
                Dim scId As Integer = i.Value
                Using ctx As New DataLibrary.ModelEntities
                    Dim deletePro = (From sc In ctx.ShoppingCarts
                                     Where sc.CartID = scId
                                     Select sc).SingleOrDefault()

                    ctx.ShoppingCarts.Remove(deletePro)
                    ctx.SaveChanges()
                End Using
            Next

            JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="Delete", message:="Products delete successfully!")
            BindGrid()
        End If

    End Sub

    Protected Sub btnCheck_Click(sender As Object, e As EventArgs) Handles btnCheck.Click
        Response.Redirect("ShoppingCheckout.aspx")
    End Sub
End Class
