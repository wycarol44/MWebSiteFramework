Imports Telerik.Web.UI

Public Class MilesDateTimePicker
    Inherits RadDateTimePicker

#Region "Properties"

    Public Property ShowTime As Boolean
        Get
            Return Me.TimePopupButton.Visible
        End Get
        Set(value As Boolean)
            Me.TimePopupButton.Visible = value
            If value Then
                Me.DateInput.DateFormat = "MM/dd/yyyy h:mm tt"
            Else
                Me.DateInput.DateFormat = "MM/dd/yyyy"
            End If
        End Set
    End Property

    Public WriteOnly Property SetFocus As Boolean
        Set(value As Boolean)
            If value Then
                Me.Focus()
            End If
        End Set
    End Property

#End Region

    Public Sub New()
        'set initial properties
        Me.MinDate = Date.MinValue

        Dim calendarDay = New RadCalendarDay()
        calendarDay.Repeatable = Telerik.Web.UI.Calendar.RecurringEvents.Today
        calendarDay.ItemStyle.BackColor = Drawing.Color.PowderBlue
        Me.ShowTime = False


        Me.Calendar.SpecialDays.Add(calendarDay)

    End Sub

End Class
