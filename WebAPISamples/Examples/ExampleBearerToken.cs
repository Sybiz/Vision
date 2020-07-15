using Newtonsoft.Json;
using System.Net;
using System.IO;

public static class ExampleBearerToken
{
    //Change to valid username & password
    static string userName = "xxx";
    static string password = "yyy";

	public static BearerToken GetBearerToken()
	{
		BearerToken token = null;
		var jsonToken = $"grant_type=password&database=MobileRelease&username={userName}&password={password}"; 
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