Imports Telerik.Web.UI
Imports System.ComponentModel
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports CommonLibrary


Public Class MilesGrid
    Inherits RadGrid
    Implements ISearchPanelBoundListControl



    Private _containsActiveColumn As Boolean = False

#Region "Event Delegates"
    Public Event ToggleArchived As EventHandler(Of GridCommandEventArgs)
#End Region

#Region "Properties"

    Public Property DataArchivedCssClass As String
    Public Property DataArchivedCssClassAlternating() As String
    Public Property DataArchivedField As String

    Public Property AutoBind As Boolean
        Get
            Return ViewState("AutoBind")
        End Get
        Set(value As Boolean)
            ViewState("AutoBind") = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the script executed when the add new button is clicked
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AddNewOnClientClick As String
        Get
            Return ViewState("AddNewOnClientClick")
        End Get
        Set(value As String)
            ViewState("AddNewOnClientClick") = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the grid's associated ajax manager proxy. If set, history points will be enabled
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RadAjaxManagerID As String
        Get
            Return ViewState("RadAjaxManagerID")
        End Get
        Set(value As String)
            ViewState("RadAjaxManagerID") = value
        End Set
    End Property


    ''' <summary>
    ''' Gets or sets whether history points will be added to the script manager
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Property EnableHistory As Boolean
        Get
            Return ViewState("EnableHistory")
        End Get
        Set(value As Boolean)
            ViewState("EnableHistory") = value
        End Set
    End Property

#End Region

#Region "Override Properties"


#End Region

#Region "Methods"
    Public Sub New()
        MyBase.New()

        Me.MasterTableView.PagerStyle.PageSizes = New Integer() {10, 20, 50, 100}
        Me.AutoGenerateColumns = False
        Me.PagerStyle.AlwaysVisible = True


        Me.GroupingEnabled = False
        Me.AllowPaging = True
        Me.AllowSorting = True

        Me.MasterTableView.CommandItemSettings.ShowRefreshButton = False
        Me.MasterTableView.EditMode = GridEditMode.InPlace
        Me.MasterTableView.CommandItemSettings.AddNewRecordText = "Add New Record"

        'Me.ItemStyle.VerticalAlign = VerticalAlign.Top
        'Me.AlternatingItemStyle.VerticalAlign = VerticalAlign.Top

        'custom properties

        DataArchivedField = "Archived"
        DataArchivedCssClass = "rgDataArchived"
        DataArchivedCssClassAlternating = "rgDataArchivedAlt"
    End Sub

    Private Sub SetGridHistoryPoints(page As Integer, pageSize As Integer)

        If Not EnableHistory Then Return

        'set history points
        Dim sm = ScriptManager.GetCurrent(Me.Page)


        If sm.IsInAsyncPostBack And Not sm.IsNavigating Then
            'add history points
            sm.AddHistoryPoint("p", page)
            sm.AddHistoryPoint("ps", pageSize)

            Dim se = GetSortExpression()

            'add history points
            sm.AddHistoryPoint("s", se.FieldName)
            sm.AddHistoryPoint("so", CInt(se.SortOrder))

        End If
    End Sub

    ''' <summary>
    ''' Gets the current sort expression
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSortExpression() As GridSortExpression
        Dim sorting As GridSortExpression

        If Me.MasterTableView.SortExpressions.Count > 0 Then
            sorting = Me.MasterTableView.SortExpressions(0)
        Else
            sorting = New GridSortExpression()
            sorting.FieldName = ""
            sorting.SortOrder = GridSortOrder.None
        End If
        Return sorting
    End Function

#End Region

