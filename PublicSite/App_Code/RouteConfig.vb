Option Strict On
Imports System.Collections.Generic
Imports System.Web
Imports System.Web.Routing
Imports Microsoft.AspNet.FriendlyUrls
Imports System.Web.Compilation

Public Module RouteConfig
    Public Sub RegisterRoutes(routes As RouteCollection)
        Dim settings = New FriendlyUrlSettings()
        settings.AutoRedirectMode = RedirectMode.Permanent
        routes.EnableFriendlyUrls(settings)

        routes.Add("test", New Route("ProductsList/{categoryId}/{productId}", New CustomRouteHandler("~/Products/ProductsList.aspx")))

    End Sub
End Module

Public Class CustomRouteHandler
    Implements IRouteHandler

    Public Property VirtualPath As String

    Public Sub New(virtualPath As String)
        Me.VirtualPath = virtualPath
    End Sub

    Public Function GetHttpHandler(requestContext As RequestContext) As IHttpHandler Implements IRouteHandler.GetHttpHandler

        'get url params
        For Each urlParm In requestContext.RouteData.Values
            requestContext.HttpContext.Items(urlParm.Key) = urlParm.Value
        Next

        'serve the page
        Dim page = CType(BuildManager.CreateInstanceFromVirtualPath(VirtualPath, GetType(Page)), IHttpHandler)
        Return page
    End Function
End Class

