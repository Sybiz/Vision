using System.Collections.Generic;

public class SalesQuote
{
	public int Id { get; set; }
	public int OrderCustomer { get; set; }
	public string TransactionNumber { get; set; }
	public string Reference { get; set; }
	public List<SalesQuoteLine> Lines { get; set; }

	public SalesQuote()
	{
		Lines = new List<SalesQuoteLine>();
	}
}