Public Class MakeJobActiveWhenWIPExistsBeforeJobSave

    'Scenario: Checks to see if the job has WIP, and if so and the user is attempting to deactivate the job, it stops them from doing so by making the job active again, preserving other changes made.
    'Prerequisities: None
    'Breakpoint: BeforeJobSave
	

	Public Sub Invoke(ByVal job As Sybiz.Vision.Platform.JobCosting.Job, ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCancelEventArgs)

		Dim id As String = job.id
		Dim jdi As Sybiz.Vision.Platform.JobCosting.JobDetailInfo = Sybiz.Vision.Platform.JobCosting.JobDetailInfo.GetObject(id)
		Dim WIP As Decimal = jdi.OutstandingCosts
				
		If WIP > 0 And job.IsActive = False Then
			job.IsActive = True
			BreakpointHelpers.ShowErrorMessage(e.Form, "WARNING", "This job has WIP and cannot be made inactive. It has been made active again")
		End If
	
	End Sub

End Class
