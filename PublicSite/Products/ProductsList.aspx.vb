Imports Microsoft.AspNet.FriendlyUrls
Imports System.Web.Routing

Partial Class Products_ProductsList
    Inherits BasePage

    Protected Overrides Sub OnLoad(e As EventArgs)

        'Response.Write(HttpContext.Current.Items("categoryId") & "<br />")
        'Response.Write(HttpContext.Current.Items("productId"))
        MyBase.OnLoad(e)
    End Sub

End Class
