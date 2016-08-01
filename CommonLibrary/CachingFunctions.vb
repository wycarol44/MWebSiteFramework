Imports Microsoft.VisualBasic
Imports System.Web.Caching

Public Class CachingFunctions

    ''' <summary>
    ''' Determines if an item is in the cache
    ''' </summary>
    ''' <param name="Key"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsInCache(ByVal Key As String) As Boolean
        Dim webCache = System.Web.HttpRuntime.Cache
        If webCache(Key) Is Nothing Then
            Return False
        Else
            Return True
        End If
    End Function

    ''' <summary>
    ''' Gets an item from the cache
    ''' </summary>
    ''' <param name="Key"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function LoadFromCache(ByVal Key As String) As Object
        Dim webCache = System.Web.HttpRuntime.Cache
        Return webCache(Key)
    End Function

    ''' <summary>
    ''' Adds an item to the cache
    ''' </summary>
    ''' <param name="Key"></param>
    ''' <param name="Value"></param>
    ''' <param name="expiresIn">Time (in minutes) when the cache should expire</param>
    ''' <remarks></remarks>
    Public Shared Sub AddToCache(ByVal Key As String, ByVal Value As Object, Optional expiresIn As Integer = 24 * 60)
        Dim webCache = System.Web.HttpRuntime.Cache
        webCache.Add(Key, Value, Nothing, DateTime.Today.AddHours(expiresIn), Cache.NoSlidingExpiration, CacheItemPriority.High, Nothing)
    End Sub

    ''' <summary>
    ''' Removes an item from the cache
    ''' </summary>
    ''' <param name="Key"></param>
    ''' <remarks></remarks>
    Public Shared Sub RemoveFromCache(ByVal Key As String)
        Dim webCache = System.Web.HttpRuntime.Cache
        webCache.Remove(Key)
    End Sub


End Class
