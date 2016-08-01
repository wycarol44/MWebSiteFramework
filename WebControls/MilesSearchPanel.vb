Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports Telerik.Web.UI
Imports System.ComponentModel
Imports System.IO
Imports System.Xml.Serialization
Imports System.Web
Imports System.Reflection
Imports CommonLibrary
Imports BusinessLibrary
Imports MilesControls
Imports System.Web.Script.Serialization

''' <summary>
''' Search Panel
''' Created by Eric Butler
''' October 2013
''' </summary>
''' <remarks></remarks>
<ParseChildren(True)>
Public Class MilesSearchPanel
    Inherits Panel
    Implements INamingContainer

#Region "Enums"

#End Region

#Region "Private fields"
    Dim ExpandButton As New LinkButton()
    Dim ClientState As New HiddenField()
    Dim FiltersLoadedImage As New Image()
    Dim searchToolBarDropDown As New RadToolBarDropDown()

#End Region

#Region "Private Properties"

    ''' <summary>
    ''' Gets the name of the session in which to store the state
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property SavedStateSessionName As String
        Get
            'get path
            Dim pageName = HttpContext.Current.Request.Path
            Return String.Format("{0}.{1}.SavedState", pageName, Me.ID)
        End Get

    End Property

    ''' <summary>
    ''' Gets the name of the session in which to store the RestoreSearch value
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property SavedStateRestoreSearchName As String
        Get
            'get path
            Dim pageName = HttpContext.Current.Request.Path
            Return String.Format("{0}.{1}.RestoreSearch", pageName, Me.ID)
        End Get

    End Property

    ''' <summary>
    ''' Gets or sets the state in session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property SavedState As XElement
        Get
            Dim state As XElement = Nothing
            Dim xml As String = Page.Session(SavedStateSessionName)
            If Not String.IsNullOrWhiteSpace(xml) Then
                state = XElement.Parse(xml)
            End If

            Return state
        End Get
        Set(value As XElement)
            Dim xml = If(value IsNot Nothing, value.ToString, Nothing)
            Page.Session(SavedStateSessionName) = xml
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets a value that indicates the search button was pressed on this panel
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property RestoreSearch As Boolean
        Get
            Return Page.Session(SavedStateRestoreSearchName)
        End Get
        Set(value As Boolean)
            Page.Session(SavedStateRestoreSearchName) = value
        End Set
    End Property


#End Region

#Region "Event Handlers"
    Public Event StateSaving As EventHandler(Of SearchPanelSaveStateEventArgs)
    Public Event StateLoading As EventHandler(Of SearchPanelSaveStateEventArgs)

    Public Event Search As EventHandler
    Public Event Reset As EventHandler(Of ResetSearchEventArgs)

    Public Event ToolBarButtonClick As EventHandler(Of RadToolBarEventArgs)
#End Region

#Region "Properties"
    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public Property ContentPanel As Panel

    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public Property SearchButton As MilesButton

    'Public Property SaveAjaxPanel As RadAjaxPanel
    'Public Property SaveAjaxLoadingPanel As RadAjaxLoadingPanel

    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public Property ToolBarCommands As List(Of RadToolBarButton)

    Public Property CommandToolBar As New RadToolBar

    Public Property Skin As String
        Get
            Return ViewState("Skin")
        End Get
        Set(value As String)
            ViewState("Skin") = value

        End Set
    End Property

    Public Property SearchOnLoadSavedState As Boolean
        Get
            Return ViewState("SearchOnLoadSavedState")
        End Get
        Set(value As Boolean)
            ViewState("SearchOnLoadSavedState") = value

        End Set
    End Property

    ''' <summary>
    ''' Gets or sets control id associated to the panel. This allows the panel to save and restore states of certain external list controls
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ListControlID As String
        Get
            Return ViewState("ListControlID")
        End Get
        Set(value As String)
            ViewState("ListControlID") = value
        End Set
    End Property

    ' ''' <summary>
    ' ''' Gets or sets the Control ID to scroll to when search is activated
    ' ''' </summary>
    ' ''' <value></value>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Property ScrollToTargetID As String
    '    Get
    '        Return ViewState("ScrollToTargetID")
    '    End Get
    '    Set(value As String)
    '        ViewState("ScrollToTargetID") = value
    '    End Set
    'End Property

    ''' <summary>
    ''' Gets or sets whether the search filters can be saved
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AllowSaveSearch As Boolean
        Get
            Return ViewState("AllowSaveSearch")
        End Get
        Set(value As Boolean)
            ViewState("AllowSaveSearch") = value
        End Set
    End Property