#Region "Overrides"
    ''' <summary>
    ''' Overrides the item command and raises appropriate events
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnItemCommand(e As GridCommandEventArgs)

        If e.CommandName.ToUpper() = "TOGGLEARCHIVED" Then
            RaiseEvent ToggleArchived(Me, e)
        End If

        MyBase.OnItemCommand(e)
    End Sub

    ''' <summary>
    ''' Overrides the item data bound event to perform custom actions
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnItemDataBound(e As GridItemEventArgs)

        If TypeOf e.Item Is GridCommandItem Then
            Dim item As GridCommandItem = e.Item

            If Not String.IsNullOrWhiteSpace(AddNewOnClientClick) Then
                Dim AddNewRecordButton As Button = item.FindControl("AddNewRecordButton")
                Dim InitInsertButton As LinkButton = item.FindControl("InitInsertButton")

                AddNewRecordButton.Attributes.Add("onclick", AddNewOnClientClick)
                InitInsertButton.Attributes.Add("onclick", AddNewOnClientClick)

            End If

        ElseIf TypeOf e.Item Is GridDataItem Then
            Dim item As GridDataItem = e.Item


            If Not String.IsNullOrWhiteSpace(DataArchivedField) Then

                'get the value of the archived field
                Dim archived As Boolean? = Nothing

                Try
                    'get field value if dataitem has it
                    archived = DataBinder.Eval(item.DataItem, DataArchivedField)
                Catch
                    'ignore if we dont have the field
                End Try

                'if we have a value
                If archived IsNot Nothing Then


                    'set each archive column
                    For Each col As GridColumn In Me.MasterTableView.Columns
                        If TypeOf col Is GridArchiveColumn Then
                            Dim archiveCol As GridArchiveColumn = col

                            archiveCol.ItemDataBound(e)

                        End If
                    Next


                    'set css classes of the alternating and normal items
                    If archived Then
                        If item.ItemType = GridItemType.Item Then
                            If Not String.IsNullOrWhiteSpace(DataArchivedCssClass) Then
                                'set the css class
                                item.CssClass = "rgRow " + DataArchivedCssClass
                            End If

                        ElseIf item.ItemType = GridItemType.AlternatingItem Then
                            If Not String.IsNullOrWhiteSpace(DataArchivedCssClassAlternating) Then
                                'set the css class
                                item.CssClass = "rgRow " + DataArchivedCssClassAlternating
                            End If
                        End If

                    End If

                End If


            End If

        End If


        MyBase.OnItemDataBound(e)
    End Sub

    Protected Overrides Sub OnPreRender(e As EventArgs)

        Dim cssUrl = Page.ClientScript.GetWebResourceUrl(Me.GetType(), "MilesControls.MilesGrid.css")

        'include css
        Dim link = "<link rel='stylesheet' text='text/css' href='{0}' />"
        Dim cssInclude As New LiteralControl(String.Format(link, cssUrl))

        Page.Header.Controls.Add(cssInclude)

        MyBase.OnPreRender(e)
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)

        'check to see if we have an ajax manager associated to the grid
        If Not String.IsNullOrWhiteSpace(RadAjaxManagerID) Then
            'get current script manager instance
            Dim sm = ScriptManager.GetCurrent(Me.Page)
            'add navigate handler
            AddHandler sm.Navigate, AddressOf ScriptManager_Navigate

            Dim ram As Control = Me.Parent.FindControl(RadAjaxManagerID)
            If ram IsNot Nothing Then

                If TypeOf ram Is RadAjaxManagerProxy Then
                    Dim aj As RadAjaxManagerProxy = ram

                    'add the script manager to the ajax manager proxy
                    aj.AjaxSettings.AddAjaxSetting(sm, Me)
                ElseIf TypeOf ram Is RadAjaxManager Then
                    Dim aj As RadAjaxManager = ram

                    'add the script manager to the ajax manager proxy
                    aj.AjaxSettings.AddAjaxSetting(sm, Me)
                End If

                'history points are enabled
                EnableHistory = True
            End If
        End If

        MyBase.OnLoad(e)
    End Sub

    ''' <summary>
    ''' overrides the onNeedDataSource event
    ''' </summary>
    ''' <param name="e"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overrides Function OnNeedDataSource(e As GridNeedDataSourceEventArgs) As Boolean

        'if autobind is off, we do not call this
        If AutoBind Then
            Me.MasterTableView.NoMasterRecordsText = "No records to display"
            Return MyBase.OnNeedDataSource(e)
        Else
            'show "click search" message
            Me.MasterTableView.NoMasterRecordsText = "Please click search to load the results"
            Me.DataSource = {}

            Return True
        End If
    End Function

    Protected Overrides Sub OnPageIndexChanged(e As GridPageChangedEventArgs)

        SetGridHistoryPoints(e.NewPageIndex, Me.PageSize)

        MyBase.OnPageIndexChanged(e)
    End Sub

    Protected Overrides Sub OnPageSizeChanged(e As GridPageSizeChangedEventArgs)

        SetGridHistoryPoints(Me.CurrentPageIndex, e.NewPageSize)

        MyBase.OnPageSizeChanged(e)
    End Sub

    Protected Overrides Sub OnSortCommand(e As GridSortCommandEventArgs)

        SetGridHistoryPoints(Me.CurrentPageIndex, Me.PageSize)

        MyBase.OnSortCommand(e)
    End Sub


