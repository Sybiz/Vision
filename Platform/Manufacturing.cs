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
		public void CreateMO_Click(object sender, EventArgs e)
		{

			try
			{
				var order = Sybiz.Vision.Platform.Inventory.Transaction.ManufactureOrder.NewObject(null);

				//If automatic numbering is disabled then field will allow editing and should be populated
				if (order.CanWriteProperty("TransactionNumber"))
				{
					order.TransactionNumber = System.Convert.ToString(Interaction.InputBox("Please enter the manufacturing order number", "Information required").Trim());
				}

				//Transaction date will default to user system date but can be changed
				order.TransactionDate = System.Convert.ToDateTime(DateTime.Today.AddDays(1));
				order.Description = string.Format("Manufacture for Sales Order {0}", order.TransactionNumber);

				int assemblyId = 177; //[choose an assemblyId]
				var newLine = order.Lines.AddNew();
				newLine.Assembly = Sybiz.Vision.Platform.Inventory.ProductDetailInfo.GetObject(assemblyId).AssemblyDetails.Id;
				newLine.Quantity = 100;

				//Source and destination will default to relevant locations based on if work orders are enabled;
				//Only change value if necessary
				if (Sybiz.Vision.Platform.Common.DefaultManager.GetManager().MF.QualityAssuranceLocation > 0) //[if using work orders]
				{
					newLine.SourceLocation = Sybiz.Vision.Platform.Common.DefaultManager.GetManager().MF.ManufactureLocation;
					newLine.DestinationLocation = Sybiz.Vision.Platform.Common.DefaultManager.GetManager().MF.QualityAssuranceLocation;
				}
				else
				{
					newLine.SourceLocation = 1; //[choose a location]
					newLine.DestinationLocation = 1; //[choose a location]
				}

				order = order.Process();
				MessageBox.Show($"Manufacture Order - Create was successful");
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Manufacture Order - Create was unsuccessful - {ex.Message}");
			}

		}

		public void CreateMIFromMO_Click(object sender, EventArgs e)
		{

			try
			{
				var orderNumber = Interaction.InputBox("Please enter manufacturing order number", "Information Required", "").Trim();

				//This is not the preferred way to get the manufacturing order unique identifer
				//If user doesn't have rights to edit or process manufacture order an error will occur;
				//see debtor invoice transactions for alternate implementation
				var order = Sybiz.Vision.Platform.Inventory.Transaction.ManufactureOrder.GetObject(null, System.Convert.ToString(orderNumber));

				//Create a new manufacture issue that will start a new batch
				var newIssue = Sybiz.Vision.Platform.Inventory.Transaction.ManufactureIssue.NewObject(null);

				//Add the manufacture order to the issue, adding all relevant lines to the issue ready to be processed.
				newIssue.AddSourceDocument(order.Id, Sybiz.Vision.Platform.Core.Enumerations.TransactionType.ManufactureOrder);

				foreach (var line in newIssue.Lines)
				{
					line.Quantity = line.QuantityOrdered - line.QuantityNetIssued; //manufacture all outstanding amounts
				}

				//Generate the standard materials list or to a specific level
				newIssue.GenerateStandardMaterialList();

				//Process the transaction and get new object back with correct new appropriate values (e.g TransactionNumber)
				newIssue = newIssue.Process();
				MessageBox.Show($"Manufacture Order And Issue was successful");
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Manufacture Order And Issue was unsuccessful - {ex.Message}");
			}

		}

		public void CreateAssembly_Click(object sender, EventArgs e)
		{
			var productCode = Interaction.InputBox("Please enter product code to create an assembly", "Information Required", "").Trim();
			var productInfo = Sybiz.Vision.Platform.Inventory.ProductDetailInfo.GetObject(System.Convert.ToString(productCode)); //Use productDetailInfo to find productId based on code

			if (productInfo.Id == 0)
			{
				return;
			}

			var newAssembly = Sybiz.Vision.Platform.Inventory.Assembly.GetObject(0);
			newAssembly.ProductId = productInfo.Id;

			//New assemblies have a default component added which should be removed
			newAssembly.Components.Clear();

			do
			{
				var componentCode = Interaction.InputBox("Please enter product code to use as a component (leave blank to exit)", "Information Required", "").Trim();

				if (string.IsNullOrWhiteSpace(System.Convert.ToString(componentCode)))
				{
					break;
				}
				else
				{
					productInfo = Sybiz.Vision.Platform.Inventory.ProductDetailInfo.GetObject(System.Convert.ToString(componentCode));
					if (productInfo.Id != 0)
					{
						var newComponent = newAssembly.Components.AddNew();
						newComponent.ProductId = productInfo.Id;
					}
				}
			} while (true);

			if (newAssembly.IsValid)
			{
				newAssembly = newAssembly.Save();
				MessageBox.Show($"AssemblyId {newAssembly.Id} for Product {productCode} created successfully");
			}
			else
			{
				foreach (var rule in newAssembly.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
				{
					MessageBox.Show($"{rule.PropertyName} Is broken on object {rule.ObjectLevel} because '{rule.Description}'");
				}
			}

		}

		public void EditAssembly_Click(object sender, EventArgs e)
		{
			var productCode = Interaction.InputBox("Please enter an assembly code to modify", "Information Required", "").Trim();
			var componentCode = Interaction.InputBox("Please enter a product code to remove as a component", "Information Required", "").Trim();
			var productInfo = Sybiz.Vision.Platform.Inventory.ProductDetailInfo.GetObject(System.Convert.ToString(productCode)); //Use productDetailInfo to find assemblyId which is different to productId

			var exisitingAssembly = Sybiz.Vision.Platform.Inventory.Assembly.GetObject(productInfo.AssemblyDetails.Id);

			exisitingAssembly.Components.Where(comp => comp.ProductDetails.Code == componentCode).ToList().ForEach(obj => exisitingAssembly.Components.Remove(obj));

			if (exisitingAssembly.IsValid)
			{
				exisitingAssembly = exisitingAssembly.Save();
				MessageBox.Show($"AssemblyId {exisitingAssembly.Id} for Product {productCode} edited successfully");
			}
			else if (exisitingAssembly.IsDirty)
			{
				foreach (var rule in exisitingAssembly.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
				{
					MessageBox.Show($"{rule.PropertyName} Is broken on object {rule.ObjectLevel} because '{rule.Description}'");
				}
			}
			else
			{
				MessageBox.Show($"No changes made to this assembly");
			}

		}
	}
}
