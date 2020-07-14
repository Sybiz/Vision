using Microsoft.VisualBasic;
using System.Windows.Forms;


namespace My3rdPartyApplication
{
	public class Breakpoints
	{
		
		//Scenario: Automatically allow the addition of documents to new sales invoices
		//Prerequisities: None
		public void AfterSalesInvoiceNew(Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice transaction, Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointEventArgs e)
		{
			
			//Create the DevExpress XtraOpenFileDialog using our NewOpenFileDialog function (optional parameters are filter, multiselect and default extension) as per comment below, running code only provided to allow compiliation
			//Using dlg As DevExpress.XtraEditors.XtraOpenFileDialog = Sybiz.Vision.WinUI.Utilities.FormFunctions.NewOpenFileDialog(filter:="All Files (*.*)|*.*", multi:=True)
			using (OpenFileDialog dlg = new OpenFileDialog())
			{
				//Add each file that was selected to the documents of the sales invoice
				if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					foreach (string document in dlg.FileNames)
					{
						transaction.Documents.AddNew(dlg.FileName);
					}
				}
			}
			
			
		}
		
		//Scenario: Enforce that the invoice quantity cannot be zero on sales invoices
		//Prerequisities: None
		public void BeforeSalesInvoiceSave(Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice transaction, Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCancelEventArgs e)
		{
			
			bool foundZeroQuantity = false;
			
			//Iterate through each sales invoice line in the sales invoice to determine if a zero invoice quantity line exists
			foreach (Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoiceLine line in transaction.Lines)
			{
				if (line.QuantityInvoice == 0)
				{
					foundZeroQuantity = true;
				}
			}
			
			if (foundZeroQuantity)
			{
				//Set to true to cancel the save
				e.Cancel = true;
				MessageBox.Show("Cancelled Save: Zero Invoice Quantity Line on Sales Invoice", "Breakpoints");
			}
			
		}
		
		//Scenario: Enforce a minimum spend of $100 on sales invoices
		//Prerequisities: None
		public void BeforeSalesInvoiceProcess(Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice transaction, Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCancelEventArgs e)
		{
			
			if (transaction.Total < 100M)
			{
				//Set to true to cancel the process
				e.Cancel = true;
				MessageBox.Show("Cancelled Process: Invoice Under $100", "Breakpoints");
			}
			
		}
		
		//Scenario: Assign any case files on the sales invoice to the current user after saving the sales invoice
		//Prerequisities: Attached case files and edit rights to case files
		public void AfterSalesInvoiceSave(Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice transaction, Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointEventArgs e)
		{
			
			//Check that there are case files on the sales invoice
			if (transaction.CaseFilesExist)
			{
				//Iterate through each of the case files
				foreach (Sybiz.Vision.Platform.ContactManagement.CaseFileStore storeCaseFile in transaction.CaseFiles)
				{
					//Get the case file object from the case file store object
					Sybiz.Vision.Platform.ContactManagement.Transaction.CaseFile objectCaseFile = Sybiz.Vision.Platform.ContactManagement.Transaction.CaseFile.GetObject(System.Convert.ToString(storeCaseFile.Id));
					//No need to do anything if the case file user is the current user
					if (objectCaseFile.User != Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal().UserId)
					{
						//Assign the current user to the case file
						objectCaseFile.User = Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal().UserId;
						//Save the case file
						objectCaseFile = objectCaseFile.Save();
					}
				}
			}
			
		}
		
