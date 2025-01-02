--Queries to assist in finding transactions where Outstanding amount is not as expected when comparing with offsets completed.
--Running this query and getting results is NOT definitive evidence of any particular conclusion, but should precipitate some further investigation, followed by the creation of a Support Request if that further investigation reveals unexplainable oddities.

--Queries as currently written revolve around offsets, PostDR/CR and also discounts. Discount calculation is not perfect; it's very tough to consider a "blend" of discounts when they appear on both the cashbook line and line offset.
--(Effort to reward ratio is not there to come up with a perfect solution, considering that this situation shouldn't be coming up at all post 24.00!)
--However, if running the non-discount variant of this query and getting a ton of unexpected results, this may be worth running to try weed out a lot of the chaff...
--IT IS STRONGLY RECOMMEND TO RUN THE NON-DISCOUNT VARIANT FIRST- https://github.com/Sybiz/Vision/blob/master/GeneralUtilities/Offset_Outstanding.sql


SELECT TransactionId AS TransactionId, cbl.CashbookLineId, CLOD.Discount, OF1.TransactionId1, OF2.TransactionId2, c.SupplierId, ForeignCredit-ForeignDebit AS ForeignAmount, ForeignOutstanding, 
ForeignOutstanding - ISNULL(OF1.ForeignOffset,0) + ISNULL(OF2.ForeignOffset,0) + 
IIF(ISNULL(cbl.ForeignDiscountAmount,0) + ISNULL(cbl.ForeignDeposit,0) + ISNULL(cbl.ForeignPayment,0) = ForeignOutstanding - ISNULL(OF1.ForeignOffset,0) + ISNULL(OF2.ForeignOffset,0), 
IIF(ISNULL(cbl.ForeignDiscountAmount,0) + ISNULL(cbl.ForeignDeposit,0) + ISNULL(cbl.ForeignPayment,0) = -(ForeignOutstanding - ISNULL(OF1.ForeignOffset,0) + ISNULL(OF2.ForeignOffset,0)), 
0, 
IIF(ISNULL(cbl.ForeignDiscountAmount,0) <> 0, 0, ISNULL(CLOD.Discount,0))),
IIF(ISNULL(cbl.ForeignDiscountAmount,0) <> 0, 0, ISNULL(CLOD.Discount,0))) AS CalculatedForeignOutstanding
FROM [trn].[PostCR] c
LEFT OUTER JOIN (SELECT TransactionId1, SUM(Amount) AS ForeignOffset FROM [cr].[Offset] GROUP BY TransactionId1) OF1 ON OF1.TransactionId1 = TransactionId
LEFT OUTER JOIN (SELECT TransactionId2, SUM(Amount) AS ForeignOffset FROM [cr].[Offset] GROUP BY TransactionId2) OF2 ON OF2.TransactionId2 = TransactionId
LEFT OUTER JOIN cb.CashbookLine cbl ON c.SourceId = cbl.CashbookLineId AND c.ParentSourceTransactionTypeId = 500
LEFT OUTER JOIN (SELECT CashbookLineId, SUM(DiscountAmount) AS Discount FROM cb.CashbookLineOffset GROUP BY CashbookLineId) CLOD ON cbl.CashbookLineId = CLOD.CashbookLineId
WHERE ForeignCredit-ForeignDebit <> ForeignOutstanding - ISNULL(OF1.ForeignOffset,0) + ISNULL(OF2.ForeignOffset,0) + 
IIF(-(ISNULL(cbl.ForeignDiscountAmount,0) + ISNULL(cbl.ForeignDeposit,0) + ISNULL(cbl.ForeignPayment,0)) = ForeignOutstanding - ISNULL(OF1.ForeignOffset,0) + ISNULL(OF2.ForeignOffset,0), 
IIF(ISNULL(cbl.ForeignDiscountAmount,0) + ISNULL(cbl.ForeignDeposit,0) + ISNULL(cbl.ForeignPayment,0) = -(ForeignOutstanding - ISNULL(OF1.ForeignOffset,0) + ISNULL(OF2.ForeignOffset,0)), 
0, 
IIF(ISNULL(cbl.ForeignDiscountAmount,0) <> 0, 0, ISNULL(CLOD.Discount,0))),
IIF(ISNULL(cbl.ForeignDiscountAmount,0) <> 0, 0, ISNULL(CLOD.Discount,0)))


SELECT TransactionId, cbl.CashbookLineId, CLOD.Discount, d.CustomerId, ForeignDebit-ForeignCredit, ForeignOutstanding, 
ForeignOutstanding - ISNULL(OF1.ForeignOffset,0) + ISNULL(OF2.ForeignOffset,0) + 
IIF(-(ISNULL(cbl.ForeignDiscountAmount,0) + ISNULL(cbl.ForeignDeposit,0) + ISNULL(cbl.ForeignPayment,0)) = ForeignOutstanding - ISNULL(OF1.ForeignOffset,0) + ISNULL(OF2.ForeignOffset,0), 
IIF(ISNULL(cbl.ForeignDiscountAmount,0) + ISNULL(cbl.ForeignDeposit,0) + ISNULL(cbl.ForeignPayment,0) = -(ForeignOutstanding - ISNULL(OF1.ForeignOffset,0) + ISNULL(OF2.ForeignOffset,0)), 
0, 
IIF(ISNULL(cbl.ForeignDiscountAmount,0) <> 0, 0, ISNULL(CLOD.Discount,0))),
IIF(ISNULL(cbl.ForeignDiscountAmount,0) <> 0, 0, ISNULL(CLOD.Discount,0)))
FROM [trn].[PostDR] d
LEFT OUTER JOIN (SELECT TransactionId1, SUM(Amount) AS ForeignOffset FROM [dr].[Offset] GROUP BY TransactionId1) OF1 ON OF1.TransactionId1 = TransactionId
LEFT OUTER JOIN (SELECT TransactionId2, SUM(Amount) AS ForeignOffset FROM [dr].[Offset] GROUP BY TransactionId2) OF2 ON OF2.TransactionId2 = TransactionId
LEFT OUTER JOIN cb.CashbookLine cbl ON d.SourceId = cbl.CashbookLineId AND d.ParentSourceTransactionTypeId = 500
LEFT OUTER JOIN (SELECT CashbookLineId, SUM(DiscountAmount) AS Discount FROM cb.CashbookLineOffset GROUP BY CashbookLineId) CLOD ON cbl.CashbookLineId = CLOD.CashbookLineId
WHERE ForeignDebit-ForeignCredit <> ForeignOutstanding - ISNULL(OF1.ForeignOffset,0) + ISNULL(OF2.ForeignOffset,0) + 
IIF(-(ISNULL(cbl.ForeignDiscountAmount,0) + ISNULL(cbl.ForeignDeposit,0) + ISNULL(cbl.ForeignPayment,0)) = ForeignOutstanding - ISNULL(OF1.ForeignOffset,0) + ISNULL(OF2.ForeignOffset,0), 
IIF(ISNULL(cbl.ForeignDiscountAmount,0) + ISNULL(cbl.ForeignDeposit,0) + ISNULL(cbl.ForeignPayment,0) = -(ForeignOutstanding - ISNULL(OF1.ForeignOffset,0) + ISNULL(OF2.ForeignOffset,0)), 
0, 
IIF(ISNULL(cbl.ForeignDiscountAmount,0) <> 0, 0, ISNULL(CLOD.Discount,0))),
IIF(ISNULL(cbl.ForeignDiscountAmount,0) <> 0, 0, ISNULL(CLOD.Discount,0)))
