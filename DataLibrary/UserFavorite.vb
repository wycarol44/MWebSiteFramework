'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated from a template.
'
'     Manual changes to this file may cause unexpected behavior in your application.
'     Manual changes to this file will be overwritten if the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Imports System
Imports System.Collections.Generic

<Serializable()>
Partial Public Class UserFavorite
    Public Property FavoriteID As Integer
    Public Property FormID As Integer
    Public Property DisplayName As String
    Public Property UserID As Integer
    Public Property IconColor As String
    Public Property ShowInFavoritesMenu As Boolean
    Public Property ShowInIconBar As Boolean
    Public Property IsLandingPage As Boolean
    Public Property SortOrder As Integer
    Public Property Archived As Boolean
    Public Property DateCreated As Date
    Public Property CreatedBy As Integer
    Public Property DateModified As Nullable(Of Date)
    Public Property ModifiedBy As Nullable(Of Integer)

    Public Overridable Property MetaForm As MetaForm
    Public Overridable Property User As User

End Class
