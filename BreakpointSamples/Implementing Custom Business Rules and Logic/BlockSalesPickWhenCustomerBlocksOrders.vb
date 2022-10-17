Public Class BlockSalesPickWhenCustomerBlocksOrders

    'Scenario: Checks to see if the customer attached to a pick's source order has stop orders enabled, then blocks the pick if so (only works to stop the pick and pack being processed)
    'Prerequisities: None
    'Breakpoint: BeforeSalesPickProcess

    Public Sub Invoke(transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesPick, e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCancelEventArgs)
        'Declare variables as needed
        Dim pid As Integer = transaction.TransactionId
        Dim oid As Integer
        Dim cid As Integer
        Dim customer As Sybiz.Vision.Platform.Debtors.CustomerDetailInfo = Nothing

        'Get order id, then customer id from the order (specifically the ORDER customer!), then customer, but ONLY if this transaction has already been processed, otherwise we get errors.
        If transaction.IsNew = False Then
            oid = Sybiz.Vision.Platform.Core.Data.ScalarCommand.Execute(Of Integer)(String.Format("SELECT TOP 1 SourceOrderId FROM dr.SalesPickLine WHERE SalesPickId = {0}", pid))
            cid = Sybiz.Vision.Platform.Core.Data.ScalarCommand.Execute(Of Integer)(String.Format("SELECT OrderCustomerId FROM dr.SalesOrder WHERE SalesOrderId = {0}", oid))
            customer = Sybiz.Vision.Platform.Debtors.CustomerDetailInfo.GetObject(cid)
        End If

        'If customer has stop orders, block transaction! Same requirement for transaction having already been processed, lest ye be faced with Object Reference not set!
        If transaction.IsNew = False Then
            If customer.StopOrders = True Then
                System.Windows.Forms.MessageBox.Show(String.Format("This customer has stop orders enabled, and this pick cannot be processed"))
                e.Cancel = True
            End If
        End If
    End Sub

End Class