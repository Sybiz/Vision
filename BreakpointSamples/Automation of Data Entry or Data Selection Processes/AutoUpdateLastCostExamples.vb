Public Class AutoUpdateLastCostExamples

  'Scenario: Automatically update last cost of products, whether a specific location, or generally
  'Prerequisities: None
  'Breakpoint: AfterStockTransferProcess and AfterManufactureIssueProcess

  Public Sub Invoke(ByVal transaction As Sybiz.Vision.Platform.Inventory.Transaction.StockTransfer, ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointEventArgs) 'Do not remove - SYBIZ

    For each l As Sybiz.Vision.Platform.Inventory.Transaction.StockTransferLine In transaction.Lines
	'If source location is elsewhere...
  	If  l.TransferType = Sybiz.Vision.Platform.Core.Enumerations.TransferType.LocationToLocation AndAlso l.Source = 0 Then		
		Try 
			Sybiz.Vision.Platform.Inventory.Product.UpdateLocationLastCost(l.Product,l.Destination,l.UnitCost,true)
		Catch ex as Exception
			BreakpointHelpers.ShowErrorMessage(e.Form, String.Format("Unable to update {0} last cost", l.ProductDetails.Code), ex.Message)
		End Try		
 	End If
    Next
		
	End Sub


  Public Sub Invoke(ByVal transaction As Sybiz.Vision.Platform.Inventory.Transaction.ManufactureIssue, ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointEventArgs) 'Do not remove - SYBIZ

    For each l As Sybiz.Vision.Platform.Inventory.Transaction.ManufactureIssueLine In transaction.Lines
      Sybiz.Vision.Platform.Inventory.Product.UpdateLastCost(l.Product,l.UnitCost,false)
      
      'Alternatively, could use destination location here if one wanted to only update specific manufacturing location last cost
      'Sybiz.Vision.Platform.Inventory.Product.UpdateLocationLastCost(l.ProductDetails.Id,l.DestinationLocation,l.UnitCost,false)
    Next
  
  End Sub

End Class
