Public Class UserFavoriteManager

#Region "Standard"

    ''' <summary>
    ''' Gets a record by its ID. If no record is found, returns a new, blank record
    ''' </summary>
    ''' <param name="favoriteid"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetById(favoriteid As Integer) As DataLibrary.UserFavorite
        Using ctx As New DataLibrary.ModelEntities()
            Dim obj = (From t In ctx.UserFavorites
                       Where t.FavoriteID = favoriteid).SingleOrDefault()
            If obj Is Nothing Then
                obj = New DataLibrary.UserFavorite
            End If

            Return obj
        End Using
    End Function

    ''' <summary>
    ''' Gets a list of records
    ''' </summary>
    ''' <param name="userId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetList(userId As Integer) As DataLibrary.UserFavorites_GetMenuList_Result()
        Using ctx As New DataLibrary.ModelEntities

            Dim list = ctx.UserFavorites_GetMenuList(userId).ToArray()

            Return list
        End Using
    End Function

    ''' <summary>
    ''' Inserts or updates the record
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Save(obj As DataLibrary.UserFavorite) As Integer
        Using ctx As New DataLibrary.ModelEntities()

            'If this obj is set to be landingpage then we need to reset another one if exists
            If obj.IsLandingPage Then
                'check if there is any other primary contact
                Dim landingpage = ctx.UserFavorites.Where(Function(x) x.UserID = obj.UserID And x.FormID <> obj.FormID And x.IsLandingPage = True).SingleOrDefault()
                'if any primarycontact exists then reset it
                If landingpage IsNot Nothing Then
                    landingpage.IsLandingPage = False
                End If
            End If

            'if the id is 0, its new, so add it
            If obj.FavoriteID = 0 Then
                'if adding a new page to the favorite list, we need to make the sortorder highest
                Dim sortorderList = ctx.UserFavorites.Where(Function(x) x.UserID = obj.UserID).Select(Function(x) x.SortOrder)

                'assign sortorder to the record
                If sortorderList.Any Then
                    obj.SortOrder = sortorderList.Max + 1
                Else
                    obj.SortOrder = 1
                End If
                ctx.UserFavorites.Add(obj)
                'set default values
                obj.DateCreated = Now
                obj.CreatedBy = UserAuthentication.User.UserID
            Else
                'set default values
                obj.DateModified = Now
                obj.ModifiedBy = UserAuthentication.User.UserID

                'its an update. attach the object to the context
                ctx.UserFavorites.Attach(obj)
                'tell the context that the entry is in the modified state
                ctx.Entry(obj).State = EntityState.Modified
            End If

            'save the record
            ctx.SaveChanges()

            Return obj.FavoriteID
        End Using
    End Function
#End Region

#Region "Custom"
    ''' <summary>
    ''' Checks if the form is already favorite for the user and returns boolean status
    ''' </summary>
    ''' <param name="userID"></param>
    ''' <param name="FormID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CheckFavorite(userID As Integer, FormID As Integer) As Boolean
        Using ctx As New DataLibrary.ModelEntities
            Dim obj = ctx.UserFavorites.Where(Function(x) x.UserID = userID And x.FormID = FormID).FirstOrDefault
            If obj IsNot Nothing Then
                Return True
            Else
                Return False
            End If
        End Using
    End Function

    ''' <summary>
    ''' Gets Landing Page for the User
    ''' </summary>
    ''' <param name="userID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetLandingPage(userID As Integer) As String
        Using ctx As New DataLibrary.ModelEntities
            Dim obj = ctx.UserFavorites.Where(Function(x) x.UserID = userID And x.IsLandingPage = True).FirstOrDefault
            If obj IsNot Nothing Then
                Return MetaFormManager.GetById(obj.FormID).FormPath
            Else
                Return Nothing
            End If
        End Using
    End Function

    ''' <summary>
    ''' Toggles ShowInFavouriteMenu
    ''' </summary>
    ''' <param name="favoriteid"></param>
    ''' <remarks></remarks>
    Public Shared Sub ToggleShowInFavouriteMenu(favoriteid As Integer)
        Using ctx As New DataLibrary.ModelEntities
            Dim obj = (From t In ctx.UserFavorites
                       Where t.FavoriteID = favoriteid).SingleOrDefault()

            If obj IsNot Nothing Then
                obj.ShowInFavoritesMenu = Not obj.ShowInFavoritesMenu

                ctx.SaveChanges()
            End If
        End Using
    End Sub

    ''' <summary>
    ''' Toggles ShowInIconBar
    ''' </summary>
    ''' <param name="favoriteid"></param>
    ''' <remarks></remarks>
    Public Shared Sub ToggleShowInIconBar(favoriteid As Integer)
        Using ctx As New DataLibrary.ModelEntities
            Dim obj = (From t In ctx.UserFavorites
                       Where t.FavoriteID = favoriteid).SingleOrDefault()

            If obj IsNot Nothing Then
                obj.ShowInIconBar = Not obj.ShowInIconBar
                ctx.SaveChanges()
            End If
        End Using
    End Sub

    ''' <summary>
    ''' Toggles IsLandingPage
    ''' </summary>
    ''' <param name="favoriteid"></param>
    ''' <remarks></remarks>
    Public Shared Sub ToggleIsLandingPage(favoriteid As Integer)
        Using ctx As New DataLibrary.ModelEntities
            Dim obj = (From t In ctx.UserFavorites
                       Where t.FavoriteID = favoriteid).SingleOrDefault()

            'If there is any previous landing page, reset it
            If Not obj.IsLandingPage Then
                'check if there is any other primary contact
                Dim landingpage = ctx.UserFavorites.Where(Function(x) x.UserID = obj.UserID And x.FormID <> obj.FormID And x.IsLandingPage = True).SingleOrDefault()
                'if any primarycontact exists then reset it
                If landingpage IsNot Nothing Then
                    landingpage.IsLandingPage = False
                End If
            End If

            If obj IsNot Nothing Then
                obj.IsLandingPage = Not obj.IsLandingPage
                ctx.SaveChanges()
            End If
        End Using
    End Sub

    ''' <summary>
    ''' Deletes Favorite
    ''' </summary>
    ''' <param name="favoriteid"></param>
    ''' <remarks></remarks>
    Public Shared Sub RemoveFavorite(favoriteid As Integer)
        Using ctx As New DataLibrary.ModelEntities
            Dim obj = (From t In ctx.UserFavorites
                       Where t.FavoriteID = favoriteid).SingleOrDefault()

            If obj IsNot Nothing Then
                ctx.UserFavorites.Remove(obj)
                ctx.SaveChanges()
            End If
        End Using
    End Sub


    ''' <summary>
    ''' Updates SortOrder
    ''' </summary>
    ''' <param name="favorites"></param>
    ''' <remarks></remarks>
    Public Shared Sub UpdateSortOrder(favorites As List(Of DataLibrary.UserFavorites_GetMenuList_Result))
        Using ctx As New DataLibrary.ModelEntities
            For Each favorite In favorites
                Dim obj = (From t In ctx.UserFavorites
                       Where t.FavoriteID = favorite.FavoriteID).SingleOrDefault()
                If obj IsNot Nothing Then obj.SortOrder = favorites.IndexOf(favorite) + 1
            Next
            ctx.SaveChanges()
        End Using
    End Sub

#End Region

End Class
