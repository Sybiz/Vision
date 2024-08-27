Public Class RemoveIdenticalLinesOnTransaction

    'Scenario: Prevent lines with identical types and items from being added to sales invoices
    'Prerequisities: None
    'Breakpoint: SalesInvoiceCellValueChanged

    Public Sub Invoke(transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice, e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCellValueChangedEventArgs(Of Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoiceLine))
        'Check for the item field
        If e.FieldName.Equals("Account") = True Then
            'Iterate through each sales invoice line in the sales invoice
            For Each line As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoiceLine In transaction.Lines
                'Check the line being added against the currently iterated sales invoice line
                If e.Line.Id <> line.Id AndAlso e.Line.AccountType = line.AccountType AndAlso e.Line.Account = line.Account Then
                    'Remove the line being added if the type and item is identical
                    BreakpointHelpers.ShowInformationMessage(e.Form, "Breakpoints", "Existing line with this item and account")
                    transaction.Lines.Remove(e.Line)
                    Exit For
                End If
            Next
        End If
    End Sub

End Class
