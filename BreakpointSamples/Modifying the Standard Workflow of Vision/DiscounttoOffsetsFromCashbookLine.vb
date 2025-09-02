Public Class DiscounttoOffsetsFromCashbookLine

  'Scenario: Client wants to reinstate ability to place discount against cashbook line as distinct from offsets, as removed by https://partner.sybiz.com/KnowledgeBase/KnowledgeBaseDetail/121935
  'Prerequisities: Custom button created with key "LineDiscount"
  'Breakpoint: CashbookCustomRibbonButtonClick

  Public Sub Invoke(ByVal transaction As Sybiz.Vision.Platform.Cashbook.Transaction.Cashbook, ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCustomRibbonButtonClickEventArgs) 'Do not remove - SYBIZ
    If e.Key = "LineDiscount" Then
      
      'This makes it affect the highlighted / clicked on line
      Dim customerLine = BreakpointHelpers.GetFocusedRow(Of Sybiz.Vision.Platform.Cashbook.Transaction.CashbookLineDR)(e.Form)
      
      If (customerLine Is Nothing)
        BreakpointHelpers.ShowErrorMessage(e.Form,"ERROR","This breakpoint only works for selected DR lines")
        Return
      End If
      					                                    
      Try                           
        Dim discountAmount As Decimal = BreakpointHelpers.GetDecimalValue(e.Form, "Customer Discount", "Enter offset discount %", customerLine.CustomerDetails.PromptPaymentDiscount)
        						
        If (Not customerLine.OffsetsLoaded) Then
          'Need to get the lines loaded if they aren't; even if just dumped into a random variable, they'll then be available
          Dim discard = customerLine.Offsets
        End If
        						                              
        customerLine.OffsetLines.RaiseListChangedEvents = False                       
        						                              
        For Each offsetLine As Sybiz.Vision.Platform.Cashbook.Transaction.CashbookLineOffset In customerLine.OffsetLines      							
          'If we haven't already pressed process on an offset line, apply discount, otherwise leave it be
          If offsetLine.Process = False
            offsetLine.Discount = discountAmount
          End If
        Next
      Finally                       
        customerLine.OffsetLines.RaiseListChangedEvents = True
      End Try
    End If
  End Sub

End Class
