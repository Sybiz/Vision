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

		public void CreateProduct_Click(object sender, EventArgs e)
		{
			//Some defaults are assumed to have a valid entry, change these values if not appropriate
			var productCode = Interaction.InputBox("Enter customer code", "Information Required").Trim();
			var newProduct = Sybiz.Vision.Platform.Inventory.Product.GetObject("0");

			newProduct.Code = System.Convert.ToString(productCode);
			newProduct.Description = "New product created by sample app";
			newProduct.ExtendedDescription = "New extended description that has longer text";
			newProduct.Group = 1;

			//newProduct.SalesTaxCodeId should not normally be set unless it is different to the debtor defaults (e.g WET)
			//newProduct.PurchasesTaxCodeId should not normally be set unless it is different to the creditor defaults (e.g GST-FREE)

			//Unit of measure
			newProduct.DefaultUOM = 1;

			//Alternate units of measure are added to a list; first entry is always the default unit of measure
			var altUnitOfMeasure = newProduct.ProductUnitsOfMeasure.AddNew();
			altUnitOfMeasure.UnitOfMeasureId = 3;
			altUnitOfMeasure.CanPurchase = false;
			altUnitOfMeasure.CanSell = true;

			altUnitOfMeasure = newProduct.ProductUnitsOfMeasure.AddNew();
			altUnitOfMeasure.UnitOfMeasureId = 4;
			altUnitOfMeasure.CanPurchase = true;
			altUnitOfMeasure.CanSell = false;

			//New products will default to have all available prices scales available, prices can be set inclusive or exclusive
			newProduct.ProductPrices.First().InclusivePrice = 10.0M;
			newProduct.ProductPrices.Last().ExclusivePrice = 9.0M;

			//New products will default to have all locations available;
			//remove last entry in code below on assumption multiple locations exist
			newProduct.ProductLocations.First().AllowPurchases = false;
			newProduct.ProductLocations.Remove(newProduct.ProductLocations.Last());

			if (newProduct.IsValid)
			{
				newProduct = newProduct.Save();
				MessageBox.Show($"ProductId {newProduct.Id} created successfully");
			}
			else
			{
				foreach (var rule in newProduct.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
				{
					MessageBox.Show($"{rule.PropertyName} Is broken on object {rule.ObjectLevel} because '{rule.Description}'");
				}
			}


		}

		public void CopyProduct_Click(object sender, EventArgs e)
		{
			//Some defaults are assumed to have a valid entry, change these values if not appropriate
			var productCode = Interaction.InputBox("Enter exisiting product code", "Information Required").Trim();
			var newProductCode = Interaction.InputBox("Enter new product code", "Information Required").Trim();

			var newProduct = Sybiz.Vision.Platform.Inventory.Product.GetObject(System.Convert.ToString(productCode));
			((Sybiz.Vision.Platform.Core.ICloneAsNew)newProduct).CloneAsNew();
			newProduct.Code = System.Convert.ToString(newProductCode);

			if (newProduct.IsValid)
			{
				newProduct = newProduct.Save();
				MessageBox.Show($"ProductId {newProduct.Id} created successfully");
			}
			else
			{
				foreach (var rule in newProduct.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
				{
					MessageBox.Show($"{rule.PropertyName} Is broken on object {rule.ObjectLevel} because '{rule.Description}'");
				}
			}

		}
	}
}
