Public Class RemoveSavedJobCostedInvoiceLines
  'Scenario: When creating a new job costed invoice, remove any lines that exist on a previously saved transaction
  'Prerequisities: None
  
  'Note, this methodology will 'break' if Refresh Lines is used as the previously removed lines will be removed; if this functionality is required
  'change the logic to not invoice the lines instead of removing them.

  'Breakpoint: SalesInvoiceCellValueChanged

  Public Sub Invoke(ByVal transaction As Sybiz.Vision.Platform.JobCosting.Transaction.JobCostedInvoice, ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointEventArgs) 'Do not remove - SYBIZ
	Dim params As New Dictionary(Of String, Object)
	params.Add("@JobId", transaction.Job)
	Dim savedTransactionLines As Object = BreakpointHelpers.ExecuteScalarCommand(CommandType.Text,"SELECT STRING_AGG(CAST(ISNULL(PostCostLineId, JobEstimateLineId) AS VARCHAR), ',') FROM [jc].[CostedInvoiceLine] CIL INNER JOIN [jc].[CostedInvoice] CI ON CI.CostedInvoiceId = CIL.CostedInvoiceId AND CI.IsPosted = 0 WHERE CI.JobId = @JobId", params)
								
	If savedTransactionLines IsNot DBNull.Value AndAlso Not String.IsNullOrEmpty(savedTransactionLines) Then
		For Each value As Integer In savedTransactionLines.Split(",")
			transaction.Lines.Remove(transaction.Lines.First(Function(ln) ln.CostLineId = value OrElse ln.EstimateLineId = value))
		Next
	End If
  End Sub
      
End Class
