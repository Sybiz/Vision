Public Module ExternalApplicationCustomRibbonButtonClick
  
  'Scenario: Looking to add images to a large series of products. 
  'Prerequisities: Creation of External Application (Breakpoint) with key "PicUpload"
  'Breakpoint: ExternalApplicationCustomRibbonButtonClick

  Public Sub Invoke(ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCustomRibbonButtonClickEventArgs)
    
	 If (e.Key = "PicUpload") Then
		Dim errorLog As List(Of String) = New List(Of String)

		Dim product As Sybiz.Vision.Platform.Inventory.Product
		Dim picture As Sybiz.Vision.Platform.Common.Picture
		Dim directoryName As String = BreakpointHelpers.GetStringValue(e.Form,"Directory Name","Please enter the directory name you wish to read images from","C:\")
		Dim continueCheck As DialogResult = BreakpointHelpers.ShowYesNoMessageBox(e.Form,"WARNING","Pressing yes will read all files in " + directoryName +" and attach them to relevant products, do you want to continue?")
		If continueCheck = DialogResult.No Then
			return
		End If
		System.Windows.Forms.Application.UseWaitCursor = true
		Try
			Dim dir As New DirectoryInfo(directoryName)
			For Each f As FileInfo In dir.GetFiles()
				If f.Extension = ".jpg" OrElse f.Extension = ".png" Then
					Try
						'There is an assumption that the image names match codes within Vision
						product = Sybiz.Vision.Platform.Inventory.Product.GetObject(f.Name.Replace(f.Extension,""))							
						If product.Pictures.Count = 0 Then
							picture = product.Pictures.AddNew()
							picture.Image = File.ReadAllBytes(f.FullName)
							If product.IsSavable = True Then
								product.Save
							Else
								Throw New Exception("Product could not be saved!")
							End If
						Else
							Throw New Exception("Pictures already exist against product!")
						End If
					Catch ex As Exception
						errorLog.Add(String.Format("Error processing file {0}:{1}",f.Name,ex.Message))
					End Try
				End If
			Next
		Catch ex As Exception
			BreakpointHelpers.ShowErrorMessage(e.Form,"ERROR",ex.Message)
		Finally
			System.Windows.Forms.Application.UseWaitCursor = false
			If errorLog.Count > 0 Then
				BreakpointHelpers.ShowErrorMessage(e.Form,"ERRORS", String.Format("During the operation,{0} errors were recorded in VisionPictureImportErrors.txt, saved to My Documents", errorLog.Count))
				System.IO.File.WriteAllLines(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),"VisionPictureImportErrors.txt"),errorLog, System.Text.Encoding.ASCII)
			End If
			BreakpointHelpers.ShowInformationMessage(e.Form,"COMPLETE!","The operation has been completed")
		End Try
	End If
  End Sub
End Module
