Public Class ImportInvoiceViaDataTable

  'Scenario: Creates an invoice based on an imported CSV, selected by the user using a load file dialog
  'Prerequisities: A pre-created CSV file. It should have 3 columns, one called Product, one called QtyInv, and another called Price. Product Codes should be used.
                  'Further, a custom button should be defined in SalesInvoiceCustomRibbonButtonRegister called "Import".
    
  'Breakpoint: SalesInvoiceCustomRibbonButtonClick
	
	
  Public Sub Invoke(ByVal transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice, ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCustomRibbonButtonClickEventArgs) 'Do not remove - SYBIZ

    If e.Key.Equals("Import") = True Then
      Dim fileName as String
        Using frm as OpenFileDialog = BreakpointHelpers.ShowOpenFileDialog("", false, "*.csv")
          If frm.ShowDialog(e.Form) = DialogResult.OK Then
            fileName = frm.FileName
          End if
        End Using
                
    Using import = BreakpointHelpers.CreateDataTableFromCSV(fileName)
        Using edr = New Sybiz.Vision.Platform.Core.Data.ExtendedSafeDataReader(import.CreateDataReader())
            While edr.Read()
                Dim productId = Sybiz.Vision.Platform.Inventory.ProductDetailInfo.GetObject(edr.GetString("Product")).Id
                If productId > 0 Then
                    Dim newLine As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoiceLine = transaction.Lines.AddNew(SalesLineType.IC)   'Assumption that only IC lines being used at this time
                    newLine.Account = productId
                    newLine.QuantityInvoice = edr.GetDecimal("QtyInv")
                    newLine.UnitChargeExclusive = edr.GetDecimal("Price")
                    newLine.Location = 3      'Assumption that header loaction has not been set, otherwise do not set                   
                End If
            End While
        End Using
    End Using
    End If
	
  End Sub

End Class
