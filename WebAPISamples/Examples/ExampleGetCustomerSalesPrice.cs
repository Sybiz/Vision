using Newtonsoft.Json;
using System.Net;
using System.IO;
using System;

public static class ExampleGetCustomerSalesPrice
{
	public static void GetCustomerSalesPrice()
	{
		CustomerSalesPrice price = null;
		var requestPrice = (HttpWebRequest)WebRequest.Create("https://api.sybiz.com/Beta/API/IC/CustomerSalesPrice?Customer=1&Product=1&TaxCode=1&Quantity=5.25&DiscountPercentage=25.25&TransactionDate=12/12/2012"); // Enter valid values
		requestPrice.Headers[HttpRequestHeader.Authorization] = $"Bearer {ExampleBearerToken.GetBearerToken().Access_Token}";
		requestPrice.ContentType = "application/JSON";
		requestPrice.Timeout = 10000;
		requestPrice.Method = "GET";

		using (var response = (HttpWebResponse)requestPrice.GetResponse())
		{
			using (var reader = new StreamReader(response.GetResponseStream()))
			{
				var data = reader.ReadToEnd();
				response.Close();
				reader.Close();
				price = JsonConvert.DeserializeObject<CustomerSalesPrice>(data);
			}
		}

		Console.WriteLine(price.ExclusiveUnitPrice);
		Console.WriteLine(price.InclusiveUnitPrice);
	}
}