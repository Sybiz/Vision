Imports System 'Do not remove - SYBIZ
Imports System.Collections.Generic 'Do not remove - SYBIZ
Imports System.Data 'Do not remove - SYBIZ
Imports System.Data.SqlClient 'Do not remove - SYBIZ
Imports System.Drawing 'Do not remove - SYBIZ
Imports System.Drawing.Imaging 'Do not remove - SYBIZ
Imports System.IO 'Do not remove - SYBIZ
Imports System.Linq 'Do not remove - SYBIZ
Imports System.Windows.Forms 'Do not remove - SYBIZ
Imports Sybiz.Vision.Module.Coordinator 'Do not remove - SYBIZ
Imports Sybiz.Vision.Platform.Core.Enumerations 'Do not remove - SYBIZ
Imports Sybiz.Vision.Platform.Security 'Do not remove - SYBIZ
Imports Sybiz.Vision.WinUI.Utilities 'Do not remove - SYBIZ
Namespace Breakpoints.Debtors.SalesOrder 'Do not remove - SYBIZ
	Public Module SalesOrderCustomRibbonButtonClick 'Do not remove - SYBIZ
		Public Sub Invoke(ByVal transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesOrder, ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCustomRibbonButtonClickEventArgs) 'Do not remove - SYBIZ
			Try 'Do not remove - SYBIZ
				'Enter your code below - SYBIZ
				If e.Key = "ClearDeposit" Then 
					If BreakpointHelpers.ShowYesNoMessageBox(e.Form, "WARNING", "Warning: This will cleanse this Sales Order and prepare an invoice for pay and process. Do you want to continue?") = DialogResult.Yes Then 
						Dim soid = transaction.Id 
						Dim depamount As Decimal 
						
						transaction.CleanseLines() 
						
						For each depositLine AS Sybiz.Vision.Platform.Debtors.Transaction.SalesOrderLine In transaction.Lines 
							If depositLine.AccountType = SalesLineType.DP Then 
								depamount = depAmount + (depositLine.QuantityOrder - depositLine.QuantityNetDelivered)
							End If 
						Next 
						
					'Need to be in Inclusive mode to be able to set UnitChargeInclusive appropriately 
					BreakpointHelpers.PerformRibbonButtonClick(e.Form,"Inclusive") 
					
					'This example has been written assuming a specific "Deposits Forfeited" account. This could be easily changed to utilise a different logic of where the forfeited deposit amount is placed. 
					Dim glLine As Sybiz.Vision.Platform.Debtors.Transaction.GLSalesOrderLine = transaction.Lines.AddNew(SalesLineType.GL) 
					glLine.Account = 2933 
					glLine.QuantityOrder = 1 
					glLine.UnitChargeInclusive = depamount 
					glLine.TaxCode = 6 'GST Applies, please note that this treatment may not be accurate for all scenarios, advice should be sought 
					
					If transaction.IsProcessable then 
						BreakpointHelpers.PerformRibbonButtonClick(e.Form,"Process & Close") 
					Else						
						For Each brokenRule as Sybiz.Vision.Platform.Validation.BrokenRuleInfo In transaction.GetBrokenRuleInfo()
							If (brokenRule.Severity = Csla.Validation.RuleSeverity.Error) Then
								BreakpointHelpers.ShowErrorMessage(e.Form, brokenRule.PropertyName, brokenRule.Description)
								Return
							End If
						Next 
					End If 
					
					Dim invoice As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice = Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice.NewObject(Nothing, Nothing, Nothing) 
					invoice.AddSourceDocument(soid, TransactionType.SalesOrder) 
					invoice.InvoiceAndDeliverAll() 
					Sybiz.Vision.Module.Coordinator.VisionApplication.GetApplication.DR.ShowSalesInvoiceForm(Nothing, invoice, False) 
					
					End If 
				End If				
			Catch ex As System.Exception 'Do not remove - SYBIZ
				Throw New Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointException("Invalid breakpoint code error", ex) 'Do not remove - SYBIZ
			Finally 'Do not remove - SYBIZ
			End Try 'Do not remove - SYBIZ
		End Sub 'Do not remove - SYBIZ
	End Module 'Do not remove - SYBIZ
End Namespace 'Do not remove - SYBIZ
