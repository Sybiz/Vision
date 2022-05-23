using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using System.IO;
using System;

public class ExampleCustomQuery
{
    public static void CustomQueryGet()
    {
        List<CustomQueryModel> list = new List<CustomQueryModel>();
        var requestCreate = (HttpWebRequest)WebRequest.Create($"{CONFIG.ADDRESS}/API/DB/CustomQuery?Key=CustomQuery1");
        requestCreate.Headers[HttpRequestHeader.Authorization] = $"Bearer {ExampleBearerToken.GetBearerToken().Access_Token}";
        requestCreate.ContentType = "application/JSON";
        requestCreate.Timeout = 10000;
        requestCreate.Method = "GET";

        using (var response = (HttpWebResponse)requestCreate.GetResponse())
        {
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var data = reader.ReadToEnd();
                response.Close();
                reader.Close();
                list = JsonConvert.DeserializeObject<List<CustomQueryModel>>(data);
            }
        }

        foreach (var model in list)
        {
            Console.WriteLine($"{model.Id} - {model.Code} ({model.Name})");
        }
    }

    public static void CustomQueryPost()
    {
        List<CustomQueryModel> list = new List<CustomQueryModel>();
        List<CustomQueryParameter> query = new List<CustomQueryParameter>
        {
            new CustomQueryParameter() { Key = "CreditLimit", Value = "1" },
            new CustomQueryParameter() { Key = "InterestRate", Value = "1" }
        };

        var jsonQuery = JsonConvert.SerializeObject(query);
        var requestCreate = (HttpWebRequest)WebRequest.Create($"{CONFIG.ADDRESS}/API/DB/CustomQuery?Key=CustomQuery2");
        requestCreate.Headers[HttpRequestHeader.Authorization] = $"Bearer {ExampleBearerToken.GetBearerToken().Access_Token}";
        requestCreate.ContentType = "application/JSON";
        requestCreate.ContentLength = Encoding.UTF8.GetBytes(jsonQuery).Length;
        requestCreate.Timeout = 20000;
        requestCreate.Method = "GET";

        using (var writer = new StreamWriter(requestCreate.GetRequestStream()))
        {
            writer.Write(jsonQuery);
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
                list = JsonConvert.DeserializeObject<List<CustomQueryModel>>(data);
            }
        }

        foreach (var model in list)
        {
            Console.WriteLine($"{model.Id} - {model.Code} ({model.Name})");
        }
    }
}