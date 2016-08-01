Imports Microsoft.VisualBasic
Public Class Documents
    Public Shared Function GetItemRow(ByVal ObjectID As Integer, ByVal KeyID As Integer, ByVal path As String) As CoreService.Documents
        If path.EndsWith("/") Then
            path = path.Substring(0, path.Length - 1)
        End If
        Dim names As String() = path.Split("/"c)
        'Start search in root;
        Dim searchedRow As CoreService.Documents = Nothing
        Dim itemId As Integer = 0
        Dim i As Integer = 0
        Dim cClient As New CoreServiceClient()
        While i < names.Length
            Dim name As String = names(i)
            '  System.Web.HttpContext.Current.Response.Write("TenantID:" & System.Web.HttpContext.Current.Session("TenantID"))
            '   System.Web.HttpContext.Current.Response.End()
            Dim rows As CoreService.Documents() = cClient.DocumentsGetList(AppSession.TenantID, ObjectID, KeyID, name, itemId, -1, 1, -1, Nothing, Nothing)
            If rows.Count = 0 Then
                Return Nothing
            End If
            searchedRow = rows(0)
            itemId = searchedRow.DocumentID
            System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1)
        End While
        cClient.Close()
        Return searchedRow
    End Function

    Public Shared Function GetItemId(ByVal ObjectID As Integer, ByVal KeyID As Integer, ByVal path As String) As Integer
        Dim itemRow As CoreService.Documents = GetItemRow(ObjectID, KeyID, path)
        If itemRow Is Nothing Then
            Return -1
        End If
        Return itemRow.DocumentID
    End Function

    Public Shared Function GetChildFileRows(ByVal ObjectID As Integer, ByVal KeyID As Integer, ByVal path As String) As CoreService.Documents()
        Dim cClient As New CoreServiceClient()
        Dim rows As CoreService.Documents() = cClient.DocumentsGetList(AppSession.TenantID, ObjectID, KeyID, "", GetItemId(ObjectID, KeyID, path), 0, 1, -1, Nothing, Nothing)
        cClient.Close()
        Return rows
    End Function

    Public Shared Function GetChildDirectoryRows(ByVal ObjectID As Integer, ByVal KeyID As Integer, ByVal path As String) As CoreService.Documents()
        Dim cClient As New CoreServiceClient()
        Dim rows As CoreService.Documents() = cClient.DocumentsGetList(AppSession.TenantID, ObjectID, KeyID, "", GetItemId(ObjectID, KeyID, path), 1, 1, -1, Nothing, Nothing)
        cClient.Close()
        Return rows
    End Function

    Public Shared Function GetAllDirectoryRows(ByVal ObjectID As Integer, ByVal KeyID As Integer, ByVal path As String) As List(Of CoreService.Documents)
        Dim rootRow As CoreService.Documents = GetItemRow(ObjectID, KeyID, path)
        If rootRow Is Nothing Then
            Return New List(Of CoreService.Documents)
        End If
        Dim allDirectoryRows As New List(Of CoreService.Documents)
        allDirectoryRows.Add(rootRow)
        FillChildDirectoryRows(ObjectID, KeyID, rootRow.DocumentID, allDirectoryRows)
        Return allDirectoryRows
    End Function

    Public Shared Sub FillChildDirectoryRows(ByVal ObjectID As Integer, ByVal KeyID As Integer, ByVal parentId As Integer, ByVal toFill As List(Of CoreService.Documents))
        Dim cClient As New CoreServiceClient()
        Dim childRows As CoreService.Documents() = cClient.DocumentsGetList(AppSession.TenantID, ObjectID, KeyID, "", parentId, 1, 1, -1, Nothing, Nothing)
        cClient.Close()
        For Each childRow As CoreService.Documents In childRows
            toFill.Add(childRow)
            FillChildDirectoryRows(ObjectID, KeyID, childRow.DocumentID, toFill)
        Next
    End Sub

    Public Shared Function GetItemPath(ByVal row As CoreService.Documents) As String
        Dim cClient As New CoreServiceClient()
        If row.ParentID = 0 Then
            Return String.Format("{0}/", row.DocumentName)
        End If
        Dim parentId As Integer = row.ParentID
        Dim d As New CoreService.Documents
        Dim parents As CoreService.Documents = cClient.DocumentsGetByID(parentId, AppSession.TenantID)
        cClient.Close()
        If parents Is Nothing Then
            Return String.Format("/{0}", row.DocumentName)
        End If

        Return GetItemPath(parents) + String.Format("{0}/", row.DocumentName)
    End Function

    Public Shared Function IsItemExists(ByVal ObjectID As Integer, ByVal KeyID As Integer, ByVal pathToItem As String) As Boolean
        Dim row As CoreService.Documents = GetItemRow(ObjectID, KeyID, pathToItem)
        Return Not row Is Nothing
    End Function
End Class
