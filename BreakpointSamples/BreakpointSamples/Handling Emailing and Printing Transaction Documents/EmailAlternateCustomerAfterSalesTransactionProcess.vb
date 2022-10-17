Public Class EmailAlternateCustomerAfterSalesTransactionProcess

    'Scenario: Email the sales invoice documents to an alternative customer (e.g. head office / branch office situations) after processing the sales invoice
    'Prerequisities: Sales invoice transaction settings (e.g. email preview)
    'Breakpoint: AfterSalesInvoiceProcess

    Public Sub Invoke(transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice, e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointEventArgs)
        'Determine if there is a difference in the order, delivery and invoice customers
        If transaction.OrderCustomer <> transaction.DeliveryCustomer OrElse transaction.DeliveryCustomer <> transaction.InvoiceCustomer Then
            'Stores the value of the alternate customer
            Dim alternateAccount As Integer = 0

            'Check if it is the order or the delivery that is different
            If transaction.OrderCustomer <> transaction.InvoiceCustomer Then
                alternateAccount = transaction.OrderCustomer
            Else
                alternateAccount = transaction.DeliveryCustomer
            End If

            'See above for explanation of parameters
            Sybiz.Vision.Platform.Core.Transaction.TransactionDocumentEngine.ProduceTransactionDocument(transaction, True, True, AddressOf Sybiz.Vision.WinUI.Utilities.FormFunctions.PrintTransaction, AddressOf Sybiz.Vision.WinUI.Utilities.FormFunctions.PreviewTransaction, AddressOf Sybiz.Vision.WinUI.Utilities.FormFunctions.ExportTransaction, AddressOf Sybiz.Vision.WinUI.Utilities.FormFunctions.EmailTransaction, AddressOf Sybiz.Vision.WinUI.Utilities.FormFunctions.EmailPreviewTransaction, AlternateAccountId:=alternateAccount)
        End If
    End Sub

End Class