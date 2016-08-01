Public Class MetaTypeItemManager
#Region "Standard"



    ''' <summary>
    ''' Gets a record by its ID. If no record is found, returns a new, blank record
    ''' </summary>
    ''' <param name="itemId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetById(itemId As Integer) As DataLibrary.MetaTypeItem
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = ctx.MetaTypeItems.Where(Function(x) x.ItemID = itemId).SingleOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.MetaTypeItem
            End If

            Return obj
        End Using
    End Function
    ''' <summary>
    ''' Gets a list of records
    ''' </summary>
    ''' <param name="typeId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetList(typeId As MilesMetaType) As DataLibrary.MetaTypeItem()
        Using ctx As New DataLibrary.ModelEntities

            Dim list = (From t In ctx.MetaTypeItems
                           Where t.TypeID = typeId
                           Select t).ToArray()

            Return list

        End Using
    End Function


    ''' <summary>
    '''Insert/Save Record
    '''Author: Sanjog Sharma
    '''Create Date: 12/23/2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function Save(obj As DataLibrary.MetaTypeItem) As Integer
        Using ctx As New DataLibrary.ModelEntities()
            If obj.ItemID = 0 Then
                'new record
                ctx.MetaTypeItems.Add(obj)
            Else
                'edit existing record
                ctx.MetaTypeItems.Attach(obj)
                'tell the context that the entry is in the modified state
                ctx.Entry(obj).State = EntityState.Modified
            End If
            'save the record
            ctx.SaveChanges()
            Return obj.TypeID
        End Using
    End Function

    ''' <summary>
    ''' Archives the specified record
    ''' </summary>
    ''' <param name="itemid"></param>
    ''' <remarks></remarks>
    Public Shared Sub Delete(itemid As Integer)
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = ctx.MetaTypeItems.Where(Function(x) x.ItemID = itemid).SingleOrDefault()
            If obj IsNot Nothing Then
                'mark as deleted
                ctx.Entry(obj).State = Entity.EntityState.Deleted

                ctx.SaveChanges()
            End If
        End Using
    End Sub
#End Region
End Class
