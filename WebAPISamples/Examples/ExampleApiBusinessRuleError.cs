using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System;

public class ExampleApiBusinessRuleError
{
	public void BusinessRuleError()
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
					var listError = JsonConvert.DeserializeObject<List<ApiBusinessRuleError>>(data);

					if (listError != null)
					{
						foreach (var error in listError)
						{
							Console.WriteLine($"Error with {error.PropertyName}");
						}
					}
				}
			}
		}
	}
}