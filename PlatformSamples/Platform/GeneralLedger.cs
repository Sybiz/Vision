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
        public void CreateGeneralLedgerJournal_Click(object sender, EventArgs e)
        {
            try
            {
                //Create a general ledger journal for a single transaction date.
                var accountCode1 = Interaction.InputBox("Enter general ledger account code", "Information Required").Trim();
                var accountCode2 = Interaction.InputBox("Enter general ledger account code", "Information Required").Trim();

                int accountId1 = Sybiz.Vision.Platform.GeneralLedger.AccountDetailInfo.GetObject(accountCode1).Id;
                int accountId2 = Sybiz.Vision.Platform.GeneralLedger.AccountDetailInfo.GetObject(accountCode2).Id;

                var generalLedger = Sybiz.Vision.Platform.GeneralLedger.Transaction.Journal.NewObject(null);
                generalLedger.TransactionDate = DateTime.Today;
                generalLedger.Description = "A new transaction created by external API";

                var newLine = generalLedger.Lines.AddNew();

                //Sum of debits and credits must balance
                newLine.Reference = "JNL01";
                newLine.Description = "First credit line";
                newLine.Account = accountId1;
                newLine.Credit = 342M;

                newLine = generalLedger.Lines.AddNew();
                newLine.Reference = "JNL01";
                newLine.Description = "First debit line";
                newLine.Account = accountId2;
                newLine.Debit = 342M;

                //Tax can be attributed to GL journal lines but is not required in most scenarios
                //newLine.TaxCode = ??
                    
                if (generalLedger.IsProcessable)
                {
                    generalLedger = generalLedger.Process();
                    MessageBox.Show($"Transaction succesfully processed for [{generalLedger.TransactionNumber}]");
                }
                else if (generalLedger.IsValid)
                {
                    generalLedger = generalLedger.Save();
                    MessageBox.Show($"Transaction succesfully saved for [{generalLedger.TransactionNumber}]");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating transaction {ex.Message}");
            }
        }

    }
}
