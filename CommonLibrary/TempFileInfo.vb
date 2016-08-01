Imports System.IO

Public Class TempFileInfo

    ''' <summary>
    ''' Gets the name of the file and extension (without the path)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OriginalFileName As String

    ''' <summary>
    ''' Gets the name of the file and extension (without the path)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FileName As String

    ''' <summary>
    ''' Gets the path without the filename
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PathName As String

    ''' <summary>
    ''' Gets the full path and filename
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property FilePath As String
        Get
            Return Path.Combine(PathName, FileName)
        End Get
    End Property

    Public Sub New(pathname As String, filename As String)
        Me.PathName = pathname
        Me.OriginalFileName = filename

        'generate a random filename, checking if the file already exists or not
        Dim newFilename As String = ""
        
        Me.FileName = String.Format("{0}-{1:MMddyyyy}-{2}", Guid.NewGuid().ToString(), Now, Me.OriginalFileName)

    End Sub
End Class