#End Region

#Region "Events"
    Protected Sub ScriptManager_Navigate(sender As Object, e As System.Web.UI.HistoryEventArgs)

        If e.State.HasKeys Then
            Me.CurrentPageIndex = ToInteger(e.State("p"))
            Me.PageSize = ToInteger(e.State("ps"))

            If Not String.IsNullOrWhiteSpace(e.State("s")) Then
                Dim s As New Telerik.Web.UI.GridSortExpression
                s.FieldName = e.State("s")
                If Not String.IsNullOrWhiteSpace(e.State("so")) Then
                    s.SortOrder = ToInteger(e.State("so"))
                End If
                Me.MasterTableView.SortExpressions.AddSortExpression(s)
            End If
            AutoBind = True
            Me.Rebind()
        End If
    End Sub

#End Region

#Region "ISearchPanelBoundListControl"
    Public ReadOnly Property ScrollToTargetID As String Implements ISearchPanelBoundListControl.ScrollToTargetID
        Get
            Return Me.ID
        End Get
    End Property

    Public Property ListPageIndex As Integer Implements ISearchPanelBoundListControl.ListPageIndex
        Get
            Return Me.CurrentPageIndex
        End Get
        Set(value As Integer)
            Me.CurrentPageIndex = value
        End Set
    End Property

    Public Property ListPageSize As Integer Implements ISearchPanelBoundListControl.ListPageSize
        Get
            Return Me.PageSize
        End Get
        Set(value As Integer)
            Me.PageSize = value
        End Set
    End Property

    Public Property SortExpression As String Implements ISearchPanelBoundListControl.SortExpression
        Get
            Dim sorting = Me.GetSortExpression()

            Return sorting.FieldName
        End Get
        Set(value As String)
            If Me.MasterTableView.SortExpressions.Count = 0 Then
                Dim sorting As New GridSortExpression
                sorting.FieldName = value

                Me.MasterTableView.SortExpressions.Add(sorting)
            Else
                Me.MasterTableView.SortExpressions(0).FieldName = value
            End If
        End Set
    End Property

    Public Property SortOrder As Integer Implements ISearchPanelBoundListControl.SortOrder
        Get
            Dim sorting = Me.GetSortExpression()

            Return sorting.SortOrder
        End Get
        Set(value As Integer)
            If Me.MasterTableView.SortExpressions.Count = 0 Then
                Dim sorting As New GridSortExpression
                sorting.SortOrder = value

                Me.MasterTableView.SortExpressions.Add(sorting)
            Else
                Me.MasterTableView.SortExpressions(0).SortOrder = value
            End If
        End Set
    End Property





#End Region

    
End Class

