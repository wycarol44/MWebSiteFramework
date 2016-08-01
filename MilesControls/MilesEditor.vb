Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.ComponentModel
Imports Telerik.Web.UI
Imports System.Web
Imports CommonLibrary

Public Class MilesEditor
    Inherits RadEditor

    Private _DefaultToolFiles As String = "~/App_Data/EditorToolFiles/BasicToolsFile.xml"
    Private _ExternalDialogsPath As String = "~/EditorDialogs/"

    Public Sub New()
        ToolsFile = _DefaultToolFiles
        ExternalDialogsPath = _ExternalDialogsPath
        StripFormattingOptions = Telerik.Web.UI.EditorStripFormattingOptions.ConvertWordLists Or Telerik.Web.UI.EditorStripFormattingOptions.MSWord
        Height = Unit.Pixel(250)
        EnableResize = False
        ToolbarMode = EditorToolbarMode.ShowOnFocus
        OnClientLoad = "MilesEditor_OnRadEditorClientLoad"
    End Sub

    Protected Overrides Sub OnPreRender(e As EventArgs)

        'output some javascript
        Dim milesEditorJs = Page.ClientScript.GetWebResourceUrl(Me.GetType(), "MilesControls.MilesEditor.js")

        'link javascript
        Page.ClientScript.RegisterClientScriptInclude(Me.GetType(), "MilesEditor", milesEditorJs)

        MyBase.OnPreRender(e)
    End Sub

    ''' <summary>
    ''' Set the editor paths for the RAD Editor
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SetEditorPaths()
        Dim DocumentsPath As String = ""

        DocumentsPath = AppSettings.DocumentsPath

        ImageManager.UploadPaths = New String() {DocumentsPath & "EditorImages"}
        ImageManager.DeletePaths = New String() {DocumentsPath & "EditorImages"}
        ImageManager.ViewPaths = New String() {DocumentsPath & "EditorImages"}

        DocumentManager.UploadPaths = New String() {DocumentsPath & "EditorDocuments"}
        DocumentManager.DeletePaths = New String() {DocumentsPath & "EditorDocuments"}
        DocumentManager.ViewPaths = New String() {DocumentsPath & "EditorDocuments"}

        FlashManager.UploadPaths = New String() {DocumentsPath & "EditorFlash"}
        FlashManager.DeletePaths = New String() {DocumentsPath & "EditorFlash"}
        FlashManager.ViewPaths = New String() {DocumentsPath & "EditorFlash"}

        MediaManager.UploadPaths = New String() {DocumentsPath & "EditorMedia"}
        MediaManager.DeletePaths = New String() {DocumentsPath & "EditorMedia"}
        MediaManager.ViewPaths = New String() {DocumentsPath & "EditorMedia"}

    End Sub
End Class
