
''' <summary>
''' Indicates an exception to be used as a warning
''' </summary>
''' <remarks></remarks>
Public Class WarningException
    Inherits Exception

    Public Sub New()

    End Sub
    Public Sub New(message As String)
        MyBase.New(message)
    End Sub
End Class