Imports CommonLibrary
Imports DataLibrary
Imports System.Text
Imports System.Web

Public Class MetaMenuManager
#Region "Methods"

    Private Shared ReadOnly Property MenuCacheKey
        Get
            Return String.Format(AppSettings.MetaMenuCacheKey, UserAuthentication.User.UserID)
        End Get
    End Property


    ''' <summary>
    '''Gets a Record By Id
    '''Author: Eric Butler
    '''Create Date: 11/25/2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function GetByID(ByVal MenuID As Integer) As DataLibrary.MetaMenu
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = (From t In ctx.MetaMenus Where
            t.MenuID = MenuID
            ).SingleOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.MetaMenu
            End If
            Return obj
        End Using
    End Function


    ''' <summary>
    '''Gets a list and returns list of objects
    '''Author: Eric Butler
    '''Create Date: 11/25/2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function GetList() As DataLibrary.MetaMenu_GetTreeList_Result()
        Using ctx As New DataLibrary.ModelEntities
            Return ctx.MetaMenu_GetTreeList().ToArray()
        End Using
    End Function

    ''' <summary>
    '''Gets a list and returns list of objects
    '''Author: Eric Butler
    '''Create Date: 11/25/2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function GetMenuList() As DataLibrary.MetaMenu_GetList_Result()

        'get the menu from cache if its there and we are NOT bypassing cache, if not get it from the db
        If CachingFunctions.IsInCache(MenuCacheKey) And Not AppSettings.BypassCaching Then
            Return CachingFunctions.LoadFromCache(MenuCacheKey)
        Else
            Using ctx As New DataLibrary.ModelEntities
                Dim menu = ctx.MetaMenu_GetList(UserAuthentication.User.UserID).ToArray()

                'store in cache if not bypassing cache
                If Not AppSettings.BypassCaching Then
                    CachingFunctions.AddToCache(MenuCacheKey, menu)
                End If

                Return menu
            End Using
        End If

    End Function

    ''' <summary>
    ''' Clears the menu for a particular user, or all users
    ''' </summary>
    ''' <param name="userId"></param>
    ''' <remarks></remarks>
    Public Shared Sub ClearMenuCache(Optional userId As Integer? = Nothing)

        'get users
        If Not userId.HasValue Then
            Dim usersList = UserManager.GetList()

            For Each u In usersList
                'create key for user
                DeleteCacheKey(u.UserID)
            Next
        Else
            DeleteCacheKey(userId)
        End If

    End Sub

    ''' <summary>
    ''' Deletes a cache entry by a specific user
    ''' </summary>
    ''' <param name="userId"></param>
    ''' <remarks></remarks>
    Private Shared Sub DeleteCacheKey(userId As Integer)
        'clear cache for specific user
        Dim cacheKey = String.Format(AppSettings.MetaMenuCacheKey, userId)
        'check to see if key is in dictionary
        If CachingFunctions.IsInCache(cacheKey) Then
            CachingFunctions.RemoveFromCache(cacheKey)
        End If
    End Sub


    ''' <summary>
    '''Insert/Save Record
    '''Author: Eric Butler
    '''Create Date: 11/25/2013
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function Save(obj As DataLibrary.MetaMenu) As Integer
        Using ctx As New DataLibrary.ModelEntities()
            If obj.MenuID = 0 Then
                'assign sort order to new record
                'if adding a new page to the favorite list, we need to make the sortorder highest
                Dim sortorderList = ctx.MetaMenus.Where(Function(x) x.ParentID = obj.ParentID).Select(Function(x) x.SortOrder)
                'assign sortorder to the record
                If sortorderList.Any Then
                    obj.SortOrder = If(sortorderList.Max Is Nothing, 0, sortorderList.Max) + 1
                Else
                    obj.SortOrder = 1
                End If

                'new record
                ctx.MetaMenus.Add(obj)
            Else
                'edit existing record
                ctx.MetaMenus.Attach(obj)
                'tell the context that the entry is in the modified state
                ctx.Entry(obj).State = EntityState.Modified
            End If
            'save the record
            ctx.SaveChanges()
            Return obj.MenuID
        End Using
    End Function

    ''' <summary>
    ''' Hard deletes a record by its id
    ''' </summary>
    ''' <param name="menuId"></param>
    ''' <remarks></remarks>
    Public Shared Sub Delete(menuId As Integer)
        Using ctx As New DataLibrary.ModelEntities
            'get record
            Dim obj = (From t In ctx.MetaMenus
                       Where t.MenuID = menuId
                       Select t).SingleOrDefault()

            'check if not null
            If obj IsNot Nothing Then
                'set state as deleted
                ctx.Entry(obj).State = Entity.EntityState.Deleted
                'save it
                ctx.SaveChanges()
            End If
        End Using
    End Sub

    ''' <summary>
    ''' Hard deletes a record and its child 
    ''' </summary>
    ''' <param name="menuId"></param>
    ''' <remarks></remarks>
    Public Shared Sub DeleteTree(menuId As Integer)
        Using ctx As New DataLibrary.ModelEntities
            'get parent record
            Dim obj = (From t In ctx.MetaMenus
                       Where t.MenuID = menuId
                       Select t).SingleOrDefault()

            'check if not null
            If obj IsNot Nothing Then
                'delete child records
                DeleteChildMenu(obj, ctx)

                'set state as deleted
                ctx.Entry(obj).State = Entity.EntityState.Deleted
                'save it
                ctx.SaveChanges()
            End If
        End Using
    End Sub

    Private Shared Sub DeleteChildMenu(menu As DataLibrary.MetaMenu, ctx As DataLibrary.ModelEntities)
        Dim childList = (From t In ctx.MetaMenus
                                Where t.ParentID = menu.MenuID
                                Select t).ToList
        If childList.Any Then childList.ForEach(Sub(x)
                                                    DeleteChildMenu(x, ctx)
                                                    ctx.Entry(x).State = Entity.EntityState.Deleted
                                                End Sub)

    End Sub
