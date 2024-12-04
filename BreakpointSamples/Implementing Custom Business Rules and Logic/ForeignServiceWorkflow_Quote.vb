Public Class ForeignServiceWorkflow_Quote

  'Scenario: In combination with ForeignServiceWorkflow_Invoice, handles a concept of processing a Service Quote (for a "Quote" based service request invoice) in "foreign currency", for invoicing.
  'Prerequisities: This breakpoint assumes that all possible IC / LB lines need "conversion" into foreign currency.
  		  'It is also noted that generally speaking, the idea here is that a form edit would be done to manage that "document" part of the problem.
  'Breakpoint: BeforeServiceQuoteProcess
	

  Public Sub Invoke(ByVal transaction As Sybiz.Vision.Platform.Service.Transaction.ServiceQuote, ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCancelEventArgs) 'Do not remove - SYBIZ

		If transaction.CustomerDetails.Group.Currency.IsBaseCurrency = False AndAlso transaction.ServiceQuoteStatus = ServiceQuoteStatus.Pending
			Dim cont As DialogResult = BreakpointHelpers.ShowYesNoMessageBox(e.Form, "WARNING", "Foreign currency customer detected; do you wish to multiply IC & Variable Labour Rate LB lines by the current foreign exhange rate?")
			If cont = DialogResult.Yes Then
				For each l As Sybiz.Vision.Platform.Service.Transaction.ServiceQuoteLine In transaction.Lines
					'Note that this example only hits VARIABLE charge labour rates. You may need to consider this factor in your business case.
					'(If we target a fixed labour rate and try to change it, all we're going to get is an error...)
					If l.LineType = ServiceLineType.IC OrElse (l.LineType = ServiceLineType.LB AndAlso l.CanWriteProperty("Charge")) Then
						l.UnitCharge = l.UnitCharge * transaction.CustomerDetails.Group.Currency.BuyRate
					End If
				Next
			End If
		Else If transaction.CustomerDetails.Group.Currency.IsBaseCurrency = False AndAlso transaction.ServiceQuoteStatus = ServiceQuoteStatus.Accepted
			BreakpointHelpers.ShowInformationMessage(e.Form, "WARNING", "This was detected as a foreign currency customer, we cannot multiple any lines as the quote has been accepted! Revert it to pending if needed")
	End If
	
  End Sub

End Class
