Public Class CustomFieldAddorEdit

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
			'We are searching for any value; if nothing returned, it is assumed that the custom field does not exist.
			Dim customFieldId As Integer? = BreakpointHelpers.ExecuteScalarCommand(CommandType.Text,"SELECT ExtendedPropertyTemplateItemId FROM admin.ExtendedPropertyTemplateItem WHERE Name = 'BreakpointCustomField'",60,Nothing)
			
			'Note that the below examples are EXHAUSTIVE; if you press the button four times, you'll get an error (since you'll try to save a custom field with the same name).
			'Below code is an example only to demonstrate how to perform each "task".
			        
			If customFieldId.HasValue = True Then
				Dim existingCustomField As Sybiz.Vision.Platform.Admin.ExtendedPropertyTemplateItem = productCustomFieldList.TemplateItems.FirstOrDefault(Function(x) x.Id = customFieldId.Value)
				existingCustomField.Name = existingCustomField.Name + "_suffix"
			Else
				Dim newCustomField As Sybiz.Vision.Platform.Admin.ExtendedPropertyTemplateItem = productCustomFieldList.TemplateItems.AddNew()
				newCustomField.Name = "BreakpointCustomField"
				newCustomField.Type = Sybiz.Vision.Platform.Core.ExtendedProperties.ExtendedPropertyType.String
				newCustomField.FieldLength = 255
				newCustomField.TabDescription = "Custom Tab 1"
				newCustomField.SortOrder = 0
				newCustomField.IsActive = true
			End If
					        
			Dim customFieldEnumId As Integer? = BreakpointHelpers.ExecuteScalarCommand(CommandType.Text,"SELECT ExtendedPropertyTemplateItemId FROM admin.ExtendedPropertyTemplateItem WHERE Name = 'BreakpointCustomFieldEnum'",60,Nothing)
			
			'Example revolving around enumerated custom field
			If customFieldEnumId.HasValue = True Then
				Dim existingCustomFieldEnum As Sybiz.Vision.Platform.Admin.ExtendedPropertyTemplateItem = productCustomFieldList.TemplateItems.FirstOrDefault(Function(x) x.Id = customFieldEnumId.Value)
				existingCustomFieldEnum.Name = existingCustomFieldEnum.Name + "_suffix"
				existingCustomFieldEnum.EnumeratedValues.Clear()
				Dim editEnumValue As Sybiz.Vision.Platform.Admin.ExtendedPropertyValue = existingCustomFieldEnum.EnumeratedValues.AddNew()
				editEnumValue.Value = "Edited"
			Else
				Dim newCustomFieldEnum As Sybiz.Vision.Platform.Admin.ExtendedPropertyTemplateItem = productCustomFieldList.TemplateItems.AddNew()
				newCustomFieldEnum.Name = "BreakpointCustomFieldEnum"
				newCustomFieldEnum.Type = Sybiz.Vision.Platform.Core.ExtendedProperties.ExtendedPropertyType.EnumerationUnsorted
				newCustomFieldEnum.TabDescription = "Custom Tab 1"
				newCustomFieldEnum.SortOrder = 0
				newCustomFieldEnum.IsActive = true
				Dim newEnumValue As Sybiz.Vision.Platform.Admin.ExtendedPropertyValue = newCustomFieldEnum.EnumeratedValues.AddNew()
				newEnumValue.Value = "New"
			End If
					
			Dim customFieldStaticId As Integer? = BreakpointHelpers.ExecuteScalarCommand(CommandType.Text,"SELECT ExtendedPropertyTemplateItemId FROM admin.ExtendedPropertyTemplateItem WHERE Name = 'BreakpointCustomFieldStatic'",60,Nothing)
			
			'Example revolving around "Static List" custom field
			If customFieldStaticId.HasValue = True Then
				Dim existingCustomFieldStatic As Sybiz.Vision.Platform.Admin.ExtendedPropertyTemplateItem = productCustomFieldList.TemplateItems.FirstOrDefault(Function(x) x.Id = customFieldStaticId.Value)
				existingCustomFieldStatic.Name = existingCustomFieldStatic.Name + "_suffix"
				existingCustomFieldStatic.EnumeratedValues.Clear()
				Dim editStaticValue As Sybiz.Vision.Platform.Admin.ExtendedPropertyValue = existingCustomFieldStatic.EnumeratedValues.AddNew()
				editStaticValue.Code = "E1"
				editStaticValue.Value = "Edited"
			Else
				Dim newCustomFieldStatic As Sybiz.Vision.Platform.Admin.ExtendedPropertyTemplateItem = productCustomFieldList.TemplateItems.AddNew()
				newCustomFieldStatic.Name = "BreakpointCustomFieldStatic"
				newCustomFieldStatic.Type = Sybiz.Vision.Platform.Core.ExtendedProperties.ExtendedPropertyType.StaticList
				newCustomFieldStatic.TabDescription = "Custom Tab 1"
				newCustomFieldStatic.SortOrder = 0
				newCustomFieldStatic.IsActive = true
				Dim newStaticValue As Sybiz.Vision.Platform.Admin.ExtendedPropertyValue = newCustomFieldStatic.EnumeratedValues.AddNew()
				newStaticValue.Code = "N1"
				newStaticValue.Value = "New"
			End If
			
			'Specifically need to save the "header", not the child object.
			productCustomFieldList = productCustomFieldList.Save()
		Catch ex As Exception
			BreakpointHelpers.ShowErrorMessage(e.Form, "DANGER WILL ROBINSON!", ex.Message)
		End Try
    End If
  
  End Sub
End Class
