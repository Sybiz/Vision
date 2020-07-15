using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

public class ExampleApiBasicError
{
	public void BasicError()
	{
		try
		{
			// Code that encounters error from the API
		}
		catch (WebException ex)
		{
			var response = (HttpWebResponse)ex.Response;

			if (response != null)
			{
				using (var reader = new StreamReader(response.GetResponseStream()))
				{
					var data = reader.ReadToEnd();
					var error = JsonConvert.DeserializeObject<ApiBasicError>(data);

					if (error != null)
					{
						Console.WriteLine($"Encountered {error.Type}");
					}
				}
			}
		}
	}
}