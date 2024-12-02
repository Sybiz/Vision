Public Class AddAvailableCostCentresToJobBeforeSave

  'Scenario: Example snippet of how to add a new, or editing an existing custom field
  'Prerequisities: External Application breakpoint button set up with key "CustomFieldExample"
  'Breakpoint: ExternalApplicationCustomRibbonButtonClick

  Public Sub Invoke(ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCustomRibbonButtonClickEventArgs) 'Do not remove - SYBIZ
  
    If e.Key = "CustomFieldExample" Then
      Try
      
        'This example looks at the Products custom field list; looking at admin.ExtendedPropertyTemplate will let you see what options you hav
        Dim templateId As Integer = BreakpointHelpers.ExecuteScalarCommand(CommandType.Text, "SELECT ExtendedPropertyTemplateId FROM admin.ExtendedPropertyTemplate WHERE ObjectType = 'Product'", 60,Nothing)
        Dim productCustomFieldList As Sybiz.Vision.Platform.Admin.ExtendedPropertyTemplate = Sybiz.Vision.Platform.Admin.ExtendedPropertyTemplate.GetExtendedPropertiesTemplate(templateId)

        'Variable could be passed in here to look for a specific custom field, or some modification to the query to check for it being a certain type, or similar.
        Dim customFieldId As Integer = BreakpointHelpers.ExecuteScalarCommand(CommandType.Text,"SELECT COALESCE((SELECT ExtendedPropertyTemplateItemId FROM admin.ExtendedPropertyTemplateItem WHERE Name = 'BreakpointCustomField'),0) AS Result",60,Nothing)

        'Note that the below examples are EXHAUSTIVE; if you press the button four times, you'll get an error (since you'll try to save a custom field with the same name).
        'Below code is an example only to demonstrate how to perform each "task".
        
        If customFieldId > 0 Then
          Dim existingCustomField As Sybiz.Vision.Platform.Admin.ExtendedPropertyTemplateItem = productCustomFieldList.TemplateItems.SingleOrDefault(Function(x) x.Id = customFieldId)
          existingCustomField.Name = existingCustomField.Name + "_suffix"
        Else
          Dim newCustomField As Sybiz.Vision.Platform.Admin.ExtendedPropertyTemplateItem = productCustomFieldList.TemplateItems.AddNew()
          newCustomField.Name = "BreakpointCustomField"
          newCustomField.Type = Sybiz.Vision.Platform.Core.ExtendedProperties.ExtendedPropertyType.String
          newCustomField.FieldLength = 255
          newCustomField.TabDescription = "Custom Tab 1"
          newCustomField.SortOrder = 0
        End If

        'Specifically need to save the "header", not the child object.
        productCustomFieldList = productCustomFieldList.Save()
      Catch ex As Exception
        BreakpointHelpers.ShowErrorMessage(e.Form, "DANGER WILL ROBINSON!", ex.Message)
      End Try
    End If
  
  End Sub
End Class
