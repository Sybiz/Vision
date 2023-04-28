Public Class RunScheduledReportsOnLaunch

    'Scenario: Runs 'due' scheduled reports on launch, if user is allowed to do so 
    'Prerequisities: N/A
    'Breakpoints: VisionCompanyLaunched
	
	
		Public Sub Invoke(ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointEventArgs) 'Do not remove - SYBIZ

			If BreakpointHelpers.CanRunScheduledReports = True Then
					BreakpointHelpers.RunScheduledReports()
			End If
				
		End Sub

End Class