Public Class GridArchiveColumn
    Inherits GridButtonColumn

#Region "Properties"
    Public Property DataArchivedField As String

    Public Property ArchiveImageUrl As String
    Public Property RestoreImageUrl As String

    Public Property ArchiveText As String
    Public Property RestoreText As String

    Public Property ArchiveConfirmText As String
    Public Property RestoreConfirmText As String


#End Region

    Public Sub New()
        Me.ButtonType = GridButtonColumnType.ImageButton
        Me.CommandName = "ToggleArchived"
        Me.UniqueName = "ToggleArchived"

        Using p As New Page

            'use the instance of page to get the clientscript object
            ArchiveImageUrl = p.ClientScript.GetWebResourceUrl(Me.GetType(), "MilesControls.Archive_16x16.png")
            RestoreImageUrl = p.ClientScript.GetWebResourceUrl(Me.GetType(), "MilesControls.Restore_16x16.png")

        End Using

        ArchiveText = "Archive"
        RestoreText = "Restore"

        ArchiveConfirmText = "Are you sure you want to archive this record?"
        RestoreConfirmText = "Are you sure you want to restore this record?"

        DataArchivedField = "Archived"

        'hides the column while in edit mode
        Me.ShowInEditForm = False
        Me.HeaderStyle.Width = "16"

    End Sub

    Public Overrides Function Clone() As GridColumn
        Dim newGridArchiveColumn As New GridArchiveColumn()

        newGridArchiveColumn.CopyBaseProperties(Me)

        Return newGridArchiveColumn
    End Function

    Public Sub ItemDataBound(e As GridItemEventArgs)

        If TypeOf e.Item Is GridDataItem Then
            Dim item As GridDataItem = e.Item

            'get the cell
            Dim tc = item(Me)

            'get the archived property
            Dim archived = DataBinder.Eval(item.DataItem, DataArchivedField)

            'check the type of button
            If TypeOf tc.Controls(0) Is ImageButton Then
                Dim btn As ImageButton = tc.Controls(0)

                'text
                btn.AlternateText = GetText(archived)
                btn.ToolTip = btn.AlternateText
                'image
                btn.ImageUrl = GetImageUrl(archived)
            ElseIf TypeOf tc.Controls(0) Is LinkButton Then
                Dim btn As LinkButton = tc.Controls(0)

                'text
                btn.Text = GetText(archived)
                btn.ToolTip = btn.Text
            ElseIf TypeOf tc.Controls(0) Is Button Then
                Dim btn As Button = tc.Controls(0)

                'text
                btn.Text = GetText(archived)
                btn.ToolTip = btn.Text
            End If

            'our control is generic
            Dim ctrl As WebControl = tc.Controls(0)

            'get the confirm text
            Dim cText = GetConfirmText(archived)

            'set the confirm text
            If Not String.IsNullOrWhiteSpace(cText) Then
                ctrl.Attributes.Add("onclick", String.Format("return confirm('{0}');", cText))
            End If

        End If

    End Sub


#Region "Private Methods"

    Private Function GetImageUrl(archived As Boolean) As String
        Return If(archived,
                  RestoreImageUrl,
                  ArchiveImageUrl)
    End Function

    Private Function GetConfirmText(archived As Boolean) As String
        Return If(archived,
                  RestoreConfirmText,
                  ArchiveConfirmText)
    End Function

    Private Function GetText(archived As Boolean) As String
        Return If(archived,
                  RestoreText,
                  ArchiveText)
    End Function

#End Region


End Class


'Public Class GridEncryptedHyperLinkColumn
'    Inherits GridHyperLinkColumn

'    Protected Overrides Function FormatDataNavigateUrlValue(dataUrlValues() As Object) As String

'        'get the format url
'        Dim url = Me.DataNavigateUrlFormatString

'        Return GlobalFunctions.GetEncryptedUrl(url, dataUrlValues)

'    End Function

'End Class
