Imports System.Web.UI

Public Class SearchPanelSaveStateEventArgs
    Inherits EventArgs

    Public Property Filter As Control
    Public Property Data As Object
    Public Property Handled As Boolean
End Class
