using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

public class ExampleEditSalesQuote
{
	public void EditSalesQuote()
	{
		SalesQuote quote = null;
		var requestList = (HttpWebRequest)WebRequest.Create("https://api.sybiz.com/Beta/API/DR/SalesQuote/1"); // Enter valid sales quote
		requestList.Headers[HttpRequestHeader.Authorization] = $"Bearer {ExampleBearerToken.GetBearerToken().Access_Token}";
		requestList.ContentType = "application/JSON";
		requestList.Timeout = 10000;
		requestList.Method = "GET";

		using (var response = (HttpWebResponse)requestList.GetResponse())
		{
			using (var reader = new StreamReader(response.GetResponseStream()))
			{
				var data = reader.ReadToEnd();
				response.Close();
				reader.Close();
				quote = JsonConvert.DeserializeObject<SalesQuote>(data);
			}
		}

		quote.Reference = "Changed Reference";

		foreach (var line in quote.Lines)
		{
			line.Description = "Changed Description";
		}

		var jsonQuote = JsonConvert.SerializeObject(quote);
		var requestCreate = (HttpWebRequest)WebRequest.Create("https://api.sybiz.com/Beta/API/DR/SalesQuote?process=true");
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

		Console.WriteLine(quote.Reference);
	}
}