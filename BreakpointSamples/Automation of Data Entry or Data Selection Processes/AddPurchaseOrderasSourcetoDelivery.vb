Public Class AddPurchaseOrderasSourcetoDelivery

  'Scenario: Adds a processed Purchase Order as source document to a newly opened Purchase Delivery
  'Prerequisities: Processed Purchase Order, and you should know the number when invoking the breakpoint
  'Breakpoint: AfterPurchaseDeliveryOpen
	
	
  Public Sub Invoke(ByVal transaction As Sybiz.Vision.Platform.Creditors.Transaction.PurchaseDelivery, ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointOpenEventArgs) 'Do not remove - SYBIZ

    Dim esc As Boolean = False
    While esc = False
      Dim pon As String = BreakpointHelpers.GetStringValue(e.Form, "PO Import", "Please enter your Purchase Order number", Nothing)
      If pon IsNot Nothing Then
        Dim param As New Dictionary(Of String, Object)
        param.Add("@pon", pon)
        Dim source As Integer = BreakpointHelpers.ExecuteScalarCommand(CommandType.Text, "SELECT PurchaseOrderId FROM [cr].[PurchaseOrder] WHERE PurchaseOrderNumber = @pon AND IsPosted = 1", param)
        If source <> 0 Then
          transaction.AddSourceDocument(source, TransactionType.PurchaseOrder)
          'Need to refresh and tab off supplier to "complete" the adding of the source document
          BreakpointHelpers.RefreshTransactionGridView(e.Form)
          SendKeys.Send("{TAB}")
          esc = True
        Else
          Dim tryagain As DialogResult = BreakpointHelpers.ShowYesNoMessageBox(e.Form, "ERROR", "Error: Purchase Order Number " & pon & " was not found! Would you like to enter another number?")
          If tryagain = DialogResult.Yes Then
            param.Clear
          Else 
            esc = True
          End If
        End If
      End If
    End While
	
  End Sub

End Class
