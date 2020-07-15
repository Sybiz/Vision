﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

public class ExampleGetSalesQuoteList
{
	public void GetSalesQuotes()
	{
		var listQuote = new List<SalesQuote>();
		var requestList = (HttpWebRequest)WebRequest.Create("https://api.sybiz.com/Beta/API/DR/SalesQuoteProcessingAllOutstandingInfoList");
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
				listQuote = JsonConvert.DeserializeObject<List<SalesQuote>>(data);
			}
		}

		foreach (var quote in listQuote)
		{
			Console.WriteLine(quote.TransactionNumber);
		}
	}
}