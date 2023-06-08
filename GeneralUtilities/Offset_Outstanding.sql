
--Queries to assist in finding transactions where Outstanding amount is not as expected when comparing with offsets completed.
--Running this query and getting results is NOT definitive evidence of any particular conclusion, but should precipitate some further investigation, followed by the creation of a Support Request if that further investigation reveals unexplainable oddities.

--Queries as currently written resolve explicitly around trn.Post and Offset tables, looking at sources of transactions may be pertinent (such as Cashbooks, quick receipts, invoices, etc.)


SELECT TransactionId AS TransactionId, OF1.TransactionId1, OF2.TransactionId2, SupplierId, ForeignCredit-ForeignDebit AS ForeignAmount, ForeignOutstanding, ForeignOutstanding - ISNULL(OF1.ForeignOffset,0) + ISNULL(OF2.ForeignOffset,0) AS CalculatedForeignOutstanding
FROM [trn].[PostCR] 
LEFT OUTER JOIN (SELECT TransactionId1, SUM(Amount) AS ForeignOffset FROM [cr].[Offset] GROUP BY TransactionId1) OF1 ON OF1.TransactionId1 = TransactionId
LEFT OUTER JOIN (SELECT TransactionId2, SUM(Amount) AS ForeignOffset FROM [cr].[Offset] GROUP BY TransactionId2) OF2 ON OF2.TransactionId2 = TransactionId
WHERE ForeignCredit-ForeignDebit <> ForeignOutstanding - ISNULL(OF1.ForeignOffset,0) + ISNULL(OF2.ForeignOffset,0)

SELECT TransactionId, CustomerId, ForeignDebit-ForeignCredit, ForeignOutstanding, ForeignOutstanding - ISNULL(OF1.ForeignOffset,0) + ISNULL(OF2.ForeignOffset,0)
FROM [trn].[PostDR] 
LEFT OUTER JOIN (SELECT TransactionId1, SUM(Amount) AS ForeignOffset FROM [dr].[Offset] GROUP BY TransactionId1) OF1 ON OF1.TransactionId1 = TransactionId
LEFT OUTER JOIN (SELECT TransactionId2, SUM(Amount) AS ForeignOffset FROM [dr].[Offset] GROUP BY TransactionId2) OF2 ON OF2.TransactionId2 = TransactionId
WHERE ForeignDebit-ForeignCredit <> ForeignOutstanding - ISNULL(OF1.ForeignOffset,0) + ISNULL(OF2.ForeignOffset,0)
