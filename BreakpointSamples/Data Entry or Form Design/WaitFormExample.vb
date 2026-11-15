Public Module ExternalApplicationCustomRibbonButtonClick 

'Scenario: Example of WaitForm BreakpointHelper
'Prerequisities: External application button must be created using code "WaitFormTest". Version 27+
'Breakpoint: ExternalApplicationCustomRibbonButtonClick

  Public Sub Invoke(ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCustomRibbonButtonClickEventArgs)		
  
    If e.Key = "WaitFormTest" Then
      Using frm = New BreakpointHelpers.WaitForm(e.Form, "Testing wait form", "This is a test of the waitform description")
        System.Threading.Thread.Sleep(10000)
        'If, for example, looping through transactions or products, can update the caption and / or description during the loop, e.g. "Now working on product XYZ"
        frm.SetCaption("Second caption!")
        frm.SetDescription("Second description!")
        System.Threading.Thread.Sleep(10000)
      End Using
    End If
  
  End Sub
End Module
