using Newtonsoft.Json;
using System.Net;
using System.IO;
using System;

public static class ExampleCreateSalesQuote
{
	public static void CreateSalesQuote()
	{
		var quote = new SalesQuote();
		quote.TransactionNumber = "New Quote"; // Only needed when automatic numbering is disabled
		quote.Reference = "New Quote";
		quote.OrderCustomer = 1; // Enter valid value

		var line = new SalesQuoteLine();
		line.Description = "New Line";
		line.AccountType = 2;
		line.Account = 1; // Enter valid value
		line.TaxCode = 1; // Enter valid value
		line.Location = 1; // Enter valid value
		line.UnitOfMeasure = 1; // Enter valid value
		line.Quantity = 5.25M;
		line.UnitChargeExclusive = 9.99M;
		quote.Lines.Add(line);

		var jsonQuote = JsonConvert.SerializeObject(quote);
		var requestCreate = (HttpWebRequest)WebRequest.Create("https://api.sybiz.com/Beta/API/DR/SalesQuote?process=true"); // Set process to false to save the quote instead
		requestCreate.Headers[HttpRequestHeader.Authorization] = $"Bearer {ExampleBearerToken.GetBearerToken().Access_Token}";
		requestCreate.ContentType = "application/JSON";
		requestCreate.ContentLength = jsonQuote.Length;
		requestCreate.Method = "POST";

		using (var writer = new StreamWriter(requestCreate.GetRequestStream()))
		{
			writer.Write(jsonQuote);
			writer.Flush();
			writer.Close();
		}

		using (var response = (HttpWebResponse)requestCreate.GetResponse())
		{
			using (var reader = new StreamReader(response.GetResponseStream()))
			{
				var data = reader.ReadToEnd();
				response.Close();
				reader.Close();
				quote = JsonConvert.DeserializeObject<SalesQuote>(data);
			}
		}

		Console.WriteLine(quote.Id);
		Console.WriteLine(quote.TransactionNumber); // When automatic numbering is enabled the API returns the generated transaction number
	}
}