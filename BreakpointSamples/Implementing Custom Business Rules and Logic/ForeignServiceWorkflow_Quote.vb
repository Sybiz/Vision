Public Class ForeignServiceWorkflow_Quote

  'Scenario: In combination with ForeignServiceWorkflow_Invoice, handles a concept of processing a Service Quote (for a "Quote" based service request invoice) in "foreign currency", for invoicing.
  'Prerequisities: This breakpoint assumes foreign currency price scales having been set up against products. This breakpoint is un-necessary if this has not been done; and FX values should be manually entered.
  		  'It is also noted that generally speaking, the idea here is that a form edit would be done to manage that part of the problem.
  'Breakpoint: BeforeServiceQuoteProcess
	

  Public Sub Invoke(ByVal transaction As Sybiz.Vision.Platform.Service.Transaction.ServiceQuote, ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCancelEventArgs) 'Do not remove - SYBIZ

    If transaction.CustomerDetails.Group.Currency.IsBaseCurrency = False AndAlso transaction.ServiceQuoteStatus = ServiceQuoteStatus.Pending
      Dim cont As DialogResult = BreakpointHelpers.ShowYesNoMessageBox(e.Form, "WARNING", "Foreign currency customer detected; do you wish to multiply IC lines by the current foreign exhange rate?")
      If cont = DialogResult.Yes Then
        For each l As Sybiz.Vision.Platform.Service.Transaction.ServiceQuoteLine In transaction.Lines.Where(Function(x) x.LineType = ServiceLineType.IC)
          l.UnitCharge = l.UnitCharge * transaction.CustomerDetails.Group.Currency.BuyRate
        Next
      End If
    Else If transaction.CustomerDetails.Group.Currency.IsBaseCurrency = False AndAlso transaction.ServiceQuoteStatus = ServiceQuoteStatus.Accepted
      BreakpointHelpers.ShowInformationMessage(e.Form, "WARNING", "This was detected as a foreign currency customer, we cannot multiple IC lines as the quote has been accepted! Revert it to pending if needed")
    End If
	
  End Sub

End Class
