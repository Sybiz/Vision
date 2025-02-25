Public Class RunScheduledReportsviaCLB

  'Scenario: Runs 'due' scheduled reports via Command Line Breakpoint
  'Post-requisities: Set up Command Line Breakpoint to automatically run with key "scheduledReports"; likely through Task Scheduler or another method of your choosing 
  'Breakpoints: CommandLineExecution
  	
  	
  Public Sub Invoke(ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCommandLineEventArgs) 'Do not remove - SYBIZ
  
    If e.Key = "scheduledReports" Then
      BreakpointHelpers.RunScheduledReports(False) 'MUST be runAsync = False with Command Line Breakpoint to avoid locking
    End If
  				
  End Sub

End Class
