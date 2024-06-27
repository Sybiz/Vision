using System;
using System.IO;
using System.Reflection;

namespace DynamicConsole
{
  internal class Program
  {
    private static int OrderId = 4; // Valid Sales OrderId that you can create a sales invoice from and invoice all

    static void Main(string[] args)
    {
      var userName = "Administrator";
      var password = "admin";
      var companyConnectionString = "";
      var commonConnectionString = "";

      if (VisionService.LogIn(userName, password, companyConnectionString, commonConnectionString))
      {
        WriteFirstTenCustomersToConsole();
        CreateSalesInvoiceAndSaveDocument();
        VisionService.LogOut();
      }
      Console.WriteLine("Please hit any key to exit!");
      Console.Read();
    }

    private static void WriteFirstTenCustomersToConsole()
    {
      var customers = VisionService.GetCustomerInfoList();
      var i = 0;
      foreach (var customer in customers)
      {
        i++;
        Console.WriteLine($"Customer: {customer.Code} ({customer.Name})");
        if (i >= 10)
        {
          break;
        }
      }
    }

    private static void CreateSalesInvoiceAndSaveDocument()
    {
      var invoice = VisionService.NewSalesInvoice(null);
      invoice.AddSourceDocument(OrderId, VisionService.GetTransactionType("SalesOrder"));
      invoice.InvoiceAll();

      if (invoice.IsProcessable)
      {
        invoice = invoice.Process();
        //make sure the transaction template is setup as 'Sales Invoice" 
        var fileData = VisionService.CreateTransactionDocument(invoice.Id, VisionService.GetTransactionType("SalesInvoice"), "Sales Invoice");
        File.WriteAllBytes(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Invoice.pdf"), fileData);
      }
    }
  }
}
