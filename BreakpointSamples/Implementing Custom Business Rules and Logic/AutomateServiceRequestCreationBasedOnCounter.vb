Public Class AutomateServiceRequestCreationBasedOnCounter

  'Scenario: Checks to see if counter meter has reached a 'threshold', and then creates a request based off that.
  'Prerequisities: Creation and setup of custom fields against Service Item, "LastReadingInterval" and "ReadingInterval"; both should be integers, ReadingInterval should have a non-zero value.
  'Breakpoint: AfterMeterReadingProcess

		Public Sub Invoke(ByVal transaction As Sybiz.Vision.Platform.Service.Transaction.MeterReading, ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointEventArgs) 'Do not remove - SYBIZ
      For each l As Sybiz.Vision.Platform.Service.Transaction.MeterReadingLine In transaction.Lines
        Dim param As New Dictionary(Of String,Object)
        param.Add("@Id",l.ServiceItem)
        Dim lastRead As Integer = BreakpointHelpers.ExecuteScalarCommand(CommandType.Text,"SELECT LastReadingInterval FROM sv.ServiceItemExtendedProperty WHERE ObjectId = @Id",param)
        Dim readingInterval As Integer = BreakpointHelpers.ExecuteScalarCommand(CommandType.Text,"SELECT ReadingInterval FROM sv.ServiceItemExtendedProperty WHERE ObjectId = @Id",param)
        Dim readBreak As Integer = lastRead + readingInterval
        If l.Reading >= readBreak Then

          Dim serviceRequest As Sybiz.Vision.Platform.Service.ServiceRequest = Sybiz.Vision.Platform.Service.ServiceRequest.GetObject(0)
          serviceRequest.Description = String.Format("Regular Counter Service For {0}",l.ServiceItemDetails.Description)
          serviceRequest.ServiceItem = l.ServiceItem
          'Assumption here is that Service Request Template is set up for a 'regular' service; this can be changed as needed / wanted.
          Dim requestTemplate As Integer = BreakpointHelpers.ExecuteScalarCommand(CommandType.Text,"SELECT ServiceRequestTemplateId FROM sv.ServiceRequestTemplate WHERE Code = 'REG'",Nothing)
          serviceRequest.RequestTemplate = requestTemplate
          Try
            If serviceRequest.IsSavable = true Then
              serviceRequest = serviceRequest.Save
              BreakpointHelpers.ShowInformationMessage(e.Form,"SR Created",String.Format("Service Request {0} created for {1} hour service for {2}",sr.ServiceRequestNumber,readbreak,l.ServiceItemDetails.Description))
              End If
          Catch ex As Exception
            BreakpointHelpers.ShowErrorMessage(e.Form,"ERROR",ex.Message)
          End Try

          param.Add("@readbreak",readBreak)
          BreakpointHelpers.ExecuteScalarCommand(CommandType.Text,"UPDATE sv.ServiceItemExtendedProperty SET LastReadingInterval = @readbreak WHERE ObjectId = @Id",param)
        End If
      Next
		End Sub
End Class