#End Region

#Region "Constructor"
    Public Sub New()
        If SearchButton Is Nothing Then
            SearchButton = New MilesButton
            SearchButton.CausesValidation = False
        End If

        'content panel
        If ContentPanel Is Nothing Then ContentPanel = New Panel

        'ajax
        'If SaveAjaxPanel Is Nothing Then SaveAjaxPanel = New RadAjaxPanel

        'set properties
        Skin = "Silk"
        AllowSaveSearch = True
        SearchOnLoadSavedState = True

    End Sub
#End Region

#Region "Overrides"

    Protected Overrides Sub OnInit(e As EventArgs)

        Me.CssClass = "miles-search-panel"
        Me.DefaultButton = "SearchButton"

        'set properties
        SearchButton.ID = "SearchButton"
        SearchButton.Skin = Me.Skin
        SearchButton.Text = "Search"
        SearchButton.EnableTheming = False 'ignore baseclass theme skins
        'SearchButton.Image.ImageUrl = "~/Images/16x16/find.png"
        SearchButton.Icon.PrimaryIconUrl = "~/Images/16x16/find.png"
        'set event handlers
        AddHandler SearchButton.Click, AddressOf SearchButton_Click

        Dim ResetButton As New Button

        ResetButton.CausesValidation = False

        ResetButton.ID = "ResetButton"
        ResetButton.Text = "Reset"
        ResetButton.EnableTheming = False 'ignore baseclass theme skins
        ResetButton.CssClass = "btn btn-default"
        'set event handlers
        AddHandler ResetButton.Click, AddressOf ResetButton_Click


        ClientState.ID = "ClientState"
        Dim json As New JavaScriptSerializer
        Dim state As New SearchPanelClientState
        state.Expanded = True
        ClientState.Value = json.Serialize(state)

        ExpandButton.ID = "ExpandButton"
        ExpandButton.CssClass = "search-item"
        ExpandButton.ToolTip = "Expand/Collapse"

        FiltersLoadedImage.ID = "FiltersLoadedImage"
        FiltersLoadedImage.CssClass = "search-item"
        FiltersLoadedImage.ToolTip = "Using saved search filters"
        FiltersLoadedImage.ImageAlign = ImageAlign.AbsMiddle
        FiltersLoadedImage.ImageUrl = "~/Images/24x24/filter-ok.png"
        FiltersLoadedImage.Visible = False

        'ContentPanel.Style.Add("display", "none")
        ContentPanel.ID = "ContentPanel"
        ContentPanel.CssClass = "spContentPanel"

        Dim ButtonPanel As New Panel
        ButtonPanel.ID = "ButtonPanel"
        ButtonPanel.CssClass = "spButtonPanel"

        Dim ButtonPanelRow As New Panel
        ButtonPanelRow.CssClass = "row"

        Dim ButtonPanelRowExpander As New Panel
        ButtonPanelRowExpander.CssClass = "pull-left search-item"

        Dim ButtonPanelRowTools As New Panel
        ButtonPanelRowTools.CssClass = "pull-left search-item"

        Dim SearchButtonPanelWrapper As New Panel
        SearchButtonPanelWrapper.CssClass = "pull-right search-item"

        Dim ButtonPanelRowUsingFilters As New Panel
        ButtonPanelRowUsingFilters.CssClass = "pull-left search-item"

        Dim ButtonPanelRowSearch As New Panel
        ButtonPanelRowSearch.CssClass = "pull-left search-item"



        'add items to the search bar
        CommandToolBar.ID = "CommandToolBar"
        CommandToolBar.OnClientButtonClicking = "MilesSearchPanel.CommandToolBar_OnClientButtonClicking"

        searchToolBarDropDown.DropDownWidth = 200
        searchToolBarDropDown.ImageUrl = "~/Images/24x24/find-actions.png"
        AddHandler CommandToolBar.ButtonClick, AddressOf SearchBar_ButtonClick


        'do not add the save search commands if not allowed
        If AllowSaveSearch Then

            searchToolBarDropDown.Buttons.Add(New RadToolBarButton() With {
                              .CommandName = "SaveFilters",
                              .CausesValidation = False,
                              .Text = "Save This Search",
                              .ImageUrl = "~/Images/16x16/filter-add.png"
                          })

            searchToolBarDropDown.Buttons.Add(New RadToolBarButton() With {
                              .CommandName = "RemoveFilters",
                              .CausesValidation = False,
                              .Text = "Delete This Search",
                              .ImageUrl = "~/Images/16x16/filter-delete.png"
                          })
        End If


        'if we have commands to add, add a seperator and add the items
        If ToolBarCommands IsNot Nothing AndAlso ToolBarCommands.Count > 0 Then
            searchToolBarDropDown.Buttons.Add(New RadToolBarButton() With {.IsSeparator = True})
            'add all the commands to the list
            searchToolBarDropDown.Buttons.AddRange(ToolBarCommands)

        End If


        CommandToolBar.Items.Add(searchToolBarDropDown)


        'add the items to their column divs
        ButtonPanelRowExpander.Controls.Add(ExpandButton)
        ButtonPanelRowTools.Controls.Add(CommandToolBar)
        ButtonPanelRowUsingFilters.Controls.Add(FiltersLoadedImage)
        ButtonPanelRowSearch.Controls.Add(SearchButton)
        ButtonPanelRowSearch.Controls.Add(New LiteralControl(" "))
        ButtonPanelRowSearch.Controls.Add(ResetButton)

        'add search buttons to search button wrapper
        SearchButtonPanelWrapper.Controls.Add(ButtonPanelRowUsingFilters)
        SearchButtonPanelWrapper.Controls.Add(ButtonPanelRowSearch)

        'add both panels to the button panel
        ButtonPanelRow.Controls.Add(ButtonPanelRowExpander)
        ButtonPanelRow.Controls.Add(ButtonPanelRowTools)
        ButtonPanelRow.Controls.Add(SearchButtonPanelWrapper)
        'ButtonPanelRow.Controls.Add(ButtonPanelRowSearch)

        'add the row to the button panel
        ButtonPanel.Controls.Add(ButtonPanelRow)

        'create bottom panel for buttons and other junk
        Me.Controls.Add(ContentPanel)
        Me.Controls.Add(ButtonPanel)
        Me.Controls.Add(ClientState)
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)

        MyBase.OnLoad(e)
    End Sub

    Protected Overrides Sub OnPreRender(e As EventArgs)

        'output some javascript
        Dim js = Page.ClientScript.GetWebResourceUrl(Me.GetType(), "WebControls.MilesSearchPanel.js")

        'link javascript
        Page.ClientScript.RegisterClientScriptInclude(Me.GetType(), "MilesSearchPanel", js)

        'get css resource url
        Dim cssUrl = Page.ClientScript.GetWebResourceUrl(Me.GetType(), "WebControls.SearchPanel.css")

        'include css
        Dim link = <link rel='stylesheet' text='text/css' href=<%= cssUrl %>/>
        Dim cssInclude As New LiteralControl(link.ToString())
        Page.Header.Controls.Add(cssInclude)


        'deserialize client state
        Dim json As New JavaScriptSerializer
        Dim state = json.Deserialize(Of SearchPanelClientState)(ClientState.Value)

        'set the content panel and expander button
        '  If state.Expanded AndAlso Page.Request.Form("__EVENTTARGET") IsNot Nothing AndAlso Page.Request.Form("__EVENTTARGET").Contains("SearchButton") Then state.Expanded = False
        If state.Expanded Then
            ContentPanel.Style("display") = "block"
            ExpandButton.Text = <span class="glyphicon glyphicon-chevron-up"></span>.ToString()
        Else
            ContentPanel.Style("display") = "none"
            ExpandButton.Text = <span class="glyphicon glyphicon-chevron-down"></span>.ToString()
        End If

        ' ClientState.Value = json.Serialize(state)

        'load the state if not postback
        If Not Page.IsPostBack Then

            'add scripts
            ExpandButton.OnClientClick = DecodeJS(<script>
                                                        MilesSearchPanel.toggleExpand('#<%= Me.ContentPanel.ClientID %>', '#<%= ExpandButton.ClientID %> > .glyphicon', '#<%= ClientState.ClientID %>');
                                                        return false;
                                                  </script>)


            'load the state if we have a certain token in the url
            If RestoreSearch And SavedState IsNot Nothing Then

                'load the data into the controls
                BeginLoad()

                'check if we have a saved filter
                Dim usf = GetSavedSearch()
                If usf IsNot Nothing AndAlso usf.FilterID > 0 Then
                    FiltersLoadedImage.Visible = True
                End If

                'raise the search event
                OnSearch(New EventArgs())

            Else
                LoadStateFromDataBase()

            End If

        End If

        'save the state of the panel
        BeginSave()

        If searchToolBarDropDown.Buttons.Count = 0 Then
            searchToolBarDropDown.Visible = False
        End If

        MyBase.OnPreRender(e)
    End Sub
