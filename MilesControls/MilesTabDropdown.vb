Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports CommonLibrary
Imports System.Web

Public Class MilesTabDropdown
    Inherits Panel
    Implements INamingContainer


#Region "Private Properties"

    Private ReadOnly Property tabdropdownInstance
        Get
            Return "tabDrop_" + Me.ID
        End Get
    End Property
#End Region

#Region "Public Properties"
    Property DataSource As Object
        Private Get
            Return CType(ViewState("DataSource"), Object)
        End Get

        Set(value As Object)
            ViewState("DataSource") = value
        End Set
    End Property

    Property DataTextField As String
        Private Get
            Return CStr(ViewState("DataTextField"))
        End Get
        Set(value As String)
            ViewState("DataTextField") = value
        End Set
    End Property

    Property DataValueField As String
        Private Get
            Return CStr(ViewState("DataValueField"))
        End Get
        Set(value As String)
            ViewState("DataValueField") = value
        End Set
    End Property

    ''' <summary>
    ''' Gets/Sets maximum length of title displayed on tab. If length of title is larger than specified length, the title is truncated with ellipses(...)
    ''' Tooltip will display the full tab title.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property TabTextMaxLength As Integer
        Private Get
            Dim retval As Integer = CInt(ViewState("TabTextMaxLength"))
            If retval = 0 Then
                retval = 15
            End If
            Return retval
        End Get
        Set(value As Integer)
            ViewState("TabTextMaxLength") = value
        End Set
    End Property

    ''' <summary>
    ''' Gets/Sets Css class for the tab
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property TabCssClass As String
        Private Get
            Return CStr(ViewState("TabCssClass"))
        End Get
        Set(value As String)
            ViewState("TabCssClass") = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the selected tab. To select a tab, use SelectTab Method
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property SelectedTab As MilesTabDropdownTabItem
        Get
            Return CType(ViewState("SelectedTab"), MilesTabDropdownTabItem)
        End Get
        Private Set(value As MilesTabDropdownTabItem)
            ViewState("SelectedTab") = value
        End Set
    End Property


    ''' <summary>
    ''' Gets or sets the client side event handler for tab selection
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OnClientTabSelectionChanged As String
        Get
            Return ViewState("OnClientTabSelectionChanged")
        End Get
        Set(value As String)
            ViewState("OnClientTabSelectionChanged") = value
        End Set
    End Property
#End Region

#Region "Events"
    ''' <summary>
    ''' This event is fired user clicks a tab to change the selection
    ''' </summary>
    ''' <remarks></remarks>
    Public Event OnTabSelectionChanged As EventHandler(Of MilesTabDropDownChangeEventArgs)

#End Region

    Dim SelectedTabValue As New HiddenField
    Dim TabsUL As New HtmlGenericControl("ul")
    Dim DropdownLI As New HtmlGenericControl("li")
    Dim DropdownLink As New HyperLink
    Dim DropdownUL As New HtmlGenericControl("ul")
    Dim TabsRepeater As New Repeater

#Region "Constructor"
    Sub New()

    End Sub
#End Region

#Region "Overrides"
    Protected Overrides Sub OnInit(e As EventArgs)
        SelectedTabValue.ID = "SelectedTabValue"



        TabsUL.ID = "TabsUL"
        TabsUL.Attributes.Add("class", " nav nav-tabs tabdropdown" & TabCssClass)

        DropdownLI.ID = "DropdownLI"
        DropdownLI.Attributes.Add("class", "dropdown pull-right hide")

        DropdownLink.ID = "DropdownLink"

        'set the initial text of dropdown,
  
        DropdownLink.Text = "More Options <span class='caret'></span>"

        DropdownLink.CssClass = "dropdown-toggle"
        DropdownLink.Attributes.Add("data-toggle", "dropdown")

        DropdownUL.Attributes.Add("class", "dropdown-menu")

        TabsRepeater.ID = "TabsRepeater"

        AddHandler TabsRepeater.ItemCommand, AddressOf TabsRepeater_ItemCommand
        AddHandler TabsRepeater.ItemDataBound, AddressOf TabsRepeater_ItemDataBound



        DropdownLI.Controls.Add(DropdownLink)
        DropdownLI.Controls.Add(DropdownUL)
        TabsUL.Controls.Add(DropdownLI)
        TabsUL.Controls.Add(TabsRepeater)

        Me.Controls.Add(SelectedTabValue)
        Me.Controls.Add(TabsUL)

        MyBase.OnInit(e)
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        If Page.IsPostBack Then
            If DataSource IsNot Nothing Then
                BindTabs()
            End If
        End If

        MyBase.OnLoad(e)
    End Sub

    Protected Overrides Sub OnPreRender(e As EventArgs)

        Dim milesTabDropDownJs = Page.ClientScript.GetWebResourceUrl(Me.GetType(), "MilesControls.MilesTabDropDown.js")
        System.Web.UI.ScriptManager.RegisterClientScriptInclude(Me.Page, Me.Page.GetType(), "MilesControls.MilesTabDropDown.js", milesTabDropDownJs)


        Dim cssUrl = Page.ClientScript.GetWebResourceUrl(Me.GetType(), "MilesControls.MilesTabDropDown.css")

        'include css
        Dim link = <link rel='stylesheet' text='text/css' href=<%= cssUrl %>/>
        Dim cssInclude As New LiteralControl(link.ToString())

        Page.Header.Controls.Add(cssInclude)


        'add script
        Dim js = HttpUtility.HtmlDecode(<script>
                              var <%= tabdropdownInstance %> = new MilesTabDropDown(
                                    '#<%= TabsUL.ClientID %>',
                                    '#<%= SelectedTabValue.ClientID %>',
                                    <%= If(Not String.IsNullOrWhiteSpace(Me.OnClientTabSelectionChanged), Me.OnClientTabSelectionChanged, "null") %>
                              );
                          </script>.Value)


        System.Web.UI.ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), Me.ID + "MilesTabDropDownInit", js, True)

        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "startupscript", tabdropdownInstance + ".beginLayout();", True)
        MyBase.OnPreRender(e)
    End Sub

    ''' <summary>
    ''' Binds the control using provided datasource, selectes first tab by default if no tab was selected earlier
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub BindTabs()

        TabsRepeater.DataSource = DataSource
        TabsRepeater.ItemTemplate = New MilesTabDropDownItemTemplate(DataTextField, DataValueField, TabTextMaxLength) With {.tabControl = Me}

        TabsRepeater.DataBind()

    End Sub


    ''' <summary>
    ''' selects the tab matching the tabvalue and raises TabSelected event. If no tab is found with the provided value, exception is thrown
    ''' </summary>
    ''' <param name="tabvalue"></param>
    ''' <remarks></remarks>
    Public Sub SelectTab(tabvalue As String)
        'find linkbutton with tabvalue commandargument, and update the hidden field with, li's client id
        'then call begin layout
        Dim tabbuttons = TabsRepeater.FindControls(Of LinkButton)("TabButton")

        If tabbuttons.Any Then
            Dim selectButton = tabbuttons.Where(Function(x) x.CommandArgument = tabvalue).FirstOrDefault()
            If selectButton IsNot Nothing Then
                SelectedTabValue.Value = Replace(selectButton.ClientID, "TabButton", "TabsLi")

                ' ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "selecttabscript", tabdropdownInstance + ".beginLayout();", True)


                SetSelectedTab(Nothing, selectButton, enableEventRaising:=False)

            Else
                Throw New Exception("No tab found with the value '" & tabvalue & "'")
            End If
        End If
    End Sub

    ''' <summary>
    ''' Resets the currently selected tab. First tab will be selected when the control rebinds next time.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ResetSelection()
        SelectedTab = Nothing
    End Sub

