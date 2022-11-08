Public Class EmailonLaunchTurnoverBreach

  'Scenario: Sends an email if database's turnover limit is over 80% towards being breached. Only fires for 'admins'; defined as people who have edit access to the Company, and only once every 3 days.
  'Prerequisities: A custom field called TURNOVER against the Company    
  'Breakpoint: VisionCompanyLaunched	
	
  Public Sub Invoke(ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointEventArgs) 'Do not remove - SYBIZ

    If Sybiz.Vision.Platform.Common.Company.CanEdit() AndAlso Sybiz.Vision.Platform.Common.CompanyInfo.GetObject().ExtendedProperties.GetCustomField("TURNOVER") <= System.DateTime.Today
      BreakpointHelpers.SendEmailWhenExceedTurnoverLimits(80, "turnover_manager@company.com", "WARNING: TURNOVER PROBLEM IN " + Sybiz.Vision.Platform.Common.CompanyInfo.GetCachedObject().CompanyName.ToUpper(), "TURNOVER PROBLEM!" + System.Environment.NewLine + "CURRENT TURNOVER IS $" + BreakpointHelpers.CurrentTurnoverValue.ToString("#,##0") + " OUT OF $" + Sybiz.Vision.Platform.Licensing.LicenseRights.FeatureRights.TurnoverLimit.ToString("#,##0") + "." + System.Environment.NewLine + "THIS EMAIL WILL NOT BE SENT AGAIN FOR 3 DAYS, RECTIFY THIS ISSUE IMMEDIATELY!")
      Dim company = Sybiz.Vision.Platform.Common.Company.GetObject()
      Dim newdate = company.ExtendedProperties.Item("TURNOVER")
      newdate.ObjectValue = System.DateTime.Today.AddDays(3)
      company.Save()
    End If
	
  End Sub

End Class
