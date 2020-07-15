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
		public void CreateServiceItem_Click(object sender, EventArgs e)
		{
			var newServiceItem = Sybiz.Vision.Platform.Service.ServiceItem.GetObject("0");

			//If automatic numbering is disabled then provide a unique code for this item
			if (newServiceItem.CanWriteProperty(nameof(newServiceItem.Code)))
			{
				newServiceItem.Code = System.Convert.ToString(Interaction.InputBox("Provide a new unique service item code", "Information Required").Trim());
			}

			newServiceItem.Description = "New service item created by sample app";

			//Service group is required and must be configured to allow this sample to run
			newServiceItem.ServiceGroup = 1;

			//Most service items will have a customer but it is not required to be set;
			//invoice customer will default to head-office or the customer if not a branch, do not set invoice customer in most instances
			newServiceItem.Customer = 1;
			newServiceItem.InvoiceCustomer = 1;

			//Service item can be assigned to a product OR a fixed asset (not both);
			//Does not have be assigned to either
			newServiceItem.Product = 1;
			//newServiceItem.FixedAsset = 1

			//This field normally represents a serial number or registration number, etc
			newServiceItem.UniqueIdentifier = System.Convert.ToString(Guid.NewGuid().ToString());

			//Service request details will default to information from customer
			//Allows editing to change contact details and item location (if different from physical address)

			newServiceItem.ContactName = "Barry White";
			newServiceItem.Telephone = "08 8289 1824";

			if (newServiceItem.IsValid)
			{
				newServiceItem = newServiceItem.Save();
				MessageBox.Show($"ServiceItemId {newServiceItem.Id} created successfully");
			}
			else
			{
				foreach (var rule in newServiceItem.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
				{
					MessageBox.Show($"{rule.PropertyName} Is broken on object {rule.ObjectLevel} because '{rule.Description}'");
				}
			}

		}

		public void CreateServiceRequest_Click(object sender, EventArgs e)
		{
			var serviceItemId = Interaction.InputBox("Enter service item Id", "Information Required").Trim();
			var newServiceRequest = Sybiz.Vision.Platform.Service.ServiceRequest.GetObject("0");

			//If automatic numbering is disabled then provide a unique request number for this item
			if (newServiceRequest.CanWriteProperty(nameof(newServiceRequest.ServiceRequestNumber)))
			{
				newServiceRequest.ServiceRequestNumber = System.Convert.ToString(Interaction.InputBox("Provide a new unique service request nubmer", "Information Required").Trim());
			}

			//Service item property should be set first to default all other values
			newServiceRequest.ServiceItem = Convert.ToInt32(System.Convert.ToChar(serviceItemId));

			//newServiceRequest.Customer will default from the service item and can only be changed if service item doesn't have a customer
			//newServiceRequest.InvoiceCustomer will default to the newService.Customer but can be changed to support invoicing a third-party

			//Contact details & address details will default from the service item but can be changed for this specific request
			//These values cannot be edited without a valid service item assigned.
			newServiceRequest.ContactName = "New Person";
			newServiceRequest.Telephone = "123 456 789";
			newServiceRequest.Street = "123 New House";
			newServiceRequest.Suburb = "Moved On";
			newServiceRequest.State = "NSW";
			newServiceRequest.Country = "Australia";

			//These values are all required and should be set to your values (but will default)
			newServiceRequest.Status = 1;
			newServiceRequest.Priority = 1;
			newServiceRequest.Department = 1;

			//This value is not required but will default
			newServiceRequest.Area = 1;

			if (newServiceRequest.IsValid)
			{
				newServiceRequest = newServiceRequest.Save();
				MessageBox.Show($"Service Request {newServiceRequest.ServiceRequestNumber} created successfully");
			}
			else
			{
				foreach (var rule in newServiceRequest.GetBrokenRuleInfo().Where(obj => obj.Severity == Csla.Validation.RuleSeverity.Error))
				{
					MessageBox.Show($"{rule.PropertyName} Is broken on object {rule.ObjectLevel} because '{rule.Description}'");
				}
			}

		}

		public void CreateServiceAction_Click(object sender, EventArgs e)
		{

		}
	}
}
