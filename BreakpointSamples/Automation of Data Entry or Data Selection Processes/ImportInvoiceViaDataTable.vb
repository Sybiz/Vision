Public Class ImportInvoiceViaDataTable

  'Scenario: Creates an invoice based on an imported CSV, selected by the user using a load file dialog
  'Prerequisities: A pre-created CSV file. It should have 3 columns, one called Product, one called QtyInv, and another called Price. Product Codes should be used.
                  'Further, a custom button should be defined in SalesInvoiceCustomRibbonButtonRegister called "Import".
    
  'Breakpoint: SalesInvoiceCustomRibbonButtonClick
	
	
  Public Sub Invoke(ByVal transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice, ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCustomRibbonButtonClickEventArgs) 'Do not remove - SYBIZ

    If e.Key = "Import" Then
      Dim fileName as String
        Using frm as OpenFileDialog = BreakpointHelpers.ShowOpenFileDialog("",false,"*.csv")
          If frm.ShowDialog(e.Form) = DialogResult.OK Then
            fileName = frm.FileName
          End if
        End Using
                
    Dim import As System.Data.DataTable = BreakpointHelpers.CreateDataTableFromCSV(fileName)
                
    For Each row As System.Data.DataRow In import.Rows
        Dim code = row("Product")
        Dim quantity = row("QtyInv")
        Dim price = row("Price")
        Dim productId = Sybiz.Vision.Platform.Inventory.ProductDetailInfo.GetObject(code).Id
        If productId > 0 Then
          Dim newline As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoiceLine = transaction.Lines.AddNew(SalesLineType.IC)   'Assumption that only IC lines being used at this time
          newline.Account = productId
          newline.QuantityInvoice = quantity
          newline.UnitChargeExclusive = price
          newline.Location = 3      'Assumption that header loaction has not been set, otherwise do not set                   
        End if
      Next
    End If
	
  End Sub

End Class
