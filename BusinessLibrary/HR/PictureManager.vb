Imports CommonLibrary
Imports System.IO

Public Class PictureManager
#Region "Standard"

    ''' <summary>
    ''' Gets a record by its ID. If no record is found, returns a new, blank record
    ''' </summary>
    ''' <param name="pictureId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetById(pictureId As Integer) As DataLibrary.Picture
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = (From t In ctx.Pictures
                       Where t.PictureID = pictureId).SingleOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.Picture
            End If

            Return obj
        End Using
    End Function

    Public Shared Function GetByObjectAndKey(objectID As Integer, keyId As Integer) As DataLibrary.Picture
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = (From t In ctx.Pictures
                       Where t.ObjectID = objectID And t.KeyID = keyId).FirstOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.Picture
            End If

            Return obj
        End Using
    End Function


    ''' <summary>
    ''' Inserts or updates the record
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Save(obj As DataLibrary.Picture) As Integer
        Using ctx As New DataLibrary.ModelEntities()

            'if the id is 0, its new, so add it
            If obj.PictureID = 0 Then
                ctx.Pictures.Add(obj)

                'set default values
                obj.DateCreated = Now
                obj.CreatedBy = UserAuthentication.User.UserID
            Else
                'set default values
                obj.DateModified = Now
                obj.ModifiedBy = UserAuthentication.User.UserID


                'its an update. attach the object to the context
                ctx.Pictures.Attach(obj)
                'tell the context that the entry is in the modified state
                ctx.Entry(obj).State = EntityState.Modified
            End If

            'save the record
            ctx.SaveChanges()

            Return obj.PictureID
        End Using
    End Function

    ''' <summary>
    ''' Hard deletes a record
    ''' </summary>
    ''' <param name="pictureId"></param>
    ''' <remarks></remarks>
    Public Shared Sub Delete(pictureId As Integer)
        Using ctx As New DataLibrary.ModelEntities
            Dim obj = (From t In ctx.Pictures
                       Where t.PictureID = pictureId).SingleOrDefault()

            If obj IsNot Nothing Then

                ctx.Entry(obj).State = Entity.EntityState.Deleted

                ctx.SaveChanges()

                'delete the images
                File.Delete(Path.Combine(AppSettings.PicturesFolder, obj.PicturePath))
                File.Delete(Path.Combine(AppSettings.PicturesFolder, obj.ThumbnailPath))


            End If
        End Using
    End Sub

    Public Shared Sub DeleteByObjectAndKey(objectID As Integer, keyID As Integer)
        Using ctx As New DataLibrary.ModelEntities
            Dim obj = (From t In ctx.Pictures
                       Where t.ObjectID = objectID And t.KeyID = keyID).FirstOrDefault()

            If obj IsNot Nothing Then

                ctx.Entry(obj).State = Entity.EntityState.Deleted

                ctx.SaveChanges()

                'delete the images
                File.Delete(Path.Combine(AppSettings.PicturesFolder, obj.PicturePath))
                File.Delete(Path.Combine(AppSettings.PicturesFolder, obj.ThumbnailPath))


            End If
        End Using
    End Sub
#End Region
End Class
