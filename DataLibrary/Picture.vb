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
Partial Public Class Picture
    Public Property PictureID As Integer
    Public Property ObjectID As Integer
    Public Property KeyID As Integer
    Public Property PicturePath As String
    Public Property ThumbnailPath As String
    Public Property DateCreated As Date
    Public Property CreatedBy As Integer
    Public Property DateModified As Nullable(Of Date)
    Public Property ModifiedBy As Nullable(Of Integer)

    Public Overridable Property ProductImages As ICollection(Of ProductImage) = New HashSet(Of ProductImage)

End Class
