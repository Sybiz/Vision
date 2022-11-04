Public Class DisableCompanyandUserOutput

    'Scenario: Checks to see if Company or User Output is enabled in a Sales Invoice, then disables both.
    'Prerequisities: None
    'Breakpoint: BeforeSalesInvoiceProcess
	
	
	Public Sub Invoke(ByVal transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice, ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCancelEventArgs) 'Do not remove - SYBIZ

		If BreakpointHelpers.ReadRibbonEditorValue(e.Form, "Company Output") = True OrElse BreakpointHelpers.ReadRibbonEditorValue(e.Form, "User Output") = True Then
			BreakpointHelpers.SetRibbonEditorValue(e.Form, "Company Output", False)
			BreakpointHelpers.SetRibbonEditorValue(e.Form, "User Output", False)
		End If
	
	End Sub

End Class
