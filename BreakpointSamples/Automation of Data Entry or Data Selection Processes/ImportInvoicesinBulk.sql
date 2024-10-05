--Sample query specifically made for ImportInvoicesinBulk.vb, found in the same folder as this.
--Created with a specific (real) scenario in mind, so naturally this query would require refinement to pull precisely what data per invoice is needed, as well as what invoices are needed (specific date range, btach range...etc.)

SELECT 
  pi.PurchaseInvoiceId, 
  pi.SupplierId, 
  pi.PurchaseInvoiceNumber, 
  pi.Reference, 
  pi.TransactionDate, 
  pi.DeliveryDate, 
  pil.PurchaseInvoiceLineId, 
  pil.PurchaseLineTypeId, 
  pil.SourceDocumentId, 
  pil.SourceOrderLineId, 
  pil.SourceDocumentType, 
  pil.[LineNo], 
  pil.[Description], 
  pil.PurchaseInvoiceLineId, 
  pil.AccountId, 
  pil.ProductId, 
  pil.JobId, 
  pil.QuantityOrder, 
  pil.QuantityInvoice, 
  pil.QuantityDeliver, 
  pil.ForeignUnitCost, 
  pil.TaxCodeId
FROM 
  cr.PurchaseInvoice pi
INNER JOIN 
  cr.purchaseinvoiceline pil ON pi.PurchaseInvoiceId = pil.PurchaseInvoiceId
