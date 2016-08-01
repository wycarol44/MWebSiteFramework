Imports CommonLibrary
Public Class ManagedTypeItemManager
#Region "Methods"

    ''' <summary>
    '''Gets a Record By Id
    '''Author: Sanjog Sharma
    '''Create Date: 11/29/2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function GetByID(ByVal ItemID As Integer) As DataLibrary.ManagedTypeItem
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = (From t In ctx.ManagedTypeItems Where
            t.ItemID = ItemID
            ).SingleOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.ManagedTypeItem
            End If
            Return obj
        End Using
    End Function


    ''' <summary>
    '''Gets a list and returns list of objects
    '''Author: Sanjog Sharma
    '''Create Date: 11/29/2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function GetList(ByVal TypeID As Integer, Optional ByVal ItemName As String = Nothing, Optional ByVal ParentID As Integer = Nothing, Optional ByVal Archived As Boolean? = Nothing) As DataLibrary.ManagedTypeItem()
        Using ctx As New DataLibrary.ModelEntities
            'get list of objects
            Dim objs = (From t In ctx.ManagedTypeItems
                        Order By t.ItemName
                        Where
                            (TypeID = t.TypeID) _
                        And (ItemName Is Nothing OrElse t.ItemName.Contains(ItemName)) _
                        And (ParentID = Nothing OrElse t.ParentID = ParentID) _
                        And (Archived Is Nothing OrElse t.Archived = Archived.Value)
                        Select t).ToArray()
            Return objs
        End Using
    End Function


    ''' <summary>
    '''Insert/Save Record
    '''Author: Sanjog Sharma
    '''Create Date: 11/29/2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function Save(obj As DataLibrary.ManagedTypeItem) As Integer
        Using ctx As New DataLibrary.ModelEntities()
            If obj.ItemID = 0 Then
                'new record
                ctx.ManagedTypeItems.Add(obj)
                obj.DateCreated = Now
                obj.CreatedBy = UserAuthentication.User.UserID
            Else
                'edit existing record
                ctx.ManagedTypeItems.Attach(obj)
                'tell the context that the entry is in the modified state
                ctx.Entry(obj).State = EntityState.Modified
                obj.DateModified = Now
                obj.ModifiedBy = UserAuthentication.User.UserID
            End If
            'save the record
            ctx.SaveChanges()
            Return obj.ItemID
        End Using
    End Function


    ''' <summary>
    '''Toggles the active status of a record
    '''Author: Sanjog Sharma
    '''Create Date: 11/29/2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub ToggleArchived(ByVal ItemID As Integer)
        Using ctx As New DataLibrary.ModelEntities
            Dim obj = (From t In ctx.ManagedTypeItems Where
            t.ItemID = ItemID
            ).SingleOrDefault()
            If obj IsNot Nothing Then
                obj.Archived = Not obj.Archived
                ctx.SaveChanges()
            End If
        End Using
    End Sub
#End Region
End Class