#End Region

#Region "State Methods"

    ''' <summary>
    ''' Recursively saves the search panel
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub BeginSave()
        'create blank xml doc for the states to be saved in
        Dim xml = <states>
                  </states>

        SaveState(ContentPanel.Controls, xml)

        SaveGridState(xml)

        'save the xml in the session
        SavedState = xml
    End Sub

    ''' <summary>
    ''' Recursively saves the state of each control in the content panel
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub SaveState(controlList As ControlCollection, xml As XElement)

        'loop through each control and add its state to the collection
        For Each ctrl As Control In controlList

            'look for child controls of Panels, RadAjaxPanels and UpdatePanels
            If (TypeOf ctrl Is Panel _
                Or TypeOf ctrl Is RadAjaxPanel) _
                And ctrl.Controls.Count > 0 Then

                SaveState(ctrl.Controls, xml)

            End If

            'create new event args
            Dim e As New SearchPanelSaveStateEventArgs
            e.Filter = ctrl

            'call state saving
            OnStateSaving(e)

            If Not DataIsEmpty(e.Data) Then

                'serialize the data to be stored in the states xml
                Using ms As New MemoryStream()

                    'start serializer
                    Dim s As New XmlSerializer(e.Data.GetType)

                    'serialize the data into byte stream
                    s.Serialize(ms, e.Data)

                    Dim value As String = ""
                    'convert to base64 from bytes
                    value = Convert.ToBase64String(ms.GetBuffer())

                    'write out visible xml
                    'ms.Position = 0
                    'Using sr As New StreamReader(ms)
                    '    value = sr.ReadToEnd()
                    'End Using

                    'save the value in the state
                    Dim state = <state>
                                    <control id=<%= e.Filter.ID %>/>
                                    <value type=<%= e.Data.GetType().ToString() %>><%= value %></value>
                                </state>

                    'add the state to the list of states
                    xml.Add(state)
                End Using
            End If

        Next



    End Sub

    ''' <summary>
    ''' Saves the grid's state
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub SaveGridState(xml As XElement)
        'saves the state of the grid
        If Not String.IsNullOrWhiteSpace(ListControlID) Then

            Dim foundControl = Me.Parent.FindControl(ListControlID)
            If foundControl IsNot Nothing Then
                Dim value As String = String.Empty

                'determine the type
                If TypeOf foundControl Is ISearchPanelBoundListControl Then
                    Dim ctrl As ISearchPanelBoundListControl = foundControl

                    'data will be stored in a | seperated value
                    value = String.Format("{0}|{1}|{2}|{3}",
                                          ctrl.ListPageIndex,
                                          ctrl.ListPageSize,
                                          ctrl.SortExpression,
                                          ctrl.SortOrder)


                End If

                'create a state element to append to the saved state
                Dim state = <state>
                                <control id=<%= ListControlID %>/>
                                <value type=<%= value.GetType().ToString() %>><%= value %></value>
                            </state>

                'add it to the saved state
                xml.Add(state)

            End If

        End If
    End Sub

    ''' <summary>
    ''' Recursively loads the search panel
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub BeginLoad()
        LoadState(ContentPanel.Controls)

        LoadGridState()
    End Sub

    ''' <summary>
    ''' Loads the data into the panel
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub LoadState(controlList As ControlCollection)

        'loop through each control and add its state to the collection
        For Each ctrl In controlList

            'look for child controls of Panels, RadAjaxPanels and UpdatePanels
            If (TypeOf ctrl Is Panel _
                Or TypeOf ctrl Is RadAjaxPanel) _
                And ctrl.Controls.Count > 0 Then

                LoadState(ctrl.Controls)

            End If

            'create new event args
            Dim e As New SearchPanelSaveStateEventArgs
            e.Filter = ctrl

            'find the corresponding control in the session
            Dim stateVal = (From st In SavedState.<state>
                            Where st.<control>.@id = e.Filter.ID
                            Select Data = st.<value>.Value, DataType = st.<value>.@type).FirstOrDefault()


            If stateVal IsNot Nothing Then

                'get the type from its name
                Dim a = Type.GetType(stateVal.DataType)

                'start serializer
                Dim s As New XmlSerializer(a)

                Dim bytes() As Byte = Nothing

                'convert from base64
                bytes = Convert.FromBase64String(stateVal.Data)

                'read from plain xml
                'bytes = System.Text.ASCIIEncoding.ASCII.GetBytes(stateVal.Data)

                'deserialize the data
                Try
                    Using ms As New MemoryStream(bytes)

                        'deserialize the data
                        e.Data = s.Deserialize(ms)

                    End Using
                Catch ex As Exception
                    'if something goes wrong. ignore.
                End Try


                'call state loading
                OnStateLoading(e)

            End If

        Next


    End Sub

    ''' <summary>
    ''' Loads the grid state
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub LoadGridState()

        'if we have a radgrid assigned
        If Not String.IsNullOrWhiteSpace(ListControlID) Then
            'find the corresponding grid control in the session
            Dim stateVal = (From st In SavedState.<state>
                            Where st.<control>.@id = ListControlID
                            Select Data = st.<value>.Value, DataType = st.<value>.@type).FirstOrDefault()

            If stateVal IsNot Nothing Then

                'restore the grid state
                Dim states = stateVal.Data.Split("|")
                If states.Count = 4 Then

                    Dim foundControl = Me.Parent.FindControl(ListControlID)
                    If TypeOf foundControl Is ISearchPanelBoundListControl Then
                        Dim ctrl As ISearchPanelBoundListControl = foundControl

                        ctrl.ListPageIndex = ToInteger(states(0))
                        ctrl.ListPageSize = ToInteger(states(1))
                        ctrl.SortExpression = states(2)
                        ctrl.SortOrder = ToInteger(states(3))


                    End If


                End If

            End If
        End If
    End Sub

    ''' <summary>
    ''' Handles retrieving the state of a control. The state is returned in the Data member of the event args
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub OnStateSaving(e As SearchPanelSaveStateEventArgs)

        'raise the event and handle user code first
        RaiseEvent StateSaving(Me, e)

        'check to see what kind of control this is and save its state
        'first check to see if it was handled by user code first
        If e.Handled Then Return

        'check control type
        If TypeOf e.Filter Is TextBox Then
            Dim ctrl As TextBox = e.Filter

            e.Data = ctrl.Text
        ElseIf TypeOf e.Filter Is CheckBox Then
            Dim ctrl As CheckBox = e.Filter
            'covers checkbox and radio button
            e.Data = ctrl.Checked
        ElseIf TypeOf e.Filter Is ListControl Then
            Dim ctrl As ListControl = e.Filter
            'covers all list controls (dropdown, checkbox list, radio button list, listbox, etc)
            Dim items = (From i As ListItem In ctrl.Items
                         Where i.Selected
                         Select i.Value).ToArray()

            e.Data = items
        ElseIf TypeOf e.Filter Is RadInputControl Then
            Dim ctrl As RadInputControl = e.Filter

            e.Data = ctrl.Text
        ElseIf TypeOf e.Filter Is RadDatePicker Then
            Dim ctrl As RadDatePicker = e.Filter

            e.Data = ctrl.SelectedDate
        ElseIf TypeOf e.Filter Is RadComboBox Then
            Dim ctrl As RadComboBox = e.Filter

            'normal dropdown
            If Not ctrl.CheckBoxes And Not ctrl.EnableLoadOnDemand Then
                e.Data = ctrl.SelectedValue
            ElseIf ctrl.CheckBoxes Then

                'check to see if the "Check All" option is selected. If it is, dont store an explicit
                'collection of items, just store that all are to be checked.
                If ctrl.EnableCheckAllItemsCheckBox _
                    AndAlso ctrl.CheckedItems.Count = ctrl.Items.Count Then
                    'set data to true
                    e.Data = True
                Else
                    'if not all items are checked, and check all is not enabled,
                    'then store selected items
                    Dim items = (From i In ctrl.CheckedItems
                                 Select i.Value).ToArray()
                    e.Data = items
                End If

            ElseIf ctrl.EnableLoadOnDemand Then
                Dim value = String.Format("{0}|{1}", ctrl.Text, ctrl.SelectedValue)

                e.Data = value
            End If

        End If

    End Sub

    ''' <summary>
    ''' Handles populating the panel from a saved state
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub OnStateLoading(e As SearchPanelSaveStateEventArgs)

        'raise the event and handle user code first
        RaiseEvent StateLoading(Me, e)

        'check to see what kind of control this is and save its state
        'first check to see if it was handled by user code first
        If e.Handled Then Return

        If TypeOf e.Filter Is TextBox Then
            Dim ctrl As TextBox = e.Filter
            ctrl.Font.Bold = True
            ctrl.Text = e.Data
        ElseIf TypeOf e.Filter Is CheckBox Then
            Dim ctrl As CheckBox = e.Filter

            ctrl.Checked = e.Data
        ElseIf TypeOf e.Filter Is ListControl Then
            Dim ctrl As ListControl = e.Filter
            ctrl.ClearSelection()
            Dim items As String() = e.Data

            Dim checkedItems = From i In items
                               Join c As ListItem In ctrl.Items On c.Value Equals i
                               Select c

            For Each checkedItem In checkedItems
                checkedItem.Selected = True
            Next
        ElseIf TypeOf e.Filter Is RadInputControl Then
            Dim ctrl As RadInputControl = e.Filter
            ctrl.Font.Bold = True

            ctrl.Text = e.Data
        ElseIf TypeOf e.Filter Is RadDatePicker Then
            Dim ctrl As RadDatePicker = e.Filter
            ctrl.Font.Bold = True

            ctrl.SelectedDate = e.Data
        ElseIf TypeOf e.Filter Is RadComboBox Then
            Dim ctrl As RadComboBox = e.Filter
            ctrl.Font.Bold = True
            'normal dropdown
            If Not ctrl.CheckBoxes And Not ctrl.EnableLoadOnDemand Then
                ctrl.SelectedValue = e.Data
            ElseIf ctrl.CheckBoxes Then
                'checkbox dropdown
                If TypeOf e.Data Is String() Then

                    'only certain items were selected
                    Dim items As String() = e.Data

                    Dim checkedItems = From i In items
                                       Join c In ctrl.Items On c.Value Equals i
                                       Select c

                    For Each checkedItem In checkedItems
                        checkedItem.Checked = True
                    Next

                ElseIf TypeOf e.Data Is Boolean Then
                    'its boolean which means check all was enabled, and all items were selected
                    For Each item As RadComboBoxItem In ctrl.Items
                        item.Checked = True
                    Next
                End If

            ElseIf ctrl.EnableLoadOnDemand Then
                Dim s As String = e.Data
                Dim value = s.Split("|")

                ctrl.Text = value(0)
                ctrl.SelectedValue = value(1)


            End If


        End If

    End Sub

    ''' <summary>
    ''' Gets the xml from the database and loads it into the session
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub LoadStateFromDataBase()
        Dim usf = GetSavedSearch()
        If usf IsNot Nothing AndAlso usf.FilterID > 0 Then
            'store xml in session
            Dim xdoc = XDocument.Parse(usf.SearchValue)

            'put it in the session
            SavedState = xdoc.<states>.SingleOrDefault()

            'load the values
            BeginLoad()

            'loaded from db
            FiltersLoadedImage.Visible = True

            'call search if flag is set
            If SearchOnLoadSavedState Then
                'raise the search event
                OnSearch(New EventArgs())
            End If

        End If

    End Sub

    ''' <summary>
    ''' Gets a saved filter from the db based on user id and form id
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function GetSavedSearch() As DataLibrary.UserSearchFilter
        Dim currentPage = SiteMap.CurrentNode
        If currentPage IsNot Nothing Then
            'get the current form
            Dim mf = MetaFormManager.GetByPath(currentPage.Url)
            If mf IsNot Nothing AndAlso mf.FormID > 0 AndAlso UserAuthentication.User IsNot Nothing Then
                'get the saved filter record
                Dim usf = UserSearchFilterManager.GetById(UserAuthentication.User.UserID, mf.FormID, Me.ID)
                If usf.FilterID = 0 Then
                    'save form, user, and search panel id
                    usf.FormID = mf.FormID
                    usf.UserID = UserAuthentication.User.UserID
                    usf.SearchName = Me.ID
                End If

                'return the saved filter object
                Return usf

            End If
        End If

        Return Nothing
    End Function

    ''' <summary>
    ''' Determines if the data is empty
    ''' </summary>
    ''' <param name="data"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function DataIsEmpty(data As Object) As Boolean

        'determine if the data has any data
        Dim isEmpty = (data Is Nothing _
                       OrElse (TypeOf data Is String AndAlso String.IsNullOrWhiteSpace(data)) _
                       OrElse (TypeOf data Is IEnumerable AndAlso (From i In DirectCast(data, IEnumerable)).Count() = 0)
                      )

        Return isEmpty

    End Function


