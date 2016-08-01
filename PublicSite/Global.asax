<%@ Application Language="VB" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="System.Web.Optimization" %>

<script RunAt="server">

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application startup
        AddHandler SiteMap.SiteMapResolve, AddressOf Me.ExpandSiteMap
        BundleTable.Bundles.Add(New ScriptBundle("~/scripts/global/js").Include("~/Scripts/Global/jquery-1.10.2.min.js",
                                                                                "~/Scripts/Global/jquery.easing.1.3.js",
                                                                                "~/Scripts/Global/jquery.jgrowl.min.js",
                                                                                "~/Scripts/Global/jquery.Jcrop.min.js",
                                                                                "~/Scripts/Global/highlighter.js",
                                                                                "~/Scripts/Global/bootstrap.min.js",
                                                                                "~/Scripts/Global/enquire.min.js",
                                                                                "~/Scripts/Global/spin.min.js",
                                                                                "~/Scripts/Global/global.js"
                                                                                ))
        BundleTable.Bundles.Add(New ScriptBundle("~/scripts/main/js").IncludeDirectory("~/Scripts/Main", "*.js"))
        BundleTable.Bundles.Add(New StyleBundle("~/styles/css/css").IncludeDirectory("~/styles/css", "*.css"))
        BundleTable.Bundles.Add(New StyleBundle("~/styles/css/system/css").IncludeDirectory("~/styles/css/system", "*.css"))
        BundleTable.EnableOptimizations = True
        
        'friendly URLS
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        
    End Sub
    
    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
    End Sub
        
    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when an unhandled error occurs
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a session ends. 
        ' Note: The Session_End event is raised only when the sessionstate mode
        ' is set to InProc in the Web.config file. If session mode is set to StateServer 
        ' or SQLServer, the event is not raised.
    End Sub
       
    Private Function ExpandSiteMap(ByVal sender As Object, ByVal e As SiteMapResolveEventArgs) As SiteMapNode
        If SiteMap.CurrentNode IsNot Nothing Then
            Dim QString As String = ""
            Dim CurrentQString = e.Context.Request.QueryString.Keys
            For Each key In CurrentQString
                If key Is Nothing Then Continue For
                
                If key.ToString.ToLower <> "radurid" Then
                    If QString = "" Then
                        QString = "?" & key.ToString & "=" & e.Context.Request.QueryString(key)
                    Else
                        QString = QString & "&" & key.ToString & "=" & e.Context.Request.QueryString(key)
                    End If
                End If
            Next

            Dim currentNode As SiteMapNode = SiteMap.CurrentNode.Clone(True)
            Dim tempnode As SiteMapNode = currentNode
            
            If tempnode.ParentNode IsNot Nothing AndAlso Not String.IsNullOrEmpty(tempnode("shared")) Then
                If Convert.ToBoolean(tempnode("shared")) = True AndAlso HttpContext.Current.Request.UrlReferrer IsNot Nothing Then
                    tempnode.ParentNode.Url = HttpContext.Current.Request.UrlReferrer.GetLeftPart(UriPartial.Path)
                    Dim ReferrerQString = HttpUtility.ParseQueryString(HttpContext.Current.Request.UrlReferrer.Query)
                    For Each key In ReferrerQString
                        If Not QString.Contains(key.ToString & "=") Then
                            If QString = "" Then
                                QString = "?" & key.ToString & "=" & ReferrerQString(key.ToString)
                            Else
                                QString = QString & "&" & key.ToString & "=" & ReferrerQString(key.ToString)
                            End If
                        End If
                    Next
                    tempnode.ParentNode.Title = GetSiteMapNodeTitle(HttpContext.Current.Request.UrlReferrer.ToString)
                End If
            End If
            
            While tempnode.ParentNode IsNot Nothing
                'temp node is now parent
                tempnode = tempnode.ParentNode
                'set url
                If Not String.IsNullOrWhiteSpace(tempnode.Url) Then
                    tempnode.Url = tempnode.Url & QString
                End If
            End While
            
           
            Return currentNode
        Else
            
            'Dim path = e.Context.Request.Url.AbsolutePath
            
            'Dim root = SiteMap.RootNode
            'If root IsNot Nothing Then
            '    'create a sitemap node from the url
            '    Dim node As New SiteMapNode(SiteMap.Provider, "", url:=path, title:="Products List", description:="Products List")
            
            '    root.ChildNodes.Add(node)
                
            '    Return node
            'Else
                
            '    Return Nothing
            
            'End If
            Return nothing
        End If
    End Function

    ''' <summary>
    ''' queries the sitemap and checks if it can find the url
    ''' </summary>
    ''' <param name="ReferrerURL"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetSiteMapNodeTitle(ReferrerURL As String) As String
        Dim retVal As String = "Back"
        Dim xelement1 As XElement = XElement.Load(System.Web.Hosting.HostingEnvironment.MapPath("~/web.sitemap"))
        Dim urlDescList = xelement1.Descendants().Where(Function(element) element.FirstAttribute.Name.LocalName.Contains("url")).[Select](Function(nd) New With { _
            Key .title = nd.Attribute("title").Value, _
            Key .url = nd.Attribute("url").Value _
        })
        For Each v As Object In urlDescList
            If ReferrerURL.Contains(v.url.ToString.Replace("~", "")) Then
                retVal = v.title
                Exit For
            End If
        Next
        Return retVal
    End Function
    
</script>
