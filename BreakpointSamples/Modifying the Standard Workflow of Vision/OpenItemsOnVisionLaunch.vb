Public Class OpenItemsonVisionLaunch

    'Scenario: If user is in "POS" role, automatically opens a Sales Invoice. Otherwise, opens a custom grid called "Outstanding Orders"
    'Prerequisities: A custom grid created called "Outstanding Orders"
    'Breakpoints: VisionCompanyLaunched
	
	
		Public Sub Invoke(ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointEventArgs) 'Do not remove - SYBIZ

			If Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal.IsInRole("POS") = True Then
					
				'Since scenario	is a POS environment, we are 'forcing' Kiosk mode on through this breakpoint.				
				If (Sybiz.Vision.Platform.Admin.DeviceSettings.CanAccess()) Then
					Dim ds As Sybiz.Vision.Platform.Admin.DeviceSettings = Sybiz.Vision.Platform.Admin.DeviceSettings.GetObject()
					ds.KioskMode = true
					ds = ds.Save()
				End If
								
				BreakpointHelpers.PerformRibbonButtonClick(e.Form, "Sales Invoice")
			Else
				BreakpointHelpers.PerformRibbonButtonClick(e.Form, "Outstanding Orders")
			End If
				
		End Sub

End Class
