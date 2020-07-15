public class CustomerSalesPrice
{
	public string PricingMethod { get; set; }
	public int TaxCodeId { get; set; }
	public int PriceScaleCurrencyId { get; set; }
	public decimal ExclusiveUnitPrice { get; set; }
	public decimal InclusiveUnitPrice { get; set; }
	public decimal DiscountPercentage { get; set; }
}