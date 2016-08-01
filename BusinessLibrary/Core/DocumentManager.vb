Imports CommonLibrary
Public Class DocumentManager
#Region "Standard"

    ''' <summary>
    ''' Gets a record by its ID. If no record is found, returns a new, blank record
    ''' </summary>
    ''' <param name="DocumentID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetById(DocumentID As Integer) As DataLibrary.Document
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = ctx.Documents.Where(Function(x) x.DocumentID = DocumentID).SingleOrDefault()

            If obj Is Nothing Then
                obj = New DataLibrary.Document
            End If

            Return obj
        End Using
    End Function

    ''' <summary>
    ''' Gets a record by its ID. If no record is found, returns a new, blank record
    ''' </summary>
    ''' <param name="DocumentID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetByIdDetails(DocumentID As Integer) As DataLibrary.Documents_GetByID_Result
        Using ctx As New DataLibrary.ModelEntities()
            'Dim obj = ctx.Documents.Include("CreatedUser").Where(Function(x) x.DocumentID = DocumentID).SingleOrDefault()
            Dim obj = ctx.Documents_GetByID(DocumentID).SingleOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.Documents_GetByID_Result
            End If

            Return obj
        End Using
    End Function

    Public Shared Function GetList(ObjectID As Integer, KeyID As Integer, Optional DocumentName As String = Nothing) As List(Of DataLibrary.Documents_GetList_Result)
        Using ctx As New DataLibrary.ModelEntities

         
            Dim objs As List(Of DataLibrary.Documents_GetList_Result) = ctx.Documents_GetList(ObjectID, KeyID, DocumentName).ToList()

            Return objs
        End Using
    End Function

    Public Shared Function GetListForDashBoard(ObjectID As Integer, KeyID As Integer, NumberofItems As Integer) As DataLibrary.Documents_GetListForDashBoard_Result()
        Using ctx As New DataLibrary.ModelEntities()
            Return ctx.Documents_GetListForDashBoard(ObjectID, KeyID, NumberofItems).ToArray()
        End Using
    End Function


    ''' <summary>
    ''' Inserts or updates the record
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Save(obj As DataLibrary.Document) As Integer
        Using ctx As New DataLibrary.ModelEntities()

            'if the id is 0, its new, so add it
            If obj.DocumentID = 0 Then
                ctx.Documents.Add(obj)
                obj.DateCreated = Now
                obj.CreatedBy = UserAuthentication.User.UserID

            Else
                'its an update. attach the object to the context
                ctx.Documents.Attach(obj)
                'tell the context that the entry is in the modified state
                ctx.Entry(obj).State = EntityState.Modified
                obj.DateModified = Now
                obj.ModifiedBy = UserAuthentication.User.UserID
            End If

            'save the record
            ctx.SaveChanges()

            Return obj.DocumentID

        End Using
    End Function


    ''' <summary>
    ''' Archives the specified record
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <remarks></remarks>
    Public Shared Sub Delete(obj As DataLibrary.Document)
        Using ctx As New DataLibrary.ModelEntities()
            ctx.Documents.Attach(obj)
            ctx.Documents.Remove(obj)
            ctx.SaveChanges()
        End Using
    End Sub


    ''' <summary>
    ''' Gets count of documents
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetCount(ByVal ObjectID As Integer, ByVal KeyID As Integer)
        Using ctx As New DataLibrary.ModelEntities
            Return ctx.Documents.Count(Function(x) x.ObjectID = ObjectID AndAlso x.KeyID = KeyID)
        End Using
    End Function

#End Region



End Class
