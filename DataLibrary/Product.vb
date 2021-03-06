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
Partial Public Class Product
    Public Property ProductID As Integer
    Public Property ProductName As String
    Public Property ShortDescription As String
    Public Property LongDescription As String
    Public Property CategoryID As Nullable(Of Integer)
    Public Property SubCategoryID As Nullable(Of Integer)
    Public Property Cost As Nullable(Of Double)
    Public Property Price As Nullable(Of Double)
    Public Property Archived As Nullable(Of Boolean)
    Public Property DateCreated As Nullable(Of Date)
    Public Property CreatedBy As Nullable(Of Integer)
    Public Property DateModified As Nullable(Of Date)
    Public Property ModifiedBy As Nullable(Of Integer)
    Public Property IsFeatured As Nullable(Of Boolean)
    Public Property IsPrimary As Nullable(Of Boolean)

    Public Overridable Property ProductImages As ICollection(Of ProductImage) = New HashSet(Of ProductImage)

End Class