#End Region


    Private Sub TabsRepeater_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)
        Dim TabsLI = DirectCast(e.Item.FindControl("TabsLI"), HtmlGenericControl)
        Dim TabButton = DirectCast(e.Item.FindControl("TabButton"), LinkButton)

        'select the first tab if no tab is selected by default
        If SelectedTab Is Nothing Then
            SelectedTabValue.Value = TabsLI.ClientID
            SetSelectedTab(sender, TabButton, enableEventRaising:=False)
        Else
            'if a tab was selected, then update the hidden field with selectedtabs client id, so if the tabs were rearranged it will keep on selecting the correct one
            If TabButton.CommandArgument = SelectedTab.TabValue Then
                SelectedTabValue.Value = TabsLI.ClientID
                SetSelectedTab(sender, TabButton, enableEventRaising:=False)

            End If
        End If
    End Sub

    Private Sub TabsRepeater_ItemCommand(source As Object, e As RepeaterCommandEventArgs)
        If e.CommandName = "TabClicked" Then
            Dim TabsLI = DirectCast(e.Item.FindControl("TabsLI"), HtmlGenericControl)
            Dim TabButton As LinkButton = e.Item.FindControl("TabButton")

            SelectedTabValue.Value = TabsLI.ClientID

            SetSelectedTab(source, TabButton)
        End If
    End Sub


    Private Sub SetSelectedTab(sender As Object, tabbutton As LinkButton, Optional enableEventRaising As Boolean = True)
        Dim selTab As New MilesTabDropdownTabItem

        selTab.TabValue = tabbutton.CommandArgument
        selTab.TabText = If(Not String.IsNullOrEmpty(tabbutton.ToolTip), tabbutton.ToolTip, tabbutton.Text)

        SelectedTab = selTab

        If enableEventRaising Then RaiseEvent OnTabSelectionChanged(sender, New MilesTabDropDownChangeEventArgs With {.SelectedTab = selTab})
    End Sub


    Class MilesTabDropDownItemTemplate
        Implements ITemplate

        Public Property tabControl As MilesTabDropdown

        Dim _datatextfield As String
        Dim _datavaluefield As String
        Dim _tabtextmaxlength As Integer

        Public Sub New(datatextfield, datavaluefield, Optional tabtextmaxlength = 15)
            _datatextfield = datatextfield
            _datavaluefield = datavaluefield
            _tabtextmaxlength = tabtextmaxlength
        End Sub

        Public Sub InstantiateIn(container As System.Web.UI.Control) Implements ITemplate.InstantiateIn
            'create the controls we need
            Dim TabsLI As New HtmlGenericControl("li")
            Dim TabButton As New LinkButton

            TabsLI.ID = "TabsLi"

            TabButton.ID = "TabButton"
            TabButton.CausesValidation = False
            TabButton.CommandName = "TabClicked"

            AddHandler TabButton.DataBinding, AddressOf TabButton_DataBinding

            'add all to the container
            TabsLI.Controls.Add(TabButton)
            container.Controls.Add(TabsLI)
        End Sub

        Private Sub TabButton_DataBinding(sender As Object, e As EventArgs)
            Dim ctrl As LinkButton = sender

            Dim container As RepeaterItem = ctrl.NamingContainer

            Dim tabtext As String = DataBinder.Eval(container.DataItem, _datatextfield)

            If tabtext.Length > _tabtextmaxlength Then
                ctrl.Text = tabtext.Substring(0, _tabtextmaxlength) & "..."
                ctrl.ToolTip = tabtext
            Else
                ctrl.Text = tabtext
            End If

            Dim tabValue = DataBinder.Eval(container.DataItem, _datavaluefield)
            ctrl.CommandArgument = tabValue

            ctrl.Attributes.Add("onclick", "return " & tabControl.tabdropdownInstance & ".onTabClicked(this,'" & tabtext & "','" & tabValue & "')")


        End Sub
    End Class


End Class

<Serializable()> _
Public Class MilesTabDropdownTabItem
    Property TabValue As String
    Property TabText As String
End Class

Public Class MilesTabDropDownChangeEventArgs
    Property SelectedTab As MilesTabDropdownTabItem
End Class