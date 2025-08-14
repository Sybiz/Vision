Public Module ExternalApplicationCustomRibbonButtonClick 

  'Scenario: Downloads all pictures attached to products
  'Prerequisities: External application button must be created using code "PicDownload". Also must import System.Text.RegularExpressions
  'Breakpoint: ExternalApplicationCustomRibbonButtonClick

  Public Sub Invoke(ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCustomRibbonButtonClickEventArgs)				

    If e.Key.Equals("PicDownload") = True Then
      Dim errorlog As List(Of String) = New List(Of String)
      Dim dirname As String = BreakpointHelpers.GetStringValue(e.Form,"Directory Name","Please enter the directory name you wish to save product images to","C:\")
      Dim cont As DialogResult = BreakpointHelpers.ShowYesNoMessageBox(e.Form,"WARNING","Pressing yes will take all pictures attached to products and download them to " + dirname + " do you want to continue?")
      Dim p As Sybiz.Vision.Platform.Inventory.Product
      Dim pic As Sybiz.Vision.Platform.Common.Picture
      Dim picfile As FileInfo
      Dim dl As Byte()
      If cont = DialogResult.No Then
        return
      End If
      If dirname.Chars(dirname.Length-1) <> "\" Then
        dirname = dirname + "\"
      End If
      If System.IO.Directory.Exists(dirname) = false Then
        Throw New Exception("Directory not Found!")
      End If
      System.Windows.Forms.Application.UseWaitCursor = true
      Try
        Dim infolist As Sybiz.Vision.Platform.Inventory.ProductLookupInfoList = Sybiz.Vision.Platform.Inventory.ProductLookupInfoList.GetList()
        For each info As Sybiz.Vision.Platform.Inventory.ProductLookupInfo In infolist
          p = Sybiz.Vision.Platform.Inventory.Product.GetObject(info.Id)
          If p.Pictures.Count > 0 Then
            Dim piccount As Integer = 1
            Dim picname As String
            For Each pic In p.Pictures
              dl = pic.Image
              picname = dirname + Regex.Replace(p.Code, "[^A-Za-z0-9]_-", "") + "_" + piccount.ToString() + ".jpg"
              File.WriteAllBytes(picname,dl)
              piccount += 1
            Next
            piccount = 1
          End If
        Next
      Catch ex As Exception
        errorlog.Add(String.Format("Error processing file {0}:{1}",p.Code,ex.Message))
      Finally
        System.Windows.Forms.Application.UseWaitCursor = false
        If errorlog.Count > 0 Then
          BreakpointHelpers.ShowErrorMessage(e.Form,"ERRORS", String.Format("During the operation,{0} errors were recorded in VisionPictureDownloadErrors.txt, saved to My Documents", errorLog.Count))
          System.IO.File.WriteAllLines(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),"VisionPictureDownloadErrors.txt"),errorlog, System.Text.Encoding.ASCII)
        End If
        BreakpointHelpers.ShowInformationMessage(e.Form,"COMPLETE!","The operation has been completed")
      End Try
    End If

  End Sub
End Module
