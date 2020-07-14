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

		public void CreateJobCost_Click(object sender, EventArgs e)
		{
			var newJobCost = Sybiz.Vision.Platform.JobCosting.Transaction.JobCost.NewObject(null);
			try
			{
				newJobCost.Job = 1;

				//Transaction date will be set based on the session date by default
				newJobCost.TransactionDate = DateTime.Today.AddDays(-1);

				newJobCost.Reference = "JC00001";

				//You can set a location at the header to be used as the default for the line.
				//newJobCost.Location = 1

				newJobCost.Notes = "Job Cost created by sample API app";


				Sybiz.Vision.Platform.JobCosting.Transaction.IJobTransactionLine line;


				//add a GL line
				if (Interaction.InputBox("Add GL line (Y/N)", "Information Required", "Y").Trim() == "Y")
				{
					line = CreateNewJobCostTransactionLine(newJobCost, Sybiz.Vision.Platform.Core.Enumerations.JobLineType.GL, 1, 1M, 100, 120, 1, 1);
					line.Description = "Description value changed from default";
					//line.CostCategory = 1
					line.Reference = "Ref4Line";
				}

				if (Interaction.InputBox("Add LB line (Y/N)", "Information Required", "Y").Trim() == "Y")
				{
					line = CreateNewJobCostTransactionLine(newJobCost, Sybiz.Vision.Platform.Core.Enumerations.JobLineType.LB, 1, 2M, 50, 120, 1, 1);
					line.Description = "Description value changed from default";
					line.TransactionNumber = "TS000001";
					line.Reference = "Ref4Line";
				}

				if (Interaction.InputBox("Add IC line (Y/N)", "Information Required", "Y").Trim() == "Y")
				{
					line = CreateNewJobCostTransactionLine(newJobCost, Sybiz.Vision.Platform.Core.Enumerations.JobLineType.IC, 1, 3M, 25, 50, 1, 1);
					line.Description = "Description value changed from default";
					//reference can not be edited for IC lines.
				}

				if (Interaction.InputBox("Add CR line (Y/N)", "Information Required", "Y").Trim() == "Y")
				{
					line = CreateNewJobCostTransactionLine(newJobCost, Sybiz.Vision.Platform.Core.Enumerations.JobLineType.CR, 1, 3M, 25, 50, 1, 1);
					line.Description = "Description value changed from default";
					line.TaxCode = 1;
					line.Reference = "Ref4Line";
				}

				if (Interaction.InputBox("Add CB line (Y/N)", "Information Required", "Y").Trim() == "Y")
				{
					line = CreateNewJobCostTransactionLine(newJobCost, Sybiz.Vision.Platform.Core.Enumerations.JobLineType.CB, 1, 3M, 25, 50, 1, 1);
					line.TaxCode = 1;
					line.Description = "Description value changed from default";
					line.Reference = "Ref4Line";
				}

				if (newJobCost.IsProcessable)
				{
					newJobCost = newJobCost.Process();
					MessageBox.Show($"Transaction succesfully created/processed for [{newJobCost.TransactionNumber}]");
				}
				else if (newJobCost.IsValid)
				{
					foreach (var rule in newJobCost.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
					{
						MessageBox.Show($"{rule.PropertyName} is broken on object {rule.ObjectLevel} because '{rule.Description}'");
					}
					newJobCost = newJobCost.Save();
					MessageBox.Show($"Transaction succesfully saved for [{newJobCost.TransactionNumber}]");
				}

			}
			catch (Exception ex)
			{
				foreach (var rule in newJobCost.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
				{
					MessageBox.Show($"{rule.PropertyName} is broken on object {rule.ObjectLevel} because '{rule.Description}'");
				}
				MessageBox.Show($"Error creating transaction {ex.Message}");
			}

		}

		private Sybiz.Vision.Platform.JobCosting.Transaction.IJobTransactionLine CreateNewJobCostTransactionLine(Sybiz.Vision.Platform.JobCosting.Transaction.JobCost transactionJC, Sybiz.Vision.Platform.Core.Enumerations.JobLineType lineType, int accountId, decimal quantity, decimal? cost, decimal? charge, int? stage, int? CostCentre)
		{
			Sybiz.Vision.Platform.JobCosting.Transaction.IJobTransactionLine newLine = transactionJC.Lines.AddNew(lineType);

			newLine.Account = accountId;
			newLine.Quantity = quantity;
			if (cost.HasValue)
			{
				newLine.Cost = cost.Value;
			}
			if (charge.HasValue)
			{
				newLine.Charge = charge.Value;
			}

			if (stage.HasValue)
			{
				newLine.Stage = stage.Value;
			}
			if (CostCentre.HasValue)
			{
				newLine.CostCentre = CostCentre.Value;
			}
			return newLine;
		}

		public void CreatePreInvoice_Click(object sender, EventArgs e)
		{
			var newPreInvoice = Sybiz.Vision.Platform.JobCosting.Transaction.JobPreInvoice.NewObject(null);
			try
			{
				newPreInvoice.Job = 1;

				//Transaction date will be set based on the session date by default
				newPreInvoice.TransactionDate = DateTime.Today.AddDays(-1);

				newPreInvoice.Reference = "PI00001";

				newPreInvoice.Notes = "PreInvoice created by sample API app";


				Sybiz.Vision.Platform.JobCosting.Transaction.JobPreInvoiceLine line;


				//add a GL line
				if (Interaction.InputBox("Add new line (Y/N)", "Information Required", "Y").Trim() == "Y")
				{
					line = CreateNewPreInvoiceTransactionLine(newPreInvoice, 100, 120, 1, 1);
					line.Description = "Description value changed from default";
				}

				if (newPreInvoice.IsProcessable)
				{
					newPreInvoice = newPreInvoice.Process();
					MessageBox.Show($"Transaction succesfully created/processed for [{newPreInvoice.TransactionNumber}]");
				}
				else if (newPreInvoice.IsValid)
				{
					foreach (var rule in newPreInvoice.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
					{
						MessageBox.Show($"{rule.PropertyName} is broken on object {rule.ObjectLevel} because '{rule.Description}'");
					}
					newPreInvoice = newPreInvoice.Save();
					MessageBox.Show($"Transaction succesfully saved for [{newPreInvoice.TransactionNumber}]");
				}

			}
			catch (Exception ex)
			{
				foreach (var rule in newPreInvoice.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
				{
					MessageBox.Show($"{rule.PropertyName} is broken on object {rule.ObjectLevel} because '{rule.Description}'");
				}
				MessageBox.Show($"Error creating transaction {ex.Message}");
			}
		}

		private Sybiz.Vision.Platform.JobCosting.Transaction.JobPreInvoiceLine CreateNewPreInvoiceTransactionLine(Sybiz.Vision.Platform.JobCosting.Transaction.JobPreInvoice transactionPI, decimal? cost, decimal? charge, int? stage, int? costCentre)
		{

			Sybiz.Vision.Platform.JobCosting.Transaction.JobPreInvoiceLine newLine = transactionPI.Lines.AddNew();

			if (cost.HasValue)
			{
				newLine.BaseCost = cost.Value;
			}
			if (charge.HasValue)
			{
				newLine.Charge = charge.Value;
			}
			if (stage.HasValue && newLine.CanWriteProperty("Stage"))
			{
				newLine.Stage = stage.Value;
			}
			if (costCentre.HasValue && newLine.CanWriteProperty("CostCentre"))
			{
				newLine.CostCentre = costCentre.Value;
			}
			return newLine;
		}

		public void CreateJobInvoice_Click(object sender, EventArgs e)
		{
			var newCostedInvoice = Sybiz.Vision.Platform.JobCosting.Transaction.JobCostedInvoice.NewObject(null);
			try
			{

				newCostedInvoice.Job = 1;

				//Transaction date will be set based on the session date by default
				newCostedInvoice.TransactionDate = DateTime.Today.AddDays(-1);


				newCostedInvoice.HeadOffice = true; //

				newCostedInvoice.Reference = "CI00001";
				newCostedInvoice.Notes = "Costed Invoice created by sample API app";

				foreach (Sybiz.Vision.Platform.JobCosting.Transaction.JobCostedInvoiceLine line in newCostedInvoice.Lines)
				{
					//Lines for JobCosts are generated  and you can loop through them to change select fields.
					line.ShowOnInvoice = false; //Determines if is included in printed output
					line.CostIsInvoiced = true; //Invoice transaction

					line.SetProgressPaymentPercentage(50);

					line.Quantity = line.Quantity / 2;
					line.Charge = (decimal)((double)line.Charge * 1.1);
				}

				if (newCostedInvoice.IsProcessable)
				{
					newCostedInvoice = newCostedInvoice.Process();
					MessageBox.Show($"Transaction succesfully created/processed for [{newCostedInvoice.TransactionNumber}]");
				}
				else if (newCostedInvoice.IsValid)
				{
					foreach (var rule in newCostedInvoice.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
					{
						MessageBox.Show($"{rule.PropertyName} is broken on object {rule.ObjectLevel} because '{rule.Description}'");
					}
					newCostedInvoice = newCostedInvoice.Save();
					MessageBox.Show($"Transaction succesfully saved for [{newCostedInvoice.TransactionNumber}]");
				}

			}
			catch (Exception ex)
			{
				foreach (var rule in newCostedInvoice.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
				{
					MessageBox.Show($"{rule.PropertyName} is broken on object {rule.ObjectLevel} because '{rule.Description}'");
				}
				MessageBox.Show($"Error creating transaction {ex.Message}");

			}

		}

		public void CreateJob_Click(object sender, EventArgs e)
		{


			var newJob = Sybiz.Vision.Platform.JobCosting.Job.GetObject("0");
			if (newJob.CanWriteProperty("Code"))
			{
				var jobCode = Interaction.InputBox("Enter Job code", "Information Required").Trim();
				newJob.Code = System.Convert.ToString(jobCode);
			}
			else
			{
				MessageBox.Show("Automatic numbering is on for Jobs");
			}

			newJob.AdvancedJob = true;
			newJob.Description = "New Job created in sample app";
			newJob.ExtendedDescription = "Extended Description for new Job";
			newJob.JobInvoicingMethod = Sybiz.Vision.Platform.Core.Enumerations.JobInvoicingMethod.CostPlus;
			newJob.Group = 1;
			newJob.SortCode = 1;
			newJob.Priority = 1;
			//newJob.Status = 1
			newJob.Project = 1;
			newJob.DaysQuoteAppliesFor = 4;
			newJob.Customer = 1;
			newJob.CustomerOrderNumber = "CO00001";
			//newJob.TaxCode = 1
			newJob.SalesRepresentative = 1;
			newJob.AnalysisCode = 1;
			newJob.StartDate = DateTime.Today.AddDays(-1);
			newJob.TargetCompletionDate = DateTime.Today.AddDays(30);
			newJob.Stages.AddNew().StageId = 1;
			newJob.CostCentres.AddNew().CostCentreId = 1;
			newJob.EquipmentMarkUp = 10.5M;
			newJob.LabourMarkUp = 15.5M;
			newJob.MaterialMarkUp = 21.75M;
			newJob.PurchasesMarkUp = 5M;
			newJob.SubContractMarkUp = 20M;
			//advanced Job settings.
			newJob.JobPostByMethod = Sybiz.Vision.Platform.Core.Enumerations.JobPostByMethod.CostCentre;
			//newJob.RetentionPolicy = 1
			//Ceiling only applies to variable ceiling retention policies.
			//newJob.Ceiling = 21.5D

			//credit hold
			newJob.StopCosts = false;
			newJob.StopEstimates = false;
			newJob.StopInvoices = false;
			newJob.StopOrders = false;
			newJob.HoldReason = "All good";

			Sybiz.Vision.Platform.JobCosting.JobContract tmpContract = newJob.Contracts.AddNew();
			tmpContract.Contract = 1;
			tmpContract.StartDate = DateTime.Today.AddDays(-10);
			tmpContract.EndDate = DateTime.Today.AddDays(45);

			newJob.DocumentFolder = "C:\\temp";
			newJob.Remarks = "Remarks for new Job";

			if (newJob.IsValid)
			{
				newJob = newJob.Save();
				MessageBox.Show($"JobID {newJob.Id} created successfully");
			}
			else
			{
				foreach (var rule in newJob.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
				{
					MessageBox.Show($"{rule.PropertyName} Is broken on object {rule.ObjectLevel} because '{rule.Description}'");
				}
			}
		}
	}
}
