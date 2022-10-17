Public Class EnforceInvoiceQuantityBeforeTransactionSave

    'Scenario: Enforce that the invoice quantity cannot be zero on sales invoices
    'Prerequisities: None
    'Breakpoint: BeforeSalesInvoiceSave

    Public Sub Invoke(transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice, e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCancelEventArgs)
        Dim foundZeroQuantity As Boolean = False

        'Iterate through each sales invoice line in the sales invoice to determine if a zero invoice quantity line exists
        For Each line As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoiceLine In transaction.Lines
            If line.QuantityInvoice = 0 Then
                foundZeroQuantity = True
            End If
        Next

        If foundZeroQuantity Then
            'Set to true to cancel the save
            e.Cancel = True
            MessageBox.Show("Zero invoice quantity line exists on sales invoice", "Breakpoints")
        End If
    End Sub

End Class