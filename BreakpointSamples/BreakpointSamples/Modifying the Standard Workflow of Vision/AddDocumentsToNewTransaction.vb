Public Class AddDocumentsToNewTransaction

    'Scenario: Automatically allow the addition of documents to new sales invoices
    'Prerequisities: None
    'Breakpoint: AfterSalesInvoiceNew

    Public Sub Invoke(transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice, e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointEventArgs)
        'Create the DevExpress XtraOpenFileDialog using our NewOpenFileDialog function (optional parameters are filter, multiselect and default extension) as per comment below, running code only provided to allow compiliation 
        'Using dlg As DevExpress.XtraEditors.XtraOpenFileDialog = Sybiz.Vision.WinUI.Utilities.FormFunctions.NewOpenFileDialog(filter:="All Files (*.*)|*.*", multi:=True)
        Using dlg As New OpenFileDialog()
            'Add each file that was selected to the documents of the sales invoice
            If dlg.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                For Each document As String In dlg.FileNames
                    transaction.Documents.AddNew(dlg.FileName)
                Next
            End If
        End Using
    End Sub

End Class