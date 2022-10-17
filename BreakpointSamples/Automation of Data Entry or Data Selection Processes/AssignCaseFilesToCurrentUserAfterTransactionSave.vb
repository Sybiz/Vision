Public Class AssignCaseFilesToCurrentUserAfterTransactionSave

    'Scenario: Assign any case files on the sales invoice to the current user after saving the sales invoice
    'Prerequisities: Attached case files and edit rights to case files
    'Breakpoint: AfterSalesInvoiceSave

    Public Sub Invoke(transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice, e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointEventArgs)
        'Check that there are case files on the sales invoice
        If transaction.CaseFilesExist Then
            'Iterate through each of the case files
            For Each storeCaseFile As Sybiz.Vision.Platform.ContactManagement.CaseFileStore In transaction.CaseFiles
                'Get the case file object from the case file store object
                Dim objectCaseFile As Sybiz.Vision.Platform.ContactManagement.Transaction.CaseFile = Sybiz.Vision.Platform.ContactManagement.Transaction.CaseFile.GetObject(storeCaseFile.Id)
                'No need to do anything if the case file user is the current user
                If objectCaseFile.User <> Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal().UserId Then
                    'Assign the current user to the case file
                    objectCaseFile.User = Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal().UserId
                    'Save the case file
                    objectCaseFile = objectCaseFile.Save()
                End If
            Next
        End If
    End Sub

End Class