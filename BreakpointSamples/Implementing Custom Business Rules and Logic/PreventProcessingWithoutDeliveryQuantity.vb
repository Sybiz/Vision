Public Class PreventProcessingWithoutDeliveryQuantity
	Public Sub Invoke(ByVal transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice, ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCancelEventArgs) 'Do not remove - SYBIZ			
		If transaction.Lines.Any(Function(ln) ln.QuantityDeliver = 0D)
			e.Cancel = True	
			BreakpointHelpers.ShowInformationMessage(e.Form,"Business Logic Invalidated","Unable to process as no delivered quantity entered")
		End If
	End Sub 
End Class
