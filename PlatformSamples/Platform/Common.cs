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
		public void ImportantInformationButton_Click(object sender, EventArgs e)
		{
			MessageBox.Show("When creating an external program that uses the API, your program should have the following details within their app.config file");
			Process.Start("notepad.exe", $"{Application.ExecutablePath}.config");
		}

		public void ShowEnquiryForm_Click(object sender, EventArgs e)
		{
			//This code is best suited to be used within breakpoints, otherwise application will need to exist with Vision application folder
			//Will allow editing of a customer if Id > 0 otherwise will create a new customer record
			if (!System.IO.File.Exists("profilecatalog.xml"))
			{
				MessageBox.Show("Code must be run from application directory, otherwise see code behind");
				return;
			}
			else
			{
				Sybiz.Vision.Module.Coordinator.VisionApplication.GetApplication().CM.ViewMaintenanceForm(Sybiz.Vision.Platform.Core.Enumerations.ObjectType.Customer, 0, this, null, null);
			}
		}

		public void ShowMaintenanceForm_Click(object sender, EventArgs e)
		{
			//This code is best suited to be used within breakpoints, otherwise application will need to exist with Vision application folder
			if (!System.IO.File.Exists("profilecatalog.xml"))
			{
				MessageBox.Show("Code must be run from application directory, otherwise see code behind");
				return;
			}
			else
			{
				Sybiz.Vision.Module.Coordinator.VisionApplication.GetApplication().CM.ViewEnquiryForm(ObjectType: Sybiz.Vision.Platform.Core.Enumerations.ObjectType.Customer, Parent: this, EnquiryId: Sybiz.Vision.Platform.Debtors.CustomerDetailInfo.GetObject(System.Convert.ToInt32(Interaction.InputBox("Please enter customer code", "Information Required").Trim())).Id);
			}
		}

		public void ShowTransactionForm_Click(object sender, EventArgs e)
		{
			if (!System.IO.File.Exists("profilecatalog.xml"))
			{

				MessageBox.Show("Code must be run from application directory, otherwise see code behind");
				return;
			}
			else
			{
				//Will allow editing/viewing if Id > 0 otherwise will create a new transaction
				Sybiz.Vision.Module.Coordinator.VisionApplication.GetApplication().CM.ViewTransactionForm(Sybiz.Vision.Platform.Core.Enumerations.TransactionType.SalesInvoice, 0, this, true);
			}
		}

		public void ShowNotes_Click(object sender, EventArgs e)
		{
			if (!System.IO.File.Exists("profilecatalog.xml"))
			{
				MessageBox.Show("Code must be run from application directory, otherwise see code behind");
				return;
			}
			else
			{
				//This code is best suited to be used within breakpoints, otherwise application will need to exist with Vision application folder
				Sybiz.Vision.Module.Coordinator.VisionApplication.GetApplication().CM.ShowNoteForm(this, Sybiz.Vision.Platform.Debtors.CustomerDetailInfo.GetObject(System.Convert.ToInt32(Interaction.InputBox("Please enter customer code", "Information Required").Trim())).Id, Sybiz.Vision.Platform.Core.Enumerations.NoteType.Customer, "Notes?");
			}
		}

		public void ShowReminders_Click(object sender, EventArgs e)
		{
			if (!System.IO.File.Exists("profilecatalog.xml"))
			{
				MessageBox.Show("Code must be run from application directory, otherwise see code behind");
				return;
			}
			else
			{
				//This code is best suited to be used within breakpoints, otherwise application will need to exist with Vision application folder
				Sybiz.Vision.Module.Coordinator.VisionApplication.GetApplication().CM.ViewReminders(this, Sybiz.Vision.Platform.Core.Enumerations.ReminderType.Customer, Sybiz.Vision.Platform.Debtors.CustomerDetailInfo.GetObject(System.Convert.ToInt32(Interaction.InputBox("Please enter customer code", "Information Required").Trim())).Id, "Reminders?");
			}
		}

		public void GetCompanyConnectionString_Click(object sender, EventArgs e)
		{
			MessageBox.Show($"{Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal().CompanyConnectionString}{Environment.NewLine}{Environment.NewLine}{Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal().CommonConnectionString}");
		}

		public void GetLoggedInUserInfo_Click(object sender, EventArgs e)
		{
			MessageBox.Show($"{Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal().UserId} : {Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal().UserName} -> {Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal().EmailAddress}");
		}

		public void RunScalarCommand_Click(object sender, EventArgs e)
		{
			//Connection string can be obtained by referencing current principal object
			//Using statement ensures that connection is closed and disposed if an exception is thrown
			using (var cn = new System.Data.SqlClient.SqlConnection(Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal().CompanyConnectionString))
			{
				cn.Open();
				using (var cmd = cn.CreateCommand())
				{
					cmd.CommandType = CommandType.Text;
					cmd.CommandText = "SELECT @@VERSION";
					MessageBox.Show($"{cmd.ExecuteScalar()}");
				}

			}

		}

		public void RegisterThirdPartyTool_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Please see the code behind to see how to register your breakpoint or third party tool");
			return;

			//Registering your program will result in a prompt appearing whenever a user attempts to upgrade.
			//The registration is recommended to be repeated on each attempted login so that it reflects the current version number
			//			Sybiz.Vision.Platform.ThirdPartyProduct.RegisterProduct("DEMO-API", "This is demonstration registration", Application.ProductVersion);

			//To remove the registration do the following
			//			Sybiz.Vision.Platform.ThirdPartyProduct.UnRegisterProduct("DEMO-API");
		}

		public void GetCompanySystemDefaults_Click(object sender, EventArgs e)
		{
			var userDefaults = Sybiz.Vision.Platform.Common.UserDefaultManager.GetManager();
			var systemDefaults = Sybiz.Vision.Platform.Common.DefaultManager.GetManager();

			//UserInformation is primary used to get information the processing date nominat
			MessageBox.Show($"Current session date: {userDefaults.CM.SystemDate:d} Base Currency: {userDefaults.CM.Currency.Symbol}{userDefaults.CM.Currency.Code}");

			//System defaults have a property per ledger to determine their defaults; most maintenance and transaction objects have a defaults property embedded internally that can be accessed
			MessageBox.Show($"Cash accounting in use: {systemDefaults.GL.UseCashAccounting} Show auto postings on debtor transactions:{systemDefaults.DR.ShowAutoPostingScreen} Project settings on job costing transactions:{systemDefaults.JC.ProjectLevel:g}");

			//Determine if automatic numbering is enabled
			MessageBox.Show($"Sales Invoice Automatic Numbering Enabled: {Sybiz.Vision.Platform.Common.TransactionAutoNumber.UseAutoGenerator(typeof(Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice))}");

		}

		public void BreakpointExamples_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("notepad.exe", "Breakpoints.vb");
			return;
		}
	}
}
