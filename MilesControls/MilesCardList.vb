Imports Telerik.Web.UI
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports CommonLibrary
Imports CommonLibrary.Extensions
Imports System.Web.Script.Serialization

<ParseChildren(True), PersistChildren(False)>
Public Class MilesCardList
    Inherits RadListView
    Implements ISearchPanelBoundListControl


#Region "Private Fields"

    'command row items
    Dim AddNewRecordCommandItem As New Panel
    'Dim AllowViewModeSwitchCommandItem As New Panel

    Dim AddNewButton As New Button
    'Dim DetailSwitcher As New RadButton
    Dim DetailSwitcher As New CheckBox

    Dim CardListPanel As New Panel
    Dim SelectedState As New HiddenField
    Dim CheckAll As New CheckBox

    Dim CommandPanel As New Panel
    Dim Pager As New RadDataPager

#End Region

#Region "Events"
    Public Event ToggleArchive As EventHandler(Of RadListViewCommandEventArgs)
    Public Event ItemClicked As EventHandler(Of RadListViewCommandEventArgs)
#End Region

#Region "Enums"
    Public Enum HorizontalColumns
        One = 1
        Two = 2
        Three = 3
        Four = 4
        Six = 6
        Twelve = 12
    End Enum

    Public Enum ColumnBreakPoint
        XS = 1
        SM = 2
        MD = 3
        LG = 4
    End Enum

    'Public Enum ListViewMode
    '    Details = 1
    '    List = 2
    'End Enum
#End Region

#Region "Properties"

    ' ''' <summary>
    ' ''' Allows the user to switch between detail and list templates
    ' ''' </summary>
    ' ''' <value></value>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Property AllowViewModeSwitch As Boolean
    '    Get
    '        Return ViewState("AllowViewModeSwitch")
    '    End Get
    '    Set(value As Boolean)
    '        ViewState("AllowViewModeSwitch") = value

    '        AllowViewModeSwitchCommandItem.Visible = value
    '    End Set
    'End Property


    ''' <summary>
    ''' Gets the selected card values
    ''' </summary>
    ''' <remarks></remarks>
    Private _selectedCardValues As SelectedItemCollection
    Public ReadOnly Property SelectedCardValues As SelectedItemCollection
        Get

            Dim list As SelectedItemCollection
            If _selectedCardValues Is Nothing And Not String.IsNullOrWhiteSpace(SelectedState.Value) Then
                'get it from the state
                _selectedCardValues = SelectedItemCollection.FromJSON(SelectedState.Value)
                list = _selectedCardValues
            ElseIf _selectedCardValues IsNot Nothing Then
                list = _selectedCardValues
            Else
                list = New SelectedItemCollection()
            End If

            Return list
        End Get
    End Property


    ''' <summary>
    ''' Collection of sort expressions to display in the sort dropdown
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public Property SortExpressionList As List(Of MilesCardListSortExpression)
        Get
            Return ViewState("SortExpressionList")
        End Get
        Set(value As List(Of MilesCardListSortExpression))
            ViewState("SortExpressionList") = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the minimum height of the card
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CardMinHeight As Unit
        Get
            Return ViewState("CardMinHeight")
        End Get
        Set(value As Unit)
            ViewState("CardMinHeight") = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets whether the command row is visible
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ShowCommandRow As Boolean
        Get
            Return ViewState("ShowCommandRow")
        End Get
        Set(value As Boolean)
            ViewState("ShowCommandRow") = value
            CommandPanel.Visible = value
        End Set

    End Property

    ''' <summary>
    ''' Gets or sets whether the add new record button is visible
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ShowAddNewRecordButton As Boolean
        Get
            Return ViewState("ShowAddNewRecordButton")
        End Get
        Set(value As Boolean)
            ViewState("ShowAddNewRecordButton") = value

            AddNewRecordCommandItem.Visible = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the add new record button text
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AddNewRecordText As String
        Get
            Return ViewState("AddNewRecordText")
        End Get
        Set(value As String)
            ViewState("AddNewRecordText") = value

            AddNewButton.Text = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the client event for the add new record button
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OnAddNewRecordClientClick As String
        Get
            Return ViewState("OnAddNewRecordClientClick")
        End Get
        Set(value As String)
            ViewState("OnAddNewRecordClientClick") = value

            AddNewButton.OnClientClick = value
        End Set
    End Property

    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public Property CardCommandTemplate As ITemplate

    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public Property CardInfoTemplate As ITemplate

    Dim _emptyDataTemplate As ITemplate
    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public Overrides Property EmptyDataTemplate As ITemplate
        Get
            Return MyBase.EmptyDataTemplate
        End Get
        Set(value As ITemplate)
            _emptyDataTemplate = value
        End Set
    End Property

    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public Property CardEditTemplate As MilesCardListCardTemplate

    ''' <summary>
    ''' Template used when grid is in "detail" mode
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public Property CardTemplate As MilesCardListCardTemplate

    ''' <summary>
    ''' Template used when grid is in "list" mode
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public Property CardListTemplate As MilesCardListCardTemplate

    ''' <summary>
    ''' Gets or sets the number of columns
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NumberOfColumns As HorizontalColumns
        Get
            Return ViewState("NumberOfColumns")
        End Get
        Set(value As HorizontalColumns)
            ViewState("NumberOfColumns") = value
        End Set
    End Property

    Public Property SortUpImageUrl As String
        Get
            Return ViewState("SortUpImageUrl")
        End Get
        Set(value As String)
            ViewState("SortUpImageUrl") = value

        End Set
    End Property

    Public Property SortDownImageUrl As String
        Get
            Return ViewState("SortDownImageUrl")
        End Get
        Set(value As String)
            ViewState("SortDownImageUrl") = value

        End Set
    End Property

    'Public Property SwitchToDetailsImageUrl As String
    '    Get
    '        Return ViewState("SwitchToDetailsImageUrl")
    '    End Get
    '    Set(value As String)
    '        ViewState("SwitchToDetailsImageUrl") = value

    '    End Set
    'End Property

    'Public Property SwitchToCardsImageUrl As String
    '    Get
    '        Return ViewState("SwitchToCardsImageUrl")
    '    End Get
    '    Set(value As String)
    '        ViewState("SwitchToCardsImageUrl") = value

    '    End Set
    'End Property

    Public ReadOnly Property CurrentSortExpression As RadListViewSortExpression
        Get
            Dim sorting = (From s As RadListViewSortExpression In Me.SortExpressions).FirstOrDefault()

            If sorting Is Nothing Then
                sorting = New RadListViewSortExpression
            End If

            Return sorting
        End Get

    End Property

    ''' <summary>
    ''' Gets or sets whether the OnNeedsDataSource event is fired
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AutoBind As Boolean
        Get
            Return ViewState("AutoBind")
        End Get
        Set(value As Boolean)
            ViewState("AutoBind") = value

        End Set
    End Property

    ''' <summary>
    ''' Gets or sets whether the items can be selected
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AllowItemSelection As Boolean
        Get
            Return ViewState("AllowItemSelection")
        End Get
        Set(value As Boolean)
            ViewState("AllowItemSelection") = value

            'loop through each item and hide the checkbox
            For Each item In Me.Items
                Dim ItemSelect As CheckBox = item.FindControl("ItemSelect")
                If ItemSelect IsNot Nothing Then
                    ItemSelect.Visible = value
                End If
            Next

            'hide or show the check all checkbox
            CheckAll.Visible = (value And AllowMultiItemSelection)
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets whether multiple item selection is allowed
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Property AllowMultiItemSelection As Boolean
        Get
            Return MyBase.AllowMultiItemSelection
        End Get
        Set(value As Boolean)
            MyBase.AllowMultiItemSelection = value

            CheckAll.Visible = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets whether the no records text is displayed
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ShowNoRecordText As Boolean
        Get

            Return ViewState("ShowNoRecordText")
        End Get
        Set(value As Boolean)
            ViewState("ShowNoRecordText") = value

        End Set
    End Property

    ''' <summary>
    ''' Gets or sets when the columns break. Default is MD
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ColumnBreakingPoint As ColumnBreakPoint
        Get
            Return ViewState("ColumnBreakingPoint")
        End Get
        Set(value As ColumnBreakPoint)
            ViewState("ColumnBreakingPoint") = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets whether the InfoBar is detachable when out of view
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property InfoBarDetachable As Boolean
        Get
            Return ViewState("InfoBarDetachable")
        End Get
        Set(value As Boolean)
            ViewState("InfoBarDetachable") = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the event handler for item selecting
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OnClientItemSelectionChanged As String
        Get
            Return ViewState("OnClientItemSelectionChanged")
        End Get
        Set(value As String)
            ViewState("OnClientItemSelectionChanged") = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets whether drag and drop is allowed
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AllowItemsDragDrop As String
        Get
            Return MyBase.ClientSettings.AllowItemsDragDrop
        End Get
        Set(value As String)
            MyBase.ClientSettings.AllowItemsDragDrop = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the Css Class to apply to the card list panel
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CardListPanelCssClass As String
        Get
            Return ViewState("CardListPanelCssClass")
        End Get
        Set(value As String)
            ViewState("CardListPanelCssClass") = value
        End Set
    End Property

    Public Property CardInfoTemplateCssClass As String
        Get
            Return ViewState("CardInfoTemplateCssClass")
        End Get
        Set(value As String)
            ViewState("CardInfoTemplateCssClass") = value
        End Set
    End Property

