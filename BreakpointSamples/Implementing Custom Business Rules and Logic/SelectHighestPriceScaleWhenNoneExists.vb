Public Class SelectHighestPriceScaleWhenNoneExists

  'Scenario: When selecting a product, if the customer's price scale doesn't exist against a product, selects the price scale with the highest price when changing product or UoM (when price recalculations occur)
  'Prerequisities: None, but there is an assumption in the below example that there is an intention to have at least one price scale per product.
  'If that assumption is inaccurate, then this breakpoint will behave very poorly!
  
  'Note, this methodology will 'break' if Update Prices (or similar) is used. For extra $0 line protection, consider using a before save / process breakpoint as well, in the vein of https://github.com/Sybiz/Vision/blob/master/BreakpointSamples/Implementing%20Custom%20Business%20Rules%20and%20Logic/EnforceInvoiceQuantityBeforeTransactionSave.vb

  'Breakpoint: SalesInvoiceCellValueChanged

  Public Sub Invoke(ByVal transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice, ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointSalesDeliveryInvoiceCellValueChangedEventArgs(Of Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoiceLine)) 'Do not remove - SYBIZ
    If e.FieldName.Equals("Account") = True OrElse e.FieldName.Equals("UnitOfMeasure") = True Then
      Dim p As Sybiz.Vision.Platform.Inventory.Transaction.IProductDetail = TryCast(e.Line, Sybiz.Vision.Platform.Inventory.Transaction.IProductDetail)
      
      If p IsNot Nothing AndAlso p.ProductDetails.IsValid Then 
        If p.ProductDetails.ProductPrices.Any(Function(x) x.Id = transaction.CustomerDetails.PriceScale) = False Then
            Dim highPrice As Sybiz.Vision.Platform.Inventory.ProductPriceDetailInfo = p.ProductDetails.ProductPrices.OrderByDescending(Function(x) x.Price).FirstOrDefault()
            If highPrice Is Nothing Then
              If transaction.PriceEntryMode = TransactionPriceMode.Exclusive Then
                e.Line.ChargeExclusive = 10000000
              Else
                e.Line.ChargeInclusive = 10000000
              End If
              BreakpointHelpers.ShowErrorMessage(e.Form, "ERROR", "This product has no price scales, and this should be rectified immediately")
            Else 
              If transaction.PriceEntryMode = TransactionPriceMode.Exclusive Then
                e.Line.UnitChargeExclusive = highprice.Price * e.Line.UnitOfMeasureFactor
              Else
                e.Line.UnitChargeInclusive = highprice.InclusiveCharge * e.Line.UnitOfMeasureFactor
              End If          
            End If                    
        End If
      End If
    End If
  End Sub
      
End Class
