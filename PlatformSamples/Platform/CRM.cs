using System;
using System.Linq;
using System.Data;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;
using Microsoft.VisualBasic;

namespace My3rdPartyApplication
{
	public partial class MainForm
	{
		public void btnCreateCaseFile_Click(object sender, EventArgs e)
		{
			try
			{
				var newCaseFile = Sybiz.Vision.Platform.ContactManagement.Transaction.CaseFile.GetObject("0");

				//You can use this instead to get defaults for the ledger
				//Dim caseFileDefaults = Sybiz.Vision.Platform.ContactManagement.Defaults.GetObject()
				//If caseFileDefaults.CaseFileNumber.Active Then

				if (Sybiz.Vision.Platform.Common.TransactionAutoNumber.UseAutoGenerator(typeof(Sybiz.Vision.Platform.ContactManagement.Transaction.CaseFile)))
				{
					MessageBox.Show($"Case file Numbering Enabled: {Sybiz.Vision.Platform.Common.TransactionAutoNumber.UseAutoGenerator(typeof(Sybiz.Vision.Platform.ContactManagement.Transaction.CaseFile))}");
				}
				else
				{
					newCaseFile.CaseFileNumber = System.Convert.ToString(Interaction.InputBox("Enter case file number", "Information Required").Trim());
				}

				newCaseFile.Description = $"New case file {newCaseFile.CaseFileNumber} created by sample app";
				newCaseFile.Customer = Sybiz.Vision.Platform.Debtors.CustomerLookupInfoList.GetList().Where(o => o.IsActive == true).FirstOrDefault().Id;
				newCaseFile.Category = Sybiz.Vision.Platform.ContactManagement.CaseFileCategoryLookupInfoList.GetList().Where(o => o.Active == true).FirstOrDefault().Id;
				newCaseFile.SubCategory = Sybiz.Vision.Platform.ContactManagement.CaseFileSubCategoryLookupInfoList.GetList().Where(o => o.Active == true).FirstOrDefault().Id;
				newCaseFile.Status = Sybiz.Vision.Platform.Common.StatusLookupInfoList.GetList().Where(o => o.CaseFileActive == true).FirstOrDefault().Id;
				newCaseFile.User = Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal().UserId;


				if (newCaseFile.IsValid)
				{
					newCaseFile = newCaseFile.Save();
					newCaseFile.Details = $"These are the details for {newCaseFile.CaseFileNumber}";
					newCaseFile = newCaseFile.Save();
					MessageBox.Show($"Case file {newCaseFile.CaseFileNumber} created successfully");
				}
				else
				{
					foreach (var rule in newCaseFile.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
					{
						MessageBox.Show($"{rule.PropertyName} Is broken on object {rule.ObjectLevel} because '{rule.Description}'");
					}
				}
			}
			catch (Exception)
			{
				MessageBox.Show($"{Information.Err().Description}");
			}

		}

		public void btnCreateActivity_Click(object sender, EventArgs e)
		{

			try
			{
				var typeActivity = Interaction.InputBox($"Please select an activity type to create 1 - Remark 2 - Email 3 - PhoneCall 4 - Task ", "Information Required", "").Trim();

				switch (int.Parse(System.Convert.ToString(typeActivity)))
				{
					case (System.Int32)Sybiz.Vision.Platform.Core.Enumerations.ActivityType.Remark:
						CreateRemark();
						break;
					case (System.Int32)Sybiz.Vision.Platform.Core.Enumerations.ActivityType.Email:
						CreateEmail();
						break;
					case (System.Int32)Sybiz.Vision.Platform.Core.Enumerations.ActivityType.PhoneCall:
						CreatePhoneCall();
						break;
					case (System.Int32)Sybiz.Vision.Platform.Core.Enumerations.ActivityType.Task:
						CreateTask();
						break;
					default:
						MessageBox.Show("Invalid activity");
						return;
				}

			}
			catch (Exception)
			{
				MessageBox.Show($"{Information.Err().Description}");
			}
		}

		private void CreateRemark()
		{
			try
			{
				var newActivity = Sybiz.Vision.Platform.ContactManagement.Transaction.Remark.GetObject(0);

				if (Sybiz.Vision.Platform.Common.TransactionAutoNumber.UseAutoGenerator(typeof(Sybiz.Vision.Platform.ContactManagement.Transaction.Remark)))
				{
					MessageBox.Show($"Remark numbering enabled: {Sybiz.Vision.Platform.Common.TransactionAutoNumber.UseAutoGenerator(typeof(Sybiz.Vision.Platform.ContactManagement.Transaction.CaseFile))}");
				}
				else
				{
					newActivity.ActivityNumber = Interaction.InputBox("Enter remark number", "Information Required").Trim();
				}

				//Only a single value can be assigned or both can be left as 0 for internal remarks
				//newActivity.Customer = 0
				//newActivity.Supplier = 0

				newActivity.Subject = Interaction.InputBox("Please enter a Subject", "Information Required", "").Trim();

				//These values may be set based on defaults from within the software itself but are mandatory fields
				newActivity.Category = Sybiz.Vision.Platform.ContactManagement.ActivityCategoryLookupInfoList.GetList().First(o => o.RemarkActive == true).Id;
				newActivity.Status = Sybiz.Vision.Platform.Common.StatusLookupInfoList.GetList().First(o => o.ActivityActive == true).Id;
				newActivity.Priority = Sybiz.Vision.Platform.Common.PriorityLookupInfoList.GetList().First(o => o.ActivityActive == true).Id;

				newActivity.User = Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal().UserId;

				if (newActivity.IsValid)
				{
					newActivity = newActivity.Save();
					MessageBox.Show($"Task {newActivity.ActivityNumber} created successfully");
				}
				else
				{
					foreach (var rule in newActivity.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
					{
						MessageBox.Show($"{rule.PropertyName} Is broken on object {rule.ObjectLevel} because '{rule.Description}'");
					}
				}

			}
			catch (Exception)
			{
				MessageBox.Show($"{Information.Err().Description}");
			}

		}

		private void CreateEmail()
		{
			try
			{

				var newActivity = Sybiz.Vision.Platform.ContactManagement.Transaction.Email.GetObject(0);

				if (Sybiz.Vision.Platform.Common.TransactionAutoNumber.UseAutoGenerator(typeof(Sybiz.Vision.Platform.ContactManagement.Transaction.Email)))
				{
					MessageBox.Show($"Email numbering enabled: {Sybiz.Vision.Platform.Common.TransactionAutoNumber.UseAutoGenerator(typeof(Sybiz.Vision.Platform.ContactManagement.Transaction.CaseFile))}");
				}
				else
				{
					newActivity.ActivityNumber = Interaction.InputBox("Enter email number", "Information Required").Trim();
				}
				var newContact = Sybiz.Vision.Platform.Common.EmailContact.NewObject("sybiz@Sybiz.com");

				//These values will automatically be detected based on the email address if possible, otherwise a single field can be provided a value to associate email address with customer/supplier as relevant
				//newContact.SupplierId = 0
				//newContact.CustomerId = 0

				Sybiz.Vision.Platform.ContactManagement.Transaction.ActivityContact newActivityContact = Sybiz.Vision.Platform.ContactManagement.Transaction.ActivityContact.NewObject(newContact, Sybiz.Vision.Platform.ContactManagement.Transaction.ActivityContact.EmailRecipientType.TO);

				newActivity.Contacts.Add(newActivityContact);

				newActivity.Subject = Interaction.InputBox("Please enter a Subject", "Information Required", "").Trim();


				//These values may be set based on defaults from within the software itself but are mandatory fields
				newActivity.Category = Sybiz.Vision.Platform.ContactManagement.ActivityCategoryLookupInfoList.GetList().First(o => o.EmailActive == true).Id;
				newActivity.Status = Sybiz.Vision.Platform.Common.StatusLookupInfoList.GetList().First(o => o.ActivityActive == true).Id;
				newActivity.Priority = Sybiz.Vision.Platform.Common.PriorityLookupInfoList.GetList().First(o => o.ActivityActive == true).Id;

				newActivity.User = Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal().UserId;

				if (newActivity.IsValid)
				{
					newActivity = newActivity.Save();
					MessageBox.Show($"Task {newActivity.ActivityNumber} created successfully");
				}
				else
				{
					foreach (var rule in newActivity.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
					{
						MessageBox.Show($"{rule.PropertyName} Is broken on object {rule.ObjectLevel} because '{rule.Description}'");
					}
				}

			}
			catch (Exception)
			{
				MessageBox.Show($"{Information.Err().Description}");
			}
		}

		private void CreateTask()
		{
			try
			{
				var newActivity = Sybiz.Vision.Platform.ContactManagement.Transaction.Task.GetObject(0);

				if (Sybiz.Vision.Platform.Common.TransactionAutoNumber.UseAutoGenerator(typeof(Sybiz.Vision.Platform.ContactManagement.Transaction.Task)))
				{
					MessageBox.Show($"Task numbering enabled: {Sybiz.Vision.Platform.Common.TransactionAutoNumber.UseAutoGenerator(typeof(Sybiz.Vision.Platform.ContactManagement.Transaction.CaseFile))}");
				}
				else
				{
					newActivity.ActivityNumber = Interaction.InputBox("Enter task number", "Information Required").Trim();
				}

				//Only a single value can be assigned or both can be left as 0 for internal remarks
				//newActivity.Customer = 0
				//newActivity.Supplier = 0

				newActivity.Subject = Interaction.InputBox("Please enter a Subject", "Information Required", "").Trim();

				//These values may be set based on defaults from within the software itself but are mandatory fields
				newActivity.Category = Sybiz.Vision.Platform.ContactManagement.ActivityCategoryLookupInfoList.GetList().First(o => o.TaskActive == true).Id;
				newActivity.Status = Sybiz.Vision.Platform.Common.StatusLookupInfoList.GetList().First(o => o.ActivityActive == true).Id;
				newActivity.Priority = Sybiz.Vision.Platform.Common.PriorityLookupInfoList.GetList().First(o => o.ActivityActive == true).Id;

				newActivity.User = Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal().UserId;

				if (newActivity.IsValid)
				{
					newActivity = newActivity.Save();
					MessageBox.Show($"Task {newActivity.ActivityNumber} created successfully");
				}
				else
				{
					foreach (var rule in newActivity.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
					{
						MessageBox.Show($"{rule.PropertyName} Is broken on object {rule.ObjectLevel} because '{rule.Description}'");
					}
				}

			}
			catch (Exception)
			{
				MessageBox.Show($"{Information.Err().Description}");
			}


		}

		private void CreatePhoneCall()
		{
			try
			{

				var newActivity = Sybiz.Vision.Platform.ContactManagement.Transaction.PhoneCall.GetObject(0);

				if (Sybiz.Vision.Platform.Common.TransactionAutoNumber.UseAutoGenerator(typeof(Sybiz.Vision.Platform.ContactManagement.Transaction.PhoneCall)))
				{
					MessageBox.Show($"PhoneCall numbering enabled: {Sybiz.Vision.Platform.Common.TransactionAutoNumber.UseAutoGenerator(typeof(Sybiz.Vision.Platform.ContactManagement.Transaction.CaseFile))}");
				}
				else
				{
					newActivity.ActivityNumber = Interaction.InputBox("Enter phonecall number", "Information Required").Trim();
				}

				//Only a single value can be assigned or both can be left as 0 for internal remarks
				//newActivity.Customer = 0
				//newActivity.Supplier = 0

				newActivity.Subject = Interaction.InputBox("Please enter a Subject", "Information Required", "").Trim();

				//These values may be set based on defaults from within the software itself but are mandatory fields
				newActivity.Category = Sybiz.Vision.Platform.ContactManagement.ActivityCategoryLookupInfoList.GetList().First(o => o.PhonecallActive == true).Id;
				newActivity.Status = Sybiz.Vision.Platform.Common.StatusLookupInfoList.GetList().First(o => o.ActivityActive == true).Id;
				newActivity.Priority = Sybiz.Vision.Platform.Common.PriorityLookupInfoList.GetList().First(o => o.ActivityActive == true).Id;

				newActivity.User = Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal().UserId;

				if (newActivity.IsValid)
				{
					newActivity = newActivity.Save();
					MessageBox.Show($"Task {newActivity.ActivityNumber} created successfully");
				}
				else
				{
					foreach (var rule in newActivity.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
					{
						MessageBox.Show($"{rule.PropertyName} Is broken on object {rule.ObjectLevel} because '{rule.Description}'");
					}
				}

			}
			catch (Exception)
			{
				MessageBox.Show($"{Information.Err().Description}");
			}

		}

		public void btnLinkCaseFileToRemark_Click(object sender, EventArgs e)
		{
			try
			{
				var caseNumber = Interaction.InputBox("Please enter a case file code", "Information Required", "").Trim();
				int caseFileId = 0;
				var newActivity = Sybiz.Vision.Platform.ContactManagement.Transaction.Remark.GetObject(0);

				//Retrive the case file id from the database based on some external information
				using (var cn = new System.Data.SqlClient.SqlConnection(Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal().CompanyConnectionString))
				{
					cn.Open();
					using (var cmd = cn.CreateCommand())
					{
						cmd.CommandType = CommandType.Text;
						cmd.CommandText = "SELECT CaseFileId FROM [crm].[CaseFile] WHERE CaseFileNumber = @CaseFileNumber";
						cmd.Parameters.AddWithValue("@CaseFileNumber", caseNumber);
						caseFileId = System.Convert.ToInt32(cmd.ExecuteScalar());
					}

				}


				if (Sybiz.Vision.Platform.Common.TransactionAutoNumber.UseAutoGenerator(typeof(Sybiz.Vision.Platform.ContactManagement.Transaction.Remark)))
				{
					MessageBox.Show($"Remark numbering enabled: {Sybiz.Vision.Platform.Common.TransactionAutoNumber.UseAutoGenerator(typeof(Sybiz.Vision.Platform.ContactManagement.Transaction.CaseFile))}");
				}
				else
				{
					newActivity.ActivityNumber = Interaction.InputBox("Enter remark number", "Information Required").Trim();
				}

				//Only a single value can be assigned or both can be left as 0 for internal remarks
				//newActivity.Customer = 0
				//newActivity.Supplier = 0

				newActivity.Subject = Interaction.InputBox("Please enter a Subject", "Information Required", "").Trim();
				newActivity.Category = Sybiz.Vision.Platform.ContactManagement.ActivityCategoryLookupInfoList.GetList().Where(o => o.TaskActive == true).FirstOrDefault().Id;
				newActivity.Status = Sybiz.Vision.Platform.Common.StatusLookupInfoList.GetList().Where(o => o.CaseFileActive == true).FirstOrDefault().Id;
				newActivity.User = Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal().UserId;
				newActivity.Priority = Sybiz.Vision.Platform.Common.PriorityLookupInfoList.GetList().Where(o => o.CaseFileActive == true).FirstOrDefault().Id;

				newActivity.CaseFiles.Add(Sybiz.Vision.Platform.ContactManagement.CaseFileStore.GetObject(caseFileId));

				if (newActivity.IsValid)
				{
					newActivity = newActivity.Save();
					MessageBox.Show($"Task {newActivity.ActivityNumber} created successfully");
				}
				else
				{
					foreach (var rule in newActivity.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
					{
						MessageBox.Show($"{rule.PropertyName} Is broken on object {rule.ObjectLevel} because '{rule.Description}'");
					}
				}

			}
			catch (Exception)
			{
				MessageBox.Show($"{Information.Err().Description}");
			}

		}

		public void btnLinkJobActivity_Click(object sender, EventArgs e)
		{
			try
			{
				var jobNumber = Interaction.InputBox("Please enter a job code", "Information Required", "").Trim();
				var job = Sybiz.Vision.Platform.JobCosting.Job.GetObject(System.Convert.ToString(jobNumber));

				var typeActivity = Interaction.InputBox($"Please select an activity type to link to the case file 1 - Remark 2 - Email 3 - PhoneCall 4 - Task ", "Information Required", "").Trim();

				if (!System.Enum.IsDefined(typeof(Sybiz.Vision.Platform.Core.Enumerations.ActivityType), int.Parse(System.Convert.ToString(typeActivity))))
				{
					Information.Err().Description = " Entered value is outside the given range please select from list";
				}


				var activityNumber = Interaction.InputBox("Please enter an activity number to link to the case file", "Information Required", "").Trim();

				// I dont agree with this since you alreay have the type on object but you still have to pass activity type
				int activityid = 0;
				switch (int.Parse(System.Convert.ToString(typeActivity)))
				{
					case (System.Int32)Sybiz.Vision.Platform.Core.Enumerations.ActivityType.Remark:
						var remarkactivity = Sybiz.Vision.Platform.ContactManagement.Transaction.Remark.GetObject(System.Convert.ToString(activityNumber), System.Convert.ToString(typeActivity));
						activityid = remarkactivity.Id;
						break;
					case (System.Int32)Sybiz.Vision.Platform.Core.Enumerations.ActivityType.Email:
						var emailactivity = Sybiz.Vision.Platform.ContactManagement.Transaction.Email.GetObject(System.Convert.ToString(activityNumber), System.Convert.ToString(typeActivity));
						activityid = emailactivity.Id;
						break;
					case (System.Int32)Sybiz.Vision.Platform.Core.Enumerations.ActivityType.PhoneCall:
						var phoneactivity = Sybiz.Vision.Platform.ContactManagement.Transaction.PhoneCall.GetObject(System.Convert.ToString(activityNumber), System.Convert.ToString(typeActivity));
						activityid = phoneactivity.Id;
						break;
					case (System.Int32)Sybiz.Vision.Platform.Core.Enumerations.ActivityType.Task:
						var taskactivity = Sybiz.Vision.Platform.ContactManagement.Transaction.Task.GetObject(System.Convert.ToString(activityNumber), System.Convert.ToString(typeActivity));
						activityid = taskactivity.Id;
						break;
					default:
						activityid = 0;
						break;
				}

				if (activityid <= 0)
				{
					MessageBox.Show("Activity does not exist");
					return;
				}

				if (job.Activities.Where(s => s.Id == activityid).ToList().Count > 0)
				{
					MessageBox.Show($"Sorry {activityNumber} already exist in case file {job.Code}");
				}
				else
				{
					job.Activities.AddNew(activityid);
					MessageBox.Show($"Added {activityNumber} to {job.Code}");
				}

				if (job.IsValid)
				{
					job = job.Save();
					MessageBox.Show($"{job.Code} saved successfully");
				}
				else
				{
					foreach (var rule in job.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
					{
						MessageBox.Show($"{rule.PropertyName} Is broken on object {rule.ObjectLevel} because '{rule.Description}'");
					}
				}

				string msgresults = "";
				msgresults = "\r\n";
				for (var i = 0; i <= job.Activities.Count - 1; i++)
				{
					msgresults += $"{job.Activities[System.Convert.ToInt32(i)].ActivityNumber} {job.Activities[System.Convert.ToInt32(i)].TypeName} {"\r\n"}";
				}

				MessageBox.Show($"{job.Code} actitvity list now contains {msgresults}");
			}
			catch (Exception)
			{
				MessageBox.Show($"{Information.Err().Description}");
			}
		}
	}
}