		//Scenario: Email the sales invoice documents to the specified email address after processing the sales invoice (e.g. sales representative)
		//Prerequisities: Sales invoice transaction settings (e.g. email preview)
		public void AfterSalesInvoiceProcess(Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice transaction, Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointEventArgs e)
		{
			
			//Source: Transaction to generate documents
			//CompanyTemplates: Determines whether company templates are used
			//UserTemplates: Determines whether user templates are used
			//PrintHandler: Handler for printing documents (can be nothing to not print)
			//PreviewHandler: Handler for print previewing documents (can be nothing to not print preview)
			//GenerateHandler: Handler for generating documents
			//EmailHandler: Handler for emailing documents (can be nothing to not email)
			//EmailPreviewHandler: Handler for email previewing documents (can be nothing to not email preview)
			//Subject (Optional): Specify a subject for emailing documents
			//Body (Optional): Specify a body for emailing documents
			//AlternateAccountId (Optional): Specify an alternate account (e.g. customer, supplier) for emailing documents
			//AlternateEmailAddress (Optional): Specify an alternate email address for emailing documents
			//Notes: Handlers for printing and emailing can be swapped with other handlers if desired (e.g. use the email handler for an email preview)
			
			string subject = System.String.Empty;
			string body = System.String.Empty;
			
			//Get the subject and body from the relevant email template
			Sybiz.Vision.WinUI.Utilities.FormFunctions.GetEmailSubjectAndBody(transaction, ref subject, ref body);
			Sybiz.Vision.Platform.Core.Transaction.TransactionDocumentEngine.ProduceTransactionDocument(source: transaction, companyTemplates: true, userTemplates: true, printHandler: Sybiz.Vision.WinUI.Utilities.FormFunctions.PrintTransaction, previewHandler: Sybiz.Vision.WinUI.Utilities.FormFunctions.PreviewTransaction, generateHandler: Sybiz.Vision.WinUI.Utilities.FormFunctions.ExportTransaction, emailHandler: Sybiz.Vision.WinUI.Utilities.FormFunctions.EmailTransaction, emailPreviewHandler: Sybiz.Vision.WinUI.Utilities.FormFunctions.EmailPreviewTransaction, Subject: subject, Body: body, AlternateEmailAddress: "email@email.com");
			
		}
		
		//Scenario: Email the sales invoice documents to an alternative customer (e.g. head office / branch office situations) after processing the sales invoice
		//Prerequisities: Sales invoice transaction settings (e.g. email preview)
		public void AfterSalesInvoiceProcess2(Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice transaction, Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointEventArgs e)
		{
			
			//Determine if there is a difference in the order, delivery and invoice customers
			if (transaction.OrderCustomer != transaction.DeliveryCustomer || transaction.DeliveryCustomer != transaction.InvoiceCustomer)
			{
				//Stores the value of the alternate customer
				int alternateAccount = 0;
				
				//Check if it is the order or the delivery that is different
				if (transaction.OrderCustomer != transaction.InvoiceCustomer)
				{
					alternateAccount = transaction.OrderCustomer;
				}
				else
				{
					alternateAccount = transaction.DeliveryCustomer;
				}
				
				//See above for explanation of parameters
				Sybiz.Vision.Platform.Core.Transaction.TransactionDocumentEngine.ProduceTransactionDocument(source: transaction, companyTemplates: true, userTemplates: true, printHandler: Sybiz.Vision.WinUI.Utilities.FormFunctions.PrintTransaction, previewHandler: Sybiz.Vision.WinUI.Utilities.FormFunctions.PreviewTransaction, generateHandler: Sybiz.Vision.WinUI.Utilities.FormFunctions.ExportTransaction, emailHandler: Sybiz.Vision.WinUI.Utilities.FormFunctions.EmailTransaction, emailPreviewHandler: Sybiz.Vision.WinUI.Utilities.FormFunctions.EmailPreviewTransaction, AlternateAccountId: alternateAccount);
			}
			
		}
		
