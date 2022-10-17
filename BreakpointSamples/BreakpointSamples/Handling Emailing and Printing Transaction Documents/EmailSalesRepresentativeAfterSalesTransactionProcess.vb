Public Class EmailSalesRepresentativeAfterSalesTransactionProcess

    'Scenario: Email the sales invoice documents to the specified email address after processing the sales invoice (e.g. sales representative)
    'Prerequisities: Sales invoice transaction settings (e.g. email preview)
    'Breakpoint: AfterSalesInvoiceProcess

    Public Sub Invoke(transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice, e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointEventArgs)
        'Source: Transaction to generate documents
        'CompanyTemplates: Determines whether company templates are used
        'UserTemplates: Determines whether user templates are used
        'PrintHandler: Handler for printing documents (can be nothing to not print)
        'PreviewHandler: Handler for print previewing documents (can be nothing to not print preview)
        'GenerateHandler: Handler for generating documents
        'EmailHandler: Handler for emailing documents (can be nothing to not email)
        'EmailPreviewHandler: Handler for email previewing documents (can be nothing to not email preview)
        'Subject (Optional): Specify a subject for emailing documents
        'Body (Optional): Specify a body for emailing documents
        'AlternateAccountId (Optional): Specify an alternate account (e.g. customer, supplier) for emailing documents
        'AlternateEmailAddress (Optional): Specify an alternate email address for emailing documents
        'Notes: Handlers for printing and emailing can be swapped with other handlers if desired (e.g. use the email handler for an email preview)

        Dim subject As String = String.Empty
        Dim body As String = String.Empty

        'Get the subject and body from the relevant email template
        Sybiz.Vision.WinUI.Utilities.FormFunctions.GetEmailSubjectAndBody(transaction, subject, body)
        Sybiz.Vision.Platform.Core.Transaction.TransactionDocumentEngine.ProduceTransactionDocument(transaction, True, True, AddressOf Sybiz.Vision.WinUI.Utilities.FormFunctions.PrintTransaction, AddressOf Sybiz.Vision.WinUI.Utilities.FormFunctions.PreviewTransaction, AddressOf Sybiz.Vision.WinUI.Utilities.FormFunctions.ExportTransaction, AddressOf Sybiz.Vision.WinUI.Utilities.FormFunctions.EmailTransaction, AddressOf Sybiz.Vision.WinUI.Utilities.FormFunctions.EmailPreviewTransaction, Subject:=subject, Body:=body, AlternateEmailAddress:="email@email.com")
    End Sub

End Class