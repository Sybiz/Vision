using Newtonsoft.Json;
using System.Text;
using System.Net;
using System.IO;
using System;

public static class ExampleEditSalesQuote
{
    public static void EditSalesQuote()
    {
        SalesQuote quote = ExampleGetSalesQuote.GetSalesQuote();
        quote.Reference = "Changed Reference";

        foreach (var line in quote.Lines)
        {
            line.Description = "Changed Description";
        }

        var jsonQuote = JsonConvert.SerializeObject(quote);
        var requestCreate = (HttpWebRequest)WebRequest.Create($"{CONFIG.ADDRESS}/API/DR/SalesQuote?process=true");
        requestCreate.Headers[HttpRequestHeader.Authorization] = $"Bearer {ExampleBearerToken.GetBearerToken().Access_Token}";
        requestCreate.ContentType = "application/JSON";
        requestCreate.ContentLength = Encoding.UTF8.GetBytes(jsonQuote).Length;
        requestCreate.Timeout = 10000;
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