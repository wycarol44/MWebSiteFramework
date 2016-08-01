Imports CommonLibrary


Public Class ProductImageManager

    Public Shared Function GetById(ProductImageID As Integer) As DataLibrary.ProductImage
        Using ctx As New DataLibrary.ModelEntities
            Dim obj = ctx.ProductImages.Where(Function(x) x.ProductImageID = ProductImageID).SingleOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.ProductImage
            End If
            Return obj
        End Using
    End Function



    Public Shared Function ImgGetList(Optional PageIndex As Integer = 0, Optional PageSize As Integer = 0, Optional PictureID As Integer? = Nothing, Optional ProductID As Integer? = Nothing, Optional Archived As Boolean? = Nothing, Optional IsPrimary As Boolean? = Nothing) As DataLibrary.ProductImage_GetList_Result()
        Using ctx As New DataLibrary.ModelEntities
            Return ctx.ProductImage_GetList(PageIndex,
                                         PageSize,
                                         ToNull(PictureID),
                                         ToNullableInteger(ProductID),
                                         Archived,
                                         ToNull(IsPrimary)
                                         ).ToArray()
        End Using
    End Function

    Public Shared Function Save(obj As DataLibrary.ProductImage) As Integer
        Using ctx As New DataLibrary.ModelEntities()

            'if the id is 0, its new, so add it
            If obj.ProductImageID = 0 Then
                ctx.ProductImages.Add(obj)

            Else
                'its an update. attach the object to the context
                ctx.ProductImages.Attach(obj)
                'tell the context that the entry is in the modified state
                ctx.Entry(obj).State = EntityState.Modified

            End If

            'save the record
            ctx.SaveChanges()

            Return obj.ProductImageID
        End Using
    End Function
End Class
