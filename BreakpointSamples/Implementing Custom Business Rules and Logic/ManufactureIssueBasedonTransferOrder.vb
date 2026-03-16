Public Class ManufactureIssueBasedonTransferOrder

  'Scenario: Checks to see if it is required to create assemblies based on a transfer order, then does so if required
  'Prerequisities: Nil
  'Breakpoint: BeforeStockTransferOrderProcess
  
  Public Sub Invoke(ByVal transaction As Sybiz.Vision.Platform.Inventory.Transaction.StockTransferOrder, ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCancelEventArgs) 'Do not remove - SYBIZ

    'If there are kit lines needing manufacture then ready the issue
    If transaction.IsNew = True AndAlso transaction.Lines.Any(Function (x) x.ProductDetails.IsAssembly = True AndAlso x.ProductDetails.AssemblyDetails.CanManufactureOnDelivery = True AndAlso x.ProductDetails.AssemblyDetails.IsCurrentVersion = True AndAlso x.ProductDetails.GetStockAvailable(transaction.SourceLocationDetails.Id) <= x.Quantity) = True Then
      Dim reqQuantities As New Dictionary(Of Integer, Decimal)
      
      For Each line As Sybiz.Vision.Platform.Inventory.Transaction.StockTransferOrderLine In transaction.Lines
        'Only assemble the quantity required and use existing quantities where they exist
        If line.ProductDetails.IsAssembly = True AndAlso line.ProductDetails.AssemblyDetails.CanManufactureOnDelivery = True AndAlso line.ProductDetails.AssemblyDetails.IsCurrentVersion = True AndAlso line.ProductDetails.GetStockAvailable(transaction.SourceLocationDetails.Id) <= line.Quantity Then 
          If reqQuantities.ContainsKey(line.ProductDetails.AssemblyDetails.Id) Then
            reqQuantities(line.ProductDetails.AssemblyDetails.Id) += line.Quantity
          Else
            reqQuantities.Add(line.ProductDetails.AssemblyDetails.Id,line.Quantity - line.ProductDetails.GetStockAvailable(transaction.SourceLocationDetails.Id))
          End If
        End If
      Next
      
      Dim newMI = Sybiz.Vision.Platform.Inventory.Transaction.ManufactureIssue.NewObject(Nothing, True, False)
      newMI.Description = String.Format("Auto MI for Transfer Order {0}", transaction.TransferOrderNumber)
      newMI.TransactionDate = transaction.TransactionDate
      newMI.Notes = "Created by breakpoint from Stock Transfer Order"
      newMI.UseWorkOrder = False
      
      For Each kvp As KeyValuePair(Of Integer, Decimal) In reqQuantities
        Dim newLine = newMI.Lines.AddNew()
        													
        newLine.Assembly = kvp.Key
        newLine.Quantity = kvp.Value
        newLine.SourceLocation = transaction.SourceLocationDetails.Id
        newLine.DestinationLocation = transaction.SourceLocationDetails.Id
      Next
      					
      If newMI.IsProcessable = True Then
        'Process issue and allocate stock
        newMI = newMI.Process()
        						
        'Allocate stock
        For Each line As Sybiz.Vision.Platform.Inventory.Transaction.StockTransferOrderLine In transaction.Lines
          If line.ProductDetails.IsAssembly AndAlso line.ProductDetails.AssemblyDetails.CanManufactureOnDelivery = True Then
            line.QuantityAllocate = line.Quantity
          End If
        Next
        						
        BreakpointHelpers.ShowInformationMessage(e.Form, "Success", "Kits have been assembled and allocated to transfer order lines")
        						
      Else If newMI.IsSavable = True Then
        newMI = newMI.Save()
        BreakpointHelpers.ShowErrorMessage(e.Form, "Error", String.Format("Manufacture issue was not processed, but saved manufacture issue {0} created. Review and process manually before continuing.", newMI.TransactionNumber))
        e.Cancel = True
      Else If newMI.IsValid = True
        BreakpointHelpers.ShowErrorMessage(e.Form, "Error", "Manufacture issue could not be saved, please review critical broken rules for the issue about to be displayed")
        e.Cancel = True
        For Each brokenRule As Sybiz.Vision.Platform.Validation.BrokenRuleInfo In newMI.GetBrokenRuleInfo()
          If (brokenRule.Severity = Csla.Validation.RuleSeverity.Error) Then
            BreakpointHelpers.ShowErrorMessage(e.Form, brokenRule.PropertyName, brokenRule.Description)
          End If
        Next 
      End If
    End If
  End Sub
End Class
