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
Partial Public Class Document
    Public Property DocumentID As Integer
    Public Property ObjectID As Nullable(Of Integer)
    Public Property KeyID As Nullable(Of Integer)
    Public Property DocumentName As String
    Public Property MimeType As String
    Public Property FilePath As String
    Public Property DateCreated As Nullable(Of Date)
    Public Property CreatedBy As Nullable(Of Integer)
    Public Property DateModified As Nullable(Of Date)
    Public Property ModifiedBy As Nullable(Of Integer)

    Public Overridable Property Customer As Customer
    Public Overridable Property User As User
    Public Overridable Property User1 As User

End Class