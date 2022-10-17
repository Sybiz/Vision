Public Class EnforceTaxCodeOnTransactionsBeforeProcess



	'---Ethan Levy Last Modified 17/11/21---
	'Sybiz Software

	'BeforeCashbookProcess

	'Checks that GL lines have a tax code (the only line type where tax code is enabled and optional)


	For Each line As Sybiz.Vision.Platform.Cashbook.Transaction.CashbookLine In transaction.Lines
	If line.AccountType = Sybiz.Vision.Platform.Core.Enumerations.CashbookTransactionLineType.GL Then
	If line.TaxCode = Nothing Then
						System.Windows.Forms.MessageBox.Show("You have not input a tax code into a line!")
						e.Cancel = True
					End If
	End If
	Next

End Class
