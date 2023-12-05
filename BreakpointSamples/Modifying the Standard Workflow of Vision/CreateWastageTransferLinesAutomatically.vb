	Public Class CreateWastageTransferLines
  
    'Scenario: When transferring goods to a job, only whole sheets can be used even if split accross multiple jobs. Need to manage the wastage to be disposed off (transferred elsewhere) without the need to do multiple transfers
    'Prerequisities: None, but this applies to all products which might not meet your expectations
    
    'Breakpoint: BeforeStockTransferProcess

		Public Sub Invoke(ByVal transaction As Sybiz.Vision.Platform.Inventory.Transaction.StockTransfer, ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCancelEventArgs)
		
      Dim productId as Integer = 0
			Dim locationId as Integer = 0
			Dim productDictionary as New Dictionary(Of String, Decimal)
			
			transaction.Lines.ToList().ForEach(Sub(ln) 
													If ln.Notes = "Automatically calculated wastage" Then
														transaction.Lines.Remove(ln)
													End If
												End Sub)													
			
			For Each line as Sybiz.Vision.Platform.Inventory.Transaction.StockTransferLine in transaction.Lines.OrderBy(Function(ln) ln.Product).ThenBy(Function(ln2) ln2.Source)
				If line.Source <> 0 Then
					If line.Product <> productId Then
						productId = line.Product
						locationId = line.Source
						productDictionary.Add(String.Format("{0}.{1}",productId, locationId), 0D)
					ElseIf line.Source <> locationId Then
						locationId = line.Source
						productDictionary.Add(String.Format("{0}.{1}",productId, locationId), 0D)
					End If
					
					productDictionary.Item(String.Format("{0}.{1}",productId, locationId)) += line.BaseQuantity					
				End If				
			Next 
			
			Dim pValue As Object
			
			For Each pValue in productDictionary
				If Decimal.Ceiling(pValue.Value)-pValue.Value > 0D Then
					Dim newLine As Sybiz.Vision.Platform.Inventory.Transaction.StockTransferLine = transaction.Lines.AddNew(TransferType.LocationToLocation)
					Dim keyString as String = pValue.Key.ToString()
					newLine.Product = Convert.ToInt32(pValue.Key.Split(".")(0))
					newLine.Source = Convert.ToInt32(pValue.Key.Split(".")(1))
					newLine.Destination = 0
					newLine.Quantity = Decimal.Ceiling(pValue.Value)-pValue.Value	
					newLine.Notes = "Automatically calculated wastage"
				End If
			Next 

		End Sub 
    
	End Class
