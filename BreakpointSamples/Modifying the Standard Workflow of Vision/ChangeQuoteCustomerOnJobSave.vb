Public Class ChangeQuoteCustomerOnJobSave

  'Scenario: When changing the customer on a job, automatically uses the "Change Customer" function to switch the customer on any quotes featuring the job.
  'Prerequisities: None
  'Breakpoint: AfterJobSave

  Public Sub Invoke(ByVal job As Sybiz.Vision.Platform.JobCosting.Job, ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointEventArgs) 'Do not remove - SYBIZ
    Dim params As New Dictionary(Of String, Object)
    params.Add("@customerid", job.Customer)
    params.Add("@jobid", job.Id)
    Dim query As String = "SELECT SalesQuoteId FROM dr.SalesQuote WHERE SalesQuoteId = (SELECT sq.SalesQuoteId FROM dr.SalesQuote sq INNER JOIN dr.SalesQuoteLine sql ON sql.SalesQuoteId = sq.SalesQuoteId WHERE sq.CustomerId <> @customerid AND [sql].JobId = @jobid) AND NOT EXISTS(SELECT NULL FROM dr.SalesQuoteLine sql INNER JOIN dr.SalesQuote sq ON sq.SalesQuoteId = sql.SalesQuoteId WHERE sq.CustomerId <> @customerid AND [sql].JobId = @jobid AND SourceDocumentId <> 0) AND NOT EXISTS(SELECT NULL FROM dr.SalesOrderLine sol INNER JOIN dr.SalesQuote sq ON sq.SalesQuoteId = sol.SourceDocumentId WHERE sol.SourceDocumentType = 'SalesQuote' AND sol.JobId = @jobid) AND NOT EXISTS(SELECT NULL FROM dr.SalesDeliveryLine sdl INNER JOIN dr.SalesQuote sq ON sq.SalesQuoteId = sdl.SourceDocumentId WHERE sdl.SourceDocumentType = 'SalesQuote' AND sdl.JobId = @jobid) AND NOT EXISTS(SELECT NULL FROM dr.SalesInvoiceLine sil INNER JOIN dr.SalesQuote sq ON sq.SalesQuoteId = sil.SourceDocumentId WHERE sil.SourceDocumentType = 'SalesQuote' AND sil.JobId = @jobid) AND CustomerId <> @customerId"
    
    If BreakpointHelpers.RecordExists(query,params) = true Then
      Dim result As DialogResult = BreakpointHelpers.ShowYesNoMessageBox(e.Form,"Change Customer","Incomplete Sales Quotes have been found for this job, not against the job's customer" + Environment.NewLine + "Would you like to change all quotes found to match the job's new customer?")
      If result = DialogResult.Yes Then
        Dim QuoteIds As New List (Of Integer)

        Dim dt As DataTable = BreakpointHelpers.CreateDataTableFromQuery(query, params)
        Dim quoteId As Integer

        For each row As DataRow In dt.Rows
          quoteId = row.Item("SalesQuoteId")

          Dim loadedQuote As Sybiz.Vision.Platform.Debtors.Transaction.SalesQuote = Sybiz.Vision.Platform.Debtors.Transaction.SalesQuote.GetObject(Nothing, quoteId)
          Dim copyLines As New List(Of Sybiz.Vision.Platform.Debtors.Transaction.SalesQuoteLine)

          For each l As Sybiz.Vision.Platform.Debtors.Transaction.SalesQuoteLine In loadedquote.Lines
            copyLines.Add(l)
          Next

          loadedQuote.Lines.Clear()						
          loadedQuote.ChangeCustomer(job.Customer)

          For Each copy As Sybiz.Vision.Platform.Debtors.Transaction.SalesQuoteLine In copyLines
          Dim newLine As Sybiz.Vision.Platform.Debtors.Transaction.SalesQuoteLine = loadedQuote.Lines.AddNew(copy.AccountType)
          With newLine
            .Account = copy.Account
            .Quantity = copy.Quantity    
        
            If loadedQuote.PriceEntryMode = TransactionPriceMode.Exclusive Then
              .ChargeExclusive = copy.ChargeExclusive
            Else
              .ChargeInclusive = copy.ChargeInclusive
            End If
        
            .Notes = copy.Notes
            'Any further line properties needed can be placed here
          End With
        Next
        
        loadedQuote = loadedQuote.Process()
        BreakpointHelpers.ShowInformationMessage(e.Form,"Success!","Sales Quote " + loadedQuote.TransactionNumber + " has been changed to match new customer")
        Next
      End If
    End If

  End Sub

End Class
