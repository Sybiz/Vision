Public Class EmailAlternateCustomerAfterSalesTransactionProcess

    'Scenario: Email the sales invoice documents to an alternative customer (e.g. head office / branch office situations) after processing the sales invoice
    'Prerequisities: Sales invoice transaction settings (e.g. email preview)
    'Breakpoint: AfterSalesInvoiceProcess

    Public Sub Invoke(transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice, e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointEventArgs)
        'Determine if there is a difference in the order, delivery and invoice customers
        If transaction.OrderCustomer <> transaction.DeliveryCustomer OrElse transaction.DeliveryCustomer <> transaction.InvoiceCustomer Then
            'Stores the value of the alternate customer
            Dim aaid As Integer = 0

            'Check if it is the order or the delivery that is different
            If transaction.OrderCustomer <> transaction.InvoiceCustomer Then
                aaid = transaction.OrderCustomer
            Else
                aaid = transaction.DeliveryCustomer
            End If

            Dim transactiondocuments As List(Of Byte()) = BreakpointHelpers.CreateTransactionDocuments(TransactionType.SalesInvoice,transaction.Id)

            Dim attachments As New List(Of Byte())
            Dim filenames As New List(Of String)
            Dim templatecount As Integer = 1

            For Each template As Byte() In transactiondocuments
                attachments.Add(template)
                filenames.Add(transaction.TransactionNumber & " " & templatecount & ".pdf")
                templatecount += 1
            Next

            Dim altaccount As Sybiz.Vision.Platform.Debtors.Customer = Sybiz.Vision.Platform.Debtors.Customer.GetObject(aaid)
            Dim emailTo As String

            For Each contact As Sybiz.Vision.Platform.Common.Contact In altaccount.Contacts 
                For each doc As Sybiz.Vision.Platform.Common.ContactTransactionDocumentOption In contact.TransactionDocuments
                    If doc.TransactionTypeId = 103 Then
                        emailTo = emailTo + contact.Email + ";"
                    End If
                Next
            Next

            Dim subject As String = String.Empty
            Dim body As String = String.Empty
            Sybiz.Vision.WinUI.Utilities.FormFunctions.GetEmailSubjectAndBody(transaction, subject, body)
            BreakpointHelpers.SendEmail(emailTo,Nothing,Nothing,subject,body,filenames,attachments)				

        End If
    End Sub

End Class
