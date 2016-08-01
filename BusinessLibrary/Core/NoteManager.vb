Imports CommonLibrary
Public Class NoteManager


#Region "Standard"

    ''' <summary>
    ''' Gets a record by its ID. If no record is found, returns a new, blank record
    ''' </summary>
    ''' <param name="NoteID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetById(NoteID As Integer) As DataLibrary.Note
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = ctx.Notes.Where(Function(x) x.NoteID = NoteID).SingleOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.Note
            End If

            Return obj
        End Using
    End Function

    Public Shared Function GetList(ObjectID As Integer, KeyID As Integer, Optional NoteTypeID As XElement = Nothing, Optional Title As String = Nothing, Optional CreatedBy As Integer? = Nothing, Optional DateFrom As DateTime? = Nothing, Optional DateTo As DateTime? = Nothing, Optional Archived As Boolean? = Nothing) As DataLibrary.Notes_GetList_Result()
        Using ctx As New DataLibrary.ModelEntities()
            Return ctx.Notes_GetList(ObjectID, KeyID, ToNull(NoteTypeID),
                                         ToNull(Title),
                                         CreatedBy,
                                         DateFrom,
                                         DateTo,
                                         Archived).ToArray()
        End Using
    End Function

    Public Shared Function GetListForDashBoard(ObjectID As Integer, KeyID As Integer, NumberofItems As Integer) As DataLibrary.Notes_GetListForDashBoard_Result()
        Using ctx As New DataLibrary.ModelEntities()
            Return ctx.Notes_GetListForDashBoard(ObjectID, KeyID, NumberofItems).ToArray()
        End Using
    End Function

    ''' <summary>
    ''' Inserts or updates the record
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Save(obj As DataLibrary.Note) As Integer
        Using ctx As New DataLibrary.ModelEntities()

            'if the id is 0, its new, so add it
            If obj.NoteID = 0 Then
                ctx.Notes.Add(obj)
                obj.DateCreated = Now
                obj.CreatedBy = UserAuthentication.User.UserID

            Else
                'its an update. attach the object to the context
                ctx.Notes.Attach(obj)
                'tell the context that the entry is in the modified state
                ctx.Entry(obj).State = EntityState.Modified
                obj.DateModified = Now
                obj.ModifiedBy = UserAuthentication.User.UserID
            End If

            'save the record
            ctx.SaveChanges()

            Return obj.NoteID

        End Using
    End Function

    ''' <summary>
    ''' Archives the specified record
    ''' </summary>
    ''' <param name="noteid"></param>
    ''' <remarks></remarks>
    Public Shared Sub ToggleArchived(noteID As Integer)

        Dim obj = GetById(noteID)
        If obj IsNot Nothing AndAlso obj.NoteID > 0 Then
            obj.Archived = Not obj.Archived
            Save(obj)
        End If

    End Sub


    ''' <summary>
    ''' Gets count of unarchived notes
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetCount(ByVal ObjectID As Integer, ByVal KeyID As Integer)
        Using ctx As New DataLibrary.ModelEntities
            Return ctx.Notes.Count(Function(x) x.ObjectID = ObjectID AndAlso x.KeyID = KeyID AndAlso x.Archived = False)
        End Using
    End Function

#End Region




End Class
