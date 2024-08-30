Public Class EnterCurrentQuantityIntoCustomFieldOnProductChanging

	'Scenario: Get current stock on hand an enter into a custom field
	'Prerequisities: Decimal custom field 
	'Breakpoint: PurchaseOrderCellValueChanged

	Public Sub Invoke(transaction As Sybiz.Vision.Platform.Creditors.Transaction.PurchaseOrder, e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCellValueChangedEventArgs(Of Sybiz.Vision.Platform.Creditors.Transaction.PurchaseOrderLine))
		'Get custom field
		Dim customField As Object = e.Line.ExtendedProperties.Item("QOH")

		'If field being edited is either account or location then...				
		If e.FieldName.Equals("Account") = True OrElse e.FieldName.Equals("Location") = True Then
			'Get the quantity
			Dim qty As Decimal = Sybiz.Vision.Platform.Core.Data.ScalarCommand.Execute(Of Decimal)(String.Format("SELECT Quantity FROM [ic].[ProductBalanceExpanded] WHERE ProductId = {0} AND LocationId = {1}", e.Line.Account, e.Line.Location))
			'Insert the quantuty into the custom field
			customField.ObjectValue = qty
		End If
	End Sub

End Class
