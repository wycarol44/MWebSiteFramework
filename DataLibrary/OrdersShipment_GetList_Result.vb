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
Partial Public Class OrdersShipment_GetList_Result
    Public Property TotalCount As Nullable(Of Integer)
    Public Property Row As Nullable(Of Long)
    Public Property OrderID As Integer
    Public Property CustomerID As Nullable(Of Integer)
    Public Property OrderStatusID As Nullable(Of Integer)
    Public Property OrderTotal As Nullable(Of Double)
    Public Property DateOrdered As Nullable(Of Date)
    Public Property PaymentTypeID As Nullable(Of Integer)
    Public Property TrackingNumber As String
    Public Property CustomerName As String
    Public Property PaymentTypeName As String
    Public Property OrderStatusName As String

End Class
