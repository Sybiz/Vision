Public Class EnforceMinimumTotalBeforeTransactionProcess

    'Scenario: Enforce a minimum spend of $100 on sales invoices
    'Prerequisities: None
    'Breakpoint: BeforeSalesInvoiceProcess

    Public Sub Invoke(transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice, e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCancelEventArgs)
        If transaction.Total < 100D Then
            'Set to true to cancel the process
            e.Cancel = True
            MessageBox.Show("Invoice must be over $100", "Breakpoints")
        End If
    End Sub

End Class