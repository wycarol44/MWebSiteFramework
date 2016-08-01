Imports Telerik.Web.UI
Imports System.Web
Imports System.Web.UI
Imports CommonLibrary
Imports System.Web.UI.WebControls

<ParseChildren(True)>
Public Class MilesTabStrip
    Inherits Panel
    Implements INamingContainer

    Public Sub New()
        If TabStripControl Is Nothing Then
            TabStripControl = New RadTabStrip()


        End If

        If DropDownTreeControl Is Nothing Then
            DropDownTreeControl = New RadDropDownTree()

        End If
    End Sub

#Region "Private Properties"
    Private ReadOnly Property CustomQueryParameters As Dictionary(Of String, String)
        Get
            Dim list = ViewState("CustomQueryParameters")
            If list Is Nothing Then
                list = New Dictionary(Of String, String)
                ViewState("CustomQueryParameters") = list
            End If

            Return list

        End Get

    End Property
#End Region

#Region "Public Properties"
    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public Property TabStripControl As RadTabStrip
    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public Property DropDownTreeControl As RadDropDownTree

    Public Property XmlFileName As String
        Get
            Return ViewState("XmlFileName")
        End Get
        Set(value As String)
            ViewState("XmlFileName") = value
        End Set
    End Property

    Public Property Skin As String
        Get
            Return ViewState("Skin")
        End Get
        Set(value As String)
            ViewState("Skin") = value
        End Set
    End Property

    Public Property ShowInvisible() As Boolean
        Get
            If ViewState("ShowInvisibleTab") IsNot Nothing Then
                Return ViewState("ShowInvisibleTab")
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("ShowInvisibleTab") = value
        End Set
    End Property
#End Region

#Region "Overrides"

    Protected Overrides Sub OnInit(e As EventArgs)

        TabStripControl.ID = "TabStripControl"
        TabStripControl.Skin = Skin
        TabStripControl.CausesValidation = False
        TabStripControl.EnableTheming = False
        TabStripControl.CssClass = "miles-tabstrip hidden-sm hidden-xs"

        DropDownTreeControl.ID = "DropDownTreeControl"
        DropDownTreeControl.Skin = Skin
        DropDownTreeControl.EnableTheming = False
        DropDownTreeControl.DefaultMessage = "Go to"
        DropDownTreeControl.Width = New Unit(100, UnitType.Percentage)
        DropDownTreeControl.EnableFiltering = True
        DropDownTreeControl.FilterSettings.Highlight = DropDownTreeHighlight.Matches
        DropDownTreeControl.OnClientEntryAdded = "MilesTabStrip_TreeTabClick"
        DropDownTreeControl.CssClass = "miles-tab-nav hidden-lg hidden-md"

        'add controls to panel
        Me.Controls.Add(TabStripControl)
        Me.Controls.Add(DropDownTreeControl)

        MyBase.OnInit(e)
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)

        LoadTabs()

        MyBase.OnLoad(e)
    End Sub


    Protected Overrides Sub OnPreRender(e As EventArgs)

        Dim milesTabJs = Page.ClientScript.GetWebResourceUrl(Me.GetType(), "MilesControls.MilesTabStrip.js")
        System.Web.UI.ScriptManager.RegisterClientScriptInclude(Me.Page, Me.Page.GetType(), "MilesControls.MilesTabStrip.js", milesTabJs)

        MyBase.OnPreRender(e)
    End Sub

#End Region

#Region "Public Methods"
    ''' <summary>
    ''' Loads an xml file and creates tabs
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadTabs()

        'load the xml document
        Dim xdoc = XDocument.Load(Page.Server.MapPath(XmlFileName))

        'clear the tabs
        Me.TabStripControl.Tabs.Clear()

        'populate the tabs
        Dim t As New List(Of TabList)
        PopulateTabs(Me.TabStripControl.Tabs, xdoc.Root, GetQueryParams(), t, True, 0)
        PopulateTree(t)
    End Sub

    ''' <summary>
    ''' Adds a custom query parameter to the tabs' NavigateUrl property
    ''' </summary>
    ''' <param name="key"></param>
    ''' <param name="value"></param>
    ''' <remarks></remarks>
    Public Sub AddQueryParameter(key As String, value As String)
        If Not CustomQueryParameters.ContainsKey(key) Then
            'does not exist, add it
            CustomQueryParameters.Add(key, value)
        Else
            'it already exists, update it
            CustomQueryParameters(key) = value
        End If

        'reload the tabs
        LoadTabs()
    End Sub