#End Region

#Region "Protected Methods"
    Protected Sub OnSearch(e As EventArgs)

        'set the restore search value for this panel
        RestoreSearch = True

        'raise the search event
        RaiseEvent Search(Me, e)

        If Not String.IsNullOrWhiteSpace(ListControlID) Then

            Dim lc As Control = Me.Parent.FindControl(ListControlID)
            If TypeOf lc Is ISearchPanelBoundListControl Then
                Dim target As ISearchPanelBoundListControl = lc
                Dim scrollToTarget = Me.Parent.FindControl(target.ScrollToTargetID)

                If scrollToTarget IsNot Nothing Then
                    'inject script to scroll to specified target
                    Dim scrollJs = DecodeJS(<script>
                                                setTimeout(function(){MilesSearchPanel.scrollToTarget('#<%= scrollToTarget.ClientID %>');});
                                            </script>)

                    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "MilesSearchPanelScroll", scrollJs, True)

                End If
                
            End If

        End If
    End Sub

    Protected Sub OnClearSearch(e As ResetSearchEventArgs)
        'clear state
        SavedState = Nothing

        'reset restore search
        RestoreSearch = False

        'raise the reset event
        RaiseEvent Reset(Me, e)

        If Not e.CancelRedirect Then
            Page.Response.Redirect(Page.Request.RawUrl)
        End If

    End Sub

    Protected Sub OnSaveFilters()
        'save the state of the panel
        BeginSave()

        Dim usf = GetSavedSearch()

        If usf IsNot Nothing Then
            'add search filter
            usf.SearchValue = SavedState.ToString()

            'save the search filter
            UserSearchFilterManager.Save(usf)

            'show reset button
            FiltersLoadedImage.Visible = True

            'save the search filters in the database
            JGrowl.ShowMessage(JGrowlMessageType.Success, "Your search filters have been saved")
        Else
            JGrowl.ShowMessage(JGrowlMessageType.Error, "Could not save your filters because the form is missing from the sitemap")
        End If

    End Sub

    Protected Sub OnRemoveFilters(e As ResetSearchEventArgs)
        Dim currentPage = SiteMap.CurrentNode
        If currentPage IsNot Nothing Then
            'get the current form
            Dim mf = MetaFormManager.GetByPath(currentPage.Url)
            If mf.FormID > 0 Then
                'clear state
                SavedState = Nothing
                UserSearchFilterManager.Delete(UserAuthentication.User.UserID, mf.FormID, Me.ID)

                'hide reset button
                FiltersLoadedImage.Visible = False

                'reset restore search
                RestoreSearch = False

                'raise the reset event
                RaiseEvent Reset(Me, e)

                'message
                JGrowl.ShowMessage(JGrowlMessageType.Notification, "Your search filters have been reset", isDelayed:=Not e.CancelRedirect)

                'redirect
                If Not e.CancelRedirect Then
                    Page.Response.Redirect(Page.Request.RawUrl)
                End If


            End If
        End If
    End Sub
