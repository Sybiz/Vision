Public Module ExternalApplicationCustomRibbonButtonClick
  
  'Scenario: Looking to add images to a large series of products. 
  'Prerequisities: Creation of External Application (Breakpoint) with key "PicUpload"
  'Breakpoint: ExternalApplicationCustomRibbonButtonClick

  Public Sub Invoke(ByVal e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCustomRibbonButtonClickEventArgs)
    
    If (e.Key = "PicUpload") Then
			Dim errorlog As String() = New String(2) {}
			Dim errorcount As Integer = 0
					
			Dim prod As Sybiz.Vision.Platform.Inventory.Product
			Dim pic As Sybiz.Vision.Platform.Common.Picture
			Dim dirname As String = BreakpointHelpers.GetStringValue(e.Form,"Directory Name","Please enter the directory name you wish to read images from","C:\")
			Dim cont As DialogResult = BreakpointHelpers.ShowYesNoMessageBox(e.Form,"WARNING","Pressing yes will read all files in " + dirname +" and attach them to relevant products, do you want to continue?")
			If cont = DialogResult.No Then
				return
			End If
			System.Windows.Forms.Application.UseWaitCursor = true
			Try
				Dim dir As New DirectoryInfo(dirname)
				For Each f As FileInfo In dir.GetFiles()
					If f.Extension = ".jpg" Or f.Extension = ".png" Then
						Try
							'There is an assumption that the image names match codes within Vision
							prod = Sybiz.Vision.Platform.Inventory.Product.GetObject(f.Name.Remove(f.Name.IndexOf(".")))								
							If prod.Pictures.Count = 0 Then
								pic = prod.Pictures.AddNew
								pic.Image = File.ReadAllBytes(f.FullName)
								If prod.IsSavable = True Then
									prod.Save
								Else
									Throw New Exception("Product could not be saved!")
								End If
							Else
								Throw New Exception("Pictures already exist against product!")
							End If
						Catch ex As Exception
							errorlog(errorCount) = "Error processing file " & f.Name & ": " & ex.Message
					        errorcount += 1
					        If errorcount >= errorlog.Length Then
					            Array.Resize(errorlog, errorlog.Length * 2)
					        End If
						End Try
					End If
				Next
			Catch ex As Exception
				BreakpointHelpers.ShowErrorMessage(e.Form,"ERROR",ex.Message)
			Finally
				System.Windows.Forms.Application.UseWaitCursor = false
				errorlog = Array.FindAll(errorlog, Function(s) Not String.IsNullOrEmpty(s))
				If errorlog.Length > 0 Then
					Dim errcont As DialogResult = BreakpointHelpers.ShowYesNoMessageBox(e.Form,"ERRORS","During the operation, " + errorlog.Length + " errors were recorded. Press yes to see detail about each, or no to skip this.")
					If errcont = DialogResult.Yes Then
						For Each err As String In errorlog
							BreakpointHelpers.ShowErrorMessage(e.Form,"ERROR",err)
						Next
					End If
				End If
				BreakpointHelpers.ShowInformationMessage(e.Form,"COMPLETE!","The operation has been completed")
			End Try
		End If

  End Sub
End Module
