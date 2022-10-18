Public Module ExternalApplicationCustomRibbonButtonClick 

	'Scenario: Remove the first character E from all products
	'Prerequisities: External application button must be created using code "REPLACEPRODUCTCODES"
	'Breakpoint: ExternalApplicationCustomRibbonButtonClick

	Public Sub Invoke(ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCustomRibbonButtonClickEventArgs)
		If (e.Key = "REPLACEPRODUCTCODES") Then
			System.Windows.Forms.Application.UseWaitCursor = true
			Try						
				Dim products as Sybiz.Vision.Platform.Inventory.ProductLookupInfoList = Sybiz.Vision.Platform.Inventory.ProductLookupInfoList.GetList()

				For Each info as Sybiz.Vision.Platform.Inventory.ProductLookupInfo in products
					If (info.Code.StartsWith("E", System.StringComparison.OrdinalIgnoreCase))
						Dim product as Sybiz.Vision.Platform.Inventory.Product = Sybiz.Vision.Platform.Inventory.Product.GetObject(info.Id)
						product.Code = product.Code.Substring(1,product.Code.Length-2)

						If (product.IsSavable)
							product.Save()
						End If
					End If
				Next
			Finally
				System.Windows.Forms.Application.UseWaitCursor = false
			End Try
		End If
	End Sub
End Module
