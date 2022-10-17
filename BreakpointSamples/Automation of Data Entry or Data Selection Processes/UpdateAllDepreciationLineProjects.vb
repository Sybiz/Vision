Public Class UpdateAllDepreciationLineProjects

	'Scenario: Define a custom ribbon button
	'Prerequisities: None
	'Breakpoint: DepreciationRunCustomRibbonButtonRegister

	Public Sub Invoke(sender As System.Object, e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCustomRibbonButtonRegisterEventArgs)
		'Key: Used to identify the button in the CustomRibbonButtonClick breakpoint
		'Description: Sets the description
		'Style: Sets the style
		'ToolTipTitle: Sets the tooltip title
		'ToolTipDescription: Sets the tooltip description
		'Glyph: Sets the small image
		'LargeGlyph: Sets the large image
		'BeginGroup: Determines whether to start a new group
		'RibbonPageIndex: Determines the page of the button (i.e. home = 0, actions = 1, options = 2)
		e.Register.RegisterCustomRibbonButton("ProjectPropagation", "Project Propagation", Sybiz.Vision.Platform.Admin.Breakpoints.CustomRibbonButtonStyle.Default, "Project Propagation", "Copies project from first line to all lines", Sybiz.Vision.WinUI.Properties.Resources.ArrowDown16, Sybiz.Vision.WinUI.Properties.Resources.ArrowDown32, True, 0)
	End Sub

	'Scenario: Copies the project of the first line of a depreciation run and pastes to all below empty lines (changes project if a different one is encountered)
	'Prerequisities: Custom field button defined above
	'Breakpoint: DepreciationRunCustomRibbonButtonClick

	Public Sub Invoke(transaction As Sybiz.Vision.Platform.FixedAssets.Transaction.DepreciationRun, e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCustomRibbonButtonClickEventArgs)

		'Variable for the project
		Dim project As Integer

		'When clicking the button
		If e.Key = "ProjectPropagation" AndAlso transaction.Lines.Count > 0 Then

			'Go through each line...
			For Each line As Sybiz.Vision.Platform.FixedAssets.Transaction.DepreciationRunLine In transaction.Lines
				'If there is a project, then p = the project of that line. If not, the project of the line becomes p
				If line.Project > 0 Then
					project = line.Project
				Else
					line.Project = project
				End If
			Next
		End If
	End Sub

End Class