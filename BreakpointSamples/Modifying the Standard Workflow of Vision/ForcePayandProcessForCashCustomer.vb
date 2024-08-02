Public Class ForcePayandProcessForCashCustomer

    'Scenario: Checks to see if the customer is a Cash customer and if so, forces the user to utilise Pay and Process, blocking attempts to just Process
    'Prerequisities: None
    'Breakpoint: BeforeSalesInvoiceProcess
	
	
	Public Sub Invoke(ByVal transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice, ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCancelEventArgs) 'Do not remove - SYBIZ

		If transaction.CustomerDetails.TradingTerms.Term = TradingTermType.Cash Then
			Dim salesForm As Sybiz.Vision.WinUI.Base.SalesTransactionBase = DirectCast(e.Form, Sybiz.Vision.WinUI.Base.SalesTransactionBase)
			If (salesForm.HasReceipt = false)
				e.Cancel = True
				BreakpointHelpers.PerformRibbonButtonClick(e.Form, "Pay & Process")
			End If
		End If
	
	End Sub

End Class
