Public Class EnforceMinimumMarkupBeforeSalesTransactionProcess

	'Scenario: Checks minimum markup on product, and if actual markup is less than the minimum, cancels the transaction unless the user is an admin
	'Prerequisities: None
	'Breakpoint: BeforeSalesInvoiceProcess

	Public Sub Invoke(transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice, e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCancelEventArgs)
		'Declare relevant variables
		Dim markup As Decimal
		Dim markupMin As Decimal
		Dim product As Integer
		Dim check As Boolean = False
		Dim admin As Boolean = Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal().IsAnAdministrator

		'For each line in the invoice
		For Each line As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoiceLine In transaction.Lines

			'If the line type is IC and we haven't hit any problems yet
			If line.AccountType = Sybiz.Vision.Platform.Core.Enumerations.SalesLineType.IC And check = False Then
				'Reset values, just in case...
				product = 0
				markupMin = 0
				markup = 0

				'Grab product ID, then based on that, grab that product's minimum markup, and round
				product = line.Account
				markupMin = Sybiz.Vision.Platform.Core.Data.ScalarCommand.Execute(Of Decimal)(String.Format("SELECT MinimumMarkup FROM ic.Product WHERE ProductId = {0}", product))
				markupMin = System.Math.Round(markupMin, 2)

				'Markup can safely be whatever is calculated on the line
				markup = line.SalesMarkup

				'If markup is below the minimum, and the user isn't an admin - display this to the user, stop the transaction and prevent any further lines from being checked.
				If markup < markupMin And admin = False Then
					System.Windows.Forms.MessageBox.Show(String.Format("There is a markup value in this invoice of " & markup & "%, which is less than the product's minimum markup of " & markupMin & "%. Check the invoice and resolve this issue."))
					e.Cancel = True
					check = True
				End If
			End If
		Next
	End Sub

End Class