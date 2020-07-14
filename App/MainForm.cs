using System;
using System.Linq;
using System.Data;
using System.Windows.Forms;
using System.Configuration;
using Microsoft.VisualBasic;

namespace My3rdPartyApplication
{
	
	public partial class MainForm
	{
		
#region  Constructors
		
		static MainForm()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
		}
		
		public MainForm()
		{
			
			// This call is required by the designer.
			InitializeComponent();
			
			// Add any initialization after the InitializeComponent() call.
			TabControl1.TabPages.Remove(TabAdministration);
		}
		
#endregion
		
#region  Event Management
		
		
		public void MainForm_Load(object sender, EventArgs e)
		{
			
			if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["CommonConnectionString"]) ||
					string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["CompanyConnectionString"]))
					{
					MessageBox.Show($"Commmon and company connection strings must be populated prior to starting the application in the .config file");
				this.Close();
			}
			
			
			if (Sybiz.Vision.Platform.Security.Principal.LogIn(System.Convert.ToString(ConfigurationManager.AppSettings["UserName"].ToString()), System.Convert.ToString(
					ConfigurationManager.AppSettings["Password"].ToString()), System.Convert.ToString(
					ConfigurationManager.AppSettings["CompanyConnectionString"].ToString()), System.Convert.ToString(
					ConfigurationManager.AppSettings["CommonConnectionString"].ToString())) == false)
					{
					MessageBox.Show($"Unable to login and error reported was {Sybiz.Vision.Platform.Security.Principal.ErrorMessage}");
			}
			
		}
		
		public void MainForm_Closing(object sender, FormClosingEventArgs e)
		{
			Sybiz.Vision.Platform.Security.Principal.LogOut();
		}
		
		public void OKButton_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}
		
#endregion
			
#region  Creditors
				

				
				
#endregion
				
#region  Inventory
				
		
				
				
#endregion
				
#region  Manufacturing
				

				
				
#endregion
				
#region  Cashbook
				
	
				

				
#endregion
				
#region  Administration
				
#endregion
				
#region  CRM
				

				
				
				
#endregion
				
#region  Service
				

				
#endregion
				
#region  Jobs
				

				
				
				
				
#endregion
				
				
				
			}
			
		}
