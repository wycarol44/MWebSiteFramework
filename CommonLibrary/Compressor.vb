Imports System.IO
Imports System.IO.Compression

Public NotInheritable Class Compressor
    Private Sub New()
    End Sub

    Public Shared Function Compress(data As Byte()) As Byte()
        Dim output As New MemoryStream()
        Using gzip As New GZipStream(output, CompressionMode.Compress, True)
            gzip.Write(data, 0, data.Length)
        End Using
        Return output.ToArray()
    End Function

    Public Shared Function Decompress(data As Byte()) As Byte()
        Dim input As New MemoryStream()
        input.Write(data, 0, data.Length)
        input.Position = 0
        Dim output As New MemoryStream()
        Using gzip As New GZipStream(input, CompressionMode.Decompress, True)
            Dim buff As Byte() = New Byte(63) {}
            Dim read As Integer = -1
            read = gzip.Read(buff, 0, buff.Length)
            While read > 0
                output.Write(buff, 0, read)
                read = gzip.Read(buff, 0, buff.Length)
            End While
        End Using
        Return output.ToArray()
    End Function
End Class

