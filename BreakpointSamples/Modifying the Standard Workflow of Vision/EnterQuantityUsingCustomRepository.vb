Public Class EnterQuantityUsingCustomRepository

    'Scenario: Enable the custom repository on the quantity invoice field on sales invoices
    'Prerequisities: None
    'Breakpoint: SalesInvoiceUseCustomCellRepository

    Public Sub Invoke(transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice, e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCellRepositoryEventArgs(Of Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoiceLine))
        'Check for the quantity invoice field
        If e.FieldName = "QuantityInvoice" Then
            'Set to true to enable the custom repository
            e.Handled = True
        End If
    End Sub

    'Scenario: Use the custom repository on the quantity invoice field to enter the quantity with a DevExpress XtraInputBox
    'Prerequisities: None
    'Breakpoint: SalesInvoiceUseCustomRepositoryButtonClick

    Public Sub Invoke(transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoiceLine, e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCellRepositoryEventArgs(Of Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoiceLine))
        'Create and show the DevExpress XtraInputBox with the existing quantity invoice
        Dim input As String = InputBox("Enter Quantity Invoice:", "Breakpoints", e.Line.QuantityInvoice)
        Dim quantity As Decimal = 0D

        If String.IsNullOrWhiteSpace(input) Then
            'Do Nothing - Cancel Clicked
        ElseIf Not String.IsNullOrWhiteSpace(input) AndAlso Decimal.TryParse(input, quantity) Then
            e.Line.QuantityInvoice = quantity
        Else
            MessageBox.Show("Not a decimal value", "Breakpoints")
        End If
    End Sub

End Class