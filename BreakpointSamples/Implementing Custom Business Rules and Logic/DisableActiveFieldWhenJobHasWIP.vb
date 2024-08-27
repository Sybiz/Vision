Public Class DisableActiveFieldWhenJobHasWIP

    'Scenario: Checks to see if the job has WIP, and if so, the Active checkbox is disabled
    'Prerequisities: None
    'Breakpoint: JobDisableTextEditor
	
  Public Sub Invoke(ByVal sender As System.Object, ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointPropertyDisabledEventArgs) 'Do not remove - SYBIZ

    If e.PropertyName.Equals("IsActive") = True Then
      Dim job As Sybiz.Vision.Platform.JobCosting.Job = DirectCast(sender, Sybiz.Vision.Platform.JobCosting.Job)
      If job.IsNew = false AndAlso job.IsActive Then                             
        Dim jobDetail As Sybiz.Vision.Platform.JobCosting.JobDetailInfo = Sybiz.Vision.Platform.JobCosting.JobDetailInfo.GetObject(job.id)
        Dim WIP As Decimal = jobDetail.OutstandingCosts
        If WIP > 0 Then
          e.Handled = True
        End If
      End If
    End If
	
  End Sub

End Class
