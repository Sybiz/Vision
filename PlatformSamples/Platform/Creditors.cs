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
        public void CreateSupplier_Click(object sender, EventArgs e)
        {
            //Some defaults are assumed to have a valid entry, change these values if not appropriate
            var supplierCode = Interaction.InputBox("Enter Supplier code", "Information Required").Trim();
            var newSupplier = Sybiz.Vision.Platform.Creditors.Supplier.GetObject("0");

            newSupplier.Code = System.Convert.ToString(supplierCode);
            newSupplier.Name = "New Supplier created by sample app";
            newSupplier.Group = 1;
            newSupplier.TradingTerms = 1;

            //Tax status can be changed to accomodate export / non-taxable Suppliers
            newSupplier.TaxStatus = Sybiz.Vision.Platform.Core.Enumerations.SupplierTaxStatus.Import;

            //Address information
            newSupplier.PhysicalAddress.Street = "123 Seasme Street";
            newSupplier.PhysicalAddress.State = "NY";
            newSupplier.PhysicalAddress.Suburb = "Brookyln";
            newSupplier.PhysicalAddress.PostCode = "1111";
            newSupplier.PhysicalAddress.Country = "USA";

            //Suppliers don't have delivery address but delivery instructions can be defaulted
            newSupplier.DeliveryInstructions = "Goods must be recieved before noon on Tuesday";

            var contact = newSupplier.Contacts.AddNew();
            contact.FirstName = "Sybiz";
            contact.LastName = "Software";
            contact.Position = "Developer";

            if (newSupplier.IsValid)
            {
                newSupplier = newSupplier.Save();
                MessageBox.Show($"SupplierId {newSupplier.Id} created successfully");
            }
            else
            {
                foreach (var rule in newSupplier.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
                {
                    MessageBox.Show($"{rule.PropertyName} Is broken on object {rule.ObjectLevel} because '{rule.Description}'");
                }
            }

        }

        public void EditSupplier_Click(object sender, EventArgs e)
        {
            var supplierCode = Interaction.InputBox("Enter supplier code", "Information Required").Trim();
            var supplierName = Interaction.InputBox("Enter new supplier name", "Information Required").Trim();
            Sybiz.Vision.Platform.Creditors.Supplier exisitingSupplier = default(Sybiz.Vision.Platform.Creditors.Supplier);

            try
            {
                exisitingSupplier = Sybiz.Vision.Platform.Creditors.Supplier.GetObject(System.Convert.ToString(supplierCode));
            }
            catch (Exception)
            {
                MessageBox.Show($"Supplier code: {supplierCode} does not exist");
                return;
            }

            exisitingSupplier.Name = System.Convert.ToString(supplierName);

            //Create new contact ignoring if already exists
            var contact = exisitingSupplier.Contacts.AddNew();
            contact.FirstName = "Warehouse";
            contact.LastName = "Foreman";
            contact.Position = "Warehouse Manager";
            contact.Email = "nobody@example.com";

            if (exisitingSupplier.IsValid)
            {
                exisitingSupplier = exisitingSupplier.Save();
                MessageBox.Show($"Supplier {exisitingSupplier.Name} edited successfully");
            }
            else
            {
                foreach (var rule in exisitingSupplier.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
                {
                    MessageBox.Show($"{rule.PropertyName} is broken on object {rule.ObjectLevel} because '{rule.Description}'");
                }
            }

        }

        public void ModifyCreditorDefaults_Click(object sender, EventArgs e)
        {
            var creditorDefaults = Sybiz.Vision.Platform.Creditors.Defaults.GetObject();

            //Change default tax code for import suppliers
            creditorDefaults.ImportTaxCode = 1;
            creditorDefaults.PromptPaymentDiscount = 25;

            //Disable automatic numbering on Supplier journals
            creditorDefaults.JournalNumber.Active = true;

            if (creditorDefaults.IsValid)
            {
                creditorDefaults = creditorDefaults.Save();
                MessageBox.Show($"Creditor defaults saved");
            }
            else
            {
                foreach (var rule in creditorDefaults.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
                {
                    MessageBox.Show($"{rule.PropertyName} is broken because '{rule.Description}'");
                }
            }

        }


        private Sybiz.Vision.Platform.Creditors.Transaction.IPurchaseTransactionLine CreateNewPurchaseTransactionLine(Sybiz.Vision.Platform.Creditors.Transaction.IPurchaseTransaction transaction, Sybiz.Vision.Platform.Core.Enumerations.PurchaseLineType lineType, int accountId, decimal quantity, decimal costInclusive)
        {
            var newLine = ((Sybiz.Vision.Platform.Creditors.Transaction.IPurchaseTransactionLineList)transaction.Lines).AddNew(lineType);

            newLine.Account = accountId;
            newLine.Quantity = quantity;

            //Taxcode isn't necessary to be set as it will be set with a default value based on system settings;
            //If you wish to assign you will will need to find the correct TaxCodeId (not code)
            var gstFreeTaxCode = Sybiz.Vision.Platform.Common.TaxCodeDetailInfo.GetObject("101").Id;
            if (gstFreeTaxCode != 0)
            {
                newLine.TaxCode = gstFreeTaxCode;
            }

            //Only set exclusive properties if price entry mode is exclusive; choose if you wish to set unit charges or the line total price

            if (transaction.PriceEntryMode == Sybiz.Vision.Platform.Core.Enumerations.TransactionPriceMode.Exclusive)
            {
                newLine.UnitCostExclusive = costInclusive / 1.1M / quantity;
                newLine.CostExclusive = costInclusive / 1.1M;
            }
            else
            {
                newLine.UnitCostInclusive = costInclusive / quantity;
                newLine.CostInclusive = costInclusive;
            }

            return newLine;
        }

        public void CreatePR_Click(object sender, EventArgs e)
        {
            Sybiz.Vision.Platform.Creditors.Transaction.PurchaseRequisition newPurchaseRequisition = null;

            try
            {
                newPurchaseRequisition = Sybiz.Vision.Platform.Creditors.Transaction.PurchaseRequisition.NewObject(null);
                //Transaction date will be set based on the session date by default
                newPurchaseRequisition.TransactionDate = DateTime.Today.AddDays(-1);

                //Price entry mode should be nominated based on how you wish to enter line prices
                newPurchaseRequisition.PriceEntryMode = Sybiz.Vision.Platform.Core.Enumerations.TransactionPriceMode.Inclusive;

                //Supplier not required on requisitions but is on other purchase transactions
                newPurchaseRequisition.Supplier = 1; // [choose active supplierId]
                newPurchaseRequisition.Reference = System.Convert.ToString(Interaction.InputBox("Please enter the purchase requisition reference number", "Information required").Trim());

                Sybiz.Vision.Platform.Creditors.Transaction.IPurchaseTransactionLine line;

                //add a GL line
                if (Interaction.InputBox("Add GL line (Y/N)", "Information Required", "Y").Trim() == "Y")
                {
                    line = CreateNewPurchaseTransactionLine(newPurchaseRequisition, Sybiz.Vision.Platform.Core.Enumerations.PurchaseLineType.GL, 1, 1M, 110);
                    line.Description = "Description value changed from default";
                }

                //add an IC line
                if (Interaction.InputBox("Add IC line (Y/N)", "Information Required", "Y").Trim() == "Y")
                {
                    line = CreateNewPurchaseTransactionLine(newPurchaseRequisition, Sybiz.Vision.Platform.Core.Enumerations.PurchaseLineType.IC, 1, 2M, 220);
                    line.Location = 1; //[choose valid location for this product]
                    line.UnitOfMeasure = 2; //[chose valid unit of measure for this product]
                }

                //add an JC line
                if (Interaction.InputBox("Add JC line (Y/N)", "Information Required", "N").Trim() == "Y")
                {
                    line = CreateNewPurchaseTransactionLine(newPurchaseRequisition, Sybiz.Vision.Platform.Core.Enumerations.PurchaseLineType.JC, 1, 3M, 330);
                }

                //add an JS line
                if (Interaction.InputBox("Add JS line (Y/N)", "Information Required", "N").Trim() == "Y")
                {
                    line = CreateNewPurchaseTransactionLine(newPurchaseRequisition, Sybiz.Vision.Platform.Core.Enumerations.PurchaseLineType.JS, 1, 4M, 440);
                    line.Account2 = 1; //[choose valid product]
                    line.Location = 1; //[choose valid location for this product]
                    line.UnitOfMeasure = 2; //[chose valid unit of measure for this product]
                }

                //add an SV line
                if (Interaction.InputBox("Add SV line (Y/N)", "Information Required", "N").Trim() == "Y")
                {
                    line = CreateNewPurchaseTransactionLine(newPurchaseRequisition, Sybiz.Vision.Platform.Core.Enumerations.PurchaseLineType.SV, 1, 5M, 550);
                    line.Account2 = 1; //[choose valid product]
                    line.Location = 1; //[choose valid location for this product]
                    line.UnitOfMeasure = 2; //[chose valid unit of measure for this product]
                }


                //Only process transactions that are processable, otherwise check if valid before saving.
                //GetBrokenRuleInfo navigates to all children objects and gets all broken rules (including warnings and information messages)
                if (newPurchaseRequisition.IsProcessable)
                {
                    newPurchaseRequisition = newPurchaseRequisition.Process();
                    MessageBox.Show($"Transaction succesfully created/processed for [{newPurchaseRequisition.TransactionNumber}]");
                }
                else if (newPurchaseRequisition.IsValid)
                {
                    foreach (var rule in newPurchaseRequisition.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
                    {
                        MessageBox.Show($"{rule.PropertyName} is broken on object {rule.ObjectLevel} because '{rule.Description}'");
                    }
                    newPurchaseRequisition = newPurchaseRequisition.Save();
                    MessageBox.Show($"Transaction succesfully saved for [{newPurchaseRequisition.TransactionNumber}]");
                }

            }
            catch (Exception ex)
            {
                foreach (var rule in newPurchaseRequisition.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
                {
                    MessageBox.Show($"{rule.PropertyName} is broken on object {rule.ObjectLevel} because '{rule.Description}'");
                }
                MessageBox.Show($"Error creating transaction {ex.Message}");
            }

        }

        public void ApprovePR_Click(object sender, EventArgs e)
        {
            var purchaseRequisitionNumber = Interaction.InputBox("Enter purchase requisition number to approve", "Information Required").Trim();

            try
            {
                var purchaseRequisition = Sybiz.Vision.Platform.Creditors.Transaction.PurchaseRequisition.GetObject(null, System.Convert.ToString(purchaseRequisitionNumber));

                //Can only be approved by the  user next configured to review the requisition
                purchaseRequisition.Approve();

                //Processing the requisition is required to accept the approval
                purchaseRequisition = purchaseRequisition.Process();
                MessageBox.Show($"Purchase Requisition - Approve was successful");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Purchase Requisition - Approve was unsuccessful - {ex.Message}");
            }

        }

        public void RejectPR_Click(object sender, EventArgs e)
        {

            var purchaseRequisitionNumber = Interaction.InputBox("Enter purchase requisition number to reject", "Information Required").Trim();

            try
            {
                var purchaseRequisition = Sybiz.Vision.Platform.Creditors.Transaction.PurchaseRequisition.GetObject(null, System.Convert.ToString(purchaseRequisitionNumber));

                //Can only be approved by the user next configured to review the requisition
                purchaseRequisition.Reject();

                //Processing the requisition is required to accept the rejection
                purchaseRequisition = purchaseRequisition.Process();
                MessageBox.Show($"Purchase Requisition - Approve was successful");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Purchase Requisition - Approve was unsuccessful - {ex.Message}");
            }

        }

        public void CreatePurchaseOrder_Click(object sender, EventArgs e)
        {
            var purchaseRequisitionNumber = Interaction.InputBox("Enter purchase requisition number to create order", "Information Required").Trim();
            int purchaseRequisitionId = 0;
            var defaultSupplierId = 1;

            //Retrive the sales order id from the database based on some external information
            using (var cn = new System.Data.SqlClient.SqlConnection(Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal().CompanyConnectionString))
            {
                cn.Open();
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT SalesOrderId FROM [dr].[SalesOrder] WHERE SalesOrderNumber = @OrderNumber";
                    cmd.Parameters.AddWithValue("@OrderNumber", purchaseRequisitionId);
                    purchaseRequisitionId = System.Convert.ToInt32(cmd.ExecuteScalar());
                }

            }


            try
            {
                //default supplier is to be provided when purchaseRequisition doesn't have a supplier as part of its record;
                //can be 0 if supplier always provided on purchase requisition
                var purchaseOrder = Sybiz.Vision.Platform.Creditors.Transaction.PurchaseRequisition.CreatePurchaseOrder(purchaseRequisitionId, defaultSupplierId);
                purchaseOrder.Reference = System.Convert.ToString(Interaction.InputBox("Please enter the purchase order reference number", "Information required").Trim());

                //Purchase order is only saved and not processed by the above method
                if (purchaseOrder.IsProcessable)
                {
                    purchaseOrder.Process();
                    MessageBox.Show($"Transaction succesfully created/processed for [{purchaseOrder.TransactionNumber}]");
                }
                else
                {
                    foreach (var rule in purchaseOrder.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
                    {
                        MessageBox.Show($"{rule.PropertyName} is broken on object {rule.ObjectLevel} because '{rule.Description}'");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to create purchase order from requisition - {ex.Message}");
            }

        }
    }
}
