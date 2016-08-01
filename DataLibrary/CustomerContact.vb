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
Partial Public Class CustomerContact
    Public Property ContactID As Integer
    Public Property CustomerID As Nullable(Of Integer)
    Public Property Firstname As String
    Public Property Lastname As String
    Public Property FullName As String
    Public Property Title As String
    Public Property AddressID As Nullable(Of Integer)
    Public Property Email As String
    Public Property WorkPhone As String
    Public Property WorkPhoneExt As String
    Public Property MobilePhone As String
    Public Property HomePhone As String
    Public Property Notes As String
    Public Property NotesText As String
    Public Property IsPrimary As Boolean
    Public Property Archived As Boolean
    Public Property DateCreated As Nullable(Of Date)
    Public Property CreatedBy As Nullable(Of Integer)
    Public Property DateModified As Nullable(Of Date)
    Public Property ModifiedBy As Nullable(Of Integer)

    Public Overridable Property Address As Address
    Public Overridable Property Customer As Customer

End Class
