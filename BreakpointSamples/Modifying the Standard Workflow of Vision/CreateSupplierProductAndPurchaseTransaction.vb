Public Class CreateSupplierProductAndPurchaseTransaction

    'Scenario: Create a workflow to create a new supplier and product and then use them on a new purchase order
    'Prerequisities: None
    'Breakpoint: ExternalApplicationCustomRibbonButtonClick

    Public Sub Invoke(e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCustomRibbonButtonClickEventArgs)

        If e.Key.Equals("BreakpointKey") Then
            'Show a new supplier form
            Dim formSupplier As Sybiz.Vision.Module.Coordinator.VisionDialogResult = Sybiz.Vision.Module.Coordinator.VisionApplication.GetApplication().CR.ShowSupplierForm(e.Form, 0, Nothing, Nothing)

            'Check if a supplier was created
            If formSupplier.Id > 0 Then
                'Show a new product form
                Dim formProduct As Sybiz.Vision.Module.Coordinator.VisionDialogResult = Sybiz.Vision.Module.Coordinator.VisionApplication.GetApplication().IC.ShowProductForm(e.Form, 0, Nothing, Nothing)

                'Check if a product was created
                If formProduct.Id > 0 Then
                    'Create a new purchase order with the supplier
                    Dim newOrder As Sybiz.Vision.Platform.Creditors.Transaction.PurchaseOrder = Sybiz.Vision.Platform.Creditors.Transaction.PurchaseOrder.NewObject(Nothing, True, True)
                    newOrder.Supplier = formSupplier.Id
                    newOrder.Reference = "Breakpoints Demo"

                    'Create a new purchase order line with the product
                    Dim theLine As Sybiz.Vision.Platform.Creditors.Transaction.PurchaseOrderLine = newOrder.Lines.AddNew(Sybiz.Vision.Platform.Core.Enumerations.PurchaseLineType.IC)
                    theLine.Account = formProduct.Id
                    theLine.Location = 1
                    theLine.TaxCode = 1

                    'Save the purchase order
                    newOrder = newOrder.Save()
                    Dim newOrderId = newOrder.Id

                    'Open the purchase order
                    Sybiz.Vision.Module.Coordinator.VisionApplication.GetApplication.CR.ShowPurchaseOrderForm(e.Form, newOrder.Id, newOrder.Supplier)
                End If

            End If
        End If
    End Sub

End Class
