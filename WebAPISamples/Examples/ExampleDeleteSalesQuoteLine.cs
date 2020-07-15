using System.Linq;
using System.Net;
using System.IO;

public static class ExampleDeleteSalesQuoteLine
{
    public static void DeleteSalesQuoteLine()
    {
        SalesQuote quote = ExampleGetSalesQuote.GetSalesQuote();
        var lineId = quote.Lines.Last().Id;
		var requestDelete = (HttpWebRequest)WebRequest.Create($"https://api.sybiz.com/Beta/API/DR/SalesQuote/{quote.Id}?Line={lineId}"); // Comma seperate values if there are multiple lines to delete
		requestDelete.Headers[HttpRequestHeader.Authorization] = $"Bearer {ExampleBearerToken.GetBearerToken().Access_Token}";
		requestDelete.ContentType = "application/JSON";
		requestDelete.Timeout = 10000;
		requestDelete.Method = "DELETE";

		using (var response = (HttpWebResponse)requestDelete.GetResponse())
		{
			using (var reader = new StreamReader(response.GetResponseStream()))
			{
				var data = reader.ReadToEnd();
				response.Close();
				reader.Close();
			}
		}
	}
}