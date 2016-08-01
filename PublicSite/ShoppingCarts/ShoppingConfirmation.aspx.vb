
Partial Class ShoppingCarts_ShoppingConfirmation
    Inherits BasePage



    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim InfoArray As ArrayList = CType(Session("InfoArray"), ArrayList)
        lblFName2.Text = InfoArray(0)
        lblLName2.Text = InfoArray(1)
        lblAddress12.Text = InfoArray(2)
        lblAddress22.Text = InfoArray(3)
        lblCity2.Text = InfoArray(4)
        lblState2.Text = InfoArray(5)
        lblZip2.Text = InfoArray(6)
        lblOrderID2.Text = InfoArray(7)
    End Sub
End Class
