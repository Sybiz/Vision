Public Class ForeignServiceWorkflow_Quote

  'Scenario: In combination with ForeignServiceWorkflow_Quote, handles a concept of processing a Service Quote (for a "Quote" based service request invoice) in "foreign currency", for invoicing in that same currency.
  'Prerequisities: N/A
  'Breakpoint: SalesInvoiceCellValueChanged (Could be refactored to BeforeProcess potentially)
  	
  
  Public Sub Invoke(ByVal transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice, ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointSalesDeliveryInvoiceCellValueChangedEventArgs(Of Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoiceLine)) 'Do not remove - SYBIZ

    'We are painting all Service Requests for a foreign currency customer with the same brush; additional logic / restrictions can be implemented at one's convenience
    If e.FieldName = "Account" AndAlso e.Line.AccountType = SalesLineType.SV AndAlso transaction.CustomerDetails.Group.Currency.IsBaseCurrency = False Then
      Dim cont As DialogResult = BreakpointHelpers.ShowYesNoMessageBox(e.Form, "Foreign Quote Detected", "This has been detected as a foreign currency service request, would you like to use the FX value of its quote?")
      If cont = DialogResult.Yes Then
        Dim serviceRequest As Sybiz.Vision.Platform.Service.ServiceRequestDetailInfo = Sybiz.Vision.Platform.Service.ServiceRequestDetailInfo.GetObject(e.Line.Account)
        
        'Usually the calculation is OutstandingCharges * ExchangeRate, so we short-circuit that to give us a "pure" FX value. Also, note assumption for Exclusive.
        e.Line.UnitChargeExclusive = serviceRequest.OutstandingCharges
      End If
    End If
  	
  End Sub

End Class