#End Region

#Region "Private Properties"

    Private ReadOnly Property CardListEmptyDataTemplate As ITemplate
        Get
            Return _emptyDataTemplate
        End Get
    End Property

    Private ReadOnly Property VariableName
        Get
            Return "cardList_" + Me.ID
        End Get
    End Property


#End Region

#Region "Constructor"
    Public Sub New()
        If Me.SortExpressionList Is Nothing Then Me.SortExpressionList = New List(Of MilesCardListSortExpression)

        'set default properties
        Me.AllowPaging = True
        Me.AllowItemSelection = False
        Me.CardListPanelCssClass = String.Empty
        Me.ItemPlaceholderID = "ListContainer"
        Me.NumberOfColumns = HorizontalColumns.Three
        Me.ColumnBreakingPoint = ColumnBreakPoint.MD
        Me.ShowNoRecordText = True
        Me.ShowCommandRow = True
        Me.ShowAddNewRecordButton = True
        Me.AddNewRecordText = "Add New Record"
        Me.PageSize = 12
        Me.InsertItemPosition = RadListViewInsertItemPosition.FirstItem
        Me.CardMinHeight = New Unit(150)
        Me.SortOrder = RadListViewSortOrder.Ascending
        'Me.IsInListViewMode = False
        'Me.AllowViewModeSwitch = False
        'can't use Control's page here because it is not set yet
        Using p As New Page
            'set up 
            Me.SortUpImageUrl = p.ClientScript.GetWebResourceUrl(Me.GetType(), "MilesControls.sort_up.png")
            Me.SortDownImageUrl = p.ClientScript.GetWebResourceUrl(Me.GetType(), "MilesControls.sort_down.png")
            'Me.SwitchToDetailsImageUrl = p.ClientScript.GetWebResourceUrl(Me.GetType(), "MilesControls.view_details.png")
            'Me.SwitchToCardsImageUrl = p.ClientScript.GetWebResourceUrl(Me.GetType(), "MilesControls.view_large_icons.png")
        End Using
    End Sub
