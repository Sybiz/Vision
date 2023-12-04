Public Class UseProductSalesTaxValueOnServiceInvoice
  
  'Scenario: Use the sales tax code assigned to a product, overriding the default behavior on service invoice
  'Prerequisities: None
  'Breakpoint: ServiceInvoiceServiceRequestChanged

  Public Sub Invoke(ByVal transaction As Sybiz.Vision.Platform.Service.Transaction.ServiceInvoice, ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointEventArgs)
    For Each line As Sybiz.Vision.Platform.Service.Transaction.ServiceInvoiceLine in transaction.Lines
      If line.AccountType = ServiceLineType.IC Then
        Dim salesTaxCodeId As Integer = Sybiz.Vision.Platform.Inventory.ProductDetailInfo.GetObject(line.Account).SalesTaxCodeId
        If salesTaxCodeId <> 0 Then
          line.TaxCode = salesTaxCodeId
        End If					
      End If
    Next
  End Sub
End Class
