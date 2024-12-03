Public Class ForeignServiceWorkflow_AdvInvoice

  'Scenario: In combination with ForeignServiceWorkflow_Quote, handles a concept of processing a Service Quote (for a "Quote" based service request invoice) in "foreign currency", for invoicing in that same currency, specifically for an advanced Service Request
  'Prerequisities: N/A
  'Breakpoint: ServiceInvoiceServiceRequestChanged
  	
  
  Public Sub Invoke(ByVal transaction As Sybiz.Vision.Platform.Service.Transaction.ServiceInvoice, ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointEventArgs) 'Do not remove - SYBIZ

    'Assumption is present that any foreign customer + "Quote" based SR is "eligible" for this breakpoint, but dialog present so that operator can say "no" if they want.
    If transaction.CustomerDetails.Group.Currency.IsBaseCurrency = False AndAlso transaction.ServiceRequestDetails.ServiceRequestInvoicingMethod = ServiceRequestInvoicingMethod.Quote Then
      Dim cont As DialogResult = BreakpointHelpers.ShowYesNoMessageBox(e.Form, "Foreign Customer Detected", "This has been detected as a foreign currency service request, would you like to use the FX value of its quote?")
      For each l As Sybiz.Vision.Platform.Service.Transaction.ServiceInvoiceLine In transaction.Lines
        l.Charge = l.QuotedBaseCharge
      Next
    End If
    	
  End Sub
End Class
