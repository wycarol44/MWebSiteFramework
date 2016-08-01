Imports CommonLibrary
Imports DataLibrary
Imports System.Text
Imports System.Web

Public Class CategoryMenuManager
    'I do not know what is this
    Private Shared ReadOnly Property MenuCacheKey
        Get
            Return String.Format(AppSettings.MetaMenuCacheKey, UserAuthentication.User.UserID)
        End Get
    End Property

    ' Same as CategoryManager
    Public Shared Function GetById(CategoryID As Integer) As DataLibrary.Category
        Using ctx As New DataLibrary.ModelEntities
            Dim obj = ctx.Categories.Where(Function(x) x.CategoryID = CategoryID).SingleOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.Category
            End If
            Return obj
        End Using
    End Function

    'Changed, get all, sorted by SortedOrder
    Public Shared Function GetList(Optional Archived As Boolean? = Nothing) As DataLibrary.CategoryMenu_GetTreeList_Result()
        Using ctx As New DataLibrary.ModelEntities
            Return ctx.CategoryMenu_GetTreeList(Archived).ToArray()
        End Using
    End Function

    'Save
    Public Shared Function Save(obj As DataLibrary.Category) As Integer
        Using ctx As New DataLibrary.ModelEntities()
            If obj.CategoryID = 0 Then
                'assign sort order to new record
                'if adding a new page to the favorite list, we need to make the sortorder highest
                Dim sortorderList = ctx.Categories.Where(Function(x) x.ParentID = obj.ParentID).Select(Function(x) x.SortOrder)
                'assign sortorder to the record
                If sortorderList.Any Then
                    obj.SortOrder = If(sortorderList.Max Is Nothing, 0, sortorderList.Max) + 1
                Else
                    obj.SortOrder = 1
                End If

                'new record
                ctx.Categories.Add(obj)
            Else
                'edit existing record
                ctx.Categories.Attach(obj)
                'tell the context that the entry is in the modified state
                ctx.Entry(obj).State = EntityState.Modified
            End If
            'save the record
            ctx.SaveChanges()
            Return obj.CategoryID

        End Using
    End Function

    'Change Delete to Archive
    Public Shared Sub Delete(categoryId As Integer)
        Using ctx As New DataLibrary.ModelEntities
            'get record
            Dim obj = (From c In ctx.Categories
                       Where c.CategoryID = categoryId
                       Select c).SingleOrDefault()

            'check if not null
            If obj IsNot Nothing Then
                'set state as deleted
                'ctx.Entry(obj).State = Entity.EntityState.Deleted
                obj.Archived = True
                'save it
                ctx.SaveChanges()
            End If
        End Using
    End Sub

    'Archive Tree
    Public Shared Sub DeleteTree(categoryId As Integer)
        Using ctx As New DataLibrary.ModelEntities
            'get parent record
            Dim obj = (From c In ctx.Categories
                       Where c.CategoryID = categoryId
                       Select c).SingleOrDefault()

            'check if not null
            If obj IsNot Nothing Then
                'delete child records
                DeleteChildMenu(obj, ctx)

                'set state as deleted
                'ctx.Entry(obj).State = Entity.EntityState.Deleted
                obj.Archived = True
                'save it
                ctx.SaveChanges()
            End If
        End Using
    End Sub

    Private Shared Sub DeleteChildMenu(menu As DataLibrary.Category, ctx As DataLibrary.ModelEntities)
        Dim childList = (From c In ctx.Categories
                                Where c.ParentID = menu.CategoryID
                                Select c).ToList
        If childList.Any Then childList.ForEach(Sub(x)
                                                    DeleteChildMenu(x, ctx)
                                                    'ctx.Entry(x).State = Entity.EntityState.Deleted
                                                    x.Archived = True
                                                End Sub)
        'recursion
    End Sub






End Class