#End Region

#Region "Overrides"

    Protected Overrides Sub OnInit(e As EventArgs)

        If Not Page.IsPostBack Then
            SetItemTemplate()
        End If


        'set default edit template
        If Me.CardEditTemplate IsNot Nothing Then
            Me.EditItemTemplate = New MilesCardListItemTemplate With {
                .CardList = Me,
                .CardListTemplate = Me.CardEditTemplate
            }

            Me.InsertItemTemplate = Me.EditItemTemplate
        End If

        'set default layout template
        Me.LayoutTemplate = New MilesCardListLayoutTemplate() With {
            .CardList = Me
        }


        SelectedState.Value = SelectedCardValues.ToJSON()

        MyBase.OnInit(e)
    End Sub

    Protected Overrides Sub LoadViewState(savedState As Object)
        MyBase.LoadViewState(savedState)

        'set the appropriate template
        SetItemTemplate()

    End Sub

    Protected Overrides Sub OnItemDataBound(e As RadListViewItemEventArgs)


        If TypeOf e.Item Is RadListViewDataItem Then
            Dim item As RadListViewDataItem = e.Item

            Dim prop = item.DataItem.GetType().GetProperty("Archived")

            If prop IsNot Nothing Then

                Dim value As Boolean? = prop.GetValue(item.DataItem)


                If value.GetValueOrDefault() Then
                    Dim CardPanel As Panel = item.FindControl("CardPanel")

                    CardPanel.CssClass += " archived"

                End If


            End If

            'find all ICardListDataPropery in the tree
            Dim cardListProperties = item.FindControls(Of ICardListDataPropery)()

            For Each boundProperty In cardListProperties
                boundProperty.DataItem = item.DataItem
            Next

        End If

        MyBase.OnItemDataBound(e)

    End Sub

    Protected Overrides Sub OnPreRender(e As EventArgs)

        Dim milesCardJs = Page.ClientScript.GetWebResourceUrl(Me.GetType(), "MilesControls.MilesCardList.js")
        System.Web.UI.ScriptManager.RegisterClientScriptInclude(Me.Page, Me.Page.GetType(), "MilesControls.MilesCardList.js", milesCardJs)

        Dim cssUrl = Page.ClientScript.GetWebResourceUrl(Me.GetType(), "MilesControls.MilesCardList.css")

        'include css
        Dim link = <link rel='stylesheet' text='text/css' href=<%= cssUrl %>/>
        Dim cssInclude As New LiteralControl(link.ToString())

        Page.Header.Controls.Add(cssInclude)



        'add script
        Dim js = DecodeJS(<script>
                              var <%= VariableName %> = new MilesCardList(
                                    '<%= Me.ClientID %>',
                                    '#<%= CardListPanel.ClientID %>',
                                    '#<%= SelectedState.ClientID %>',
                                    '#<%= CheckAll.ClientID %>',
                                    <%= Me.AllowItemSelection %>,
                                    <%= Me.AllowMultiItemSelection %>,
                                    <%= Me.InfoBarDetachable %>,
                                    <%= If(Not String.IsNullOrWhiteSpace(Me.OnClientItemSelectionChanged), Me.OnClientItemSelectionChanged, "null") %>
                              );
                          </script>)


        System.Web.UI.ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), Me.ID + "MilesCardListInit", js, True)



        If AllowItemsDragDrop Then
            Me.ClientSettings.ClientEvents.OnItemDragStarted = VariableName + ".itemDragStarted"
            Me.ClientSettings.ClientEvents.OnItemDragging = VariableName + ".itemDragging"
            Me.ClientSettings.ClientEvents.OnItemDropping = VariableName + ".itemDropping"
            Me.ClientSettings.ClientEvents.OnItemDropped = VariableName + ".itemDropped"
        End If

        'persist the selections
        SelectedState.Value = SelectedCardValues.ToJSON()

        MyBase.OnPreRender(e)
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)

        MyBase.OnLoad(e)
    End Sub

    Protected Overrides Sub OnItemCommand(e As RadListViewCommandEventArgs)

        If e.CommandName = "ToggleArchive" Then
            RaiseEvent ToggleArchive(Me, e)
        ElseIf e.CommandName = "ItemClick" Then
            RaiseEvent ItemClicked(Me, e)
        ElseIf e.CommandName = "SortDirection" Then

            'check the sort expression
            If SortOrder = RadListViewSortOrder.Descending Then
                SortOrder = RadListViewSortOrder.Ascending
            Else
                SortOrder = RadListViewSortOrder.Descending
            End If


            'rebind the list for the sorting to take effect
            Me.Rebind()

        End If
        MyBase.OnItemCommand(e)
    End Sub

    Protected Overrides Sub OnDataBound(e As EventArgs)
        If Me.ShowNoRecordText And Me.Items.Count = 0 And Not Me.IsItemInserted Then
            'if there are no items after we have bound, show we have no records
            Dim ph = Me.FindControl("ListContainerPanel")
            If ph IsNot Nothing Then
                'remove any children
                ph.Controls.Clear()

                'create empty container
                Dim emptyContainer = CreateEmptyDataPanel()

                'add something to the empty container
                emptyContainer.Controls.Add(New LiteralControl(<span>No results to display.</span>))

                'add empty container to the card container
                ph.Controls.Add(emptyContainer)
            End If
        End If

        MyBase.OnDataBound(e)
    End Sub

    Protected Overrides Sub OnNeedDataSource(e As RadListViewNeedDataSourceEventArgs)

        If AutoBind Then

            MyBase.OnNeedDataSource(e)
        End If

    End Sub

    Public Overrides Sub Rebind()

        Me.SelectedCardValues.Clear()

        MyBase.Rebind()
    End Sub

    ''' <summary>
    ''' Updates the current expression from the viewstate
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub UpdateSortExpression()
        'we have a list of predefined expressions. But only one can be active at a time
        'take the selected expression and make it the current one

        'clear the current expression
        Me.SortExpressions.Clear()

        'create a new one from the view state
        Dim expression As New RadListViewSortExpression()

        expression.FieldName = SortExpression
        expression.SortOrder = SortOrder

        'add the expression to the list
        Me.SortExpressions.Add(expression)
    End Sub

