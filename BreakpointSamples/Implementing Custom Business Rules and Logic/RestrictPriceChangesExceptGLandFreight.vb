Public Class RestrictPriceChangesExceptGLandFreight

    'Scenario: Disables price-change related fields wherein one is in a particular role, except for GL lines and a particular product called "FREIGHT"
    'Prerequisities: A role called "FrontOfHouse"
    'Breakpoint: SalesOrderDisableCustomCellRepository (Note: Code is relatively replicable across transactions)

  Public Sub Invoke(ByVal transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesOrder, ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCellRepositoryEventArgs(Of Sybiz.Vision.Platform.Debtors.Transaction.SalesOrderLine)) 'Do not remove - SYBIZ

    If e.Line.AccountType = SalesLineType.GL OrElse (e.Line.AccountType = SalesLineType.IC AndAlso e.Line.Account = Sybiz.Vision.Platform.Inventory.ProductDetailInfo.GetObject("FREIGHT").Id) Then
      return
    ElseIf Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal.IsInRole("FrontOfHouse") Then'OrElse...any other roles you want to restrict
    'Alternative method, do reverse where everyone BUT those in an 'admin' role are restricted.
    'ElseIf Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal.IsInRole("PriceOverride") = False Then
        If e.FieldName.Equals("Charge") = True OrElse e.FieldName.Equals("UnitChargeExclusive") = True OrElse e.FieldName.Equals("ChargeExclusive") = True OrElse e.FieldName.Equals("Discount") = True OrElse e.FieldName.Equals("DiscountPercentage") = True OrElse e.FieldName.Equals("UnitChargeInclusive") = True OrElse e.FieldName.Equals("ChargeInclusive") = True Then
          e.Handled = True
        End If
    End If

  End Sub
End Class
