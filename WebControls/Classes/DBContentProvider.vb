Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports Telerik.Web.UI.Widgets
Imports System.Collections.Generic
Imports System.Collections
Imports CommonLibrary
Imports BusinessLibrary


Public Class DBContentProvider
    Inherits FileBrowserContentProvider

#Region "Properties"

    Private _itemHandlerPath As String
    Private ReadOnly Property ItemHandlerPath() As String
        Get
            Return _itemHandlerPath
        End Get
    End Property

    Public ReadOnly Property ObjectID As Integer
        Get
            Return ToInteger(Context.Session("Documents_Control_ObjectID").ToString)
        End Get

    End Property

    Public ReadOnly Property KeyID As Integer
        Get
            Return ToInteger(Context.Session("Documents_Control_KeyID").ToString)
        End Get

    End Property

    Public ReadOnly Property DocumentList As List(Of DataLibrary.Documents_GetList_Result)
        Get
            If Context.Session("DocumentsList" & ObjectID & "_" & KeyID) Is Nothing Then
                Context.Session("DocumentsList" & ObjectID & "_" & KeyID) = DocumentManager.GetList(ObjectID, KeyID)
            End If

            Return DirectCast(Context.Session("DocumentsList" & ObjectID & "_" & KeyID), List(Of DataLibrary.Documents_GetList_Result))

        End Get
    End Property

#End Region

#Region "Constructor"

    Public Sub New(ByVal context As HttpContext, ByVal searchPatterns As String(), ByVal viewPaths As String(), ByVal uploadPaths As String(), ByVal deletePaths As String(), ByVal selectedUrl As String, ByVal selectedItemTag As String)
        MyBase.New(context, searchPatterns, viewPaths, uploadPaths, deletePaths, selectedUrl, selectedItemTag)

        _itemHandlerPath = "~/Handlers/DocumentsHandler.ashx"
        If _itemHandlerPath.StartsWith("~/") Then
            _itemHandlerPath = AppSettings.AdminSiteURL + _itemHandlerPath.Substring(1)
        End If
        If selectedItemTag <> Nothing AndAlso selectedItemTag <> String.Empty Then
            selectedItemTag = ExtractPath(RemoveProtocolNameAndServerName(selectedItemTag))
        End If
    End Sub

#End Region