		//Scenario: Automatically add a sales template to the sales invoice if it exists
		//Prerequisities: None
		public void SalesInvoiceCustomerChanged(Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice transaction, Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointEventArgs e)
		{
			
			//Use the scalar command to get the top sales template for the customer on the sales invoice
			int templateId = System.Convert.ToInt32(Sybiz.Vision.Platform.Core.Data.ScalarCommand.Execute<int>(string.Format("SELECT TOP 1 SalesTemplateId FROM [dr].[SalesTemplate] WHERE CustomerId = {0}", transaction.Customer)));
			//Cast the sales transaction to an ISalesTransaction
			Sybiz.Vision.Platform.Debtors.Transaction.ISalesTransaction iTransaction = (Sybiz.Vision.Platform.Debtors.Transaction.ISalesTransaction) transaction;
			
			//Only add the template document if one was returned from the scalar command
			if (templateId > 0)
			{
				iTransaction.AddTemplateDocument(templateId);
			}
			
		}
		
		//Scenario: Prevent lines with identical types and items from being added to sales invoices
		//Prerequisities: None
		public void SalesInvoiceCellValueChanged(Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice transaction, Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCellValueChangedEventArgs<Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoiceLine> e)
		{
			
			//Check for the item field
			if (e.FieldName == "Account")
			{
				//Iterate through each sales invoice line in the sales invoice
				foreach (Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoiceLine line in transaction.Lines)
				{
					//Check the line being added against the currently iterated sales invoice line
					if (e.Line.Id != line.Id && e.Line.AccountType == line.AccountType && e.Line.Account == line.Account)
					{
						//Remove the line being added if the type and item is identical
						MessageBox.Show("Removing Line: Existing Line with Item and Account", "Breakpoints");
						transaction.Lines.Remove(e.Line);
						break;
					}
				}
			}
			
		}
		
		//Scenario: Define a custom ribbon button
		//Prerequisities: None
		public void SalesInvoiceCustomRibbonButtonRegister(System.Object sender, Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCustomRibbonButtonRegisterEventArgs e)
		{
			
			//Key: Used to identify the button in the CustomRibbonButtonClick breakpoint
			//Description: Sets the description
			//Style: Sets the style
			//ToolTipTitle: Sets the tooltip title
			//ToolTipDescription: Sets the tooltip description
			//Glyph: Sets the small image
			//LargeGlyph: Sets the large image
			//BeginGroup: Determines whether to start a new group
			//RibbonPageIndex: Determines the page of the button (i.e. home = 0, actions = 1, options = 2)
			
			e.Register.RegisterCustomRibbonButton("GoogleMaps", "Google Maps", Sybiz.Vision.Platform.Admin.Breakpoints.CustomRibbonButtonStyle.Default, "Google Maps", "Shows the delivery address on google maps", null, null, true, 0);
			
		}
		
		//Scenario: Show the delivery address for the customer on the sales invoice in google maps
		//Prerequisities: Custom ribbon button with the key "GoogleMaps" (see above)
		public void SalesInvoiceCustomRibbonButtonClick(Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice transaction, Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCustomRibbonButtonClickEventArgs e)
		{
			
			//Check the key for the button that was clicked
			if (e.Key == "GoogleMaps" && transaction.Customer > 0)
			{
				
				//Get the default delivery address for the customer on the sales invoice
				Sybiz.Vision.Platform.Debtors.CustomerDetailInfo customer = Sybiz.Vision.Platform.Debtors.CustomerDetailInfo.GetObject(transaction.Customer);
				Sybiz.Vision.Platform.Debtors.DeliveryAddressDetailInfo deliveryAddress = customer.DeliveryAddress.DefaultAddress;
				
				if (!System.String.IsNullOrWhiteSpace(deliveryAddress.Street))
				{
					//Launch google maps using the default delivery address
					System.Diagnostics.Process.Start(System.String.Format("https://www.google.com/maps/search/?api=1&query={0}+{1}+{2}", deliveryAddress.Street, deliveryAddress.Suburb, deliveryAddress.PostCode));
				}
				else
				{
					//Need a delivery address to launch google maps
					MessageBox.Show("No Delivery Address", "Breakpoints");
				}
				
			}
			else
			{
				//Need a customer to get the delivery address to launch google maps
				MessageBox.Show("No Customer", "Breakpoints");
			}
			
		}
		
