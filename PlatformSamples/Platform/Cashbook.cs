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
        public void btnCreateCashbook_ByOffset_Click(object sender, EventArgs e)
        {

            var newCashbook = Sybiz.Vision.Platform.Cashbook.Transaction.Cashbook.NewObject(null);

            try
            {
                var bankAccountId = GetBankAccountId();
                var customerId = GetCustomerAccountId();
                var transactionId = GetOffTransactionToOffset();
                var offsetAmount = GetOffSetAmount();

                newCashbook.BankAccount = bankAccountId;
                newCashbook.OffsetMethod = Sybiz.Vision.Platform.Core.Enumerations.OffsetMethod.ByOffset;

                newCashbook.DefaultGroupReference = GetCashBookGroupReference();
                newCashbook.Description = GetCashBookDescription();

                Sybiz.Vision.Platform.Cashbook.Transaction.ICashbookTransactionLine line = default(Sybiz.Vision.Platform.Cashbook.Transaction.ICashbookTransactionLine);
                line = CreateNewCashbookTransactionLine(newCashbook, Sybiz.Vision.Platform.Core.Enumerations.CashbookTransactionLineType.DR, customerId, 0M, 0M, newCashbook.DefaultGroupReference, "DR OFFSET");
                line.Description = "Customer Payment of Invoice";

                //do this to load the list of offsets
                var offsets = ((Sybiz.Vision.Platform.Cashbook.Transaction.CashbookLineDR)line).Offsets;
                var offsetRecord = offsets.FirstOrDefault(ofs => ofs.TransactionId == transactionId && ofs.TransactionType == Sybiz.Vision.Platform.Core.Enumerations.TransactionType.SalesInvoice);

                if (offsetRecord != null)
                {
                    offsetRecord.Process = true; //setting this to true is optional as assigning a value will also set to true
                    offsetRecord.Amount = offsetAmount;
                }
                else
                {
                    MessageBox.Show("Invalid transaction selection - process cancelled");
                    return;
                }

                //Only process transactions that are processable, otherwise check if valid before saving.
                //GetBrokenRuleInfo navigates to all children objects and gets all broken rules (including warnings and information messages)
                if (newCashbook.IsProcessable)
                {
                    newCashbook = newCashbook.Process();
                    MessageBox.Show($"Transaction succesfully created/processed for [{newCashbook.TransactionNumber}]");
                }
                else if (newCashbook.IsValid)
                {
                    foreach (var rule in newCashbook.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
                    {
                        MessageBox.Show($"{rule.PropertyName} is broken on object {rule.ObjectLevel} because '{rule.Description}'");
                    }
                    newCashbook = newCashbook.Save();
                    MessageBox.Show($"Transaction succesfully saved for [{newCashbook.TransactionNumber}]");
                }

            }
            catch (Exception ex)
            {
                foreach (var rule in newCashbook.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
                {
                    MessageBox.Show($"{rule.PropertyName} is broken on object {rule.ObjectLevel} because '{rule.Description}'");
                }
                MessageBox.Show($"Error creating transaction {ex.Message}");
            }


            //Use linq for this instead
            //For Each offsetLine In offsets
            //  If Not offsetLine.Process AndAlso offsetLine.TransactionNumber.Equals(transactionReference) Then
            //    offsetLine.Process = True
            //    offsetLine.Amount = line.Total 'set Amount to how much you wish to pay - setting process to true assumes whole amount
            //  End If
            //Next

        }

        public void btnCreateCashbook_AutoOffset_Click(object sender, EventArgs e)
        {

            var newCashbook = Sybiz.Vision.Platform.Cashbook.Transaction.Cashbook.NewObject(null);

            try
            {

                var bankAccountId = GetBankAccountId();
                var customerId = GetCustomerAccountId();
                var offsetAmount = GetOffSetAmount();

                newCashbook.BankAccount = bankAccountId;
                newCashbook.OffsetMethod = Sybiz.Vision.Platform.Core.Enumerations.OffsetMethod.ByTransaction;
                newCashbook.DefaultGroupReference = GetCashBookGroupReference();
                newCashbook.Description = GetCashBookDescription();

                Sybiz.Vision.Platform.Cashbook.Transaction.ICashbookTransactionLine line = default(Sybiz.Vision.Platform.Cashbook.Transaction.ICashbookTransactionLine);
                line = CreateNewCashbookTransactionLine(newCashbook, Sybiz.Vision.Platform.Core.Enumerations.CashbookTransactionLineType.DR, customerId, offsetAmount, 0M, newCashbook.DefaultGroupReference, "DR OFFSET");
                line.Description = "Customer Payment of Invoice";
                Sybiz.Vision.Platform.Core.Transaction.ITransactionLineOffsets data = (Sybiz.Vision.Platform.Core.Transaction.ITransactionLineOffsets)line;
                data.AutomaticallyOffset();

                //Only process transactions that are processable, otherwise check if valid before saving.
                //GetBrokenRuleInfo navigates to all children objects and gets all broken rules (including warnings and information messages)
                if (newCashbook.IsProcessable)
                {
                    newCashbook = newCashbook.Process();
                    MessageBox.Show($"Transaction succesfully created/processed for [{newCashbook.TransactionNumber}]");
                }
                else if (newCashbook.IsValid)
                {
                    foreach (var rule in newCashbook.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
                    {
                        MessageBox.Show($"{rule.PropertyName} is broken on object {rule.ObjectLevel} because '{rule.Description}'");
                    }
                    newCashbook = newCashbook.Save();
                    MessageBox.Show($"Transaction succesfully saved for [{newCashbook.TransactionNumber}]");
                }

            }
            catch (Exception ex)
            {
                foreach (var rule in newCashbook.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
                {
                    MessageBox.Show($"{rule.PropertyName} is broken on object {rule.ObjectLevel} because '{rule.Description}'");
                }
                MessageBox.Show($"Error creating transaction {ex.Message}");
            }

        }

        public void btnCreateCashbook_ByTransaction_Click(object sender, EventArgs e)
        {

            var newCashbook = Sybiz.Vision.Platform.Cashbook.Transaction.Cashbook.NewObject(null);

            try
            {
                var bankAccountId = GetBankAccountId();

                newCashbook.BankAccount = bankAccountId;
                newCashbook.OffsetMethod = Sybiz.Vision.Platform.Core.Enumerations.OffsetMethod.ByTransaction;
                newCashbook.DefaultGroupReference = GetCashBookGroupReference();
                newCashbook.Description = GetCashBookDescription();

                Sybiz.Vision.Platform.Cashbook.Transaction.ICashbookTransactionLine line;

                //add a GL line
                if (Interaction.InputBox("Add GL line (Y/N)", "Information Required", "Y").Trim() == "Y")
                {
                    line = CreateNewCashbookTransactionLine(newCashbook, Sybiz.Vision.Platform.Core.Enumerations.CashbookTransactionLineType.GL, 1, 10M, 0M, newCashbook.DefaultGroupReference, "GL REF1");
                }

                //add an DR line
                if (Interaction.InputBox("Add DR line (Y/N)", "Information Required", "Y").Trim() == "Y")
                {
                    line = CreateNewCashbookTransactionLine(newCashbook, Sybiz.Vision.Platform.Core.Enumerations.CashbookTransactionLineType.DR, 1, 20M, 0M, newCashbook.DefaultGroupReference, "DR REF1");
                }

                //add an CR line
                if (Interaction.InputBox("Add CR line (Y/N)", "Information Required", "Y").Trim() == "Y")
                {
                    line = CreateNewCashbookTransactionLine(newCashbook, Sybiz.Vision.Platform.Core.Enumerations.CashbookTransactionLineType.CR, 1, 30M, 0M, newCashbook.DefaultGroupReference, "CR REF1");
                }

                //Only process transactions that are processable, otherwise check if valid before saving.
                //GetBrokenRuleInfo navigates to all children objects and gets all broken rules (including warnings and information messages)
                if (newCashbook.IsProcessable)
                {
                    newCashbook = newCashbook.Process();
                    MessageBox.Show($"Transaction succesfully created/processed for [{newCashbook.TransactionNumber}]");
                }
                else if (newCashbook.IsValid)
                {
                    foreach (var rule in newCashbook.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
                    {
                        MessageBox.Show($"{rule.PropertyName} is broken on object {rule.ObjectLevel} because '{rule.Description}'");
                    }
                    newCashbook = newCashbook.Save();
                    MessageBox.Show($"Transaction succesfully saved for [{newCashbook.TransactionNumber}]");
                }

            }
            catch (Exception ex)
            {
                foreach (var rule in newCashbook.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
                {
                    MessageBox.Show($"{rule.PropertyName} is broken on object {rule.ObjectLevel} because '{rule.Description}'");
                }
                MessageBox.Show($"Error creating transaction {ex.Message}");
            }

        }

        public void btnCreateCashbook_SplitLines_Click(object sender, EventArgs e)
        {

            var newCashbook = Sybiz.Vision.Platform.Cashbook.Transaction.Cashbook.NewObject(null);

            try
            {

                var bankAccountId = GetBankAccountId();
                var accountId = Sybiz.Vision.Platform.GeneralLedger.AccountLookupInfoList.GetList().FirstOrDefault(obj => obj.Active).Id;
                var supplierId = Sybiz.Vision.Platform.Creditors.SupplierLookupInfoList.GetList().FirstOrDefault(obj => obj.IsActive).Id;
                var customerId = Sybiz.Vision.Platform.Debtors.CustomerLookupInfoList.GetList().FirstOrDefault(obj => obj.IsActive).Id;

                newCashbook.BankAccount = bankAccountId;
                newCashbook.OffsetMethod = Sybiz.Vision.Platform.Core.Enumerations.OffsetMethod.ByTransaction;
                newCashbook.DefaultGroupReference = GetCashBookGroupReference();
                newCashbook.Description = GetCashBookDescription();

                Sybiz.Vision.Platform.Cashbook.Transaction.CashbookLine line;
                line = (Sybiz.Vision.Platform.Cashbook.Transaction.CashbookLine)CreateNewCashbookTransactionLine(newCashbook, Sybiz.Vision.Platform.Core.Enumerations.CashbookTransactionLineType.SPLIT, 0, 0M, 0M, newCashbook.DefaultGroupReference, "SPLIT REF1");
                line.SplitLines.Clear(); //This is important as new split lines add a default line which will be invalid


                //add a GL line
                if (Interaction.InputBox("Add GL line (Y/N)", "Information Required", "Y").Trim() == "Y")
                {
                    line.SplitLines.Add(CreateNewCashbookTransactionLine(newCashbook, Sybiz.Vision.Platform.Core.Enumerations.CashbookTransactionLineType.GL, accountId, 100M, 0M, newCashbook.DefaultGroupReference, "GL SPLIT REF 1", line));
                }

                //add an DR line
                if (Interaction.InputBox("Add DR line (Y/N)", "Information Required", "Y").Trim() == "Y")
                {
                    line.SplitLines.Add(CreateNewCashbookTransactionLine(newCashbook, Sybiz.Vision.Platform.Core.Enumerations.CashbookTransactionLineType.DR, customerId, 200M, 0M, newCashbook.DefaultGroupReference, "DR SPLIT REF 1", line));
                }

                //add an CR line
                if (Interaction.InputBox("Add CR line (Y/N)", "Information Required", "Y").Trim() == "Y")
                {
                    line.SplitLines.Add(CreateNewCashbookTransactionLine(newCashbook, Sybiz.Vision.Platform.Core.Enumerations.CashbookTransactionLineType.CR, supplierId, 0M, 300M, newCashbook.DefaultGroupReference, "CR SPLIT REF 1", line));
                }

                //Only process transactions that are processable, otherwise check if valid before saving.
                //GetBrokenRuleInfo navigates to all children objects and gets all broken rules (including warnings and information messages)
                if (newCashbook.IsProcessable)
                {
                    newCashbook = newCashbook.Process();
                    MessageBox.Show($"Transaction succesfully created/processed for [{newCashbook.TransactionNumber}]");
                }
                else if (newCashbook.IsValid)
                {
                    foreach (var rule in newCashbook.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
                    {
                        MessageBox.Show($"{rule.PropertyName} is broken on object {rule.ObjectLevel} because '{rule.Description}'");
                    }
                    newCashbook = newCashbook.Save();
                    MessageBox.Show($"Transaction succesfully saved for [{newCashbook.TransactionNumber}]");
                }

            }
            catch (Exception ex)
            {
                foreach (var rule in newCashbook.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
                {
                    MessageBox.Show($"{rule.PropertyName} is broken on object {rule.ObjectLevel} because '{rule.Description}'");
                }
                MessageBox.Show($"Error creating transaction {ex.Message}");
            }

        }

        private Sybiz.Vision.Platform.Cashbook.Transaction.ICashbookTransactionLine CreateNewCashbookTransactionLine(Sybiz.Vision.Platform.Cashbook.Transaction.Cashbook transaction, Sybiz.Vision.Platform.Core.Enumerations.CashbookTransactionLineType lineType, int accountId, decimal deposit, decimal payment, string groupReference, string reference, Sybiz.Vision.Platform.Cashbook.Transaction.CashbookLine splitLine = null)
        {

            var newLine = transaction.Lines.AddNew(lineType);

            if (splitLine != null)
            {
                //Split lines items cannot be linked to main list but above method is easiest way to create
                transaction.Lines.Remove(newLine);
            }

            if (!(lineType == Sybiz.Vision.Platform.Core.Enumerations.CashbookTransactionLineType.SPLIT))
            {
                newLine.Account = accountId;
                newLine.GroupReference = groupReference;
                newLine.Reference = reference;

                if (deposit != 0M)
                {
                    newLine.Description = "Deposit";
                    newLine.Deposit = deposit;
                    newLine.Payment = 0M;
                }
                else if (payment != 0M)
                {
                    newLine.Description = "Payment";
                    newLine.Payment = payment;
                    newLine.Deposit = 0M;
                }

            }

            if (lineType == Sybiz.Vision.Platform.Core.Enumerations.CashbookTransactionLineType.GL)
            {
                var account = Sybiz.Vision.Platform.GeneralLedger.AccountDetailInfo.GetObject(accountId);
                newLine.Payee = account.Description;
                newLine.Description = "GL Line Description";

                //Taxcode isn't necessary to be set as it will be set with a default value based on system settings;
                //If you wish to assign you will will need to find the correct TaxCodeId (not code)
                var gstFreeTaxCode = Sybiz.Vision.Platform.Common.TaxCodeDetailInfo.GetObject("101").Id;
                if (gstFreeTaxCode != 0)
                {
                    newLine.TaxCode = gstFreeTaxCode;
                }
            }

            if (lineType == Sybiz.Vision.Platform.Core.Enumerations.CashbookTransactionLineType.DR)
            {
                var account = Sybiz.Vision.Platform.Debtors.CustomerDetailInfo.GetObject(accountId);
                newLine.Payee = account.Name;
                newLine.Description = "DR Line Description";
            }

            if (lineType == Sybiz.Vision.Platform.Core.Enumerations.CashbookTransactionLineType.CR)
            {
                var account = Sybiz.Vision.Platform.Creditors.SupplierDetailInfo.GetObject(accountId);
                newLine.Payee = account.Name;
                newLine.Description = "CR Line Description";
            }

            return newLine;

        }

        #region  Cashbook Helpers

        public int GetBankAccountId()
        {
            var BankAccountGLNumber = Interaction.InputBox("Please enter the BANK ACCOUNT GL ACCOUNT NUMBER", "Information required");
            var BankAccount = Sybiz.Vision.Platform.GeneralLedger.AccountDetailInfo.GetObject(BankAccountGLNumber);

            if (BankAccount.Id < 1 || !BankAccount.AccountType.IsBank)
            {
                MessageBox.Show($"Invalid GL Account number");
                return 0;
            }
            return BankAccount.Id;
        }

        public int GetCustomerAccountId()
        {
            var customerCode = Interaction.InputBox("Please enter the customer code (that has a transaction to offset)", "Information required");
            int customerId = Sybiz.Vision.Platform.Debtors.CustomerDetailInfo.GetObject(customerCode).Id;

            if (customerId == 0)
            {
                MessageBox.Show($"Invalid Customer number");
                return 0;
            }
            return customerId;
        }

        public decimal GetOffSetAmount()
        {
            decimal offsetAmount = decimal.Parse(Interaction.InputBox("Please enter the amount you wish to pay off/offset", "Information required"));
            if (offsetAmount == 0M)
            {
                MessageBox.Show($"Invalid offset amount");
                return 0M;
            }
            return offsetAmount;
        }

        public string GetCashBookGroupReference()
        {
            return Interaction.InputBox("Please enter the cashbook group reference", "Information required", "GREF1");
        }

        public string GetCashBookDescription()
        {
            return Interaction.InputBox("Please enter the cashbook description", "Information required", "CB Description");
        }

        public int GetOffTransactionToOffset()
        {
            //choose transaction to offset
            //ask for invoice reference

            int transactionId = 0;
            var transactionReference = Interaction.InputBox("Please enter the Customer Invoice reference to be offset", "Information required", "SINV000205");
            if (string.IsNullOrWhiteSpace(transactionReference))
            {
                MessageBox.Show("Invalid transaction reference - process cancelled");
                return 0;
            }

            //Run a SQL query on PostDR to find relevant OUTSTANDING SALES INVOICE record
            using (var cn = new System.Data.SqlClient.SqlConnection(Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal().CompanyConnectionString))
            {
                cn.Open();
                using (var cmd = cn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT transactionId FROM [trn].[PostDR] WHERE Reference = @transactionReference AND TransactionTypeId = 103 AND Outstanding <> 0";
                    cmd.Parameters.AddWithValue("@transactionReference", transactionReference);
                    transactionId = System.Convert.ToInt32(cmd.ExecuteScalar());
                }

            }

            return transactionId;

        }

        #endregion
    }
}
