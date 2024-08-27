Public Class DisableHeaderTaxCodeFieldOnTransactions

    'Scenario: Disable the tax code field on sales invoices to enforce the sales tax code on products
    'Prerequisities: None
    'Breakpoint: SalesInvoiceDisableCustomCellRepository

    Public Sub Invoke(transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice, e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCellRepositoryEventArgs(Of Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoiceLine))
        'Check for the tax code field
        If e.FieldName.Equals("TaxCode") Then
            'Set to true to disable the tax code field
            e.Handled = True
        End If
    End Sub

End Class
