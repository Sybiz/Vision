Public Class AutomateServiceRequestCreationBasedOnCounter

  'Scenario: Checks to see if counter meter has reached a 'threshold', and then creates a request based off that.
  'Prerequisities: Creation and setup of custom fields against Service Item, "LastReadingInterval" and "ReadingInterval"; both should be integers, ReadingInterval should have a non-zero value.
  'Breakpoint: AfterMeterReadingProcess

		Public Sub Invoke(ByVal transaction As Sybiz.Vision.Platform.Service.Transaction.MeterReading, ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointEventArgs) 'Do not remove - SYBIZ
      For each l As Sybiz.Vision.Platform.Service.Transaction.MeterReadingLine In transaction.Lines
        Dim param As New Dictionary(Of String,Object)
        param.Add("@Id",l.ServiceItem)
        Dim lastread As Integer = BreakpointHelpers.ExecuteScalarCommand(CommandType.Text,"SELECT LastReadingInterval FROM sv.ServiceItemExtendedProperty WHERE ObjectId = @Id",param)
        Dim interval As Integer = BreakpointHelpers.ExecuteScalarCommand(CommandType.Text,"SELECT ReadingInterval FROM sv.ServiceItemExtendedProperty WHERE ObjectId = @Id",param)
        Dim readbreak As Integer = lastread + interval
        If l.Reading >= readbreak Then

          Dim sr As Sybiz.Vision.Platform.Service.ServiceRequest = Sybiz.Vision.Platform.Service.ServiceRequest.GetObject(0)
          sr.Description = String.Format("Regular Counter Service For {0}",l.ServiceItemDetails.Description)
          sr.ServiceItem = l.ServiceItem
          'Assumption here is that Service Request Template is set up for a 'regular' service; this can be changed as needed / wanted.
          Dim rt As Integer = BreakpointHelpers.ExecuteScalarCommand(CommandType.Text,"SELECT ServiceRequestTemplateId FROM sv.ServiceRequestTemplate WHERE Code = 'REG'",Nothing)
          sr.RequestTemplate = rt
          Try
            If sr.IsSavable = true Then
              sr.Save
              BreakpointHelpers.ShowInformationMessage(e.Form,"SR Created",String.Format("Service Request created for {1} hour service for {2}",readbreak,l.ServiceItemDetails.Description))
              End If
          Catch ex As Exception
            BreakpointHelpers.ShowErrorMessage(e.Form,"ERROR",ex.Message)
          End Try

          param.Add("@readbreak",readbreak)
          BreakpointHelpers.ExecuteScalarCommand(CommandType.Text,"UPDATE sv.ServiceItemExtendedProperty SET LastReadingInterval = @readbreak WHERE ObjectId = @Id",param)
        End If
      Next
		End Sub
End Class
