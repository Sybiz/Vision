Public Class AddSalesTemplateOnCustomerChange

    'Scenario: Automatically add a sales template to the sales invoice if it exists
    'Prerequisities: None
    'Breakpoint: SalesInvoiceCustomerChanged

    Public Sub Invoke(transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice, e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointEventArgs)
        'Use the scalar command to get the top sales template for the customer on the sales invoice
        Dim templateId As Integer = Sybiz.Vision.Platform.Core.Data.ScalarCommand.Execute(Of Integer)(String.Format("SELECT TOP 1 SalesTemplateId FROM [dr].[SalesTemplate] WHERE CustomerId = {0}", transaction.Customer))
        'Cast the sales transaction to an ISalesTransaction
        Dim iTransaction As Sybiz.Vision.Platform.Debtors.Transaction.ISalesTransaction = DirectCast(transaction, Sybiz.Vision.Platform.Debtors.Transaction.ISalesTransaction)

        'Only add the template document if one was returned from the scalar command
        If templateId > 0 Then
            iTransaction.AddTemplateDocument(templateId)
        End If
    End Sub

End Class
