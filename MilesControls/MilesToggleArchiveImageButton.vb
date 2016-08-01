Imports System.Web.UI.WebControls

Public Class MilesToggleArchiveImageButton
    Inherits ImageButton

    Public Property ArchiveImageUrl As String
    Public Property RestoreImageUrl As String

    Public Property ArchiveText As String
    Public Property RestoreText As String

    Public Property ArchiveConfirmText As String
    Public Property RestoreConfirmText As String

    ''' <summary>
    ''' Gets or sets the archived value. If true, button switched to unarchive mode
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Archived As Boolean

    Public Sub New()


        ArchiveText = "Archive"
        RestoreText = "Restore"

        ArchiveConfirmText = "Are you sure you want to archive this record?"
        RestoreConfirmText = "Are you sure you want to restore this record?"

        Me.CommandName = "ToggleArchive"
    End Sub

    Protected Overrides Sub OnInit(e As EventArgs)

        'use the instance of page to get the clientscript object
        ArchiveImageUrl = Me.Page.ClientScript.GetWebResourceUrl(Me.GetType(), "MilesControls.archive-import.png")
        RestoreImageUrl = Me.Page.ClientScript.GetWebResourceUrl(Me.GetType(), "MilesControls.archive-export.png")

        MyBase.OnInit(e)
    End Sub

    Protected Overrides Sub OnPreRender(e As EventArgs)
        Dim confirmJs = "return confirm('{0}');"
        'check the archive value, and switch modes
        If Archived Then
            ImageUrl = RestoreImageUrl
            AlternateText = RestoreText
            ToolTip = RestoreText
            OnClientClick = String.Format(confirmJs, RestoreConfirmText)

        Else

            ImageUrl = ArchiveImageUrl
            AlternateText = ArchiveText
            ToolTip = ArchiveText
            OnClientClick = String.Format(confirmJs, ArchiveConfirmText)

        End If

        MyBase.OnPreRender(e)
    End Sub



End Class