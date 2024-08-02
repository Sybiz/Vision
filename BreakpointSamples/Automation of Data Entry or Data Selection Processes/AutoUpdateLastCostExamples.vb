Public Class AutoUpdateLastCostExamples

  'Scenario: Automatically update last cost of products, whether a specific location, or generally
  'Prerequisities: None
  'Breakpoint: AfterStockTransferProcess and AfterManufactureIssueProcess

  Public Sub Invoke(ByVal transaction As Sybiz.Vision.Platform.Inventory.Transaction.StockTransfer, ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointEventArgs) 'Do not remove - SYBIZ

    For each line As Sybiz.Vision.Platform.Inventory.Transaction.StockTransferLine In transaction.Lines
	'If source location is elsewhere...
  	If  line.TransferType = Sybiz.Vision.Platform.Core.Enumerations.TransferType.LocationToLocation AndAlso line.Source = 0 Then		
		Try 
			Sybiz.Vision.Platform.Inventory.Product.UpdateLocationLastCost(line.Product,line.Destination,line.UnitCost,true)
		Catch ex as Exception
			BreakpointHelpers.ShowErrorMessage(e.Form, String.Format("Unable to update {0} last cost", line.ProductDetails.Code), ex.Message)
		End Try		
 	End If
    Next
		
	End Sub


  Public Sub Invoke(ByVal transaction As Sybiz.Vision.Platform.Inventory.Transaction.ManufactureIssue, ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointEventArgs) 'Do not remove - SYBIZ

    For each line As Sybiz.Vision.Platform.Inventory.Transaction.ManufactureIssueLine In transaction.Lines
      Sybiz.Vision.Platform.Inventory.Product.UpdateLastCost(line.Product,line.UnitCost,false)
      
      'Alternatively, could use destination location here if one wanted to only update specific manufacturing location last cost
      'Sybiz.Vision.Platform.Inventory.Product.UpdateLocationLastCost(line.ProductDetails.Id,line.DestinationLocation,line.UnitCost,false)
    Next
  
  End Sub

End Class
