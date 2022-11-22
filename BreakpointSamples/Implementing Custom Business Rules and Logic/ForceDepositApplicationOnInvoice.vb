Public Class ForceDepositApplicationOnInvoice

  'Scenario: Ensures that an invoice is fully using any deposit that is present
  'Prerequisities: None
  'Breakpoint: BeforeSalesInvoiceProcess
	
	
  Public Sub Invoke(ByVal transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice, ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCancelEventArgs) 'Do not remove - SYBIZ

    For Each line As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoiceLine in transaction.Lines
      If line.AccountType = SalesLineType.DP Then
        If line.QuantityInvoice < line.QuantityOrder Then
          BreakpointHelpers.ShowErrorMessage(e.Form, "Deposit Error", "You must use the full deposit, please fully apply it now")
          BreakpointHelpers.PerformRibbonButtonClick(e.Form, "Apply Deposit")
          e.Cancel = True
        End If
      End If
    Next
	
  End Sub

End Class