#End Region

#Region "Private Methods"

    Private Sub SetItemTemplate()
        'use the regular card template for details
        If Me.CardTemplate IsNot Nothing Then
            Me.ItemTemplate = New MilesCardListItemTemplate With {
                .CardList = Me,
                .CardListTemplate = Me.CardTemplate
            }
        End If
    End Sub

    Private Function CreateEmptyDataPanel() As Panel
        Dim emptyContainer As New Panel
        emptyContainer.CssClass = "col-md-12 empty"

        Return emptyContainer

    End Function
#End Region

#Region "Private Classes"

    ''' <summary>
    ''' Layout class for CardList
    ''' </summary>
    ''' <remarks></remarks>
    Class MilesCardListCardTemplate

#Region "Enums"
        Public Enum ActionTemplatePosition
            Right
            Bottom
        End Enum

        Public Enum ItemHeaderLinkType
            None
            HyperLink
            LinkButton
        End Enum

#End Region

#Region "Properties"
        Public Property HeaderNavigateUrlFormatString As String
        Public Property HeaderNavigateUrlFields As String
        Public Property DefaultButton As String
        Public Property ActionPosition As ActionTemplatePosition
        Public Property HeaderLinkType As ItemHeaderLinkType
        Public Property HeaderLinkTarget As String
        ''' <summary>
        ''' Gets or sets the minimum height of the card
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CardMinHeight As Unit

        <PersistenceMode(PersistenceMode.InnerProperty)>
        Public Property CardHeaderTemplate As ITemplate

        <PersistenceMode(PersistenceMode.InnerProperty)>
        Public Property CardItemTemplate As ITemplate

        <PersistenceMode(PersistenceMode.InnerProperty)>
        Public Property CardActionTemplate As ITemplate

        Public Sub New()
            HeaderLinkType = ItemHeaderLinkType.None
        End Sub
