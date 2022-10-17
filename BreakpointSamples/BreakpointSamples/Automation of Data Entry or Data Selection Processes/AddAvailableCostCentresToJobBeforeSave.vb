Public Class AddAvailableCostCentresToJobBeforeSave

	'Scenario: Automatically allow the addition of documents to new sales invoices
	'Prerequisities: None
	'Breakpoint: BeforeJobSave

	Public Sub Invoke(job As Sybiz.Vision.Platform.JobCosting.Job, e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCancelEventArgs)
		'List of all job cost centers
		Dim listCostCentres As Sybiz.Vision.Platform.JobCosting.CostCentreLookupInfoList = Sybiz.Vision.Platform.JobCosting.CostCentreLookupInfoList.GetList()

		'For every cost centre not on the job already add it
		For Each list As Sybiz.Vision.Platform.JobCosting.CostCentreLookupInfo In listCostCentres
			If Not job.CostCentres.Any(Function(x) x.CostCentreId = list.Id) Then
				job.CostCentres.AddNew().CostCentreId = list.Id
			End If
		Next
	End Sub

End Class