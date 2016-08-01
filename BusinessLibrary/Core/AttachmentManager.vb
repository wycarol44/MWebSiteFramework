Imports CommonLibrary
Public Class AttachmentManager

#Region "Standard"

    ''' <summary>
    ''' Gets a record by its ID. If no record is found, returns a new, blank record
    ''' </summary>
    ''' <param name="AttachmentID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetById(AttachmentID As Integer) As DataLibrary.Attachment
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = ctx.Attachments.Where(Function(x) x.AttachmentID = AttachmentID).SingleOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.Attachment
            End If

            Return obj
        End Using
    End Function

    Public Shared Function GetList(ObjectID As Integer, KeyID As Integer, Optional AttachmentTypeID As Integer? = Nothing) As DataLibrary.Attachment()
        Using ctx As New DataLibrary.ModelEntities()

            Dim obj = (From a In ctx.Attachments.Include("User")
                        Where a.ObjectID = ObjectID And a.KeyID = KeyID _
            And (AttachmentTypeID Is Nothing OrElse a.AttachmentTypeID = AttachmentTypeID)).ToArray

            Return obj

        End Using

    End Function


    ''' <summary>
    ''' Inserts or updates the record
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Save(obj As DataLibrary.Attachment) As Integer
        Using ctx As New DataLibrary.ModelEntities()

            'if the id is 0, its new, so add it
            If obj.AttachmentID = 0 Then

                ctx.Attachments.Add(obj)
                obj.DateCreated = Now
                obj.CreatedBy = UserAuthentication.User.UserID

            End If

            'save the record
            ctx.SaveChanges()

            Return obj.AttachmentID

        End Using
    End Function

    ''' <summary>
    ''' Archives the specified record
    ''' </summary>
    ''' <param name="AttachmentID"></param>
    ''' <remarks></remarks>
    Public Shared Sub Delete(AttachmentID As Integer)

        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = ctx.Attachments.Where(Function(x) x.AttachmentID = AttachmentID).SingleOrDefault()
            ctx.Attachments.Remove(obj)
            ctx.SaveChanges()

        End Using


    End Sub

#End Region

End Class
