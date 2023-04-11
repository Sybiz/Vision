Public Class OpenItemsonVisionLaunch

    'Scenario: If user is in "POS" role, automatically opens a Sales Invoice. Otherwise, opens a custom grid called "Outstanding Orders"
    'Prerequisities: A custom grid created called "Outstanding Orders"
    'Breakpoints: VisionCompanyLaunched
	
	
		Public Sub Invoke(ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointEventArgs) 'Do not remove - SYBIZ

			If Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal.IsInRole("POS") = True Then
					
				'Since it's POS; is it possible to 'force' KioskMode on? This seems like the way to do it, but does not work (due to Object Reference not Set...); guessing it's intentional but can't hurt to ask!
				Dim ds As Sybiz.Vision.Platform.Admin.DeviceSettings
				ds.KioskMode = true
								
				BreakpointHelpers.PerformRibbonButtonClick(e.Form, "Sales Invoice")
			Else
				BreakpointHelpers.PerformRibbonButtonClick(e.Form, "Outstanding Orders")
			End If
				
		End Sub

End Class
