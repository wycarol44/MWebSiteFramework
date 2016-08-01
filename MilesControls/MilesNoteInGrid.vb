Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports Telerik.Web.UI
Imports System.Text.RegularExpressions
Imports CommonLibrary

''' <summary>
''' Author: Pashupati Shrestha
''' </summary>
''' <remarks></remarks>
<ParseChildren(True)>
Public Class MilesNoteInGrid
    Inherits CompositeControl

#Region "Properties"

    ''' <summary>
    ''' Lenght of the Note need to be displayed on grid
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MaxSize As Integer
        Get
            Return ToInteger(ViewState("NotesInGridMax_Size"))
        End Get
        Set(ByVal value As Integer)

            ViewState("NotesInGridMax_Size") = value

        End Set
    End Property


    Public Property NotesHTML As String
        Get
            If ViewState("NotesHTML") IsNot Nothing Then
                Return ViewState("NotesHTML")
            Else
                Return ""
            End If
        End Get
        Set(ByVal value As String)
            ViewState("NotesHTML") = value

        End Set
    End Property

    Public Property NotesNote As String
        Get
            If ViewState("NotesNote") IsNot Nothing Then
                Return ViewState("NotesNote")
            Else
                Return ""
            End If
        End Get
        Set(ByVal value As String)
            ViewState("NotesNote") = value
        End Set
    End Property

    Public Property NotesLabel As Label
    Public Property LinkViewNotes As HyperLink

    'expost this control as child contol
    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public Property NotesDetailToolTip As RadToolTip

#End Region

#Region "Overrides"

#End Region

    Public Sub New()
        NotesLabel = New Label()
        LinkViewNotes = New HyperLink()
        If NotesDetailToolTip Is Nothing Then NotesDetailToolTip = New RadToolTip
        MaxSize = 100
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
    End Sub

    Protected Overrides Sub CreateChildControls()

        LinkViewNotes.Text = "..."
        LinkViewNotes.NavigateUrl = "javascript: void(0);"
        LinkViewNotes.ID = "LinkViewNotes"

        NotesDetailToolTip.EnableShadow = True
        NotesDetailToolTip.ShowCallout = True
        NotesDetailToolTip.ShowEvent = ToolTipShowEvent.OnClick
        NotesDetailToolTip.HideEvent = ToolTipHideEvent.ManualClose
        NotesDetailToolTip.ManualCloseButtonText = "Close"
        NotesDetailToolTip.RelativeTo = ToolTipRelativeDisplay.Element

        NotesDetailToolTip.Overlay = True
        NotesDetailToolTip.TargetControlID = LinkViewNotes.ID

        'add controls to panel
        Me.Controls.Add(NotesLabel)
        Me.Controls.Add(LinkViewNotes)
        Me.Controls.Add(NotesDetailToolTip)
        LoadValues()
        MyBase.CreateChildControls()

    End Sub

#Region "Methods"

    Private Sub LoadValues()

        If Regex.Replace(NotesNote, "<.*?>", "").Length > MaxSize Then
            NotesLabel.Text = Left(Regex.Replace(NotesNote, "<.*?>", ""), MaxSize)
            LinkViewNotes.Visible = True
        Else
            NotesLabel.Text = NotesNote
            LinkViewNotes.Visible = False
        End If

        NotesDetailToolTip.Text = NotesHTML

    End Sub

#End Region



End Class