#End Region

    End Class

    ''' <summary>
    ''' Default ItemTemplate
    ''' </summary>
    ''' <remarks></remarks>
    Class MilesCardListItemTemplate
        Implements ITemplate
        Public Property CardList As MilesCardList
        Public Property CardListTemplate As MilesCardListCardTemplate


        Public Sub InstantiateIn(container As Control) Implements ITemplate.InstantiateIn



            'create the controls we need
            Dim CardDataItem As New Panel
            CardDataItem.ID = "CardDataItem"


            'set properties
            CardDataItem.CssClass = String.Format("col-{0}-{1}",
                                            CardList.ColumnBreakingPoint.ToString().ToLower(),
                                            12 / CInt(CardList.NumberOfColumns))
            Dim CardPanel As New Panel
            CardPanel.ID = "CardPanel"
            CardPanel.CssClass = "card " + If(CardList.NumberOfColumns = HorizontalColumns.One And Not CardList.AllowItemSelection, "narrow",
                                              If(CardList.AllowItemSelection Or CardList.AllowMultiItemSelection, "wide", ""))



            CardPanel.DefaultButton = CardListTemplate.DefaultButton
            AddHandler CardPanel.DataBinding, AddressOf Card_DataBinding

            'add header first
            If CardListTemplate.CardHeaderTemplate IsNot Nothing Then

                Dim pnlHeader As New Panel
                pnlHeader.CssClass = "header clearfix"

                Dim headerLinkControl As Control = Nothing

                'determine what type of link the header is
                If CardListTemplate.HeaderLinkType = MilesCardListCardTemplate.ItemHeaderLinkType.HyperLink Then
                    Dim HeaderLink As New HyperLink
                    HeaderLink.ID = "HeaderLink"
                    HeaderLink.CssClass = "header-action"
                    HeaderLink.Target = CardListTemplate.HeaderLinkTarget
                    AddHandler HeaderLink.DataBinding, AddressOf HeaderLink_DataBinding

                    'set to our link
                    headerLinkControl = HeaderLink

                ElseIf CardListTemplate.HeaderLinkType = MilesCardListCardTemplate.ItemHeaderLinkType.LinkButton Then

                    Dim HeaderLink As New LinkButton
                    HeaderLink.ID = "HeaderLink"
                    HeaderLink.CommandName = "ItemClick"
                    HeaderLink.CssClass = "header-action"

                    'set to our link
                    headerLinkControl = HeaderLink
                Else

                    Dim HeaderLink As New Label
                    HeaderLink.ID = "HeaderLink"
                    HeaderLink.CssClass = "header-action"

                    'set to our link
                    headerLinkControl = HeaderLink
                End If



                'create the header in the card
                CardListTemplate.CardHeaderTemplate.InstantiateIn(pnlHeader)

                'add the header to the link
                headerLinkControl.Controls.Add(pnlHeader)

                'add the link to the card
                CardPanel.Controls.Add(headerLinkControl)

            End If

            'create objects in template and place them in the pnlCard
            If CardListTemplate.CardItemTemplate IsNot Nothing Then

                'determine padding. if our action position is right, then we pad right, else if its bottom
                'we pad the bottom
                Dim padding As String = ""
                If CardListTemplate.CardActionTemplate IsNot Nothing Then
                    If CardListTemplate.ActionPosition = MilesCardListCardTemplate.ActionTemplatePosition.Right Then
                        padding = "pad-right"
                    ElseIf CardListTemplate.ActionPosition = MilesCardListCardTemplate.ActionTemplatePosition.Bottom Then
                        padding = "pad-bottom"
                    End If
                End If

                Dim pnlBody As New Panel
                pnlBody.Style.Add("min-height", If(
                                  CardListTemplate.CardMinHeight.IsEmpty,
                                  CardList.CardMinHeight.ToString(),
                                  CardListTemplate.CardMinHeight.ToString()))

                pnlBody.CssClass = String.Format("body {0}", padding)

                CardListTemplate.CardItemTemplate.InstantiateIn(pnlBody)

                'create the action template
                If CardListTemplate.CardActionTemplate IsNot Nothing Then
                    Dim ActionPanel As New Panel
                    ActionPanel.ID = "ActionPanel"
                    ActionPanel.CssClass = String.Format("action {0}", GetActionPosCssClass())

                    CardListTemplate.CardActionTemplate.InstantiateIn(ActionPanel)

                    'add action panel to the card
                    pnlBody.Controls.Add(ActionPanel)
                End If


                CardPanel.Controls.Add(pnlBody)
            End If

            'If CardList.AllowItemSelection Then
            'if we are allowing selections, add a checkbox
            Dim ItemSelect As New CheckBox
            ItemSelect.Visible = CardList.AllowItemSelection
            ItemSelect.ID = "ItemSelect"
            ItemSelect.CssClass = "select"

            CardPanel.Controls.Add(ItemSelect)
            'End If

            'add the card to the column div
            CardDataItem.Controls.Add(CardPanel)

            'add all to the container
            container.Controls.Add(CardDataItem)

            'add a clear fix every so many columns to prevent varying heights from warping our card list
            Dim item As RadListViewDataItem = container
            If (item.DisplayIndex + 1) Mod CardList.NumberOfColumns = 0 And CardList.NumberOfColumns <> HorizontalColumns.One Then
                'add clear fix
                Dim pnlClear As New Panel
                pnlClear.CssClass = "clearfix visible-xs visible-sm visible-md visible-lg"

                container.Controls.Add(pnlClear)
            End If

        End Sub

        Private Sub HeaderLink_DataBinding(sender As Object, e As EventArgs)
            Dim ctrl As HyperLink = sender
            Dim item As RadListViewDataItem = ctrl.NamingContainer

            If String.IsNullOrWhiteSpace(CardListTemplate.HeaderNavigateUrlFields) Then
                Return
            End If

            'get the navigate url fields
            Dim fields = CardListTemplate.HeaderNavigateUrlFields.Split({","}, StringSplitOptions.RemoveEmptyEntries)

            Dim fieldValues As New List(Of Object)

            For Each field In fields
                Dim value = DataBinder.Eval(item.DataItem, field)

                fieldValues.Add(value)
            Next

            'plug in the values
            ctrl.NavigateUrl = String.Format(CardListTemplate.HeaderNavigateUrlFormatString, fieldValues.ToArray())

        End Sub

        Private Sub Card_DataBinding(sender As Object, e As EventArgs)
            Dim ctrl As Panel = sender
            Dim item As RadListViewDataItem = ctrl.NamingContainer

            If item.DataItem IsNot Nothing Then
                Dim keyvalues As New List(Of Object)
                For Each key In CardList.DataKeyNames
                    keyvalues.Add(item.GetDataKeyValue(key))
                Next

                Dim value = String.Join("|", keyvalues)

                ctrl.Attributes.Add("data-value", value)

                If CardList.AllowItemsDragDrop Then
                    'data-index is used to indicate which card is being dragged
                    ctrl.Attributes.Add("data-item-id", item.ID)
                    ctrl.Attributes.Add("data-index", item.DisplayIndex)
                    ctrl.Attributes.Add("onmouseover", CardList.VariableName + ".onMouseOverCard(this)")
                    ctrl.Attributes.Add("onmouseout", CardList.VariableName + ".onMouseOutCard(this)")
                    ctrl.Attributes.Add("onmouseup", CardList.VariableName + ".onMouseUpCard(this)")
                End If

            End If
        End Sub

        ''' <summary>
        ''' Gets the positioning css class
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetActionPosCssClass() As String
            If CardListTemplate.ActionPosition = MilesCardListCardTemplate.ActionTemplatePosition.Right Then
                Return "right"
            ElseIf CardListTemplate.ActionPosition = MilesCardListCardTemplate.ActionTemplatePosition.Bottom Then
                Return "bottom"
            Else
                Return ""
            End If
        End Function

    End Class

    ''' <summary>
    ''' Default LayoutTemplate
    ''' </summary>
    ''' <remarks></remarks>
    Class MilesCardListLayoutTemplate
        Implements ITemplate

        Public Property CardList As MilesCardList

        'Public Event SortingChanged As EventHandler

        Public Sub InstantiateIn(container As Control) Implements ITemplate.InstantiateIn
            'create the controls we need

            CardList.SelectedState.ID = "SelectedState"


            Dim ListContainerPanel As New Panel
            Dim ListContainer As New PlaceHolder



            'set properties
            CardList.CardListPanel.ID = "CardListPanel"
            CardList.CardListPanel.CssClass = "card-list row " + CardList.CardListPanelCssClass




            ListContainerPanel.ID = "ListContainerPanel"
            ListContainerPanel.CssClass = "card-container "

            ListContainer.ID = "ListContainer"

            CardList.Pager.ID = "Pager"
            CardList.Pager.PageSize = CardList.PageSize
            CardList.Pager.PagedControlID = CardList.ID
            CardList.Pager.Skin = CardList.Skin
            CardList.Pager.CssClass = "miles-data-pager"

            'add fields to pager
            CardList.Pager.Fields.Add(New RadDataPagerButtonField() With {.FieldType = PagerButtonFieldType.FirstPrev})
            CardList.Pager.Fields.Add(New RadDataPagerButtonField() With {.FieldType = PagerButtonFieldType.Numeric})
            CardList.Pager.Fields.Add(New RadDataPagerButtonField() With {.FieldType = PagerButtonFieldType.NextLast})


            CardList.Pager.Fields.Add(New RadDataPagerPageSizeField() With {.PageSizeText = "Page size: ", .PageSizes = {6, 12, 24, 48, 96, 192}})

            CardList.Pager.Visible = CardList.AllowPaging


            CardList.Pager.Fields.Add(New RadDataPagerTemplatePageField() With {.PagerTemplate = New MilesCardListPagerTemplate()})

            'create the command holder row. This will hold the command row and info row
            Dim holderPanel As New Panel
            holderPanel.CssClass = "header-row-holder row " + If(CardList.NumberOfColumns = HorizontalColumns.One And Not CardList.AllowItemSelection, "narrow",
                                              If(CardList.AllowItemSelection Or CardList.AllowMultiItemSelection, "wide", ""))


            'create the command row column
            CardList.CommandPanel.CssClass = "col-md-12 header-row-item"
            CardList.CommandPanel.ID = "CommandPanel"
            CardList.CommandPanel.Visible = CardList.ShowCommandRow
            Dim commandHolderPanel As New Panel
            commandHolderPanel.CssClass = "command-row header-row"


            Dim commandRowHolderRow As New Panel
            commandRowHolderRow.CssClass = "row"

            'header check box item
            CardList.CheckAll.Visible = (CardList.AllowItemSelection And CardList.AllowMultiItemSelection)
            'CardList.CheckAll.Text = "Check All"
            CardList.CheckAll.CssClass = "select"
            CardList.CheckAll.Attributes.Add("data-role", "check-all")

            'add the check all option to the command row
            commandHolderPanel.Controls.Add(CardList.CheckAll)


            'determine the column cost of the command items
            Dim commandTemplateReserved As Integer = 12

            'each item that is displayed is less space for the command template
            If CardList.ShowAddNewRecordButton Then commandTemplateReserved -= 2
            'If CardList.AllowViewModeSwitch Then commandTemplateReserved -= 2
            If CardList.SortExpressionList.Count > 0 Then commandTemplateReserved -= 2


            'determine if we should add the new record button
            If CardList.ShowAddNewRecordButton Then
                'use default
                Dim blockCss = If(CardList.CardCommandTemplate Is Nothing, "", "btn-block")

                CardList.AddNewRecordCommandItem.CssClass = "col-sm-2"

                'add new item button
                CardList.AddNewButton.Visible = CardList.ShowAddNewRecordButton
                CardList.AddNewButton.CommandName = "InitInsert"
                CardList.AddNewButton.CssClass = "btn btn-secondary " & blockCss
                CardList.AddNewButton.Text = CardList.AddNewRecordText
                CardList.AddNewButton.CausesValidation = False
                'add custom client events to the add new record button
                CardList.AddNewButton.OnClientClick = CardList.OnAddNewRecordClientClick

                CardList.AddNewRecordCommandItem.Controls.Add(CardList.AddNewButton)
                commandRowHolderRow.Controls.Add(CardList.AddNewRecordCommandItem)
            End If




            'Now reserve space for the command template
            Dim commandRowTemplateItem As New Panel
            commandRowTemplateItem.CssClass = String.Format("col-sm-{0}", commandTemplateReserved)

            'instantiate the template inside this panel
            If CardList.CardCommandTemplate IsNot Nothing Then
                CardList.CardCommandTemplate.InstantiateIn(commandRowTemplateItem)
            End If

            'add this panel to the holder row
            commandRowHolderRow.Controls.Add(commandRowTemplateItem)





            'view switch
            'If CardList.AllowViewModeSwitch Then

            '    CardList.AllowViewModeSwitchCommandItem.CssClass = "col-sm-2 text-right"

            '    CardList.DetailSwitcher.ID = "DetailSwitcher"
            '    CardList.DetailSwitcher.AutoPostBack = True
            '    CardList.DetailSwitcher.CausesValidation = False

            '    AddHandler CardList.DetailSwitcher.CheckedChanged, AddressOf DetailSwitcher_CheckedChanged

            '    CardList.AllowViewModeSwitchCommandItem.Controls.Add(CardList.DetailSwitcher)

            '    'add this panel to the holder row
            '    commandRowHolderRow.Controls.Add(CardList.AllowViewModeSwitchCommandItem)
            'End If


            If CardList.SortExpressionList.Count > 0 Then

                Dim sortCommandRowItem As New Panel
                sortCommandRowItem.CssClass = "col-sm-2" ' String.Format("col-sm-4 col-sm-offset-{0}", If(CardList.ShowAddNewRecordButton, 6, 8))

                Dim sortInputGroupPanel As New Panel
                sortInputGroupPanel.CssClass = "input-group"

                Dim addOnSpan As New Label
                addOnSpan.CssClass = "input-group-addon"

                Dim SortList As New RadComboBox



                'sort dropdown
                SortList.ID = "SortList"
                SortList.AutoPostBack = True
                SortList.CausesValidation = False
                SortList.Skin = CardList.Skin
                SortList.CssClass = "form-control"
                SortList.Width = Unit.Percentage(100)
                'SortList.ItemTemplate = New SortItemTemplate()
                AddHandler SortList.PreRender, AddressOf SortList_OnPreRender
                AddHandler SortList.SelectedIndexChanged, AddressOf SortList_SelectedIndexChanged

                Dim SortDirection As New ImageButton
                SortDirection.ID = "SortDirection"
                SortDirection.CausesValidation = False
                SortDirection.CommandName = "SortDirection"
                'SortDirection.ImageAlign = ImageAlign.AbsMiddle
                AddHandler SortDirection.PreRender, AddressOf SortDirection_OnPreRender

                'add the controls to the item
                sortInputGroupPanel.Controls.Add(SortList)
                addOnSpan.Controls.Add(SortDirection)

                'add the span to the input group
                sortInputGroupPanel.Controls.Add(addOnSpan)

                'add the input group to the item
                sortCommandRowItem.Controls.Add(sortInputGroupPanel)


                'add the item to the command row
                commandRowHolderRow.Controls.Add(sortCommandRowItem)

            End If


            'add the holder row (the row that holds all the command items)
            commandHolderPanel.Controls.Add(commandRowHolderRow)

            'add the holder panel to the command panel
            CardList.CommandPanel.Controls.Add(commandHolderPanel)

            'add the command panel to the holder
            holderPanel.Controls.Add(CardList.CommandPanel)










            'add info section
            'if there is an info template, add it after the command items
            If CardList.CardInfoTemplate IsNot Nothing Then

                Dim infoPanel As New Panel
                infoPanel.CssClass = "col-md-12 header-row-item"

                If CardList.CardInfoTemplateCssClass <> String.Empty Then
                    infoPanel.CssClass += " " & CardList.CardInfoTemplateCssClass
                End If

                'determine padding. if our action position is right, then we pad right, else if its bottom
                'we pad the bottom
                Dim padding As String = ""
                If CardList.CardTemplate IsNot Nothing AndAlso CardList.CardTemplate.CardActionTemplate IsNot Nothing Then
                    If CardList.CardTemplate.ActionPosition = MilesCardListCardTemplate.ActionTemplatePosition.Right Then
                        padding = "pad-right"
                    End If
                End If

                'create the holder panel. this is where the custom html goes
                Dim commandHolderPanel2 As New Panel
                commandHolderPanel2.CssClass = "info-row header-row " + padding

                'add the command-row to the col
                CardList.CardInfoTemplate.InstantiateIn(commandHolderPanel2)
                'add the col to the cardlist
                infoPanel.Controls.Add(commandHolderPanel2)

                'add the info panel to the holder
                holderPanel.Controls.Add(infoPanel)

            End If

            'add the holder to the card list panel
            CardList.CardListPanel.Controls.Add(holderPanel)


            Dim emptyContainer = CardList.CreateEmptyDataPanel()

            If CardList.CardListEmptyDataTemplate IsNot Nothing Then
                'use the template
                CardList.CardListEmptyDataTemplate.InstantiateIn(emptyContainer)

            Else
                'use default
                emptyContainer.Controls.Add(New LiteralControl(<span>Please click search to load the results</span>))

            End If
            'add to the container
            ListContainer.Controls.Add(emptyContainer)

            'add container to container panel
            ListContainerPanel.Controls.Add(ListContainer)

            'add placeholder to the row panel
            CardList.CardListPanel.Controls.Add(ListContainerPanel)

            'add all to the container
            container.Controls.Add(CardList.CardListPanel)
            container.Controls.Add(CardList.Pager)
            container.Controls.Add(CardList.SelectedState)
        End Sub

        Protected Sub SortList_OnPreRender(sender As Object, e As EventArgs)
            Dim SortList As RadComboBox = sender

            'bind the list of expressions
            ControlBinding.BindListControl(SortList, CardList.SortExpressionList, "DisplayName", "FieldName", True, defaultText:="Sort By")

            SortList.SelectedValue = (From se As RadListViewSortExpression In CardList.SortExpressions Select se.FieldName).FirstOrDefault()
        End Sub

        Protected Sub SortDirection_OnPreRender(sender As Object, e As EventArgs)
            Dim SortDirection As ImageButton = sender
            'determine which direction we're sorting currently
            If CardList.CurrentSortExpression.SortOrder = RadListViewSortOrder.Descending Then
                SortDirection.ImageUrl = CardList.SortDownImageUrl
            Else
                SortDirection.ImageUrl = CardList.SortUpImageUrl
            End If

            SortDirection.ToolTip = CardList.CurrentSortExpression.SortOrder.ToString()
        End Sub

        Protected Sub SortList_SelectedIndexChanged(sender As Object, e As EventArgs)
            Dim SortList As RadComboBox = sender

            'clear the sort expressions
            CardList.SortExpressions.Clear()

            'add new sort expression
            If SortList.SelectedIndex > 0 Then

                'get the expression from the list
                Dim expression As RadListViewSortExpression = CardList.SortExpressionList(SortList.SelectedIndex - 1)

                'change the sort expression (but leave the sort direction the same)
                CardList.SortExpression = expression.FieldName

            End If

            'rebind
            CardList.Rebind()
        End Sub

        'Protected Sub DetailSwitcher_CheckedChanged(sender As Object, e As EventArgs)
        '    Dim chk As CheckBox = sender
        '    CardList.IsInListViewMode = chk.Checked

        '    CardList.Rebind()
        'End Sub

        ''' <summary>
        ''' Pager item template
        ''' </summary>
        ''' <remarks></remarks>
        Class MilesCardListPagerTemplate
            Implements ITemplate

            Public Sub InstantiateIn(container As Control) Implements ITemplate.InstantiateIn
                Dim item As RadDataPagerFieldItem = container

                Dim html = <div>
                            Page <%= item.Owner.CurrentPageIndex + 1 %> |
                            Items <%= item.Owner.StartRowIndex + 1 %>
                            to <%= If(item.Owner.TotalRowCount > (item.Owner.StartRowIndex + item.Owner.PageSize),
                                   item.Owner.StartRowIndex + item.Owner.PageSize, item.Owner.TotalRowCount) %>
                            of <%= item.Owner.TotalRowCount %>
                           </div>

                Dim itemInfo As New LiteralControl(html)

                container.Controls.Add(itemInfo)
            End Sub
        End Class


        Class SortItemTemplate
            Implements ITemplate

            Public Sub InstantiateIn(container As Control) Implements ITemplate.InstantiateIn
                Dim lnkItem As New LinkButton
                lnkItem.CausesValidation = False
                lnkItem.CommandName = "Sort"

                AddHandler lnkItem.DataBinding, AddressOf lnkItem_DataBinding

                container.Controls.Add(lnkItem)
            End Sub

            Protected Sub lnkItem_DataBinding(sender As Object, e As EventArgs)
                Dim lnkItem As LinkButton = sender

                Dim container As RadComboBoxItem = lnkItem.NamingContainer

                lnkItem.Text = DataBinder.Eval(container.DataItem, "DisplayName")
                lnkItem.CommandArgument = DataBinder.Eval(container.DataItem, "FieldName")
            End Sub



        End Class

    End Class

