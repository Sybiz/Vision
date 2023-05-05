Public Class AutoUpdateLastCostExamples

  'Scenario: Automatically update last cost of products, whether a specific location, or generally
  'Prerequisities: None
  'Breakpoint: AfterStockTransferProcess and AfterManufactureIssueProcess

  Public Sub Invoke(ByVal transaction As Sybiz.Vision.Platform.Inventory.Transaction.StockTransfer, ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointEventArgs) 'Do not remove - SYBIZ

    For each l As Sybiz.Vision.Platform.Inventory.Transaction.StockTransferLine In transaction.Lines
      'If source location is elsewhere...
      If l.SourceLocationDetails.Id = 0 Then
      Sybiz.Vision.Platform.Inventory.Product.UpdateLocationLastCost(l.Product,l.Destination,l.UnitCost,true)
      End If
	  Next
		
	End Sub


  Public Sub Invoke(ByVal transaction As Sybiz.Vision.Platform.Inventory.Transaction.ManufactureIssue, ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointEventArgs) 'Do not remove - SYBIZ

    For each l As Sybiz.Vision.Platform.Inventory.Transaction.ManufactureIssueLine In transaction.Lines
      Sybiz.Vision.Platform.Inventory.Product.UpdateLastCost(l.Product,l.UnitCost,true)
      
      'Alternatively, could use destination location here if one wanted to only update specific manufacturing location last cost
      'Sybiz.Vision.Platform.Inventory.Product.UpdateLocationLastCost(l.ProductDetails.Id,l.DestinationLocation,l.UnitCost,true)
    Next
  
  End Sub

End Class
