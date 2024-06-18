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
      Dim cont As DialogResult = BreakpointHelpers.ShowYesNoMessageBox(e.Form,"Change Customer","Incomplete Sales Quotes have been found for this job, not against the job's customer" + Environment.NewLine + "Would you like to change all quotes found to match the job's new customer?")
      If cont = DialogResult.Yes Then
        Dim QuoteIds As New List (Of Integer)
        						
        Using dt = BreakpointHelpers.CreateDataTableFromQuery(query, params)
          Using edr = new Sybiz.Vision.Platform.Core.Data.ExtendedSafeDataReader(dt.CreateDataReader())
            While edr.Read()
            									
              Dim loadedquote As Sybiz.Vision.Platform.Debtors.Transaction.SalesQuote = Sybiz.Vision.Platform.Debtors.Transaction.SalesQuote.GetObject(Nothing, edr.GetInteger("SalesQuoteId"))
              Dim copylines As New List(Of Sybiz.Vision.Platform.Debtors.Transaction.SalesQuoteLine)
              																																
              For each l As Sybiz.Vision.Platform.Debtors.Transaction.SalesQuoteLine In loadedquote.Lines
                copylines.Add(l)
              Next
              																	
              loadedquote.Lines.Clear()						
              								
              loadedquote.ChangeCustomer(job.Customer)
              										
              For Each copy As Sybiz.Vision.Platform.Debtors.Transaction.SalesQuoteLine In copylines
                Dim newline As Sybiz.Vision.Platform.Debtors.Transaction.SalesQuoteLine = loadedquote.Lines.AddNew(copy.AccountType)
                With newline
                  .Account = copy.Account
                  If loadedquote.PriceEntryMode = TransactionPriceMode.Exclusive Then
                    .ChargeExclusive = copy.ChargeExclusive
                  Else
                    .ChargeInclusive = copy.ChargeInclusive
                  End If
                  .Notes = copy.Notes
                  'Any further line properties needed can be placed here
                End With
              Next
              								
              loadedquote = loadedquote.Process()
              BreakpointHelpers.ShowInformationMessage(e.Form,"Success!","Sales Quote " + loadedquote.TransactionNumber + " has been changed to match new customer")
            End While
          End Using
        End Using
      End If
    End If

  End Sub

End Class
