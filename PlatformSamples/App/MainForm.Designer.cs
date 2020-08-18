// VBConversions Note: VB project level imports
using System.Collections.Generic;
using System;
using System.Linq;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using System.Xml.Linq;
using Microsoft.VisualBasic;
using System.Collections;
using System.Windows.Forms;
// End of VB project level imports

using My3rdPartyApplication;

namespace My3rdPartyApplication
{
	[global::Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]public 
	partial class MainForm : System.Windows.Forms.Form
	{
		[STAThread]
		static void Main()
		{
			System.Windows.Forms.Application.Run(new MainForm());
		}
		
		//Form overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && components != null)
				{
					components.Dispose();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}
		
		//Required by the Windows Form Designer
		private System.ComponentModel.Container components = null;
		
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]private void InitializeComponent()
		{
			this.OKButton = new System.Windows.Forms.Button();
			this.TabJobs = new System.Windows.Forms.TabPage();
			this.CreateJobInvoice = new System.Windows.Forms.Button();
			this.CreatePreInvoice = new System.Windows.Forms.Button();
			this.CreateJobCost = new System.Windows.Forms.Button();
			this.CreateJob = new System.Windows.Forms.Button();
			this.TabService = new System.Windows.Forms.TabPage();
			this.CreateServiceAction = new System.Windows.Forms.Button();
			this.CreateServiceRequest = new System.Windows.Forms.Button();
			this.CreateServiceItem = new System.Windows.Forms.Button();
			this.TabCRM = new System.Windows.Forms.TabPage();
			this.btnLinkJobActivity = new System.Windows.Forms.Button();
			this.btnLinkCaseFileToRemark = new System.Windows.Forms.Button();
			this.btnCreateActivity = new System.Windows.Forms.Button();
			this.btnCreateCaseFile = new System.Windows.Forms.Button();
			this.TabAdministration = new System.Windows.Forms.TabPage();
			this.btnCreateUser = new System.Windows.Forms.Button();
			this.btnCreateRole = new System.Windows.Forms.Button();
			this.TabCashbook = new System.Windows.Forms.TabPage();
			this.btnCreateCashbook_SplitLines = new System.Windows.Forms.Button();
			this.btnCreateCashbook_ByTransaction = new System.Windows.Forms.Button();
			this.btnCreateCashbook_AutoOffset = new System.Windows.Forms.Button();
			this.btnCreateCashbook_ByOffset = new System.Windows.Forms.Button();
			this.TabManufacturing = new System.Windows.Forms.TabPage();
			this.EditAssembly = new System.Windows.Forms.Button();
			this.CreateAssembly = new System.Windows.Forms.Button();
			this.CreateManufactureIssue = new System.Windows.Forms.Button();
			this.CreateMO = new System.Windows.Forms.Button();
			this.TabInventory = new System.Windows.Forms.TabPage();
			this.CopyProduct = new System.Windows.Forms.Button();
			this.CreateProduct = new System.Windows.Forms.Button();
			this.TabCreditors = new System.Windows.Forms.TabPage();
			this.CreateSupplierJournal = new System.Windows.Forms.Button();
			this.EditSupplier = new System.Windows.Forms.Button();
			this.ModifyCreditorDefaults = new System.Windows.Forms.Button();
			this.CreateSupplier = new System.Windows.Forms.Button();
			this.CreatePurchaseOrder = new System.Windows.Forms.Button();
			this.RejectPR = new System.Windows.Forms.Button();
			this.CreatePR = new System.Windows.Forms.Button();
			this.ApprovePR = new System.Windows.Forms.Button();
			this.TabDebtors = new System.Windows.Forms.TabPage();
			this.CreateCustomerJournal = new System.Windows.Forms.Button();
			this.EmailTransactionDocument = new System.Windows.Forms.Button();
			this.PrintTransactionDocument = new System.Windows.Forms.Button();
			this.CreateNewIndustry = new System.Windows.Forms.Button();
			this.CreateSalesInvoice = new System.Windows.Forms.Button();
			this.EditCustomer = new System.Windows.Forms.Button();
			this.CreateSalesCredit = new System.Windows.Forms.Button();
			this.ModifyDebtorDefaults = new System.Windows.Forms.Button();
			this.CreateSalesOrder = new System.Windows.Forms.Button();
			this.CreateCustomer = new System.Windows.Forms.Button();
			this.TabCommon = new System.Windows.Forms.TabPage();
			this.RegisterThirdPartyTool = new System.Windows.Forms.Button();
			this.ShowNotes = new System.Windows.Forms.Button();
			this.ShowReminders = new System.Windows.Forms.Button();
			this.ShowTransactionForm = new System.Windows.Forms.Button();
			this.ShowMaintenanceForm = new System.Windows.Forms.Button();
			this.ShowEnquiryForm = new System.Windows.Forms.Button();
			this.RunScalarCommand = new System.Windows.Forms.Button();
			this.ImportantInformationButton = new System.Windows.Forms.Button();
			this.GetCompanySystemDefaults = new System.Windows.Forms.Button();
			this.GetLoggedInUserInfo = new System.Windows.Forms.Button();
			this.GetCompanyConnectionString = new System.Windows.Forms.Button();
			this.TabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.CreateGeneralLedgerJournal = new System.Windows.Forms.Button();
			this.TabJobs.SuspendLayout();
			this.TabService.SuspendLayout();
			this.TabCRM.SuspendLayout();
			this.TabAdministration.SuspendLayout();
			this.TabCashbook.SuspendLayout();
			this.TabManufacturing.SuspendLayout();
			this.TabInventory.SuspendLayout();
			this.TabCreditors.SuspendLayout();
			this.TabDebtors.SuspendLayout();
			this.TabCommon.SuspendLayout();
			this.TabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.SuspendLayout();
			// 
			// OKButton
			// 
			this.OKButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.OKButton.Location = new System.Drawing.Point(527, 250);
			this.OKButton.Name = "OKButton";
			this.OKButton.Size = new System.Drawing.Size(99, 23);
			this.OKButton.TabIndex = 4;
			this.OKButton.Text = "&OK";
			this.OKButton.UseVisualStyleBackColor = true;
			this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
			// 
			// TabJobs
			// 
			this.TabJobs.Controls.Add(this.CreateJobInvoice);
			this.TabJobs.Controls.Add(this.CreatePreInvoice);
			this.TabJobs.Controls.Add(this.CreateJobCost);
			this.TabJobs.Controls.Add(this.CreateJob);
			this.TabJobs.Location = new System.Drawing.Point(4, 22);
			this.TabJobs.Name = "TabJobs";
			this.TabJobs.Padding = new System.Windows.Forms.Padding(3);
			this.TabJobs.Size = new System.Drawing.Size(611, 206);
			this.TabJobs.TabIndex = 11;
			this.TabJobs.Text = "Jobs";
			// 
			// CreateJobInvoice
			// 
			this.CreateJobInvoice.Location = new System.Drawing.Point(238, 52);
			this.CreateJobInvoice.Name = "CreateJobInvoice";
			this.CreateJobInvoice.Size = new System.Drawing.Size(178, 23);
			this.CreateJobInvoice.TabIndex = 7;
			this.CreateJobInvoice.Text = "Create Job Invoice";
			this.CreateJobInvoice.UseVisualStyleBackColor = true;
			this.CreateJobInvoice.Click += new System.EventHandler(this.CreateJobInvoice_Click);
			// 
			// CreatePreInvoice
			// 
			this.CreatePreInvoice.Location = new System.Drawing.Point(238, 23);
			this.CreatePreInvoice.Name = "CreatePreInvoice";
			this.CreatePreInvoice.Size = new System.Drawing.Size(178, 23);
			this.CreatePreInvoice.TabIndex = 6;
			this.CreatePreInvoice.Text = "Create PreInvoice";
			this.CreatePreInvoice.UseVisualStyleBackColor = true;
			this.CreatePreInvoice.Click += new System.EventHandler(this.CreatePreInvoice_Click);
			// 
			// CreateJobCost
			// 
			this.CreateJobCost.Location = new System.Drawing.Point(18, 52);
			this.CreateJobCost.Name = "CreateJobCost";
			this.CreateJobCost.Size = new System.Drawing.Size(178, 23);
			this.CreateJobCost.TabIndex = 5;
			this.CreateJobCost.Text = "Create Job Cost";
			this.CreateJobCost.UseVisualStyleBackColor = true;
			this.CreateJobCost.Click += new System.EventHandler(this.CreateJobCost_Click);
			// 
			// CreateJob
			// 
			this.CreateJob.Location = new System.Drawing.Point(18, 23);
			this.CreateJob.Name = "CreateJob";
			this.CreateJob.Size = new System.Drawing.Size(178, 23);
			this.CreateJob.TabIndex = 4;
			this.CreateJob.Text = "Create Job";
			this.CreateJob.UseVisualStyleBackColor = true;
			this.CreateJob.Click += new System.EventHandler(this.CreateJob_Click);
			// 
			// TabService
			// 
			this.TabService.Controls.Add(this.CreateServiceAction);
			this.TabService.Controls.Add(this.CreateServiceRequest);
			this.TabService.Controls.Add(this.CreateServiceItem);
			this.TabService.Location = new System.Drawing.Point(4, 22);
			this.TabService.Name = "TabService";
			this.TabService.Padding = new System.Windows.Forms.Padding(3);
			this.TabService.Size = new System.Drawing.Size(611, 206);
			this.TabService.TabIndex = 10;
			this.TabService.Text = "Service";
			// 
			// CreateServiceAction
			// 
			this.CreateServiceAction.Location = new System.Drawing.Point(6, 76);
			this.CreateServiceAction.Name = "CreateServiceAction";
			this.CreateServiceAction.Size = new System.Drawing.Size(178, 23);
			this.CreateServiceAction.TabIndex = 5;
			this.CreateServiceAction.Text = "Create Service Action";
			this.CreateServiceAction.UseVisualStyleBackColor = true;
			this.CreateServiceAction.Click += new System.EventHandler(this.CreateServiceAction_Click);
			// 
			// CreateServiceRequest
			// 
			this.CreateServiceRequest.Location = new System.Drawing.Point(6, 47);
			this.CreateServiceRequest.Name = "CreateServiceRequest";
			this.CreateServiceRequest.Size = new System.Drawing.Size(178, 23);
			this.CreateServiceRequest.TabIndex = 4;
			this.CreateServiceRequest.Text = "Create Service Request";
			this.CreateServiceRequest.UseVisualStyleBackColor = true;
			this.CreateServiceRequest.Click += new System.EventHandler(this.CreateServiceRequest_Click);
			// 
			// CreateServiceItem
			// 
			this.CreateServiceItem.Location = new System.Drawing.Point(6, 18);
			this.CreateServiceItem.Name = "CreateServiceItem";
			this.CreateServiceItem.Size = new System.Drawing.Size(178, 23);
			this.CreateServiceItem.TabIndex = 3;
			this.CreateServiceItem.Text = "Create Service Item";
			this.CreateServiceItem.UseVisualStyleBackColor = true;
			this.CreateServiceItem.Click += new System.EventHandler(this.CreateServiceItem_Click);
			// 
			// TabCRM
			// 
			this.TabCRM.Controls.Add(this.btnLinkJobActivity);
			this.TabCRM.Controls.Add(this.btnLinkCaseFileToRemark);
			this.TabCRM.Controls.Add(this.btnCreateActivity);
			this.TabCRM.Controls.Add(this.btnCreateCaseFile);
			this.TabCRM.Location = new System.Drawing.Point(4, 22);
			this.TabCRM.Name = "TabCRM";
			this.TabCRM.Padding = new System.Windows.Forms.Padding(3);
			this.TabCRM.Size = new System.Drawing.Size(611, 206);
			this.TabCRM.TabIndex = 9;
			this.TabCRM.Text = "CRM";
			// 
			// btnLinkJobActivity
			// 
			this.btnLinkJobActivity.Location = new System.Drawing.Point(245, 45);
			this.btnLinkJobActivity.Name = "btnLinkJobActivity";
			this.btnLinkJobActivity.Size = new System.Drawing.Size(178, 23);
			this.btnLinkJobActivity.TabIndex = 10;
			this.btnLinkJobActivity.Text = "Link Activity to Job";
			this.btnLinkJobActivity.UseVisualStyleBackColor = true;
			this.btnLinkJobActivity.Click += new System.EventHandler(this.btnLinkJobActivity_Click);
			// 
			// btnLinkCaseFileToRemark
			// 
			this.btnLinkCaseFileToRemark.Location = new System.Drawing.Point(245, 16);
			this.btnLinkCaseFileToRemark.Name = "btnLinkCaseFileToRemark";
			this.btnLinkCaseFileToRemark.Size = new System.Drawing.Size(178, 23);
			this.btnLinkCaseFileToRemark.TabIndex = 9;
			this.btnLinkCaseFileToRemark.Text = "Link CaseFile to Remark";
			this.btnLinkCaseFileToRemark.UseVisualStyleBackColor = true;
			this.btnLinkCaseFileToRemark.Click += new System.EventHandler(this.btnLinkCaseFileToRemark_Click);
			// 
			// btnCreateActivity
			// 
			this.btnCreateActivity.Location = new System.Drawing.Point(17, 45);
			this.btnCreateActivity.Name = "btnCreateActivity";
			this.btnCreateActivity.Size = new System.Drawing.Size(178, 23);
			this.btnCreateActivity.TabIndex = 3;
			this.btnCreateActivity.Text = "Create Activity";
			this.btnCreateActivity.UseVisualStyleBackColor = true;
			this.btnCreateActivity.Click += new System.EventHandler(this.btnCreateActivity_Click);
			// 
			// btnCreateCaseFile
			// 
			this.btnCreateCaseFile.Location = new System.Drawing.Point(17, 16);
			this.btnCreateCaseFile.Name = "btnCreateCaseFile";
			this.btnCreateCaseFile.Size = new System.Drawing.Size(178, 23);
			this.btnCreateCaseFile.TabIndex = 2;
			this.btnCreateCaseFile.Text = "Create CaseFile";
			this.btnCreateCaseFile.UseVisualStyleBackColor = true;
			this.btnCreateCaseFile.Click += new System.EventHandler(this.btnCreateCaseFile_Click);
			// 
			// TabAdministration
			// 
			this.TabAdministration.Controls.Add(this.btnCreateUser);
			this.TabAdministration.Controls.Add(this.btnCreateRole);
			this.TabAdministration.Location = new System.Drawing.Point(4, 22);
			this.TabAdministration.Name = "TabAdministration";
			this.TabAdministration.Padding = new System.Windows.Forms.Padding(3);
			this.TabAdministration.Size = new System.Drawing.Size(611, 206);
			this.TabAdministration.TabIndex = 8;
			this.TabAdministration.Text = "Administration";
			// 
			// btnCreateUser
			// 
			this.btnCreateUser.Location = new System.Drawing.Point(19, 47);
			this.btnCreateUser.Name = "btnCreateUser";
			this.btnCreateUser.Size = new System.Drawing.Size(124, 23);
			this.btnCreateUser.TabIndex = 4;
			this.btnCreateUser.Text = "Create User";
			this.btnCreateUser.UseVisualStyleBackColor = true;
			// 
			// btnCreateRole
			// 
			this.btnCreateRole.Location = new System.Drawing.Point(19, 18);
			this.btnCreateRole.Name = "btnCreateRole";
			this.btnCreateRole.Size = new System.Drawing.Size(124, 23);
			this.btnCreateRole.TabIndex = 3;
			this.btnCreateRole.Text = "Create Role";
			this.btnCreateRole.UseVisualStyleBackColor = true;
			// 
			// TabCashbook
			// 
			this.TabCashbook.Controls.Add(this.btnCreateCashbook_SplitLines);
			this.TabCashbook.Controls.Add(this.btnCreateCashbook_ByTransaction);
			this.TabCashbook.Controls.Add(this.btnCreateCashbook_AutoOffset);
			this.TabCashbook.Controls.Add(this.btnCreateCashbook_ByOffset);
			this.TabCashbook.Location = new System.Drawing.Point(4, 22);
			this.TabCashbook.Name = "TabCashbook";
			this.TabCashbook.Padding = new System.Windows.Forms.Padding(3);
			this.TabCashbook.Size = new System.Drawing.Size(611, 206);
			this.TabCashbook.TabIndex = 7;
			this.TabCashbook.Text = "Cashbook";
			// 
			// btnCreateCashbook_SplitLines
			// 
			this.btnCreateCashbook_SplitLines.Location = new System.Drawing.Point(43, 64);
			this.btnCreateCashbook_SplitLines.Name = "btnCreateCashbook_SplitLines";
			this.btnCreateCashbook_SplitLines.Size = new System.Drawing.Size(178, 23);
			this.btnCreateCashbook_SplitLines.TabIndex = 8;
			this.btnCreateCashbook_SplitLines.Text = "Create Cashbook (Split Line)";
			this.btnCreateCashbook_SplitLines.UseVisualStyleBackColor = true;
			this.btnCreateCashbook_SplitLines.Click += new System.EventHandler(this.btnCreateCashbook_SplitLines_Click);
			// 
			// btnCreateCashbook_ByTransaction
			// 
			this.btnCreateCashbook_ByTransaction.Location = new System.Drawing.Point(341, 35);
			this.btnCreateCashbook_ByTransaction.Name = "btnCreateCashbook_ByTransaction";
			this.btnCreateCashbook_ByTransaction.Size = new System.Drawing.Size(178, 23);
			this.btnCreateCashbook_ByTransaction.TabIndex = 7;
			this.btnCreateCashbook_ByTransaction.Text = "Create Cashbook (Transaction)";
			this.btnCreateCashbook_ByTransaction.UseVisualStyleBackColor = true;
			this.btnCreateCashbook_ByTransaction.Click += new System.EventHandler(this.btnCreateCashbook_ByTransaction_Click);
			// 
			// btnCreateCashbook_AutoOffset
			// 
			this.btnCreateCashbook_AutoOffset.Location = new System.Drawing.Point(341, 64);
			this.btnCreateCashbook_AutoOffset.Name = "btnCreateCashbook_AutoOffset";
			this.btnCreateCashbook_AutoOffset.Size = new System.Drawing.Size(178, 23);
			this.btnCreateCashbook_AutoOffset.TabIndex = 6;
			this.btnCreateCashbook_AutoOffset.Text = "Create Cashbook (Auto-Offset)";
			this.btnCreateCashbook_AutoOffset.UseVisualStyleBackColor = true;
			this.btnCreateCashbook_AutoOffset.Click += new System.EventHandler(this.btnCreateCashbook_AutoOffset_Click);
			// 
			// btnCreateCashbook_ByOffset
			// 
			this.btnCreateCashbook_ByOffset.Location = new System.Drawing.Point(43, 35);
			this.btnCreateCashbook_ByOffset.Name = "btnCreateCashbook_ByOffset";
			this.btnCreateCashbook_ByOffset.Size = new System.Drawing.Size(178, 23);
			this.btnCreateCashbook_ByOffset.TabIndex = 5;
			this.btnCreateCashbook_ByOffset.Text = "Create Cashbook (Offset)";
			this.btnCreateCashbook_ByOffset.UseVisualStyleBackColor = true;
			this.btnCreateCashbook_ByOffset.Click += new System.EventHandler(this.btnCreateCashbook_ByOffset_Click);
			// 
			// TabManufacturing
			// 
			this.TabManufacturing.Controls.Add(this.EditAssembly);
			this.TabManufacturing.Controls.Add(this.CreateAssembly);
			this.TabManufacturing.Controls.Add(this.CreateManufactureIssue);
			this.TabManufacturing.Controls.Add(this.CreateMO);
			this.TabManufacturing.Location = new System.Drawing.Point(4, 22);
			this.TabManufacturing.Name = "TabManufacturing";
			this.TabManufacturing.Padding = new System.Windows.Forms.Padding(3);
			this.TabManufacturing.Size = new System.Drawing.Size(611, 206);
			this.TabManufacturing.TabIndex = 6;
			this.TabManufacturing.Text = "Manufacturing";
			// 
			// EditAssembly
			// 
			this.EditAssembly.Location = new System.Drawing.Point(17, 45);
			this.EditAssembly.Name = "EditAssembly";
			this.EditAssembly.Size = new System.Drawing.Size(178, 23);
			this.EditAssembly.TabIndex = 8;
			this.EditAssembly.Text = "Edit Assembly";
			this.EditAssembly.UseVisualStyleBackColor = true;
			this.EditAssembly.Click += new System.EventHandler(this.EditAssembly_Click);
			// 
			// CreateAssembly
			// 
			this.CreateAssembly.Location = new System.Drawing.Point(17, 16);
			this.CreateAssembly.Name = "CreateAssembly";
			this.CreateAssembly.Size = new System.Drawing.Size(178, 23);
			this.CreateAssembly.TabIndex = 7;
			this.CreateAssembly.Text = "Create Assembly";
			this.CreateAssembly.UseVisualStyleBackColor = true;
			this.CreateAssembly.Click += new System.EventHandler(this.CreateAssembly_Click);
			// 
			// CreateManufactureIssue
			// 
			this.CreateManufactureIssue.Location = new System.Drawing.Point(210, 45);
			this.CreateManufactureIssue.Name = "CreateManufactureIssue";
			this.CreateManufactureIssue.Size = new System.Drawing.Size(178, 23);
			this.CreateManufactureIssue.TabIndex = 6;
			this.CreateManufactureIssue.Text = "Create Manufacture Issue";
			this.CreateManufactureIssue.UseVisualStyleBackColor = true;
			this.CreateManufactureIssue.Click += new System.EventHandler(this.CreateMIFromMO_Click);
			// 
			// CreateMO
			// 
			this.CreateMO.Location = new System.Drawing.Point(210, 16);
			this.CreateMO.Name = "CreateMO";
			this.CreateMO.Size = new System.Drawing.Size(178, 23);
			this.CreateMO.TabIndex = 5;
			this.CreateMO.Text = "Create Manufacture Order";
			this.CreateMO.UseVisualStyleBackColor = true;
			this.CreateMO.Click += new System.EventHandler(this.CreateMO_Click);
			// 
			// TabInventory
			// 
			this.TabInventory.Controls.Add(this.CopyProduct);
			this.TabInventory.Controls.Add(this.CreateProduct);
			this.TabInventory.Location = new System.Drawing.Point(4, 22);
			this.TabInventory.Name = "TabInventory";
			this.TabInventory.Padding = new System.Windows.Forms.Padding(3);
			this.TabInventory.Size = new System.Drawing.Size(611, 206);
			this.TabInventory.TabIndex = 5;
			this.TabInventory.Text = "Inventory";
			// 
			// CopyProduct
			// 
			this.CopyProduct.Location = new System.Drawing.Point(15, 49);
			this.CopyProduct.Name = "CopyProduct";
			this.CopyProduct.Size = new System.Drawing.Size(178, 23);
			this.CopyProduct.TabIndex = 7;
			this.CopyProduct.Text = "Copy Product";
			this.CopyProduct.UseVisualStyleBackColor = true;
			this.CopyProduct.Click += new System.EventHandler(this.CopyProduct_Click);
			// 
			// CreateProduct
			// 
			this.CreateProduct.Location = new System.Drawing.Point(15, 20);
			this.CreateProduct.Name = "CreateProduct";
			this.CreateProduct.Size = new System.Drawing.Size(178, 23);
			this.CreateProduct.TabIndex = 6;
			this.CreateProduct.Text = "Create Product";
			this.CreateProduct.UseVisualStyleBackColor = true;
			this.CreateProduct.Click += new System.EventHandler(this.CreateProduct_Click);
			// 
			// TabCreditors
			// 
			this.TabCreditors.Controls.Add(this.CreateSupplierJournal);
			this.TabCreditors.Controls.Add(this.EditSupplier);
			this.TabCreditors.Controls.Add(this.ModifyCreditorDefaults);
			this.TabCreditors.Controls.Add(this.CreateSupplier);
			this.TabCreditors.Controls.Add(this.CreatePurchaseOrder);
			this.TabCreditors.Controls.Add(this.RejectPR);
			this.TabCreditors.Controls.Add(this.CreatePR);
			this.TabCreditors.Controls.Add(this.ApprovePR);
			this.TabCreditors.Location = new System.Drawing.Point(4, 22);
			this.TabCreditors.Name = "TabCreditors";
			this.TabCreditors.Padding = new System.Windows.Forms.Padding(3);
			this.TabCreditors.Size = new System.Drawing.Size(611, 206);
			this.TabCreditors.TabIndex = 3;
			this.TabCreditors.Text = "Creditors";
			// 
			// CreateSupplierJournal
			// 
			this.CreateSupplierJournal.Location = new System.Drawing.Point(415, 6);
			this.CreateSupplierJournal.Name = "CreateSupplierJournal";
			this.CreateSupplierJournal.Size = new System.Drawing.Size(178, 23);
			this.CreateSupplierJournal.TabIndex = 22;
			this.CreateSupplierJournal.Text = "Create Supplier Journal";
			this.CreateSupplierJournal.UseVisualStyleBackColor = true;
			this.CreateSupplierJournal.Click += new System.EventHandler(this.CreateSupplierJournal_Click);
			// 
			// EditSupplier
			// 
			this.EditSupplier.Location = new System.Drawing.Point(6, 35);
			this.EditSupplier.Name = "EditSupplier";
			this.EditSupplier.Size = new System.Drawing.Size(178, 23);
			this.EditSupplier.TabIndex = 10;
			this.EditSupplier.Text = "Edit Supplier";
			this.EditSupplier.UseVisualStyleBackColor = true;
			this.EditSupplier.Click += new System.EventHandler(this.EditSupplier_Click);
			// 
			// ModifyCreditorDefaults
			// 
			this.ModifyCreditorDefaults.Location = new System.Drawing.Point(6, 105);
			this.ModifyCreditorDefaults.Name = "ModifyCreditorDefaults";
			this.ModifyCreditorDefaults.Size = new System.Drawing.Size(178, 23);
			this.ModifyCreditorDefaults.TabIndex = 11;
			this.ModifyCreditorDefaults.Text = "Modify Creditor Defaults";
			this.ModifyCreditorDefaults.UseVisualStyleBackColor = true;
			this.ModifyCreditorDefaults.Click += new System.EventHandler(this.ModifyCreditorDefaults_Click);
			// 
			// CreateSupplier
			// 
			this.CreateSupplier.Location = new System.Drawing.Point(6, 6);
			this.CreateSupplier.Name = "CreateSupplier";
			this.CreateSupplier.Size = new System.Drawing.Size(178, 23);
			this.CreateSupplier.TabIndex = 9;
			this.CreateSupplier.Text = "Create Supplier";
			this.CreateSupplier.UseVisualStyleBackColor = true;
			this.CreateSupplier.Click += new System.EventHandler(this.CreateSupplier_Click);
			// 
			// CreatePurchaseOrder
			// 
			this.CreatePurchaseOrder.Location = new System.Drawing.Point(209, 106);
			this.CreatePurchaseOrder.Name = "CreatePurchaseOrder";
			this.CreatePurchaseOrder.Size = new System.Drawing.Size(178, 23);
			this.CreatePurchaseOrder.TabIndex = 5;
			this.CreatePurchaseOrder.Text = "Create Purchase Order";
			this.CreatePurchaseOrder.UseVisualStyleBackColor = true;
			this.CreatePurchaseOrder.Click += new System.EventHandler(this.CreatePurchaseOrder_Click);
			// 
			// RejectPR
			// 
			this.RejectPR.Location = new System.Drawing.Point(209, 63);
			this.RejectPR.Name = "RejectPR";
			this.RejectPR.Size = new System.Drawing.Size(178, 23);
			this.RejectPR.TabIndex = 4;
			this.RejectPR.Text = "Reject Purchase Requisition";
			this.RejectPR.UseVisualStyleBackColor = true;
			this.RejectPR.Click += new System.EventHandler(this.RejectPR_Click);
			// 
			// CreatePR
			// 
			this.CreatePR.Location = new System.Drawing.Point(209, 5);
			this.CreatePR.Name = "CreatePR";
			this.CreatePR.Size = new System.Drawing.Size(178, 23);
			this.CreatePR.TabIndex = 2;
			this.CreatePR.Text = "Create Purchase Requisition";
			this.CreatePR.UseVisualStyleBackColor = true;
			this.CreatePR.Click += new System.EventHandler(this.CreatePR_Click);
			// 
			// ApprovePR
			// 
			this.ApprovePR.Location = new System.Drawing.Point(209, 34);
			this.ApprovePR.Name = "ApprovePR";
			this.ApprovePR.Size = new System.Drawing.Size(178, 23);
			this.ApprovePR.TabIndex = 3;
			this.ApprovePR.Text = "Approve Purchase Requisition";
			this.ApprovePR.UseVisualStyleBackColor = true;
			this.ApprovePR.Click += new System.EventHandler(this.ApprovePR_Click);
			// 
			// TabDebtors
			// 
			this.TabDebtors.Controls.Add(this.CreateCustomerJournal);
			this.TabDebtors.Controls.Add(this.EmailTransactionDocument);
			this.TabDebtors.Controls.Add(this.PrintTransactionDocument);
			this.TabDebtors.Controls.Add(this.CreateNewIndustry);
			this.TabDebtors.Controls.Add(this.CreateSalesInvoice);
			this.TabDebtors.Controls.Add(this.EditCustomer);
			this.TabDebtors.Controls.Add(this.CreateSalesCredit);
			this.TabDebtors.Controls.Add(this.ModifyDebtorDefaults);
			this.TabDebtors.Controls.Add(this.CreateSalesOrder);
			this.TabDebtors.Controls.Add(this.CreateCustomer);
			this.TabDebtors.Location = new System.Drawing.Point(4, 22);
			this.TabDebtors.Name = "TabDebtors";
			this.TabDebtors.Padding = new System.Windows.Forms.Padding(3);
			this.TabDebtors.Size = new System.Drawing.Size(611, 206);
			this.TabDebtors.TabIndex = 2;
			this.TabDebtors.Text = "Debtors";
			// 
			// CreateCustomerJournal
			// 
			this.CreateCustomerJournal.Location = new System.Drawing.Point(402, 12);
			this.CreateCustomerJournal.Name = "CreateCustomerJournal";
			this.CreateCustomerJournal.Size = new System.Drawing.Size(178, 23);
			this.CreateCustomerJournal.TabIndex = 21;
			this.CreateCustomerJournal.Text = "Create Customer Journal";
			this.CreateCustomerJournal.UseVisualStyleBackColor = true;
			this.CreateCustomerJournal.Click += new System.EventHandler(this.CreateCustomerJournal_Click);
			// 
			// EmailTransactionDocument
			// 
			this.EmailTransactionDocument.Location = new System.Drawing.Point(203, 149);
			this.EmailTransactionDocument.Name = "EmailTransactionDocument";
			this.EmailTransactionDocument.Size = new System.Drawing.Size(178, 23);
			this.EmailTransactionDocument.TabIndex = 20;
			this.EmailTransactionDocument.Text = "Email Transaction Document";
			this.EmailTransactionDocument.UseVisualStyleBackColor = true;
			this.EmailTransactionDocument.Click += new System.EventHandler(this.EmailTransactionDocument_Click);
			// 
			// PrintTransactionDocument
			// 
			this.PrintTransactionDocument.Location = new System.Drawing.Point(203, 120);
			this.PrintTransactionDocument.Name = "PrintTransactionDocument";
			this.PrintTransactionDocument.Size = new System.Drawing.Size(178, 23);
			this.PrintTransactionDocument.TabIndex = 19;
			this.PrintTransactionDocument.Text = "Print Transaction Document";
			this.PrintTransactionDocument.UseVisualStyleBackColor = true;
			this.PrintTransactionDocument.Click += new System.EventHandler(this.PrintTransactionDocument_Click);
			// 
			// CreateNewIndustry
			// 
			this.CreateNewIndustry.Location = new System.Drawing.Point(3, 149);
			this.CreateNewIndustry.Name = "CreateNewIndustry";
			this.CreateNewIndustry.Size = new System.Drawing.Size(178, 23);
			this.CreateNewIndustry.TabIndex = 8;
			this.CreateNewIndustry.Text = "Create New Industry";
			this.CreateNewIndustry.UseVisualStyleBackColor = true;
			this.CreateNewIndustry.Click += new System.EventHandler(this.CreateNewIndustry_Click);
			// 
			// CreateSalesInvoice
			// 
			this.CreateSalesInvoice.Location = new System.Drawing.Point(203, 41);
			this.CreateSalesInvoice.Name = "CreateSalesInvoice";
			this.CreateSalesInvoice.Size = new System.Drawing.Size(178, 23);
			this.CreateSalesInvoice.TabIndex = 18;
			this.CreateSalesInvoice.Text = "Create Sales Invoice";
			this.CreateSalesInvoice.UseVisualStyleBackColor = true;
			this.CreateSalesInvoice.Click += new System.EventHandler(this.CreateSalesInvoice_Click);
			// 
			// EditCustomer
			// 
			this.EditCustomer.Location = new System.Drawing.Point(6, 41);
			this.EditCustomer.Name = "EditCustomer";
			this.EditCustomer.Size = new System.Drawing.Size(178, 23);
			this.EditCustomer.TabIndex = 3;
			this.EditCustomer.Text = "Edit Customer";
			this.EditCustomer.UseVisualStyleBackColor = true;
			this.EditCustomer.Click += new System.EventHandler(this.EditCustomer_Click);
			// 
			// CreateSalesCredit
			// 
			this.CreateSalesCredit.Location = new System.Drawing.Point(203, 70);
			this.CreateSalesCredit.Name = "CreateSalesCredit";
			this.CreateSalesCredit.Size = new System.Drawing.Size(178, 23);
			this.CreateSalesCredit.TabIndex = 17;
			this.CreateSalesCredit.Text = "Create Sales Credit";
			this.CreateSalesCredit.UseVisualStyleBackColor = true;
			this.CreateSalesCredit.Click += new System.EventHandler(this.CreateSalesCredit_Click);
			// 
			// ModifyDebtorDefaults
			// 
			this.ModifyDebtorDefaults.Location = new System.Drawing.Point(3, 120);
			this.ModifyDebtorDefaults.Name = "ModifyDebtorDefaults";
			this.ModifyDebtorDefaults.Size = new System.Drawing.Size(178, 23);
			this.ModifyDebtorDefaults.TabIndex = 5;
			this.ModifyDebtorDefaults.Text = "Modify Debtor Defaults";
			this.ModifyDebtorDefaults.UseVisualStyleBackColor = true;
			this.ModifyDebtorDefaults.Click += new System.EventHandler(this.ModifyDebtorDefaults_Click);
			// 
			// CreateSalesOrder
			// 
			this.CreateSalesOrder.Location = new System.Drawing.Point(203, 12);
			this.CreateSalesOrder.Name = "CreateSalesOrder";
			this.CreateSalesOrder.Size = new System.Drawing.Size(178, 23);
			this.CreateSalesOrder.TabIndex = 1;
			this.CreateSalesOrder.Text = "Create Sales Order";
			this.CreateSalesOrder.UseVisualStyleBackColor = true;
			this.CreateSalesOrder.Click += new System.EventHandler(this.CreateSalesOrder_Click);
			// 
			// CreateCustomer
			// 
			this.CreateCustomer.Location = new System.Drawing.Point(6, 12);
			this.CreateCustomer.Name = "CreateCustomer";
			this.CreateCustomer.Size = new System.Drawing.Size(178, 23);
			this.CreateCustomer.TabIndex = 2;
			this.CreateCustomer.Text = "Create Customer";
			this.CreateCustomer.UseVisualStyleBackColor = true;
			this.CreateCustomer.Click += new System.EventHandler(this.CreateCustomer_Click);
			// 
			// TabCommon
			// 
			this.TabCommon.Controls.Add(this.RegisterThirdPartyTool);
			this.TabCommon.Controls.Add(this.ShowNotes);
			this.TabCommon.Controls.Add(this.ShowReminders);
			this.TabCommon.Controls.Add(this.ShowTransactionForm);
			this.TabCommon.Controls.Add(this.ShowMaintenanceForm);
			this.TabCommon.Controls.Add(this.ShowEnquiryForm);
			this.TabCommon.Controls.Add(this.RunScalarCommand);
			this.TabCommon.Controls.Add(this.ImportantInformationButton);
			this.TabCommon.Controls.Add(this.GetCompanySystemDefaults);
			this.TabCommon.Controls.Add(this.GetLoggedInUserInfo);
			this.TabCommon.Controls.Add(this.GetCompanyConnectionString);
			this.TabCommon.Location = new System.Drawing.Point(4, 22);
			this.TabCommon.Name = "TabCommon";
			this.TabCommon.Padding = new System.Windows.Forms.Padding(3);
			this.TabCommon.Size = new System.Drawing.Size(611, 206);
			this.TabCommon.TabIndex = 0;
			this.TabCommon.Text = "Common";
			// 
			// RegisterThirdPartyTool
			// 
			this.RegisterThirdPartyTool.Location = new System.Drawing.Point(411, 6);
			this.RegisterThirdPartyTool.Name = "RegisterThirdPartyTool";
			this.RegisterThirdPartyTool.Size = new System.Drawing.Size(178, 23);
			this.RegisterThirdPartyTool.TabIndex = 27;
			this.RegisterThirdPartyTool.Text = "Register Third Party Tool";
			this.RegisterThirdPartyTool.UseVisualStyleBackColor = true;
			this.RegisterThirdPartyTool.Click += new System.EventHandler(this.RegisterThirdPartyTool_Click);
			// 
			// ShowNotes
			// 
			this.ShowNotes.Location = new System.Drawing.Point(411, 64);
			this.ShowNotes.Name = "ShowNotes";
			this.ShowNotes.Size = new System.Drawing.Size(178, 23);
			this.ShowNotes.TabIndex = 26;
			this.ShowNotes.Text = "Show Notes";
			this.ShowNotes.UseVisualStyleBackColor = true;
			this.ShowNotes.Click += new System.EventHandler(this.ShowNotes_Click);
			// 
			// ShowReminders
			// 
			this.ShowReminders.Location = new System.Drawing.Point(411, 93);
			this.ShowReminders.Name = "ShowReminders";
			this.ShowReminders.Size = new System.Drawing.Size(178, 23);
			this.ShowReminders.TabIndex = 25;
			this.ShowReminders.Text = "Show Reminders";
			this.ShowReminders.UseVisualStyleBackColor = true;
			this.ShowReminders.Click += new System.EventHandler(this.ShowReminders_Click);
			// 
			// ShowTransactionForm
			// 
			this.ShowTransactionForm.Location = new System.Drawing.Point(211, 93);
			this.ShowTransactionForm.Name = "ShowTransactionForm";
			this.ShowTransactionForm.Size = new System.Drawing.Size(178, 23);
			this.ShowTransactionForm.TabIndex = 24;
			this.ShowTransactionForm.Text = "Show Transaction Form";
			this.ShowTransactionForm.UseVisualStyleBackColor = true;
			this.ShowTransactionForm.Click += new System.EventHandler(this.ShowTransactionForm_Click);
			// 
			// ShowMaintenanceForm
			// 
			this.ShowMaintenanceForm.Location = new System.Drawing.Point(211, 64);
			this.ShowMaintenanceForm.Name = "ShowMaintenanceForm";
			this.ShowMaintenanceForm.Size = new System.Drawing.Size(178, 23);
			this.ShowMaintenanceForm.TabIndex = 22;
			this.ShowMaintenanceForm.Text = "Show Maintenance Form";
			this.ShowMaintenanceForm.UseVisualStyleBackColor = true;
			this.ShowMaintenanceForm.Click += new System.EventHandler(this.ShowMaintenanceForm_Click);
			// 
			// ShowEnquiryForm
			// 
			this.ShowEnquiryForm.Location = new System.Drawing.Point(211, 122);
			this.ShowEnquiryForm.Name = "ShowEnquiryForm";
			this.ShowEnquiryForm.Size = new System.Drawing.Size(178, 23);
			this.ShowEnquiryForm.TabIndex = 23;
			this.ShowEnquiryForm.Text = "Show Enquiry Form";
			this.ShowEnquiryForm.UseVisualStyleBackColor = true;
			this.ShowEnquiryForm.Click += new System.EventHandler(this.ShowEnquiryForm_Click);
			// 
			// RunScalarCommand
			// 
			this.RunScalarCommand.Location = new System.Drawing.Point(411, 122);
			this.RunScalarCommand.Name = "RunScalarCommand";
			this.RunScalarCommand.Size = new System.Drawing.Size(178, 23);
			this.RunScalarCommand.TabIndex = 19;
			this.RunScalarCommand.Text = "Run SQL scalar command";
			this.RunScalarCommand.UseVisualStyleBackColor = true;
			this.RunScalarCommand.Click += new System.EventHandler(this.RunScalarCommand_Click);
			// 
			// ImportantInformationButton
			// 
			this.ImportantInformationButton.Location = new System.Drawing.Point(211, 6);
			this.ImportantInformationButton.Name = "ImportantInformationButton";
			this.ImportantInformationButton.Size = new System.Drawing.Size(178, 23);
			this.ImportantInformationButton.TabIndex = 18;
			this.ImportantInformationButton.Text = "IMPORTANT INFORMATION";
			this.ImportantInformationButton.UseVisualStyleBackColor = true;
			this.ImportantInformationButton.Click += new System.EventHandler(this.ImportantInformationButton_Click);
			// 
			// GetCompanySystemDefaults
			// 
			this.GetCompanySystemDefaults.Location = new System.Drawing.Point(6, 122);
			this.GetCompanySystemDefaults.Name = "GetCompanySystemDefaults";
			this.GetCompanySystemDefaults.Size = new System.Drawing.Size(178, 23);
			this.GetCompanySystemDefaults.TabIndex = 12;
			this.GetCompanySystemDefaults.Text = "Get company system defaults";
			this.GetCompanySystemDefaults.UseVisualStyleBackColor = true;
			this.GetCompanySystemDefaults.Click += new System.EventHandler(this.GetCompanySystemDefaults_Click);
			// 
			// GetLoggedInUserInfo
			// 
			this.GetLoggedInUserInfo.Location = new System.Drawing.Point(6, 93);
			this.GetLoggedInUserInfo.Name = "GetLoggedInUserInfo";
			this.GetLoggedInUserInfo.Size = new System.Drawing.Size(178, 23);
			this.GetLoggedInUserInfo.TabIndex = 15;
			this.GetLoggedInUserInfo.Text = "Get logged in user information";
			this.GetLoggedInUserInfo.UseVisualStyleBackColor = true;
			this.GetLoggedInUserInfo.Click += new System.EventHandler(this.GetLoggedInUserInfo_Click);
			// 
			// GetCompanyConnectionString
			// 
			this.GetCompanyConnectionString.Location = new System.Drawing.Point(6, 64);
			this.GetCompanyConnectionString.Name = "GetCompanyConnectionString";
			this.GetCompanyConnectionString.Size = new System.Drawing.Size(178, 23);
			this.GetCompanyConnectionString.TabIndex = 13;
			this.GetCompanyConnectionString.Text = "Get company connection string";
			this.GetCompanyConnectionString.UseVisualStyleBackColor = true;
			this.GetCompanyConnectionString.Click += new System.EventHandler(this.GetCompanyConnectionString_Click);
			// 
			// TabControl1
			// 
			this.TabControl1.Controls.Add(this.TabCommon);
			this.TabControl1.Controls.Add(this.TabDebtors);
			this.TabControl1.Controls.Add(this.TabCreditors);
			this.TabControl1.Controls.Add(this.TabInventory);
			this.TabControl1.Controls.Add(this.TabManufacturing);
			this.TabControl1.Controls.Add(this.TabCashbook);
			this.TabControl1.Controls.Add(this.TabAdministration);
			this.TabControl1.Controls.Add(this.TabCRM);
			this.TabControl1.Controls.Add(this.TabService);
			this.TabControl1.Controls.Add(this.TabJobs);
			this.TabControl1.Controls.Add(this.tabPage1);
			this.TabControl1.Location = new System.Drawing.Point(12, 12);
			this.TabControl1.Name = "TabControl1";
			this.TabControl1.SelectedIndex = 0;
			this.TabControl1.Size = new System.Drawing.Size(619, 232);
			this.TabControl1.TabIndex = 5;
			// 
			// tabPage1
			// 
			this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
			this.tabPage1.Controls.Add(this.CreateGeneralLedgerJournal);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(611, 206);
			this.tabPage1.TabIndex = 12;
			this.tabPage1.Text = "General Ledger";
			// 
			// CreateGeneralLedgerJournal
			// 
			this.CreateGeneralLedgerJournal.Location = new System.Drawing.Point(216, 92);
			this.CreateGeneralLedgerJournal.Name = "CreateGeneralLedgerJournal";
			this.CreateGeneralLedgerJournal.Size = new System.Drawing.Size(178, 23);
			this.CreateGeneralLedgerJournal.TabIndex = 22;
			this.CreateGeneralLedgerJournal.Text = "Create Journal";
			this.CreateGeneralLedgerJournal.UseVisualStyleBackColor = true;
			this.CreateGeneralLedgerJournal.Click += new System.EventHandler(this.CreateGeneralLedgerJournal_Click);
			// 
			// MainForm
			// 
			this.AcceptButton = this.OKButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.OKButton;
			this.ClientSize = new System.Drawing.Size(638, 287);
			this.ControlBox = false;
			this.Controls.Add(this.TabControl1);
			this.Controls.Add(this.OKButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Sybiz Vision API Samples";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_Closing);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.TabJobs.ResumeLayout(false);
			this.TabService.ResumeLayout(false);
			this.TabCRM.ResumeLayout(false);
			this.TabAdministration.ResumeLayout(false);
			this.TabCashbook.ResumeLayout(false);
			this.TabManufacturing.ResumeLayout(false);
			this.TabInventory.ResumeLayout(false);
			this.TabCreditors.ResumeLayout(false);
			this.TabDebtors.ResumeLayout(false);
			this.TabCommon.ResumeLayout(false);
			this.TabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		internal Button OKButton;
		internal TabPage TabJobs;
		internal Button CreateJobInvoice;
		internal Button CreatePreInvoice;
		internal Button CreateJobCost;
		internal Button CreateJob;
		internal TabPage TabService;
		internal Button CreateServiceAction;
		internal Button CreateServiceRequest;
		internal Button CreateServiceItem;
		internal TabPage TabCRM;
		internal Button btnLinkJobActivity;
		internal Button btnLinkCaseFileToRemark;
		internal Button btnCreateActivity;
		internal Button btnCreateCaseFile;
		internal TabPage TabAdministration;
		internal Button btnCreateUser;
		internal Button btnCreateRole;
		internal TabPage TabCashbook;
		internal Button btnCreateCashbook_SplitLines;
		internal Button btnCreateCashbook_ByTransaction;
		internal Button btnCreateCashbook_AutoOffset;
		internal Button btnCreateCashbook_ByOffset;
		internal TabPage TabManufacturing;
		internal Button EditAssembly;
		internal Button CreateAssembly;
		internal Button CreateManufactureIssue;
		internal Button CreateMO;
		internal TabPage TabInventory;
		internal Button CopyProduct;
		internal Button CreateProduct;
		internal TabPage TabCreditors;
		internal Button EditSupplier;
		internal Button ModifyCreditorDefaults;
		internal Button CreateSupplier;
		internal Button CreatePurchaseOrder;
		internal Button RejectPR;
		internal Button CreatePR;
		internal Button ApprovePR;
		internal TabPage TabDebtors;
		internal Button EmailTransactionDocument;
		internal Button PrintTransactionDocument;
		internal Button CreateNewIndustry;
		internal Button CreateSalesInvoice;
		internal Button EditCustomer;
		internal Button CreateSalesCredit;
		internal Button ModifyDebtorDefaults;
		internal Button CreateSalesOrder;
		internal Button CreateCustomer;
		internal TabPage TabCommon;
		internal Button RegisterThirdPartyTool;
		internal Button ShowNotes;
		internal Button ShowReminders;
		internal Button ShowTransactionForm;
		internal Button ShowMaintenanceForm;
		internal Button ShowEnquiryForm;
		internal Button RunScalarCommand;
		internal Button ImportantInformationButton;
		internal Button GetCompanySystemDefaults;
		internal Button GetLoggedInUserInfo;
		internal Button GetCompanyConnectionString;
		internal TabControl TabControl1;
        internal Button CreateSupplierJournal;
        internal Button CreateCustomerJournal;
        private TabPage tabPage1;
        internal Button CreateGeneralLedgerJournal;
    }
	
}
