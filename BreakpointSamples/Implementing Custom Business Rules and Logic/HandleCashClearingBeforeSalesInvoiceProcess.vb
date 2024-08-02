Public Class HandleCashClearingBeforeSalesInvoiceProcess

	'Scenario: Provides a method to delininate cash clearing into different GL clearing accounts (with user accounts representing 'tills')
	'Prerequisities: None
	'Breakpoint: BeforeSalesInvoiceProcess

	Public Sub Invoke(transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice, e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCancelEventArgs)
		Dim receiptNumber As String
		Dim accountClearing As Integer = 111 'Main cash clearing account
		Dim accountCounter As Integer
		Dim amount As Decimal

		Try
			receiptNumber = transaction.Receipt.TransactionNumber 'Get receipt

			For Each receiptLine As Sybiz.Vision.Platform.Debtors.Transaction.ReceiptLine In transaction.Receipt.Lines 'Get total of cash receipt
				If receiptLine.PayProcessType = 1 Then 'In our DB - PayProcessType of 1 is cash - yours may be different!
					amount += receiptLine.Amount
				End If
			Next

			If Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal().UserId = 1 AndAlso amount > 0 Then 'We're using administrator, this should be the user ID for 'counter 1'. We're also only doing this for receipts with any cash amount
				accountCounter = 2639 'Cash clearing for counter 1 - in this example we chose a random GL account.

				'Create a new GL Journal, reference is the Receipt
				Dim journal As Sybiz.Vision.Platform.GeneralLedger.Transaction.Journal = Sybiz.Vision.Platform.GeneralLedger.Transaction.Journal.NewObject(Nothing, True, True)
				journal.Description = "Clearing for " & receiptNumber

				'Line 1 goes to Main Clearing as credit
				Dim journalLine As Sybiz.Vision.Platform.GeneralLedger.Transaction.JournalLine = journal.Lines.AddNew()
				journalLine.Account = accountClearing
				journalLine.Reference = "Clearing for " & receiptNumber
				journalLine.Description = "Clearing for " & receiptNumber
				journalLine.Credit = amount

				'Line 2 goes to Specific Counter Clearing as debit
				Dim journalLineCounter As Sybiz.Vision.Platform.GeneralLedger.Transaction.JournalLine = journal.Lines.AddNew()
				journalLineCounter.Account = accountCounter
				journalLineCounter.Reference = "Clearing for " & receiptNumber
				journalLineCounter.Description = "Clearing for " & receiptNumber
				journalLineCounter.Debit = amount

				'If processable, do so and flag the user that success was had, including the journal receiptNumber. If not, display why in business rule terms and CANCEL THE WHOLE TRANSACTION
				If journal.IsProcessable Then
					journal = journal.Process
					receiptNumber = journal.TransactionNumber
					System.Windows.Forms.MessageBox.Show(String.Format("Cash Clearing Successfully Processed as " & receiptNumber))
				ElseIf (journal.IsValid) Then
					For Each line As Sybiz.Vision.Platform.GeneralLedger.Transaction.JournalLine In journal.Lines
						For Each rule As Sybiz.Vision.Platform.Validation.BrokenRuleInfo In line.GetBrokenRuleInfo
							System.Windows.Forms.MessageBox.Show(String.Format(rule.Description))
						Next
					Next

					e.Cancel = True
				End If
			End If
		Catch ex As System.Exception
			'This should never be seen, but just in case...
			System.Windows.Forms.MessageBox.Show(String.Format("Exception Caught. Please contact your BP."))
		End Try
	End Sub

End Class
