Public Class PreventExcessUsersFromLoggingIn
  
    'Scenario: If the user count is going to exceed a set number of users, preventing other tools from running kick the user out.
    'Prerequisities: None
    'Breakpoint: VisionCompanyLaunched

 		Public Sub Invoke(ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointEventArgs) 'Do not remove - SYBIZ
      If Not Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal().IsAnAdministrator() Then
        Dim maxAllowedUserCount = 9

        Using mgr = Sybiz.Vision.Platform.Core.Data.ConnectionManager.GetCommonManager()
          Using cmd = mgr.Connection.CreateCommand()                                                
            cmd.CommandText = "SELECT COUNT(*) FROM [vsn].[OnlineUserLicenseTokens] WHERE WebApiToken = 0"
            If cmd.ExecuteScalar() > maxAllowedUserCount
              BreakpointHelpers.ShowMessage(e.Form, "Too many users", "You have been selected to leave the software!")
              Application.Exit()
            End If
          End Using
        End Using
      End If
    End Sub
End Class
