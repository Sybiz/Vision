using Newtonsoft.Json;
using System.IO;
using System.Net;

public static class ExampleBearerToken
{
	public static BearerToken GetBearerToken()
	{
		BearerToken token = null;
		var jsonToken = $"grant_type=password&database=MobileRelease&username=XXX&password=XXX"; // Enter valid username & password
		var requestBearer = (HttpWebRequest)WebRequest.Create("https://api.sybiz.com/Beta/API/ADM/Bearer");
		requestBearer.ContentType = "application/x-www-form-urlencoded";
		requestBearer.ContentLength = jsonToken.Length;
		requestBearer.Method = "POST";

		using (var writer = new StreamWriter(requestBearer.GetRequestStream()))
		{
			writer.Write(jsonToken);
			writer.Flush();
			writer.Close();
		}

		using (var response = (HttpWebResponse)requestBearer.GetResponse())
		{
			using (var reader = new StreamReader(response.GetResponseStream()))
			{
				var data = reader.ReadToEnd();
				response.Close();
				reader.Close();
				token = JsonConvert.DeserializeObject<BearerToken>(data);
			}
		}

		return token;
	}
}