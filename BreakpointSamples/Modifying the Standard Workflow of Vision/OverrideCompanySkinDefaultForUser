Public Class ChangeSkinonLogin

    'Scenario: Automatically change the skin that a user is using; this example details the idea of a particular user wanting a different skin for the given company
    'Prerequisities: None
    'Breakpoint: VisionCompanyLaunched

    Public Sub Invoke(ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointEventArgs)
      Dim UserID As Integer = Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal.UserId
      If UserID = 5 Then
        'Must use "Bezier", as we only support the one style. The Pallette of the skin is what can be changed.
        DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(DevExpress.LookAndFeel.SkinStyle.Bezier, "High Contrast Black") 
      End If
    End Sub

End Class
