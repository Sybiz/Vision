Public Module ExternalApplicationCustomRibbonButtonClick 

'Scenario: Example of the "GetLookupControlValue" BreakpointHelper
'Prerequisities: External application button must be created using code "LookupControlExample". Version 26+
'Breakpoint: ExternalApplicationCustomRibbonButtonClick

  Public Sub Invoke(ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCustomRibbonButtonClickEventArgs)		
  
    If e.Key = "LookupControlExample" Then
      'ObjectType. options are Customer, Supplier, Product, Job, Account, Service Request, Service Item, Fixed Asset, Project, Staff, Sales Representative
      Dim custId As Integer = BreakpointHelpers.GetLookupControlValue(e.Form, "Customer selector", "Select a customer from the dropdown", ObjectType.Customer) 
      BreakpointHelpers.ShowInformationMessage(e.Form, "TEST - Should show selected Customer Name", Sybiz.Vision.Platform.Debtors.CustomerDetailInfo.GetObject(custId).Name)
      
      'Can in theory though call any lookup, it's just we're not shortcutting them all via ObjectType! Example...
      Dim delMethodId As Integer = BreakpointHelpers.GetLookupControlValue(e.Form, "Delivery method", "Select a delivery method", New Sybiz.Vision.WinUI.Controls.DeliveryMethodLookupEdit)
      BreakpointHelpers.ShowInformationMessage(e.Form, "TEST2 - Should show selected Delivery Method Description", Sybiz.Vision.Platform.Common.DeliveryMethodDetailInfo.GetObject(delMethodId).Description)
      
      'Could even make your own lookup if you wanted to get particularly fancy, just has to be a base of LookUpEditBase...beyond the scope of this example though ;)
    End If
  
  End Sub
End Module
