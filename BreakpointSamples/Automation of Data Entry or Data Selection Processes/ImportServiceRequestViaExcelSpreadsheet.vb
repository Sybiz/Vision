Public Class ImportServiceRequestViaExcelSpreadsheet

  'Scenario: Uses a spreadsheet with service request data to create service requests from an historical data source, additional columns can be added as necessary
  '          It does assume that the invoice value is known and will be captured by using a fixed pricing method for historical data only
  'Prerequisities: A pre-created XLSX file using codes for requests, departments, status, etc.
                  'Further, a custom button should be defined with the key "ServiceRequestImport".
    
  'Breakpoint: ExternalApplicationCustomRibbonButtonClick
	
	
	Public Sub Invoke(ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCustomRibbonButtonClickEventArgs) 'Do not remove - SYBIZ
        If e.Key = "ServiceRequestImport" Then
            Try
                Dim fileName as String
                Using frm as OpenFileDialog = BreakpointHelpers.ShowOpenFileDialog("",false,"*.xslx")
                    If frm.ShowDialog(e.Form) = DialogResult.OK Then
                        fileName = frm.FileName
                    End if
                End Using

                Using dt = Sybiz.Vision.WinUI.Utilities.DocumentImportExport.ExportXlsxToDataTable(fileName,"Sheet1")
                    Using edr = new Sybiz.Vision.Platform.Core.Data.ExtendedSafeDataReader(dt.CreateDataReader())
                        While edr.Read()
                            Dim newRequest = Sybiz.Vision.Platform.Service.ServiceRequest.GetObject(0)
                            
                            If newRequest.UseAutomaticNumbering then
                                newRequest.IsAutomatic = false
                            End If
                            
                            newRequest.ServiceRequestNumber = edr.GetString("ServiceRequestNumber")
                            newRequest.Description = edr.GetString("Description")
                            newRequest.RequestType =  Sybiz.Vision.Platform.Service.RequestTypeDetailInfo.GetObject(edr.GetString("RequestType")).Id
                            newRequest.Priority = Sybiz.Vision.Platform.Common.Priority.GetObject(edr.GetString("Priority")).Id
                            newRequest.Department = Sybiz.Vision.Platform.Service.DepartmentDetailInfo.GetObject(edr.GetString("Department")).Id
                            newRequest.Status = Sybiz.Vision.Platform.Common.StatusDetailInfo.GetObject(edr.GetString("Status")).Id
                            newRequest.ServiceItem = Sybiz.Vision.Platform.Service.ServiceItemDetailInfo.GetObject(edr.GetString("ServiceItem")).Id
                            newRequest.Customer = Sybiz.Vision.Platform.Debtors.CustomerDetailInfo.GetObject(edr.GetString("Customer")).Id
                            newRequest.Issue = edr.GetString("Issue")
                            newRequest.Resolution = edr.GetString("Resolution")
                            newRequest.ServiceRequestInvoicingMethod = Sybiz.Vision.Platform.Core.Enumerations.ServiceRequestInvoicingMethod.Fixed
                            newRequest.FixedPrice = edr.ParseDecimal("FixedPrice")						
                            
                            If (newRequest.IsSavable)
                                newRequest = newRequest.Save()
                            Else
                                'BreakpointHelpers.ShowInformationMessage(e.Form,String.Format("{0}-{1}",newRequest.ServiceRequestNumber, newRequest.GetAllBrokenRules()(0).Property),newRequest.GetAllBrokenRules()(0).Description)
                            End If								
                        End While
                    End Using
                End Using	
            Catch ex as Exception
                BreakpointHelpers.ShowErrorMessage(e.Form,"ERROR",ex.Message)
            End Try                    
        End If	
	End Sub 
End Class
