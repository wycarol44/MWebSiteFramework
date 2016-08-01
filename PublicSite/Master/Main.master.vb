
Partial Class Master_Main
    Inherits System.Web.UI.MasterPage

#Region "Properties"


#End Region

#Region "Page Events"
    Protected Overrides Sub OnLoad(e As EventArgs)

        If System.Web.SiteMap.CurrentNode IsNot Nothing Then
            'set the page title
            Me.Page.Title = SiteMap.CurrentNode.Title + " | " + AppSettings.ApplicationName
            'set the description
            If SiteMap.CurrentNode.Description IsNot Nothing AndAlso Not String.IsNullOrEmpty(SiteMap.CurrentNode.Description) Then
                metaDescription.Visible = True
                metaDescription.Attributes.Add("content", SiteMap.CurrentNode.Description)
            End If
            'set the keywords
            If SiteMap.CurrentNode("keywords") IsNot Nothing AndAlso Not String.IsNullOrEmpty(SiteMap.CurrentNode("keywords")) Then
                metaKeywords.Visible = True
                metaKeywords.Attributes.Add("content", SiteMap.CurrentNode("keywords"))
            End If
            'set the robots
            If SiteMap.CurrentNode("robots") IsNot Nothing AndAlso Not String.IsNullOrEmpty(SiteMap.CurrentNode("robots")) Then
                metaRobots.Visible = True
                metaRobots.Attributes.Add("content", SiteMap.CurrentNode("robots"))
            End If
            'set the author
            If SiteMap.CurrentNode("author") IsNot Nothing AndAlso Not String.IsNullOrEmpty(SiteMap.CurrentNode("author")) Then
                metaAuthor.Visible = True
                metaAuthor.Attributes.Add("content", SiteMap.CurrentNode("author"))
            End If
            'set the copyright
            If SiteMap.CurrentNode("copyright") IsNot Nothing AndAlso Not String.IsNullOrEmpty(SiteMap.CurrentNode("copyright")) Then
                metaCopyright.Visible = True
                metaCopyright.Attributes.Add("content", SiteMap.CurrentNode("copyright"))
            End If
        Else
            Me.Page.Title = AppSettings.ApplicationName
        End If


        MyBase.OnLoad(e)
    End Sub
#End Region

#Region "Methods"

#End Region

#Region "Events"
   
#End Region


End Class