#End Region

#Region "Events"

    Protected Sub SearchBar_ButtonClick(sender As Object, e As RadToolBarEventArgs)

        If TypeOf e.Item Is RadToolBarButton Then
            Dim btn As RadToolBarButton = e.Item

            If btn.CommandName = "Search" Then
                'raise the search event
                OnSearch(New EventArgs())
            ElseIf btn.CommandName = "ClearSearch" Then
                'raise the clear event
                OnClearSearch(New ResetSearchEventArgs)
            ElseIf btn.CommandName = "SaveFilters" Then
                'raise the search event
                OnSaveFilters()
            ElseIf btn.CommandName = "RemoveFilters" Then
                'raise the search event
                OnRemoveFilters(New ResetSearchEventArgs())

            End If
        End If


        'raise event when the toolbar button is clicked
        RaiseEvent ToolBarButtonClick(Me, e)

    End Sub

    Protected Sub SearchButton_Click(sender As Object, e As EventArgs)

        'raise the search event
        OnSearch(New EventArgs())
    End Sub

    Protected Sub ResetButton_Click(sender As Object, e As EventArgs)

        'raise the clear event
        OnClearSearch(New ResetSearchEventArgs)
    End Sub
#End Region


End Class

Class SearchPanelClientState
    Public Property Expanded As Boolean
End Class
