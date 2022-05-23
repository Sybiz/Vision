using Newtonsoft.Json;
using System.Text;
using System.Net;
using System.IO;

public static class ExampleBearerToken
{
	public static BearerToken GetBearerToken()
	{
		BearerToken token = null;
		var jsonToken = $"grant_type=password&database={CONFIG.DATABASE}&username={CONFIG.USERNAME}&password={CONFIG.PASSWORD}&keys={CONFIG.KEYS}";
		var requestBearer = (HttpWebRequest)WebRequest.Create($"{CONFIG.ADDRESS}/API/Bearer");
		requestBearer.ContentType = "application/x-www-form-urlencoded";
		requestBearer.ContentLength = Encoding.UTF8.GetBytes(jsonToken).Length;
		requestBearer.Timeout = 10000;
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