#End Region

#Region "Mobile Menu"

    ''' <summary>
    ''' Builds the html menu for the Mobile Menu. This will be a one level menu.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetMobileMenuHTML() As String
        Dim sb As New StringBuilder
        Dim menuList = GetMenuList().ToList
        Dim parentList = menuList.Where(Function(x) x.ParentID Is Nothing).ToList
        Dim childList As New List(Of MetaMenu_GetList_Result)
        Dim indentation As String = ""
        Dim Depth As Integer = 0
        For Each p As MetaMenu_GetList_Result In parentList
            indentation = ""
            Depth = 0
            FlattenMenu(menuList.Except(parentList).ToList, p.MenuID, p.MenuID, childList, Depth)
        Next

        For Each p As MetaMenu_GetList_Result In parentList
            If p.MenuPath <> String.Empty AndAlso childList.Where(Function(x) x.ParentID = p.MenuID).Count = 0 Then
                Dim ms = <li>
                             <a href=<%= p.MenuPath %>><%= p.Title %></a>
                         </li>
                sb.Append(ms.ToString)
            Else
                ' Root for each menu
                Dim ms = <li class="dropdown">
                             <a href="#" class="dropdown-toggle" data-toggle="dropdown"><%= p.Title %><b class="caret"></b></a>
                             <%= CreateMenuLineItem(p.MenuID, childList.Where(Function(x) x.ParentID = p.MenuID).ToList) %>
                         </li>
                sb.Append(ms.ToString)

            End If
        Next
        Return sb.ToString
    End Function

    Private Shared Function CreateMenuLineItem(ParentID As Integer, childList As List(Of MetaMenu_GetList_Result)) As XElement
        Dim s As XElement = New XElement(<ul class="dropdown-menu"></ul>)
        For Each c As MetaMenu_GetList_Result In childList
            Dim TitleArr = c.Title.Split("~")
            Dim Title = TitleArr(1)
            Dim Depth As Integer = TitleArr(0)
            Dim Padding As Integer = 0
            Padding = 8 + (Depth * 15)
            Dim PaddingText As String = "padding-left:" & Padding.ToString & "px;"
            If c.MenuPath <> String.Empty Then
                Dim cm = <li><a href=<%= c.MenuPath %>><span style=<%= PaddingText %>><%= Title %></span></a></li>
                s.Add(cm)
            Else
                Dim cm = <li><%= Title %></li>
                s.Add(cm)
            End If
        Next
        Return s
    End Function

    Private Shared Sub FlattenMenu(l As List(Of MetaMenu_GetList_Result), parentID As Integer, RootParentID As Integer, ByRef childList As List(Of MetaMenu_GetList_Result), ByRef Depth As Integer)
        For Each m As MetaMenu_GetList_Result In l.Where(Function(x) x.ParentID = parentID)
            'If m.MenuPath <> String.Empty Then
            Dim cm As New MetaMenu_GetList_Result
            cm.ParentID = RootParentID
            cm.MenuID = m.MenuID
            cm.MenuPath = m.MenuPath
            cm.Title = Depth.ToString & "~" & m.Title
            childList.Add(cm)
            'End If

            If l.Where(Function(x) x.ParentID = m.MenuID).Any Then
                Depth += 1
                FlattenMenu(l, m.MenuID, RootParentID, childList, Depth)

            End If

        Next
        Depth = Depth - 1
    End Sub



#End Region
End Class
