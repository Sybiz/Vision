Public Class ImportInvoicesinBulk

  'Scenario: Import a number of transactions from 
  'Prerequisities: External Application "breakpoint" created with key PIImport. CSV file created (with headers) that contains transaction information. 
    'Sample query (which will work for precisely this breakpoint) is found in same folder as ImportInvoicesinBulk.sql
  'Breakpoint: ExternalApplicationCustomRibbonButtonClick
  
  Public Sub Invoke(ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCustomRibbonButtonClickEventArgs) 'Do not remove - SYBIZ
    If e.Key = "PIImport" Then
    					
      'Concept added for specific users to only have access to this function given its potential impact. Commented out line suggests "Is Admin", second commented line for particular role (which could be expanded to roles)
      'If Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal.IsAnAdministrator = True Then
      'If Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal.IsInRole = "CanImport" = True Then
      					
      If Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal.UserId <> 1 Then
        BreakpointHelpers.ShowErrorMessage(e.Form, "ERROR", "You are not authorised to run this function")
        return
      End If
      					
      Try
        Dim lastInvoiceId As Integer = 0
        Dim currentInvoice As Sybiz.Vision.Platform.Creditors.Transaction.PurchaseInvoice = Nothing
        Dim errorLog As List(Of String) = New List(Of String)
        						
        Dim fileName as String
        Using frm as OpenFileDialog = BreakpointHelpers.ShowOpenFileDialog("", false, "*.csv")
          If frm.ShowDialog(e.Form) = DialogResult.OK Then
            fileName = frm.FileName
          End if
        End Using
        						
        System.Windows.Forms.Application.UseWaitCursor = True
        						
        Using import = BreakpointHelpers.CreateDataTableFromCSV(fileName)
          Using edr = New Sybiz.Vision.Platform.Core.Data.ExtendedSafeDataReader(import.CreateDataReader())
            While edr.Read()
              Try
                Dim currentInvoiceId As Integer = edr.GetInteger("PurchaseInvoiceId")
                
                'We are ONLY saving transactions...if you're confident, commented line with process
                If lastInvoiceId = 0 OrElse currentInvoiceId <> lastInvoiceId Then
                  If currentInvoice IsNot Nothing Then
                    currentInvoice = currentInvoice.Save()
                    'currentInvoice = currentInvoice.Process()
                    currentInvoice = Nothing
                  End If
                  											
                  currentInvoice = Sybiz.Vision.Platform.Creditors.Transaction.PurchaseInvoice.NewObject(Nothing, False, False)
                  											
                  currentInvoice.Supplier = edr.GetInteger("SupplierId")
                  currentInvoice.TransactionNumber = edr.GetString("PurchaseInvoiceNumber") 'Remove this line if using auto-numbering
                  											
                  'If you're importing some invoices which have source documents and some that don't, this is important to consider. Assumption here made for all POs, can expand of course if further logic needed.
                  If edr.GetInteger("SourceDocumentId") <> 0 Then
                    currentInvoice.AddSourceDocument(edr.GetInteger("SourceDocumentId"), TransactionType.PurchaseOrder)
                  End If
                  											
                  currentInvoice.Reference = edr.GetString("Reference")
                  currentInvoice.TransactionDate = DateTime.ParseExact(edr.GetString("TransactionDate"), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                  currentInvoice.DeliveryDate = DateTime.ParseExact(edr.GetString("DeliveryDate"), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                End If
                										
                If edr.GetInteger("SourceDocumentId") <> 0 Then
                  'Linq is a bit funky here, so we use For Each to 'find' the correct line in each instance. We then set a limited number of properties, which can of course be expanded upon
                  For Each l As Sybiz.Vision.Platform.Creditors.Transaction.PurchaseInvoiceLine In currentInvoice.Lines
                    If l.LineNo = edr.GetInteger("LineNo") Then
                      l.Description = edr.GetString("Description")
                      l.QuantityInvoice = edr.GetDecimal("QuantityInvoice")
                      l.QuantityDeliver = edr.GetDecimal("QuantityDeliver")
                      l.UnitCostExclusive = edr.GetDecimal("ForeignUnitCost")
                      l.TaxCode = edr.GetInteger("TaxCodeId")
                      Exit For
                    End If
                  Next
                  Else
                    'If we do have SourceDoc = 0, then we need to add the lines. I'm assuming IC for ease, noting that cr.PurchaseInvoiceLine has PurchaseLineTypeId for extension.
                    Dim newInvoiceLine As Sybiz.Vision.Platform.Creditors.Transaction.PurchaseInvoiceLine = Nothing
                    											
                    newInvoiceLine = currentInvoice.Lines.AddNew(PurchaseLineType.IC, False)
                    newInvoiceLine.Account = edr.GetInteger("ProductId")
                    											
                    'Again, limited set of items set, can expand if needed
                    newInvoiceLine.Description = edr.GetString("Description")
                    newInvoiceLine.QuantityInvoice = edr.GetDecimal("QuantityInvoice")
                    newInvoiceLine.QuantityDeliver = edr.GetDecimal("QuantityDeliver")
                    newInvoiceLine.UnitCostExclusive = edr.GetDecimal("ForeignUnitCost")
                    newInvoiceLine.TaxCode = edr.GetInteger("TaxCodeId")
                End If
                										
                lastInvoiceId = currentInvoiceId
                Catch ex As Exception
                  'This will mean that individual invoices can fail out but the overall import continue, since this is 99% going to be a Save failing (if it's a cast the entire breakpoint is stuffed) we foce currentInvoice to Nothing.
                  errorlog.Add(String.Format("Error processing Invoice {0} Line {1}:{1}",edr.GetString("PurchaseInvoiceNumber"),edr.GetInteger("LineNo").ToString(),ex.Message))
                  currentInvoice = Nothing
                Finally
                  System.Windows.Forms.Application.UseWaitCursor = False
                  If errorlog.Count > 0 Then
                    BreakpointHelpers.ShowErrorMessage(e.Form,"ERRORS", String.Format("During the operation,{0} errors were recorded in VisionInvoiceImportErrors.txt, saved to My Documents", errorLog.Count))
                    System.IO.File.WriteAllLines(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),"VisionInvoiceImportErrors.txt"),errorlog, System.Text.Encoding.ASCII)
                  End If
              End Try				
            End While
          End Using
        End Using
        						
        'Need to save the final one!
        If currentInvoice IsNot Nothing Then
          currentInvoice = currentInvoice.Save()
        End If
        						
        BreakpointHelpers.ShowInformationMessage(e.Form, "Success", "Import completed!")
        Catch ex As Exception
          'This'll catch broad exceptions not related to specific lines
          BreakpointHelpers.ShowErrorMessage(e.Form, "ERROR", ex.Message)
      End Try
    End If
  
  End Sub
End Class