#End Region

    ''' <summary>
    ''' This section is for the ISearchPanelBoundListControl implementation
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
#Region "ISearchPanelBoundListControl"
    Public ReadOnly Property ScrollToTargetID As String Implements ISearchPanelBoundListControl.ScrollToTargetID
        Get
            Return Me.ID + ":CardListPanel"
        End Get
    End Property

    Public Property ListPageIndex As Integer Implements ISearchPanelBoundListControl.ListPageIndex
        Get
            Return Me.CurrentPageIndex
        End Get
        Set(value As Integer)
            'Me.CurrentPageIndex = value

            'force the pager to go to specific page
            Me.Pager.FireCommand(RadDataPager.PageCommandName, (value + 1).ToString())
        End Set
    End Property

    Public Property ListPageSize As Integer Implements ISearchPanelBoundListControl.ListPageSize
        Get
            Return Me.PageSize
        End Get
        Set(value As Integer)
            Me.PageSize = value

            Dim pager As RadDataPager = Me.FindControl("Pager")
            If pager IsNot Nothing Then
                pager.PageSize = value
            End If

        End Set
    End Property

    Public Property SortExpression As String Implements ISearchPanelBoundListControl.SortExpression
        Get
            Return ViewState("SortExpression")
        End Get
        Set(value As String)
            ViewState("SortExpression") = value

            'update the expression
            UpdateSortExpression()
        End Set
    End Property

    Public Property SortOrder As Integer Implements ISearchPanelBoundListControl.SortOrder
        Get
            Return ViewState("SortOrder")
        End Get
        Set(value As Integer)
            ViewState("SortOrder") = value

            'update the expression
            UpdateSortExpression()
        End Set
    End Property



#End Region

End Class

#Region "Miles Card List utility classes"

Public Class MilesCardListSortExpression
    Inherits RadListViewSortExpression

    Public Property DisplayName As String

End Class

Public Class SelectedCard
    Public Property Index As Integer
    Public Property Value As String
End Class

Public Class SelectedItemState
    Public SelectedItems As SelectedCard() = {}
End Class

Public Class SelectedItemCollection
    Inherits List(Of SelectedCard)

    Public Sub New()

    End Sub
    Public Sub New(collection As IEnumerable(Of SelectedCard))
        MyBase.New(collection)
    End Sub

    Public Shared Function FromJSON(input As String)
        Dim json As New JavaScriptSerializer()
        Dim state = json.Deserialize(Of SelectedItemState)(input)

        If state IsNot Nothing Then
            Return New SelectedItemCollection(state.SelectedItems)
        Else
            Return New SelectedItemCollection()
        End If
    End Function

    Public Function ToJSON() As String
        Dim json As New JavaScriptSerializer()
        Dim state = New SelectedItemState
        state.SelectedItems = Me.ToArray()
        Return json.Serialize(state)
    End Function

End Class

#End Region