#Region "Methods"

    Private Sub ClearDataListSession()
        Context.Session("DocumentsList" & ObjectID & "_" & KeyID) = Nothing
    End Sub

    Private Function GetItemUrl(ByVal virtualItemPath As String) As String
        Dim escapedPath As String = Context.Server.UrlEncode(virtualItemPath)
        Return String.Format("{0}?path={1}&ObjectID={2}&KeyID={3}", ItemHandlerPath, escapedPath, ObjectID, KeyID)

    End Function

    Private Function ExtractPath(ByVal itemUrl As String) As String
        If itemUrl = Nothing Then
            Return String.Empty
        End If
        If itemUrl.StartsWith(_itemHandlerPath) Then
            Return itemUrl.Substring(GetItemUrl(String.Empty).Length)
        End If
        Return itemUrl
    End Function

    Private Function GetName(ByVal path As String) As String
        If [String].IsNullOrEmpty(path) OrElse path = "/" Then
            Return String.Empty
        End If
        path = VirtualPathUtility.RemoveTrailingSlash(path)
        Return path.Substring(path.LastIndexOf("/"c) + 1)
    End Function

    Private Function GetDirectoryPath(ByVal path As String) As String
        Return path.Substring(0, path.LastIndexOf("/"c) + 1)
    End Function

    Private Function IsChildOf(ByVal parentPath As String, ByVal childPath As String) As Boolean
        Return childPath.StartsWith(parentPath)
    End Function

    Private Function CombinePath(ByVal path1 As String, ByVal path2 As String) As String
        If path1.EndsWith("/") Then
            Return String.Format("{0}{1}", path1, path2)
        End If
        If path1.EndsWith("\") Then
            path1 = path1.Substring(0, path1.Length - 1)
        End If
        Return String.Format("{0}/{1}", path1, path2)
    End Function

    Private Function GetChildDirectories(ByVal path As String) As DirectoryItem()

        Dim directories As New List(Of DirectoryItem)()
        Try
            Dim childRows = GetChildDirectoryRows(ObjectID, KeyID, path, DocumentList)
            Dim i As Integer = 0
            While i < childRows.Count
                Dim childRow = childRows(i)
                Dim name As String = childRow.DocumentName.ToString()
                Dim itemFullPath As String = VirtualPathUtility.AppendTrailingSlash(CombinePath(path, name))

                ' The files are added in ResolveDirectory() 
                ' Directories are added in ResolveRootDirectoryAsTree()
                Dim newDirItem As New DirectoryItem(name, String.Empty, itemFullPath, String.Empty, GetPermissions(itemFullPath), Nothing, _
                 Nothing)

                directories.Add(newDirItem)
                i = i + 1
            End While
            Return directories.ToArray()
        Catch generatedExceptionName As Exception
            Return New DirectoryItem() {}
        End Try
        Return New DirectoryItem() {}
    End Function

    Private Function GetChildFiles(ByVal _path As String) As FileItem()
        Try
            Dim childRows = GetChildFileRows(ObjectID, KeyID, _path, DocumentList)
            Dim files As New List(Of FileItem)()

            Dim i As Integer = 0
            While i < childRows.Count
                Dim childRow = childRows(i)
                Dim name As String = childRow.DocumentName
                If IsExtensionAllowed(System.IO.Path.GetExtension(name)) Then
                    Dim itemFullPath As String = CombinePath(_path, name)

                    Dim newFileItem As New FileItem(name, Path.GetExtension(name), childRow.Size, itemFullPath, GetItemUrl(itemFullPath), String.Empty, _
                     GetPermissions(itemFullPath))

                    files.Add(newFileItem)
                End If
                System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1)
            End While
            Return files.ToArray()
        Catch generatedExceptionName As Exception
            Return New FileItem() {}
        End Try
    End Function

    Private Function IsExtensionAllowed(ByVal extension As String) As Boolean
        Return Array.IndexOf(SearchPatterns, "*.*") >= 0 OrElse Array.IndexOf(SearchPatterns, "*" + extension.ToLower()) >= 0
    End Function

    ''' <summary>
    ''' Checks Upload permissions
    ''' </summary>
    ''' <param name="path">Path to an item</param>
    ''' <returns></returns>
    Private Function HasUploadPermission(ByVal path As String) As Boolean
        For Each uploadPath As String In Me.UploadPaths
            If path.StartsWith(uploadPath, StringComparison.CurrentCultureIgnoreCase) Then
                Return True
            End If
        Next
        Return False
    End Function

    ''' <summary>
    ''' Checks Delete permissions
    ''' </summary>
    ''' <param name="path">Path to an item</param>
    ''' <returns></returns>
    Private Function HasDeletePermission(ByVal path As String) As Boolean
        For Each deletePath As String In Me.DeletePaths
            If path.StartsWith(deletePath, StringComparison.CurrentCultureIgnoreCase) Then
                Return True
            End If
        Next
        Return False
    End Function

    ''' <summary>
    ''' Returns the permissions for the provided path
    ''' </summary>
    ''' <param name="pathToItem">Path to an item</param>
    ''' <returns></returns>
    Private Function GetPermissions(ByVal pathToItem As String) As PathPermissions
        Dim permission As PathPermissions = PathPermissions.Read
        permission = If(HasUploadPermission(pathToItem), permission Or PathPermissions.Upload, permission)
        permission = If(HasDeletePermission(pathToItem), permission Or PathPermissions.Delete, permission)

        Return permission
    End Function

#End Region

#Region "Override Methods"
    ''' <summary>
    ''' Loads a root directory with given path, where all subdirectories 
    ''' contained in the SelectedUrl property are loaded
    ''' </summary>
    ''' <remarks>
    ''' The ImagesPaths, DocumentsPaths, etc properties of RadEditor
    ''' allow multiple root items to be specified, separated by comma, e.g.
    ''' Photos,Paintings,Diagrams. The FileBrowser class calls the 
    ''' ResolveRootDirectoryAsTree method for each of them.
    ''' </remarks>
    ''' <param name="path">the root directory path, passed by the FileBrowser</param>
    ''' <returns>The root DirectoryItem or null if such does not exist</returns>
    Public Overloads Overrides Function ResolveRootDirectoryAsTree(ByVal path As String) As DirectoryItem
        ' The files  are added in ResolveDirectory()
        Dim returnValue As New DirectoryItem(GetName(path), GetDirectoryPath(path), path, String.Empty, GetPermissions(path), Nothing, _
        GetChildDirectories(path))
        For Each FileItem As DirectoryItem In returnValue.Directories
            Dim itemId As Integer = GetItemId(ObjectID, KeyID, FileItem.Path, DocumentList)
            If itemId > 0 Then

                Dim d = DocumentList.Where(Function(n) n.DocumentID = itemId).FirstOrDefault

                FileItem.Attributes.Add("DateCreated", FormatDateTime(d.DateCreated, DateFormat.ShortDate))
                FileItem.Attributes.Add("CreatedBy", d.UserFullName)
            Else
                FileItem.Attributes.Add("DateCreated", "")
                FileItem.Attributes.Add("CreatedBy", "")
            End If
        Next

        Return returnValue
    End Function

    Public Overloads Overrides Function ResolveDirectory(ByVal path As String) As DirectoryItem
        Dim directories As DirectoryItem() = GetChildDirectories(path)

        ' Directories are added in ResolveRootDirectoryAsTree()
        Dim returnValue As New DirectoryItem(GetName(path), VirtualPathUtility.AppendTrailingSlash(GetDirectoryPath(path)), path, String.Empty, GetPermissions(path), GetChildFiles(path), _
         Nothing)

        For Each FileItem As FileItem In returnValue.Files
            Dim itemId As Integer = GetItemId(ObjectID, KeyID, FileItem.Path, DocumentList)
            If itemId > 0 Then
                Dim d = DocumentList.Where(Function(n) n.DocumentID = itemId).FirstOrDefault

                FileItem.Attributes.Add("DateCreated", FormatDateTime(d.DateCreated, DateFormat.ShortDate))
                FileItem.Attributes.Add("CreatedBy", d.UserFullName)

            Else
                FileItem.Attributes.Add("DateCreated", "")
                FileItem.Attributes.Add("CreatedBy", "")
            End If
        Next

        Return returnValue
    End Function

    Public Overloads Overrides Function GetFileName(ByVal url As String) As String
        Return GetName(url)
    End Function

    Public Overloads Overrides Function GetPath(ByVal url As String) As String
        Return GetDirectoryPath(ExtractPath(RemoveProtocolNameAndServerName(url)))
    End Function

    Public Overloads Overrides Function GetFile(ByVal url As String) As Stream
        Dim newItemPath As String = ExtractPath(RemoveProtocolNameAndServerName(url))
        Dim itemId As Integer = GetItemId(ObjectID, KeyID, newItemPath, DocumentList)

        Dim d As DataLibrary.Documents_GetList_Result = DocumentList.Where(Function(n) n.DocumentID = itemId).FirstOrDefault

        Dim content As Byte() = Nothing
        If Not IsNothing(d) AndAlso d.FilePath <> String.Empty Then
            Dim documentFolder As String = AppSettings.UploadedDocumentsFolder
            Dim FilePath As String = documentFolder & d.FilePath
            Using inputStream As FileStream = File.OpenRead(FilePath)
                Dim size As Long = inputStream.Length
                content = New Byte(size) {}
                inputStream.Read(content, 0, Convert.ToInt32(size))
            End Using
        End If

        If Not [Object].Equals(content, Nothing) Then
            Return New MemoryStream(content)
        End If
        Return Nothing
    End Function

    Public Overloads Overrides Function StoreBitmap(ByVal bitmap As Bitmap, ByVal url As String, ByVal format As ImageFormat) As String
        Dim newItemPath As String = ExtractPath(RemoveProtocolNameAndServerName(url))
        Dim name As String = GetName(newItemPath)
        Dim _path As String = GetPath(newItemPath)
        Dim tempFilePath As String = System.IO.Path.GetTempFileName()

        bitmap.Save(tempFilePath)
        Dim content As Byte()
        Using inputStream As FileStream = File.OpenRead(tempFilePath)
            Dim size As Long = inputStream.Length
            content = New Byte(size) {}
            inputStream.Read(content, 0, Convert.ToInt32(size))
        End Using

        Dim parentId = GetItemId(ObjectID, KeyID, _path, DocumentList)
        Dim d As New DataLibrary.Document
        d.ObjectID = ObjectID
        d.KeyID = KeyID
        d.DocumentName = name
        d.ParentID = parentId
        d.MimeType = "image/gif"
        If File.Exists(tempFilePath) Then
            d.FilePath = System.IO.Path.GetFileName(tempFilePath)
        End If
        d.Size = content.Length
        d.IsDirectory = False
        DocumentManager.Save(d)

        Return String.Empty
    End Function

    Public Overloads Overrides Function MoveFile(ByVal path As String, ByVal newPath As String) As String
        Try

            Dim destFileExists As Boolean = IsItemExists(ObjectID, KeyID, newPath, DocumentList)

            If destFileExists Then
                Return "A file with the same name exists in the destination folder"
            End If

            Dim newFileName As String = GetName(newPath)
            Dim destinationDirPath As String = newPath.Substring(0, newPath.Length - newFileName.Length)

            If destinationDirPath.Length = 0 Then
                destinationDirPath = path.Substring(0, path.LastIndexOf("/"))
            End If

            ' destination directory row
            Dim newPathRow = GetItemRow(ObjectID, KeyID, destinationDirPath, DocumentList)
            Dim itemId = GetItemId(ObjectID, KeyID, path, DocumentList)

            Dim d = DocumentManager.GetById(itemId)
            '  Dim d = DocumentList.Where(Function(n) n.DocumentID = itemId).FirstOrDefault
            d.DocumentName = newFileName
            d.ParentID = newPathRow.DocumentID
            DocumentManager.Save(d)

        Catch e As Exception
            Return e.Message
        End Try

        ClearDataListSession()
        Return String.Empty
    End Function

    Public Overloads Overrides Function MoveDirectory(ByVal path As String, ByVal newPath As String) As String

        If newPath.EndsWith("/") Then
            newPath = newPath.Remove(newPath.Length - 1, 1)
        End If
        Dim destFileExists As Boolean = IsItemExists(ObjectID, KeyID, newPath, DocumentList)
        If destFileExists Then
            Return "A directory with the same name exists in the destination folder"
        End If
        ClearDataListSession()
        Return MoveFile(path, newPath)
    End Function

    Public Overloads Overrides Function CopyFile(ByVal path As String, ByVal newPath As String) As String
        Try

            Dim destFileExists As Boolean = IsItemExists(ObjectID, KeyID, newPath, DocumentList)
            If destFileExists Then
                Return "A file with the same name exists in the destination folder"
            End If

            Dim newFileName As String = GetName(newPath)
            Dim newFilePath As String = newPath.Substring(0, newPath.Length - newFileName.Length)
            If newFilePath.Length = 0 Then
                newFilePath = path.Substring(0, path.LastIndexOf("/"))
            End If
            Dim oldPathRow = GetItemRow(ObjectID, KeyID, path, DocumentList)
            Dim parentId As Integer = GetItemId(ObjectID, KeyID, newFilePath, DocumentList)
            Dim d As New DataLibrary.Document
            d.ObjectID = ObjectID
            d.KeyID = KeyID
            d.DocumentName = newFileName
            d.ParentID = parentId
            d.MimeType = oldPathRow.MimeType
            If oldPathRow.FilePath <> String.Empty Then
                Dim documentFolder As String = AppSettings.UploadedDocumentsFolder
                If File.Exists(documentFolder & oldPathRow.FilePath) Then
                    Dim copyFileName As String = ""
                    Dim ntf As New TempFileInfo(documentFolder, oldPathRow.FilePath)

                    copyFileName = ntf.FilePath
                    File.Copy(documentFolder & oldPathRow.FilePath, copyFileName)
                    d.FilePath = copyFileName
                End If
            End If

            d.Size = oldPathRow.Size
            d.IsDirectory = oldPathRow.IsDirectory

            DocumentManager.Save(d)

            If oldPathRow.IsDirectory Then
                'copy all child items of the folder as well
                Dim files As FileItem() = GetChildFiles(path)
                For Each childFile As FileItem In files
                    CopyFile(childFile.Tag, CombinePath(newPath, childFile.Name))
                Next
                'copy all child folders as well
                Dim subFolders As DirectoryItem() = GetChildDirectories(path)
                For Each subFolder As DirectoryItem In subFolders
                    CopyFile(subFolder.Tag, CombinePath(newPath, subFolder.Name))
                Next
            End If
        Catch e As Exception
            Return e.Message
        End Try
        ClearDataListSession()
        Return String.Empty
    End Function

    Public Overloads Overrides Function CopyDirectory(ByVal path As String, ByVal newPath As String) As String

        Dim pathParts As String() = path.Split(New Char() {"/"c}, StringSplitOptions.RemoveEmptyEntries)
        If pathParts.Length > 0 Then

            Dim fullNewPath As String = CombinePath(newPath, pathParts(pathParts.Length - 1))
            Dim destFileExists As Boolean = IsItemExists(ObjectID, KeyID, fullNewPath, DocumentList)
            If destFileExists Then
                Return "A file with the same name exists in the destination folder"
            End If

            Return CopyFile(path, fullNewPath)
        Else
            Return "Old path is invalid"
        End If
        ClearDataListSession()
    End Function

    Public Overloads Overrides Function StoreFile(ByVal file As Telerik.Web.UI.UploadedFile, ByVal path As String, ByVal name As String, ByVal ParamArray arguments As String()) As String

        Dim uploadedFile As Telerik.Web.UI.UploadedFile = file

        Dim documentFolder As String = AppSettings.UploadedDocumentsFolder
        Dim uploadedDocument As New TempFileInfo(documentFolder, uploadedFile.FileName)

        Dim documentFileName As String = uploadedDocument.FilePath

        'save the document
        uploadedFile.SaveAs(documentFileName)

        'get file length
        Dim fileLength As Integer = file.ContentLength
        'get file path
        Dim fullPath As String = CombinePath(path, name)
        If Not [Object].Equals(GetItemRow(ObjectID, KeyID, fullPath, DocumentList), Nothing) Then

            Dim itemId As Integer = GetItemId(ObjectID, KeyID, fullPath, DocumentList)
            Dim d = DocumentManager.GetById(itemId)
            d.FilePath = uploadedDocument.FileName
            DocumentManager.Save(d)

        Else
            Dim parentId As Integer = GetItemId(ObjectID, KeyID, path, DocumentList)

            Dim d As New DataLibrary.Document
            d.ObjectID = ObjectID
            d.KeyID = KeyID
            d.DocumentName = name
            d.ParentID = parentId
            d.MimeType = file.ContentType
            d.FilePath = uploadedDocument.FileName
            d.Size = fileLength
            d.IsDirectory = False

            DocumentManager.Save(d)

        End If
        ClearDataListSession()
        Return String.Empty
    End Function

    Public Overloads Overrides Function DeleteFile(ByVal path As String) As String

        Dim itemId As Integer = GetItemId(ObjectID, KeyID, path, DocumentList)

        Dim d = DocumentManager.GetById(itemId)

        If d.FilePath <> String.Empty Then
            Try
                Dim documentFolder As String = AppSettings.UploadedDocumentsFolder
                If File.Exists(documentFolder & d.FilePath) Then
                    File.Delete(documentFolder & d.FilePath)
                End If
            Catch ex As Exception

            End Try
        End If
        'write function for delete
        DocumentManager.Delete(d)
        ClearDataListSession()
        Return String.Empty

    End Function

    Public Overloads Overrides Function DeleteDirectory(ByVal path As String) As String

        Dim itemId As Integer = GetItemId(ObjectID, KeyID, path, DocumentList)

        'Delete all files and folders below the hierarchy
        'Write Recursive function to delete folders/files Below the hierarchy
        Dim dl = DocumentList.Where(Function(n) n.ParentID = itemId).ToList
        If dl.Any Then
            For Each doc As DataLibrary.Documents_GetList_Result In dl
                If doc.IsDirectory Then
                    DeleteDirectory(GetItemPath(doc, DocumentList))
                Else
                    DeleteFile(GetItemPath(doc, DocumentList))
                End If
            Next
        End If

        Dim d = DocumentManager.GetById(itemId)
        DocumentManager.Delete(d)

        ClearDataListSession()
        Return String.Empty
    End Function

    Public Overloads Overrides Function CreateDirectory(ByVal path As String, ByVal name As String) As String
        Dim parentId As Integer = GetItemId(ObjectID, KeyID, path, DocumentList)

        Dim d As New DataLibrary.Document
        d.ObjectID = ObjectID
        d.KeyID = KeyID
        d.DocumentName = name
        d.ParentID = parentId
        d.MimeType = String.Empty
        d.Size = 0
        d.IsDirectory = True
        DocumentManager.Save(d)
        ClearDataListSession()
        Return String.Empty
    End Function

    Public Overloads Overrides ReadOnly Property CanCreateDirectory() As Boolean
        Get
            Return True
        End Get
    End Property
#End Region

#Region "Document Display"

    Public Shared Function GetItemRow(ByVal ObjectID As Integer, ByVal KeyID As Integer, ByVal path As String, Optional DocumentList As List(Of DataLibrary.Documents_GetList_Result) = Nothing) As DataLibrary.Documents_GetList_Result
        If path.EndsWith("/") Then
            path = path.Substring(0, path.Length - 1)
        End If
        Dim names As String() = path.Split("/"c)
        'Start search in root;
        Dim searchedRow As DataLibrary.Documents_GetList_Result = Nothing
        Dim itemId As Integer = 0
        Dim i As Integer = 0

        While i < names.Length
            Dim name As String = names(i)
            'System.Web.HttpContext.Current.Response.Write("TenantID:" & System.Web.HttpContext.Current.Session("TenantID"))
            'System.Web.HttpContext.Current.Response.End()
            Dim rows As List(Of DataLibrary.Documents_GetList_Result)

            If DocumentList Is Nothing Then
                'documentList is passed as nothing when flusing the file
                rows = DocumentManager.GetList(ObjectID, KeyID, name, itemId).ToList()
            Else
                rows = DocumentList.Where(Function(n) n.DocumentName.Contains(name) And n.ParentID = itemId).ToList
            End If
            If rows.Count = 0 Then
                Return Nothing
            End If
            searchedRow = rows(0)
            itemId = searchedRow.DocumentID
            System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1)
        End While

        Return searchedRow
    End Function

    Public Shared Function GetItemId(ByVal ObjectID As Integer, ByVal KeyID As Integer, ByVal path As String, DocumentList As List(Of DataLibrary.Documents_GetList_Result)) As Integer
        Dim itemRow = GetItemRow(ObjectID, KeyID, path, DocumentList)
        If itemRow Is Nothing Then
            Return -1
        End If
        Return itemRow.DocumentID
    End Function

    Public Shared Function GetChildFileRows(ByVal ObjectID As Integer, ByVal KeyID As Integer, ByVal path As String, DocumentList As List(Of DataLibrary.Documents_GetList_Result)) As List(Of DataLibrary.Documents_GetList_Result)

        Dim rows = DocumentList.Where(Function(n) n.ParentID = GetItemId(ObjectID, KeyID, path, DocumentList) And n.IsDirectory = False).ToList
        Return rows

    End Function

    Public Shared Function GetChildDirectoryRows(ByVal ObjectID As Integer, ByVal KeyID As Integer, ByVal path As String, DocumentList As List(Of DataLibrary.Documents_GetList_Result)) As List(Of DataLibrary.Documents_GetList_Result)

        Dim rows = DocumentList.Where(Function(n) n.ParentID = GetItemId(ObjectID, KeyID, path, DocumentList) And n.IsDirectory = True).ToList()
        Return rows

    End Function

    Public Shared Function GetAllDirectoryRows(ByVal ObjectID As Integer, ByVal KeyID As Integer, ByVal path As String, DocumentList As List(Of DataLibrary.Documents_GetList_Result)) As List(Of DataLibrary.Documents_GetList_Result)
        Dim rootRow = GetItemRow(ObjectID, KeyID, path, DocumentList)
        If rootRow Is Nothing Then
            Return Nothing
        End If
        Dim allDirectoryRows As New List(Of DataLibrary.Documents_GetList_Result)
        allDirectoryRows.Add(rootRow)
        FillChildDirectoryRows(ObjectID, KeyID, rootRow.DocumentID, allDirectoryRows, DocumentList)
        Return allDirectoryRows
    End Function

    Public Shared Sub FillChildDirectoryRows(ByVal ObjectID As Integer, ByVal KeyID As Integer, ByVal parentId As Integer, ByVal toFill As List(Of DataLibrary.Documents_GetList_Result), DocumentList As List(Of DataLibrary.Documents_GetList_Result))

        Dim childRows = DocumentList.Where(Function(n) n.ParentID = parentId And n.IsDirectory = True).ToList

        For Each childRow As DataLibrary.Documents_GetList_Result In childRows
            toFill.Add(childRow)
            FillChildDirectoryRows(ObjectID, KeyID, childRow.DocumentID, toFill, DocumentList)
        Next

    End Sub

    Public Shared Function GetItemPath(ByVal row As DataLibrary.Documents_GetList_Result, DocumentsList As List(Of DataLibrary.Documents_GetList_Result)) As String

        If row.ParentID = 0 Then
            Return String.Format("{0}/", row.DocumentName)
        End If
        Dim parentId As Integer = ToNullableInteger(row.ParentID)

        Dim parents = DocumentsList.Where(Function(n) n.DocumentID = parentId).FirstOrDefault
        If parents Is Nothing Then
            Return String.Format("/{0}", row.DocumentName)
        End If

        Return GetItemPath(parents, DocumentsList) + String.Format("{0}/", row.DocumentName)
    End Function

    Public Shared Function IsItemExists(ByVal ObjectID As Integer, ByVal KeyID As Integer, ByVal pathToItem As String, DocumentList As List(Of DataLibrary.Documents_GetList_Result)) As Boolean
        Dim row = GetItemRow(ObjectID, KeyID, pathToItem, DocumentList)
        Return Not row Is Nothing
    End Function

#End Region

End Class
