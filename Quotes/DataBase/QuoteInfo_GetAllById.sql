ALTER proc [dbo].[QuoteAndQuoteItemsAndBidInfo_GetByQuoteId]/*this is the name of your SQL function, CREATE FIRST then ALTER*/
	@QuoteId int
AS

BEGIN

SELECT q.[Id]
	,q.QuoteRequestId
	,q.QuoteRequestItemUniqueId
	,b.Amount
	,b.ExpirationDate
	,qr.Name AS quoteRequestName
	,qr.DueDate
	,qri.Name AS quoteRequestItemName
	,aShipFrom.Address1 AS shipFromAddress1
	,aShipFrom.City AS shipFromCity
	,aShipFrom.[State] AS shipFromState
	,aShipFrom.ZipCode AS shipFromZipCode
	,aShipTo.Address1 AS shipToAddress1
	,aShipTo.City AS shipToCity
	,aShipTo.[State] AS shipToState
	,aShipTo.ZipCode AS shipToZipCode
	,buyerCompany.name AS buyerCompanyName
	,sellerCompany.name AS sellerCompanyName
	,q.BuyerCompanyId
	,q.SellerCompanyId


FROM dbo.Quotes AS q
LEFT JOIN dbo.Bids AS b
ON q.bidId = b.Id
LEFT JOIN dbo.QuoteRequests AS qr
ON b.QuoteRequestId = qr.Id
LEFT JOIN dbo.QuoteRequestItems AS qri
ON b.QriUniqueId = qri.Id
LEFT JOIN dbo.Addresses AS aShipFrom
ON b.ShippingAddressId = aShipFrom.Id
LEFT JOIN dbo.Addresses AS aShipTo
ON b.ReceivingAddressId = aShipTo.Id
LEFT JOIN dbo.Company AS buyerCompany
ON q.BuyerCompanyId = buyerCompany.id
LEFT JOIN dbo.Company AS sellerCompany
ON q.SellerCompanyId = sellerCompany.id
WHERE q.[Id] = @QuoteId;

END