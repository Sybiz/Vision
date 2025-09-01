Public Class AddServiceQuoteLinesToTransferOrder

	'Scenario: Prompt for a service quote number when a custom ribbon button is pressed and add the IC lines to the transfer order with quantities from the service quote
	'Prerequisities: Custom button to be added, additional logic to only process accepted quotes might be accepted
	'Breakpoint: StockTransferOrderCustomRibbonButtonClick
  
		Public Sub Invoke(ByVal transaction As Sybiz.Vision.Platform.Inventory.Transaction.StockTransferOrder, ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCustomRibbonButtonClickEventArgs)
			Dim serviceQuoteNumber As String = BreakpointHelpers.GetInputValue(e.Form, "Add Source Document", "Enter Service Quote Number", "")
			
			If Not String.IsNullOrEmpty(serviceQuoteNumber) Then
				Dim serviceQuote = Sybiz.Vision.Platform.Service.Transaction.ServiceQuote.GetObject(Nothing, serviceQuoteNumber)
				Dim serviceQuoteLine As Sybiz.Vision.Platform.Service.Transaction.ServiceQuoteLine

				transaction.TransferOrderType = TransferOrderType.LocationToService			
				transaction.Description = serviceQuote.TransactionNumber
				transaction.Destination = serviceQuote.ServiceRequest
				
				For Each serviceQuoteLine In serviceQuote.Lines
					If (serviceQuoteLine.LineType = ServiceLineType.IC AndAlso TryCast(serviceQuoteLine, sybiz.Vision.Platform.Inventory.Transaction.IProductDetail).ProductDetails.IsDiminishingStock) Then
						If (transaction.Source <= 0)
							transaction.Source =  serviceQuoteLine.Location
						End If
						
						Dim newLine = transaction.Lines.AddNew()
						newLine.Product = serviceQuoteLine.Account
						newLine.UnitOfMeasure = serviceQuoteLine.UnitOfMeasure
						newLine.Quantity = serviceQuoteLine.Quantity
					End If
				Next	
				
				BreakpointHelpers.FocusOnFirstRowAndColumn(e.Form, "QuantityAllocate")
			End If				
	End Sub
End Class