#End Region

#Region "Private Methods"
    Private Sub PopulateTabs(tabCollection As RadTabCollection, tabs As XElement, qs As String, ByRef tList As List(Of TabList), IsRoot As Boolean, ParentID As Integer)
        Dim CurrentNodeID As Integer = 0
        'get the current page
        Dim currentPage As String = Nothing
        'if we have a current node in the site map
        If SiteMap.Enabled AndAlso SiteMap.CurrentNode IsNot Nothing Then
            currentPage = SiteMap.CurrentNode.Url
        End If

        'create a tab for each item in the root
        Dim tbs = From tab In tabs.<tab>
                   Select tab

        'create tab
        For Each tb In tbs
            If IsRoot Then CurrentNodeID += 1

            Dim url = Page.ResolveUrl(tb.@url)

            Dim t As New RadTab




            t.Text = tb.@text

            If t.Text = "Billing" Then
                t.NavigateUrl = url + "?" + If(qs.Contains("AddressTypeID"), Left(qs, qs.IndexOf("&AddressTypeID")), qs) + "&AddressTypeID=2"
            ElseIf t.Text = "Shipping" Then
                t.NavigateUrl = url + "?" + If(qs.Contains("AddressTypeID"), Left(qs, qs.IndexOf("&AddressTypeID")), qs) + "&AddressTypeID=1"
            Else
                t.NavigateUrl = url + "?" + qs.Replace("&AddressTypeID=1", "").Replace("&AddressTypeID=2", "")
            End If



            tabCollection.Add(t)



            If url = currentPage Then
                'select the tab and all the parents
                If t.Text = "Shipping" And qs.Contains("AddressTypeID=1") Then
                    t.Selected = True
                    t.SelectParents()
                ElseIf t.Text = "Billing" And qs.Contains("AddressTypeID=2") Then
                    t.Selected = True
                    t.SelectParents()
                ElseIf t.Text <> "Shipping" And t.Text <> "Billing" Then
                    t.Selected = True
                    t.SelectParents()
                End If
            End If




            'hide the tab if visible attribute is false and showinvisible property is false(default value)
            If Not ShowInvisible Then
                t.Visible = IIf(tb.@Visible Is Nothing, True, Convert.ToBoolean(tb.@Visible))
            Else
                t.Visible = True
            End If

            If t.Visible Then
                Dim treeTab As New TabList
                treeTab.TabID = CurrentNodeID
                treeTab.TabName = t.Text
                treeTab.TabUrl = t.NavigateUrl
                treeTab.ParentID = ParentID
                If t.Selected Then treeTab.Selected = True
                tList.Add(treeTab)
            End If

            'now create any child tabs
            PopulateTabs(t.Tabs, tb, qs, tList, False, CurrentNodeID)

        Next



    End Sub

    Private Sub PopulateTree(tList As List(Of TabList))
        DropDownTreeControl.DataSource = tList
        DropDownTreeControl.DataTextField = "TabName"
        DropDownTreeControl.DataValueField = "TabUrl"
        DropDownTreeControl.DataFieldID = "TabID"
        DropDownTreeControl.DataFieldParentID = "ParentID"
        DropDownTreeControl.DataBind()
        DropDownTreeControl.SelectedValue = TabStripControl.SelectedTab.NavigateUrl
    End Sub

    Private Function GetQueryParams() As String

        'custom parameters take precedence
        Dim customParams = (From cqp In CustomQueryParameters
                            Select String.Format("{0}={1}", cqp.Key, cqp.Value)).ToArray()

        'now get the params from the url where the key does NOT exist in the custom params
        Dim urlParams = (From key As String In Page.Request.QueryString
                         Where Not CustomQueryParameters.ContainsKey(key)
                         Select String.Format("{0}={1}", key, Page.Request.QueryString(key))).ToArray()

        Dim params = urlParams.Concat(customParams)

        'join the params together
        Dim qs = String.Join("&", params)

        Return qs

    End Function


#End Region

    Protected Class TabList
        Property TabID As Integer
        Property TabName As String
        Property TabUrl As String
        Property ParentID As Integer
        Property Selected As Boolean
    End Class
End Class


