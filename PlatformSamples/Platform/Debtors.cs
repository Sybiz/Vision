using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace My3rdPartyApplication
{
    public partial class MainForm
    {
        private Sybiz.Vision.Platform.Debtors.Transaction.ISalesTransactionLine CreateNewSalesTransactionLine(Sybiz.Vision.Platform.Debtors.Transaction.ISalesTransaction transaction, Sybiz.Vision.Platform.Core.Enumerations.SalesLineType lineType, int accountId, decimal quantity, decimal? unitChargeExclusive)
        {
            var newLine = transaction.TransactionLines.AddNew(lineType);

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
            if (unitChargeExclusive.HasValue)
            {
                if (transaction.PriceEntryMode == Sybiz.Vision.Platform.Core.Enumerations.TransactionPriceMode.Exclusive)
                {
                    newLine.UnitChargeExclusive = unitChargeExclusive.Value;
                    newLine.ChargeExclusive = unitChargeExclusive.Value * quantity;
                }
                else
                {
                    newLine.UnitChargeInclusive = unitChargeExclusive.Value * 1.1M;
                    newLine.ChargeInclusive = unitChargeExclusive.Value * quantity * 1.1M;
                }
                //It is important to reset discount if not used by special prices and is not reset when price is changed!!
                newLine.DiscountPercentage = 0M;
            }

            return newLine;
        }

        public void CreateCustomer_Click(object sender, EventArgs e)
        {
            //Some defaults are assumed to have a valid entry, change these values if not appropriate
            var customerCode = Interaction.InputBox("Enter customer code", "Information Required").Trim();
            var newCustomer = Sybiz.Vision.Platform.Debtors.Customer.GetObject("0");

            newCustomer.Code = System.Convert.ToString(customerCode);
            newCustomer.Name = "New customer created by sample app";
            newCustomer.GroupId = 1;
            newCustomer.TradingTermsId = 1;

            //Tax status can be changed to accomodate export / non-taxable customers
            newCustomer.TaxStatus = Sybiz.Vision.Platform.Core.Enumerations.CustomerTaxStatus.Export;

            //Address information
            newCustomer.PhysicalAddress.Street = "123 Seasme Street";
            newCustomer.PhysicalAddress.State = "NY";
            newCustomer.PhysicalAddress.Suburb = "Brookyln";
            newCustomer.PhysicalAddress.PostCode = "1111";
            newCustomer.PhysicalAddress.Country = "USA";

            //Multiple delivery addresses can be created
            var deliveryAddress = newCustomer.DeliveryAddress.AddNew();
            deliveryAddress.DeliveryCode = "CORE";
            deliveryAddress.PrimaryAddress = true;
            newCustomer.PhysicalAddress.Street = "Round the back";
            newCustomer.PhysicalAddress.State = "MP";
            newCustomer.PhysicalAddress.Suburb = "Right Here";
            newCustomer.PhysicalAddress.PostCode = "9999";
            newCustomer.PhysicalAddress.Country = "Australia";

            var contact = newCustomer.Contacts.AddNew();
            contact.FirstName = "Sybiz";
            contact.LastName = "Software";
            contact.Position = "Developer";

            if (newCustomer.IsValid)
            {
                newCustomer = newCustomer.Save();
                MessageBox.Show($"CustomerId {newCustomer.Id} created successfully");
            }
            else
            {
                foreach (var rule in newCustomer.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
                {
                    MessageBox.Show($"{rule.PropertyName} Is broken on object {rule.ObjectLevel} because '{rule.Description}'");
                }
            }

        }

        public void EditCustomer_Click(object sender, EventArgs e)
        {
            var customerCode = Interaction.InputBox("Enter customer code", "Information Required").Trim();
            var customerName = Interaction.InputBox("Enter new customer name", "Information Required").Trim();
            Sybiz.Vision.Platform.Debtors.Customer exisitingCustomer = null;

            try
            {
                exisitingCustomer = Sybiz.Vision.Platform.Debtors.Customer.GetObject(customerCode);
            }
            catch (Exception)
            {
                MessageBox.Show($"Customer code: {customerCode} does not exist");
                return;
            }

            exisitingCustomer.Name = customerName;

            //Change primary delivery address details
            var deliveryAddress = exisitingCustomer.DeliveryAddress.FirstOrDefault(obj => obj.PrimaryAddress);
            if (deliveryAddress != null)
            {
                exisitingCustomer.PhysicalAddress.Street = "Changes made";
                exisitingCustomer.PhysicalAddress.State = "To this value";
                exisitingCustomer.PhysicalAddress.Suburb = "Right Here";
                exisitingCustomer.PhysicalAddress.PostCode = "9999";
                exisitingCustomer.PhysicalAddress.Country = "Australia";
            }

            //Create new contact ignoring if already exists
            var contact = exisitingCustomer.Contacts.AddNew();
            contact.FirstName = "Sybiz";
            contact.LastName = "Software";
            contact.Position = "Developer";

            if (exisitingCustomer.IsValid)
            {
                exisitingCustomer = exisitingCustomer.Save();
                MessageBox.Show($"Customer {exisitingCustomer.Name} edited successfully");
            }
            else
            {
                foreach (var rule in exisitingCustomer.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
                {
                    MessageBox.Show($"{rule.PropertyName} is broken on object {rule.ObjectLevel} because '{rule.Description}'");
                }
            }

        }

        public void CreateNewIndustry_Click(object sender, EventArgs e)
        {
            //Some defaults are assumed to have a valid entry, change these values if not appropriate
            var territoryCode = Interaction.InputBox("Enter new territory code", "Information Required").Trim();
            var newTerritory = Sybiz.Vision.Platform.Debtors.Territory.GetObject(0);

            newTerritory.Code = System.Convert.ToString(territoryCode);
            newTerritory.Description = "By default the description is too long and should be shorten to only allow 50 characters";

            if (newTerritory.IsValid)
            {
                newTerritory = newTerritory.Save();
                MessageBox.Show($"TerritoryId {newTerritory.Id} created successfully");
            }
            else
            {
                foreach (var rule in newTerritory.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
                {
                    MessageBox.Show($"{rule.PropertyName} is broken because '{rule.Description}'");
                }
            }

        }

        public void ModifyDebtorDefaults_Click(object sender, EventArgs e)
        {
            var debtorDefaults = Sybiz.Vision.Platform.Debtors.Defaults.GetObject();

            debtorDefaults.AlwaysUseSpecialPrices = true;
            debtorDefaults.SalesRepLevel = Sybiz.Vision.Platform.Core.Enumerations.TransactionLevelMethod.ByHeader;
            debtorDefaults.AnalysisCodeLevel = Sybiz.Vision.Platform.Core.Enumerations.TransactionLevelMethod.ByLine;

            //Disable automatic numbering on customer journals
            debtorDefaults.JournalNumber.Active = true;

            if (debtorDefaults.IsValid)
            {
                debtorDefaults = debtorDefaults.Save();
                MessageBox.Show($"Debtor defaults saved");
            }
            else
            {
                foreach (var rule in debtorDefaults.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
                {
                    MessageBox.Show($"{rule.PropertyName} is broken because '{rule.Description}'");
                }
            }

        }

        public void CreateSalesOrder_Click(object sender, EventArgs e)
        {

            var newSalesOrder = Sybiz.Vision.Platform.Debtors.Transaction.SalesOrder.NewObject(null);
            try
            {
                newSalesOrder.OrderCustomer = 31;

                //Delivery & Invoice customer are set based on head-office/branch office settings but these default values can be changed if necessary
                //newSalesOrder.DeliveryCustomer = 31
                //newsalesorder.InvoiceCustomer = 31

                //Transaction date will be set based on the session date by default
                newSalesOrder.TransactionDate = DateTime.Today.AddDays(-1);

                newSalesOrder.Notes = "Sales order created by sample API app";

                //Price entry mode should be nominated based on how you wish to enter line prices
                newSalesOrder.PriceEntryMode = Sybiz.Vision.Platform.Core.Enumerations.TransactionPriceMode.Exclusive;


                Sybiz.Vision.Platform.Debtors.Transaction.ISalesTransactionLine line;

                //add a GL line
                if (Interaction.InputBox("Add GL line (Y/N)", "Information Required", "Y").Trim() == "Y")
                {
                    line = CreateNewSalesTransactionLine(newSalesOrder, Sybiz.Vision.Platform.Core.Enumerations.SalesLineType.GL, 1, 1M, 100);
                    line.Description = "Description value changed from default";
                }

                //add an IC line
                if (Interaction.InputBox("Add IC line (Y/N)", "Information Required", "Y").Trim() == "Y")
                {
                    line = CreateNewSalesTransactionLine(newSalesOrder, Sybiz.Vision.Platform.Core.Enumerations.SalesLineType.IC, 1, 2M, 200);
                    line.Location = 1; //[choose valid location for this product]
                    line.UnitOfMeasure = 2; //[chose valid unit of measure for this product]
                }

                //add an SP line
                if (Interaction.InputBox("Add SP line (Y/N)", "Information Required", "N").Trim() == "Y")
                {
                    line = CreateNewSalesTransactionLine(newSalesOrder, Sybiz.Vision.Platform.Core.Enumerations.SalesLineType.SP, 1, 3M, 200);
                    line.Account2 = 10; //[choose actual product related to this structured product]
                    line.Location = 1; //[choose valid location for this product]
                    line.UnitOfMeasure = 2; //[chose valid unit of measure for this product]
                }

                //add an IR line
                if (Interaction.InputBox("Add IR line (Y/N)", "Information Required", "Y").Trim() == "Y")
                {
                    line = CreateNewSalesTransactionLine(newSalesOrder, Sybiz.Vision.Platform.Core.Enumerations.SalesLineType.IR, 1, 1M, 300);
                    line.Location = 1; //[choose valid location for this product]
                    line.UnitOfMeasure = 2; //[chose valid unit of measure for this product]
                }

                //add an JC line
                if (Interaction.InputBox("Add JC line (Y/N)", "Information Required", "N").Trim() == "Y")
                {
                    line = CreateNewSalesTransactionLine(newSalesOrder, Sybiz.Vision.Platform.Core.Enumerations.SalesLineType.JC, 1, 1M, default(System.Decimal));
                }

                //add an SV line
                if (Interaction.InputBox("Add SV line (Y/N)", "Information Required", "N").Trim() == "Y")
                {
                    line = CreateNewSalesTransactionLine(newSalesOrder, Sybiz.Vision.Platform.Core.Enumerations.SalesLineType.SV, 1, 1M, default(System.Decimal));
                }


                //Only process transactions that are processable, otherwise check if valid before saving.
                //GetBrokenRuleInfo navigates to all children objects and gets all broken rules (including warnings and information messages)
                if (newSalesOrder.IsProcessable)
                {
                    newSalesOrder = newSalesOrder.Process();
                    MessageBox.Show($"Transaction succesfully created/processed for [{newSalesOrder.TransactionNumber}]");
                }
                else if (newSalesOrder.IsValid)
                {
                    foreach (var rule in newSalesOrder.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
                    {
                        MessageBox.Show($"{rule.PropertyName} is broken on object {rule.ObjectLevel} because '{rule.Description}'");
                    }
                    newSalesOrder = newSalesOrder.Save();
                    MessageBox.Show($"Transaction succesfully saved for [{newSalesOrder.TransactionNumber}]");
                }

            }
            catch (Exception ex)
            {
                foreach (var rule in newSalesOrder.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
                {
                    MessageBox.Show($"{rule.PropertyName} is broken on object {rule.ObjectLevel} because '{rule.Description}'");
                }
                MessageBox.Show($"Error creating transaction {ex.Message}");
            }

        }

        public void CreateSalesOrderWithDeposit_Click(object sender, EventArgs e)
        {

            var newSalesOrder = Sybiz.Vision.Platform.Debtors.Transaction.SalesOrder.NewObject(null);
            try
            {
                newSalesOrder.OrderCustomer = 31;

                //Delivery & Invoice customer are set based on head-office/branch office settings but these default values can be changed if necessary
                //newSalesOrder.DeliveryCustomer = 31
                //newsalesorder.InvoiceCustomer = 31

                //Transaction date will be set based on the session date by default
                newSalesOrder.TransactionDate = DateTime.Today.AddDays(-1);

                newSalesOrder.Notes = "Sales order with deposit created by sample API app";

                //Price entry mode should be nominated based on how you wish to enter line prices
                newSalesOrder.PriceEntryMode = Sybiz.Vision.Platform.Core.Enumerations.TransactionPriceMode.Exclusive;


                Sybiz.Vision.Platform.Debtors.Transaction.ISalesTransactionLine line;

                //add an IC line
                if (Interaction.InputBox("Add IC line (Y/N)", "Information Required", "Y").Trim() == "Y")
                {
                    line = CreateNewSalesTransactionLine(newSalesOrder, Sybiz.Vision.Platform.Core.Enumerations.SalesLineType.IC, 1, 2M, 200);
                    line.Location = 1; //[choose valid location for this product]
                    line.UnitOfMeasure = 2; //[chose valid unit of measure for this product]
                }


                //add an deposit line based on inclusive order total (100% deposit) and should be a positive value
                //deposit system must be setup within Vision in debtor defaults, group postings accounts and tax code assignment
                //otherwise this line will not work!
                var newLine = newSalesOrder.Lines.AddNew(Sybiz.Vision.Platform.Core.Enumerations.SalesLineType.DP);

                if (newSalesOrder.PriceEntryMode == Sybiz.Vision.Platform.Core.Enumerations.TransactionPriceMode.Exclusive)
                {
                    newLine.ChargeExclusive = newSalesOrder.Total;
                }
                else
                {
                    newLine.ChargeInclusive = newSalesOrder.Total;
                }

                //Only process transactions that are processable, otherwise check if valid before saving.
                //GetBrokenRuleInfo navigates to all children objects and gets all broken rules (including warnings and information messages)
                if (newSalesOrder.IsProcessable)
                {
                    //Create a pay & process receipt object by using transaction as source
                    var receipt = Sybiz.Vision.Platform.Debtors.Transaction.Receipt.NewObject(newSalesOrder);

                    //Change the payment type from the default to another type based on values in [dr].[PayProcessDefault]
                    //Otherwise it will default to the first value based on sort order set within software
                    //receipt.Lines[0].PayProcessType = 2

                    newSalesOrder = (Sybiz.Vision.Platform.Debtors.Transaction.SalesOrder)newSalesOrder.PayAndProcess(receipt);
                    MessageBox.Show($"Transaction succesfully created/processed for [{newSalesOrder.TransactionNumber}]");
                }
                else if (newSalesOrder.IsValid)
                {
                    foreach (var rule in newSalesOrder.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
                    {
                        MessageBox.Show($"{rule.PropertyName} is broken on object {rule.ObjectLevel} because '{rule.Description}'");
                    }
                    newSalesOrder = newSalesOrder.Save();
                    MessageBox.Show($"Transaction succesfully saved for [{newSalesOrder.TransactionNumber}]");
                }

            }
            catch (Exception ex)
            {
                foreach (var rule in newSalesOrder.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
                {
                    MessageBox.Show($"{rule.PropertyName} is broken on object {rule.ObjectLevel} because '{rule.Description}'");
                }
                MessageBox.Show($"Error creating transaction {ex.Message}");
            }

        }

        public void CreateSalesInvoice_Click(object sender, EventArgs e)
        {
            try
            {
                //See sales order code for basics on how to create new lines, etc.
                //This example focuses on taking an outstanding order and createing an invoice and delivery for all lines

                var salesOrderNumber = Interaction.InputBox("Enter outstanding sales order number", "Information Required").Trim();
                int salesOrderId = 0;

                //Retrive the sales order id from the database based on some external information
                using (var cn = new System.Data.SqlClient.SqlConnection(Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal().CompanyConnectionString))
                {
                    cn.Open();
                    using (var cmd = cn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT SalesOrderId FROM [dr].[SalesOrder] WHERE SalesOrderNumber = @OrderNumber";
                        cmd.Parameters.AddWithValue("@OrderNumber", salesOrderNumber);
                        salesOrderId = System.Convert.ToInt32(cmd.ExecuteScalar());
                    }

                }



                var newSalesInvoice = Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice.NewObject(null);
                newSalesInvoice.AddSourceDocument(salesOrderId, Sybiz.Vision.Platform.Core.Enumerations.TransactionType.SalesOrder);
                newSalesInvoice.InvoiceAndDeliverAll();

                if (newSalesInvoice.IsProcessable)
                {
                    newSalesInvoice = newSalesInvoice.Process();
                    MessageBox.Show($"Transaction succesfully processed for [{newSalesInvoice.TransactionNumber}]");
                }
                else if (newSalesInvoice.IsValid)
                {
                    newSalesInvoice = newSalesInvoice.Save();
                    MessageBox.Show($"Transaction succesfully saved for [{newSalesInvoice.TransactionNumber}]");
                }
                else
                {
                    foreach (var rule in newSalesInvoice.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
                    {
                        MessageBox.Show($"{rule.PropertyName} is broken on object {rule.ObjectLevel} because '{rule.Description}'");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating transaction {ex.Message}");
            }


        }

        public void CreateSalesCredit_Click(object sender, EventArgs e)
        {
            try
            {
                //See sales order code for basics on how to create new lines, etc.
                //This example focuses on taking an invoice and crediting it (with automatic offsets)

                var salesInvoiceNumber = Interaction.InputBox("Enter outstanding sales invoice number", "Information Required").Trim();
                int salesInvoiceId = 0;

                //Retrive the sales order id from the database based on some external information
                using (var cn = new System.Data.SqlClient.SqlConnection(Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal().CompanyConnectionString))
                {
                    cn.Open();
                    using (var cmd = cn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT SalesInvoiceId FROM [dr].[SalesInvoice] WHERE SalesInvoiceNumber = @InvoiceNumber";
                        cmd.Parameters.AddWithValue("@InvoiceNumber", salesInvoiceNumber);
                        salesInvoiceId = System.Convert.ToInt32(cmd.ExecuteScalar());
                    }

                }



                var newSalesCredit = Sybiz.Vision.Platform.Debtors.Transaction.SalesCredit.NewObject(null);
                newSalesCredit.AddSourceDocument(salesInvoiceId, Sybiz.Vision.Platform.Core.Enumerations.TransactionType.SalesInvoice);

                //Credit and return all quantities
                newSalesCredit.CreditAndReturnAll();

                //Ensure nothing is left on back-order (completing order in most instances)
                newSalesCredit.ClearBackOrderQtys();

                if (newSalesCredit.IsProcessable)
                {
                    newSalesCredit = newSalesCredit.Process();
                    MessageBox.Show($"Transaction succesfully processed for [{newSalesCredit.TransactionNumber}]");
                }
                else if (newSalesCredit.IsValid)
                {
                    newSalesCredit = newSalesCredit.Save();
                    MessageBox.Show($"Transaction succesfully saved for [{newSalesCredit.TransactionNumber}]");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating transaction {ex.Message}");
            }
        }

        public void CreateCustomerJournal_Click(object sender, EventArgs e)
        {
            try
            {
                //Create a customer journal in transaction mode without offsetting

                var customerCode = Interaction.InputBox("Enter customer code", "Information Required").Trim();
                var accountCode = Interaction.InputBox("Enter general ledger account code", "Information Required").Trim();

                int customerId = Sybiz.Vision.Platform.Debtors.CustomerDetailInfo.GetObject(customerCode).Id;
                int accountId = Sybiz.Vision.Platform.GeneralLedger.AccountDetailInfo.GetObject(accountCode).Id;

                var customerJournal = Sybiz.Vision.Platform.Debtors.Transaction.CustomerJournal.NewObject(null);
                customerJournal.OffsetMethod = Sybiz.Vision.Platform.Core.Enumerations.OffsetMethod.ByTransaction;

                var newLine = customerJournal.Lines.AddNew();

                //Customer account primary key value
                newLine.Customer = customerId;

                //General ledger primary key value, required to offset postings if not offsetting
                //Normally to either bad debts, suspense account or discounts given
                newLine.Account = accountId;

                //Only set a single entry to a value, otherwise the other value will be overriden
                newLine.Debit = 25M;
                newLine.Credit = 0M;

                if (customerJournal.IsProcessable)
                {
                    customerJournal = customerJournal.Process();
                    MessageBox.Show($"Transaction succesfully processed for [{customerJournal.TransactionNumber}]");
                }
                else if (customerJournal.IsValid)
                {
                    customerJournal = customerJournal.Save();
                    MessageBox.Show($"Transaction succesfully saved for [{customerJournal.TransactionNumber}]");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating transaction {ex.Message}");
            }
        }

        public void PrintTransactionDocument_Click(object sender, EventArgs e)
        {
            MessageBox.Show("See code behind to understand how this feature works");
            return;

            ////ALL FILES FROM THE VISION INSTALLATION MUST BE IN THE PROGRAM BIN DIRECTORY FOR REPRINTING TO WORK
            //			var salesOrderNumber = Interaction.InputBox("Enter a sales order number", "Information Required").Trim();
            //			int salesOrderId = 0;

            ////Retrive the sales order id from the database based on some external information
            //			using (var cn = new System.Data.SqlClient.SqlConnection(Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal().CompanyConnectionString))
            //			{
            //				cn.Open();
            //				using (var cmd = cn.CreateCommand())
            //				{
            //					cmd.CommandType = CommandType.Text;
            //					cmd.CommandText = "SELECT SalesOrderId FROM [dr].[SalesOrder] WHERE SalesOrderNumber = @OrderNumber";
            //					cmd.Parameters.AddWithValue("@OrderNumber", salesOrderNumber);
            //					salesOrderId = System.Convert.ToInt32(cmd.ExecuteScalar());
            //					}
            //					
            //					}
            //					


            //					var salesOrderTransaction = Sybiz.Vision.Platform.Debtors.Transaction.SalesOrder.GetObject(null, System.Convert.ToString(salesOrderId));
            //					var companyOutput = true;
            //					var userOutput = true;

            ////This will print or preview the document as determined by the template settings; email templates will be ignored
            //					Sybiz.Vision.Platform.Core.Transaction.TransactionDocumentEngine.ProduceTransactionDocument(source: salesOrderTransaction, companyTemplates: companyOutput, userTemplates: userOutput, printHandler:
            //					Sybiz.Vision.WinUI.Utilities.FormFunctions.PrintTransaction, previewHandler:
            //					Sybiz.Vision.WinUI.Utilities.FormFunctions.PreviewTransaction, generateHandler:
            //					Sybiz.Vision.WinUI.Utilities.FormFunctions.ExportTransaction, emailHandler: null, emailPreviewHandler: null);

        }

        public void EmailTransactionDocument_Click(object sender, EventArgs e)
        {
            MessageBox.Show("See code behind to understand how this feature works");
            return;
            ////ALL FILES FROM THE VISION INSTALLATION MUST BE IN THE PROGRAM BIN DIRECTORY FOR REPRINTING TO WORK
            //					var salesOrderNumber = Interaction.InputBox("Enter a sales order number", "Information Required").Trim();

            //					var salesOrderTransaction = Sybiz.Vision.Platform.Debtors.Transaction.SalesOrder.GetObject(null, System.Convert.ToString(salesOrderNumber));
            //					var companyOutput = true;
            //					var userOutput = true;
            //					var subject = "";
            //					var body = "";

            ////Get the default subject and body template for this transaction type
            //					Sybiz.Vision.WinUI.Utilities.FormFunctions.GetEmailSubjectAndBody(salesOrderTransaction, ref subject, ref body);

            ////This will email or email preview the document as determined by the template settings; print templates will be ignored
            //					Sybiz.Vision.Platform.Core.Transaction.TransactionDocumentEngine.ProduceTransactionDocument(salesOrderTransaction, companyOutput, userOutput, null, null,
            //					Sybiz.Vision.WinUI.Utilities.FormFunctions.ExportTransaction,
            //					Sybiz.Vision.WinUI.Utilities.FormFunctions.EmailTransaction,
            //					Sybiz.Vision.WinUI.Utilities.FormFunctions.EmailPreviewTransaction,
            //					subject, body, null, null);

        }

        public void ReprintTransactionDocument_Click(object sender, EventArgs e)
        {
            MessageBox.Show("See code behind to understand how this feature works");
            return;

            ////ALL FILES FROM THE VISION INSTALLATION MUST BE IN THE PROGRAM BIN DIRECTORY FOR REPRINTING TO WORK
            
            //string salesOrderNumber = Interaction.InputBox("Enter a sales order number", "Information Required").Trim();
            //int salesOrderId = 0;
            //int templateId = 0;

            ////Retrive the sales order id from the database based on some external information
            //using (var cn = new System.Data.SqlClient.SqlConnection(Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal().CompanyConnectionString))
            //{
            //    cn.Open();
            //    using (var cmd = cn.CreateCommand())
            //    {
            //        cmd.CommandType = CommandType.Text;
            //        cmd.CommandText = "SELECT SalesOrderId FROM [dr].[SalesOrder] WHERE SalesOrderNumber = @OrderNumber";
            //        cmd.Parameters.AddWithValue("@OrderNumber", salesOrderNumber);
            //        salesOrderId = System.Convert.ToInt32(cmd.ExecuteScalar());
            //    }

            //}

            ////Retrive the first template used by transaction document
            //using (var cn = new System.Data.SqlClient.SqlConnection(Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal().CompanyConnectionString))
            //{
            //    cn.Open();
            //    using (var cmd = cn.CreateCommand())
            //    {
            //        cmd.CommandType = CommandType.Text;
            //        cmd.CommandText = "SELECT TOP 1 TransactionTemplateId FROM [cm].[TransactionTemplate] WHERE TransactionTypeId = @TransactionTypeId";
            //        cmd.Parameters.AddWithValue("@TransactionTypeId", Convert.ToInt32(Sybiz.Vision.Platform.Core.Enumerations.TransactionType.SalesOrder));
            //        templateId = System.Convert.ToInt32(cmd.ExecuteScalar());
            //    }

            //}

            ////This will return a list of PDF binary data
            //Sybiz.Vision.Platform.Core.Transaction.TransactionDocumentEngine.GenerateNonArchivedTransactionDocument(salesOrderId, Sybiz.Vision.Platform.Core.Enumerations.TransactionType.SalesOrder, Sybiz.Vision.WinUI.Utilities.FormFunctions.ExportTransaction, new List<int>() { templateId });
        }

    }
}