		//Scenario: Disable the tax code field on sales invoices to enforce the sales tax code on products
		//Prerequisities: None
		public void SalesInvoiceDisableCustomCellRepository(Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice transaction, Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCellRepositoryEventArgs<Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoiceLine> e)
		{
			
			//Check for the tax code field
			if (e.FieldName == "TaxCode")
			{
				//Set to true to disable the tax code field
				e.Handled = true;
			}
			
		}
		
		//Scenario: Enable the custom repository on the quantity invoice field on sales invoices
		//Prerequisities: None
		public void SalesInvoiceUseCustomCellRepository(Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice transaction, Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCellRepositoryEventArgs<Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoiceLine> e)
		{
			
			//Check for the quantity invoice field
			if (e.FieldName == "QuantityInvoice")
			{
				//Set to true to enable the custom repository
				e.Handled = true;
			}
			
		}
		
		//Scenario: Use the custom repository on the quantity invoice field to enter the quantity with a DevExpress XtraInputBox
		//Prerequisities: None
		public void SalesInvoiceUseCustomRepositoryButtonClick(Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoiceLine transaction, Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCellRepositoryEventArgs<Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoiceLine> e)
		{
			
			//Create and show the DevExpress XtraInputBox with the existing quantity invoice
			string input = Interaction.InputBox("Enter Quantity Invoice:", "Breakpoints", System.Convert.ToString(e.Line.QuantityInvoice));
			decimal quantity = 0M;
			
			if (string.IsNullOrWhiteSpace(input))
			{
				//Do Nothing - Cancel Clicked
			}
			else if (!string.IsNullOrWhiteSpace(input) && decimal.TryParse(input, out quantity))
			{
				e.Line.QuantityInvoice = quantity;
			}
			else
			{
				MessageBox.Show("Not a Decimal Value", "Breakpoints");
			}
			
		}
		
		//Scenario: Create a workflow to create a new supplier and product and then use them on a new purchase order
		//Prerequisities: None
		public void ExternalApplicationCustomRibbonButtonClick(Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCustomRibbonButtonClickEventArgs e)
		{
			
			if (e.Key == "BreakpointKey")
			{
				//Show a new supplier form
				Sybiz.Vision.Module.Coordinator.VisionDialogResult formSupplier = Sybiz.Vision.Module.Coordinator.VisionApplication.GetApplication().CR.ShowSupplierForm(e.Form, 0, null, null);
				
				//Check if a supplier was created
				if (formSupplier.Id > 0)
				{
					//Show a new product form
					Sybiz.Vision.Module.Coordinator.VisionDialogResult formProduct = Sybiz.Vision.Module.Coordinator.VisionApplication.GetApplication().IC.ShowProductForm(e.Form, 0, null, null);
					
					//Check if a product was created
					if (formProduct.Id > 0)
					{
						//Create a new purchase order with the supplier
						Sybiz.Vision.Platform.Creditors.Transaction.PurchaseOrder newOrder = Sybiz.Vision.Platform.Creditors.Transaction.PurchaseOrder.NewObject(null, true, true);
						newOrder.Supplier = formSupplier.Id;
						newOrder.Reference = "Breakpoints Demo";
						
						//Create a new purchase order line with the product
						Sybiz.Vision.Platform.Creditors.Transaction.IPurchaseTransactionLine theLine = newOrder.Lines.AddNew(Sybiz.Vision.Platform.Core.Enumerations.PurchaseLineType.IC);
						theLine.Account = formProduct.Id;
						theLine.Location = 1;
						theLine.TaxCode = 1;
						
						//Save the purchase order
						newOrder = newOrder.Save();
						var newOrderId = newOrder.Id;
						
						//Open the purchase order
						Sybiz.Vision.Module.Coordinator.VisionApplication.GetApplication().CR.ShowPurchaseOrderForm(e.Form, newOrder.Id, newOrder.Supplier);
					}
					
				}
				
			}
			
		}
		
	}
}
