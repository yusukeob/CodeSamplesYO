ALTER proc [dbo].[Quote_Insert]

		  @bidId int
		, @buyerCompanyId int
		, @sellerCompanyId int
		, @quoteRequestId int
		, @quoteRequestItemUniqueId int
		, @id int OUTPUT

AS


BEGIN


INSERT INTO [dbo].[Quotes]
		   (
		     [State]
		   , [BidId]
		   , [DateCreated]	
		   , [BuyerCompanyId]
		   , [SellerCompanyId]
		   , [QuoteRequestId]
		   , [QuoteRequestItemUniqueId]
           )

     VALUES
           (
		     1
		   , @bidId
		   , GETDATE()
		   , @buyerCompanyId
		   , @sellerCompanyId
		   , @quoteRequestId
		   , @quoteRequestItemUniqueId	   
		   )

		   
SET @id = SCOPE_IDENTITY()

END