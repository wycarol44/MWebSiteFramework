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
Partial Public Class ProductImage
    Public Property ProductImageID As Integer
    Public Property ProductID As Nullable(Of Integer)
    Public Property ImageName As String
    Public Property IsPrimary As Nullable(Of Boolean)
    Public Property PictureID As Nullable(Of Integer)
    Public Property Archived As Nullable(Of Boolean)

    Public Overridable Property Picture As Picture
    Public Overridable Property Product As Product

End Class
