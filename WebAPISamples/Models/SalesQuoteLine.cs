public class SalesQuoteLine
{
	public int Id { get; set; }
	public int AccountType { get; set; }
	public int Account { get; set; }
	public string Description { get; set; }
	public int UnitOfMeasure { get; set; }
	public decimal Quantity { get; set; }
	public decimal UnitChargeExclusive { get; set; }
	public int TaxCode { get; set; }
	public int Location { get; set; }
}