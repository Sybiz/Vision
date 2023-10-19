Public Class SelectHighestPriceScaleWhenNoneExists

  'Scenario: When selecting a product, if the customer's price scale doesn't exist against a product, selects the price scale with the highest price
  'Prerequisities: None, but there is an assumption in the below example that there is an intention to have at least one price scale per product.
  'If that assumption is inaccurate, then this breakpoint will behave very poorly!
      
  'Breakpoint: SalesInvoiceCellValueChanged

  Public Sub Invoke(ByVal transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice, ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointSalesDeliveryInvoiceCellValueChangedEventArgs(Of Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoiceLine)) 'Do not remove - SYBIZ
    If e.FieldName = "Account" Then
      Dim cx As Sybiz.Vision.Platform.Debtors.CustomerDetailInfo = Sybiz.Vision.Platform.Debtors.CustomerDetailInfo.GetObject(transaction.Customer)
      Dim p As Sybiz.Vision.Platform.Inventory.ProductDetailInfo = Sybiz.Vision.Platform.Inventory.ProductDetailInfo.GetObject(e.Line.Account)
      If p.ProductPrices.Any(Function(x) x.GetType().GetProperty("Id").Getvalue(x).Equals(cx.PriceScale)) = False Then
        Try
          Dim highprice As Sybiz.Vision.Platform.Inventory.ProductPriceDetailInfo = p.ProductPrices.OrderByDescending(Function(x) x.GetType().GetProperty("Price").GetValue(x)).FirstOrDefault()
          e.Line.UnitChargeExclusive = highprice.Price
        Catch ex As Exception
          If p.ProductPrices.Count = 0 Then
          BreakpointHelpers.ShowErrorMessage(e.Form, "ERROR", "This product has no price scales, and this should be rectified immediately")
          Else
          BreakpointHelpers.ShowErrorMessage(e.Form, "ERROR", ex.Message)
          End If
        End Try
      End If
    End If
  End Sub
End Class
