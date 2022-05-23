using Newtonsoft.Json;
using System.Net;
using System.IO;

public static class ExampleGetSalesQuote
{
    public static SalesQuote GetSalesQuote()
    {
        SalesQuote quote = null;
        HttpWebRequest requestList = (HttpWebRequest)WebRequest.Create($"{CONFIG.ADDRESS}/API/DR/SalesQuote/1"); // Enter valid sales quote
        requestList.Headers[HttpRequestHeader.Authorization] = $"Bearer {ExampleBearerToken.GetBearerToken().Access_Token}";
        requestList.ContentType = "application/JSON";
        requestList.Timeout = 10000;
        requestList.Method = "GET";

        using (HttpWebResponse response = (HttpWebResponse)requestList.GetResponse())
        {
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string data = reader.ReadToEnd();
                response.Close();
                reader.Close();
                quote = JsonConvert.DeserializeObject<SalesQuote>(data);
            }
        }

        return quote;
    }
}