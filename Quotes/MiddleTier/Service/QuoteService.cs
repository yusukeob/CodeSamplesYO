using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Sabio.Web.Services
{
    public class QuoteService : BaseService, IQuoteService
    {

        public QRInfoAndBidDomain InfoGetByBidId(int BidId)
        {


            QRInfoAndBidDomain SingleQrBid = null;

            try
            {

                DataProvider.ExecuteCmd(GetConnection, "dbo.QRandQRIandCompanyandBidInfo_GetByBidId"
                  , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                  {
                      paramCollection.AddWithValue("@bidId", BidId);

                  }, map: delegate (IDataReader reader, short set)
                  {
                      int startingIndex = 0; //startingOrdinal

                      SingleQrBid = new QRInfoAndBidDomain();

                      SingleQrBid.BidId = reader.GetSafeInt32(startingIndex++);
                      SingleQrBid.BidAmount = reader.GetSafeDecimal(startingIndex++);
                      SingleQrBid.BidContractId = reader.GetSafeInt32(startingIndex++);
                      SingleQrBid.BidSubmittingCompanyId = reader.GetSafeInt32(startingIndex++);
                      SingleQrBid.BidReceivingCompanyId = reader.GetSafeInt32(startingIndex++);
                      SingleQrBid.BidCreatedDate = reader.GetSafeDateTime(startingIndex++);
                      SingleQrBid.BidUpdatedDate = reader.GetSafeDateTime(startingIndex++);
                      SingleQrBid.BidShippingAddressId = reader.GetSafeInt32(startingIndex++);
                      SingleQrBid.BidReceivingAddressId = reader.GetSafeInt32(startingIndex++);
                      SingleQrBid.BidExpirationDate = reader.GetSafeDateTime(startingIndex++);
                      SingleQrBid.BidStatusId = reader.GetSafeInt32(startingIndex++);
                      SingleQrBid.BidAddress1 = reader.GetSafeString(startingIndex++);
                      SingleQrBid.BidCity = reader.GetSafeString(startingIndex++);
                      SingleQrBid.BidState = reader.GetSafeString(startingIndex++);
                      SingleQrBid.BidZipCode = reader.GetSafeString(startingIndex++);
                      SingleQrBid.QRId = reader.GetSafeInt32(startingIndex++);
                      SingleQrBid.QuoteRequestName = reader.GetSafeString(startingIndex++);
                      SingleQrBid.QuoteRequestDueDate = reader.GetSafeDateTime(startingIndex++);
                      SingleQrBid.Address1 = reader.GetSafeString(startingIndex++);
                      SingleQrBid.City = reader.GetSafeString(startingIndex++);
                      SingleQrBid.State = reader.GetSafeString(startingIndex++);
                      SingleQrBid.ZipCode = reader.GetSafeString(startingIndex++);
                      SingleQrBid.QriUniqueId = reader.GetSafeInt32(startingIndex++);
                      SingleQrBid.QuoteRequestItemId = reader.GetSafeInt32(startingIndex++);
                      SingleQrBid.QuoteRequestItemName = reader.GetSafeString(startingIndex++);
                      SingleQrBid.Quantity = reader.GetSafeInt32(startingIndex++);
                      SingleQrBid.Unit = reader.GetSafeString(startingIndex++);
                      SingleQrBid.CompanyName = reader.GetSafeString(startingIndex++);

                  }
               );

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return SingleQrBid;
        }


        public int QuoteInsert(QuoteInsertRequest model)
        {
            int id = 0;

            try
            {
                DataProvider.ExecuteNonQuery(GetConnection, "dbo.Quote_Insert"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@bidId", model.BidId);
                    paramCollection.AddWithValue("@buyerCompanyId", model.BuyerCompanyId);
                    paramCollection.AddWithValue("@sellerCompanyId", model.SellerCompanyId);
                    paramCollection.AddWithValue("@quoteRequestId", model.QuoteRequestId);
                    paramCollection.AddWithValue("@quoteRequestItemUniqueId", model.QuoteRequestItemUniqueId);

                    SqlParameter p = new SqlParameter("@id", System.Data.SqlDbType.Int);
                    p.Direction = System.Data.ParameterDirection.Output;

                    paramCollection.Add(p);
                }, returnParameters: delegate (SqlParameterCollection param)
                {
                    int.TryParse(param["@id"].Value.ToString(), out id);
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return id;
        }


        public QuoteDomain QuoteGetByQuoteId(int QuoteId)
        {
            QuoteDomain quote = null;

            try
            {
                DataProvider.ExecuteCmd(GetConnection, "dbo.Quote_GetById"
                  , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                  {
                      paramCollection.AddWithValue("@Id", QuoteId);

                  }, map: delegate (IDataReader reader, short set)
                  {
                      int startingIndex = 0; //startingOrdinal

                      quote = new QuoteDomain();

                      quote.QuoteId = reader.GetSafeInt32(startingIndex++);
                      quote.QuoteState = (QuoteState)reader.GetSafeInt32(startingIndex++);
                      quote.BidId = reader.GetSafeInt32(startingIndex++);
                      quote.DateCreated = reader.GetSafeDateTime(startingIndex++);
                      quote.DateAccepted = reader.GetSafeDateTime(startingIndex++);
                      quote.BuyerCompanyId = reader.GetSafeInt32(startingIndex++);
                      quote.SellerCompanyId = reader.GetSafeInt32(startingIndex++);
                      quote.QuoteRequestId = reader.GetSafeInt32(startingIndex++);
                      quote.QuoteRequestItemUniqueId = reader.GetSafeInt32(startingIndex++);


                  }
               );
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return quote;
        }


        public int QuoteItemInsert(QuoteItemInsertRequest model)
        {
            int id = 0;

            try
            {
                DataProvider.ExecuteNonQuery(GetConnection, "dbo.QuoteItem_Insert"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@quoteId", model.QuoteId);
                    paramCollection.AddWithValue("@name", model.Name);
                    paramCollection.AddWithValue("@volume", model.Volume);
                    paramCollection.AddWithValue("@weight", model.Weight);
                    paramCollection.AddWithValue("@quantity", model.Quantity);
                    paramCollection.AddWithValue("@unit", model.Unit);

                    SqlParameter p = new SqlParameter("@id", System.Data.SqlDbType.Int);
                    p.Direction = System.Data.ParameterDirection.Output;

                    paramCollection.Add(p);
                }, returnParameters: delegate (SqlParameterCollection param)
                {
                    int.TryParse(param["@id"].Value.ToString(), out id);
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return id;
        }


        public List<QuoteItemDomain> QuoteItemsGetByQuoteId(int quoteId)
        {
            List<QuoteItemDomain> quoteItemList = null;
            QuoteItemDomain quoteItem = null;

            try
            {
                DataProvider.ExecuteCmd(GetConnection, "dbo.QuoteItems_GetAllByQuoteId"
                  , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                  {
                      paramCollection.AddWithValue("@QuoteId", quoteId);

                  }, map: delegate (IDataReader reader, short set)
                  {
                      int startingIndex = 0; //startingOrdinal

                      quoteItem = new QuoteItemDomain();

                      quoteItem.QuoteItemId = reader.GetSafeInt32(startingIndex++);
                      quoteItem.QuoteId = reader.GetSafeInt32(startingIndex++);
                      quoteItem.Name = reader.GetSafeString(startingIndex++);
                      quoteItem.Volume = reader.GetSafeInt32(startingIndex++);
                      quoteItem.Weight = reader.GetSafeInt32(startingIndex++);
                      quoteItem.Quantity = reader.GetSafeInt32(startingIndex++);
                      quoteItem.Unit = reader.GetSafeString(startingIndex++);


                      if (quoteItemList == null)
                      {
                          quoteItemList = new List<QuoteItemDomain>();
                      }

                      quoteItemList.Add(quoteItem);
                  }
               );
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return quoteItemList;
        }


        public List<QuoteDomain> QuoteGetByBuyerCompanyId(int buyerCompanyId)
        {
            List<QuoteDomain> quoteList = null;

            try
            {
                DataProvider.ExecuteCmd(GetConnection, "dbo.Quote_GetByBuyerCompanyId"
                  , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                  {
                      paramCollection.AddWithValue("@buyerCompanyId", buyerCompanyId);

                  }, map: delegate (IDataReader reader, short set)
                  {
                      int startingIndex = 0; //startingOrdinal

                      var quote = new QuoteDomain();

                      quote.QuoteId = reader.GetSafeInt32(startingIndex++);
                      quote.QuoteState = (QuoteState) reader.GetSafeInt32(startingIndex++);
                      quote.BidId = reader.GetSafeInt32(startingIndex++);
                      quote.DateCreated = reader.GetSafeDateTime(startingIndex++);
                      quote.DateAccepted = reader.GetSafeDateTime(startingIndex++);
                      quote.BuyerCompanyId = reader.GetSafeInt32(startingIndex++);
                      quote.SellerCompanyId = reader.GetSafeInt32(startingIndex++);
                      quote.BidAmount = reader.GetSafeDecimal(startingIndex++);
                      quote.QuoteRequestName = reader.GetSafeString(startingIndex++);
                      quote.QuoteRequestDueDate = reader.GetSafeDateTime(startingIndex++);
                      quote.QuoteRequestItemName = reader.GetSafeString(startingIndex++);
                      quote.BuyerCompanyName = reader.GetSafeString(startingIndex++);
                      quote.SellerCompanyName = reader.GetSafeString(startingIndex++);
                      quote.QuoteRequestId = reader.GetSafeInt32(startingIndex++);
                      quote.QuoteRequestItemUniqueId = reader.GetSafeInt32(startingIndex++);


                      quote.StateName = quote.QuoteState.ToString();                     



                      if (quoteList == null)
                      {
                          quoteList = new List<QuoteDomain>();
                      }

                      quoteList.Add(quote);
                  }
               );
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return quoteList;
        }






        public bool QuoteItemDelete(int quoteItemId)
        {
            bool success = false;

            try
            {
                DataProvider.ExecuteNonQuery(GetConnection, "dbo.QuoteItem_Delete"
                    , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                    {
                        paramCollection.AddWithValue("@Id", quoteItemId);

                        success = true;
                    });

                return success;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public QuoteDomain QuoteInfoForQuoteReviewGetByQuoteId(int QuoteId)
        {
            QuoteDomain quote = null;

            try
            {
                DataProvider.ExecuteCmd(GetConnection, "dbo.QuoteAndQuoteItemsAndBidInfo_GetByQuoteId"
                  , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                  {
                      paramCollection.AddWithValue("@QuoteId", QuoteId);

                  }, map: delegate (IDataReader reader, short set)
                  {
                      int startingIndex = 0; //startingOrdinal

                      quote = new QuoteDomain();

                      quote.QuoteId = reader.GetSafeInt32(startingIndex++);
                      quote.QuoteRequestId = reader.GetSafeInt32(startingIndex++);
                      quote.QuoteRequestItemUniqueId = reader.GetSafeInt32(startingIndex++);
                      quote.BidAmount = reader.GetSafeDecimal(startingIndex++);
                      quote.BidExpirationDate = reader.GetSafeDateTime(startingIndex++);
                      quote.QuoteRequestName = reader.GetSafeString(startingIndex++);
                      quote.QuoteRequestDueDate = reader.GetSafeDateTime(startingIndex++);
                      quote.QuoteRequestItemName = reader.GetSafeString(startingIndex++);
                      quote.SellerCompanyAddress1 = reader.GetSafeString(startingIndex++);
                      quote.SellerCompanyCity = reader.GetSafeString(startingIndex++);
                      quote.SellerCompanyState = reader.GetSafeString(startingIndex++);
                      quote.SellerCompanyZipCode = reader.GetSafeString(startingIndex++);
                      quote.BuyerCompanyAddress1 = reader.GetSafeString(startingIndex++);
                      quote.BuyerCompanyCity = reader.GetSafeString(startingIndex++);
                      quote.BuyerCompanyState = reader.GetSafeString(startingIndex++);
                      quote.BuyerCompanyZipCode = reader.GetSafeString(startingIndex++);
                      quote.BuyerCompanyName = reader.GetSafeString(startingIndex++);
                      quote.SellerCompanyName = reader.GetSafeString(startingIndex++);
                      quote.BuyerCompanyId = reader.GetSafeInt32(startingIndex++);
                      quote.SellerCompanyId = reader.GetSafeInt32(startingIndex++);
                      quote.QuoteItemList = QuoteItemsGetByQuoteId(QuoteId);


                  }
               );
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return quote;
        }


        public bool UpdateQuoteStatus(QuoteUpdateStatusRequest model)
        {
            bool success = false;

            try
            {
                DataProvider.ExecuteNonQuery(GetConnection, "dbo.Quote_Update_Status"
                   , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                   {
                       paramCollection.AddWithValue("@StateId", model.QuoteState);
                       paramCollection.AddWithValue("@Id", model.QuoteId);

                       success = true;
                   });
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return success;
        }








        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------
        //- Service Calls That Deal With State Machine
        //-----------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------



        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Get Current Status of a Quote by its QuoteId
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public QuoteDomain GetQuoteStateByQuoteId(int quoteId)
        {
            QuoteDomain quoteStatus = null;

            try
            {
                DataProvider.ExecuteCmd(GetConnection, "dbo.Quotes_GetStatusByQuoteId"
            , inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@QuoteId", quoteId);

            }, map: delegate (IDataReader reader, short set)
            {
                quoteStatus = new QuoteDomain();

                int startingIndex = 0; //startingOrdinal

                quoteStatus.QuoteId = reader.GetSafeInt32(startingIndex++);
                quoteStatus.QuoteState = (QuoteState)reader.GetSafeInt32(startingIndex++);

                // Passing through the values to the Domain Object
                quoteStatus.StateName = quoteStatus.QuoteState.ToString();

            });
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return quoteStatus;
        }

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


        public bool QuoteSMAttemptSubmit(UpdateQuoteStateMachineRequest updateModel)
        {
            bool success = false;
            // Cancelling is the right of the originator
            QuoteDomain quoteModel = QuoteGetByQuoteId(updateModel.QuoteId);

            // - this should be a check against the companyId instead of userId
            if (updateModel.CompanyId == quoteModel.SellerCompanyId)
            {
                try
                {
                    QuoteStateService StateHandler = new QuoteStateService(quoteModel);

                    // Verify this is valid action
                    StateHandler.StateMachine.Fire(QuoteEvent.Submit);

                    // Update Status on THIS model
                    quoteModel.QuoteState = QuoteState.Active;

                    // Update status in DB
                    QuoteUpdateStatusRequest QuoteUpdate = new QuoteUpdateStatusRequest
                    {
                        QuoteId = quoteModel.QuoteId,
                        QuoteState = quoteModel.QuoteState
                    };

                    UpdateQuoteStatus(QuoteUpdate);

                    success = true;
                }
                // - Implement a catch for InvalidOperationException that sets success to false
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return success;
        }


        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


        public bool QuoteSMAttemptAccept(UpdateQuoteStateMachineRequest updateModel)
        {
            bool success = false;
            // Cancelling is the right of the originator
            QuoteDomain quoteModel = QuoteGetByQuoteId(updateModel.QuoteId);

            // - this should be a check against the companyId instead of userId
            if (updateModel.CompanyId == quoteModel.BuyerCompanyId)
            {
                try
                {
                    var StateHandler = new QuoteStateService(quoteModel);

                    // Verify this is valid action
                    StateHandler.StateMachine.Fire(QuoteEvent.Accept);

                    // Update Status on THIS model
                    quoteModel.QuoteState = QuoteState.Approved;

                    // Update status in DB
                    QuoteUpdateStatusRequest QuoteUpdate = new QuoteUpdateStatusRequest
                    {
                        QuoteId = quoteModel.QuoteId,
                        QuoteState = quoteModel.QuoteState
                    };

                    UpdateQuoteStatus(QuoteUpdate);

                    success = true;
                }
                // - Implement a catch for InvalidOperationException that sets success to false
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return success;
        }


        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


        public bool QuoteSMAttemptAcceptedAddContract(UpdateQuoteStateMachineRequest updateModel)
        {
            bool success = false;
            // Cancelling is the right of the originator
            QuoteDomain quoteModel = QuoteGetByQuoteId(updateModel.QuoteId);

            //- this should be a check against the companyId instead of userId
            if (updateModel.CompanyId == quoteModel.BuyerCompanyId)
            {
                try
                {
                    var StateHandler = new QuoteStateService(quoteModel);

                    // Verify this is valid action
                    StateHandler.StateMachine.Fire(QuoteEvent.AcceptedAddContract);

                    // Update Status on THIS model
                    quoteModel.QuoteState = QuoteState.ApprovedAddContract;

                    // Update status in DB
                    QuoteUpdateStatusRequest QuoteUpdate = new QuoteUpdateStatusRequest
                    {
                        QuoteId = quoteModel.QuoteId,
                        QuoteState = quoteModel.QuoteState
                    };

                    UpdateQuoteStatus(QuoteUpdate);

                    success = true;
                }
                // - Implement a catch for InvalidOperationException that sets success to false
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return success;
        }


        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


        public bool QuoteSMAttemptContractAdded(UpdateQuoteStateMachineRequest updateModel)
        {
            bool success = false;
            // Cancelling is the right of the originator
            QuoteDomain quoteModel = QuoteGetByQuoteId(updateModel.QuoteId);

            // - this should be a check against the companyId instead of userId
            if (updateModel.CompanyId == quoteModel.BuyerCompanyId)
            {
                try
                {
                    var StateHandler = new QuoteStateService(quoteModel);

                    // Verify this is valid action
                    StateHandler.StateMachine.Fire(QuoteEvent.ContractAdded);

                    // Update Status on THIS model
                    quoteModel.QuoteState = QuoteState.ContractAddedForApproval;

                    // Update status in DB
                    QuoteUpdateStatusRequest QuoteUpdate = new QuoteUpdateStatusRequest
                    {
                        QuoteId = quoteModel.QuoteId,
                        QuoteState = quoteModel.QuoteState
                    };

                    UpdateQuoteStatus(QuoteUpdate);

                    success = true;
                }
                // - Implement a catch for InvalidOperationException that sets success to false
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return success;
        }


        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


        public bool QuoteSMAttemptAlter(UpdateQuoteStateMachineRequest updateModel)
        {
            bool success = false;
            // Cancelling is the right of the originator
            QuoteDomain quoteModel = QuoteGetByQuoteId(updateModel.QuoteId);

            // - this should be a check against the companyId instead of userId
            if (updateModel.CompanyId == quoteModel.BuyerCompanyId)
            {
                try
                {
                    var StateHandler = new QuoteStateService(quoteModel);

                    // Verify this is valid action
                    StateHandler.StateMachine.Fire(QuoteEvent.Alter);

                    // Update Status on THIS model
                    quoteModel.QuoteState = QuoteState.Alter;

                    // Update status in DB
                    QuoteUpdateStatusRequest QuoteUpdate = new QuoteUpdateStatusRequest
                    {
                        QuoteId = quoteModel.QuoteId,
                        QuoteState = quoteModel.QuoteState
                    };

                    UpdateQuoteStatus(QuoteUpdate);

                    success = true;
                }
                // - Implement a catch for InvalidOperationException that sets success to false
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return success;
        }


        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


        public bool QuoteSMAttemptDecline(UpdateQuoteStateMachineRequest updateModel)
        {
            bool success = false;
            // Cancelling is the right of the originator
            QuoteDomain quoteModel = QuoteGetByQuoteId(updateModel.QuoteId);

            //- this should be a check against the companyId instead of userId
            if (updateModel.CompanyId == quoteModel.BuyerCompanyId)
            {
                try
                {
                    var StateHandler = new QuoteStateService(quoteModel);

                    // Verify this is valid action
                    StateHandler.StateMachine.Fire(QuoteEvent.Decline);

                    // Update Status on THIS model
                    quoteModel.QuoteState = QuoteState.Declined;

                    // Update status in DB
                    QuoteUpdateStatusRequest QuoteUpdate = new QuoteUpdateStatusRequest
                    {
                        QuoteId = quoteModel.QuoteId,
                        QuoteState = quoteModel.QuoteState
                    };

                    UpdateQuoteStatus(QuoteUpdate);

                    success = true;
                }
                // - Implement a catch for InvalidOperationException that sets success to false
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return success;
        }


        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


        public bool QuoteSMAttemptContractApproved(UpdateQuoteStateMachineRequest updateModel)
        {
            bool success = false;
            // Cancelling is the right of the originator
            QuoteDomain quoteModel = QuoteGetByQuoteId(updateModel.QuoteId);

            //- this should be a check against the companyId instead of userId
            if (updateModel.CompanyId == quoteModel.SellerCompanyId)
            {
                try
                {
                    var StateHandler = new QuoteStateService(quoteModel);

                    // Verify this is valid action
                    StateHandler.StateMachine.Fire(QuoteEvent.ContractApproved);

                    // Update Status on THIS model
                    quoteModel.QuoteState = QuoteState.Completed;

                    // Update status in DB
                    QuoteUpdateStatusRequest QuoteUpdate = new QuoteUpdateStatusRequest
                    {
                        QuoteId = quoteModel.QuoteId,
                        QuoteState = quoteModel.QuoteState
                    };

                    UpdateQuoteStatus(QuoteUpdate);

                    success = true;
                }
                //-Implement a catch for InvalidOperationException that sets success to false
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return success;
        }


        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


        public bool QuoteSMAttemptComplete(UpdateQuoteStateMachineRequest updateModel)
        {
            bool success = false;
            // Cancelling is the right of the originator
            QuoteDomain quoteModel = QuoteGetByQuoteId(updateModel.QuoteId);

            //- this should be a check against the companyId instead of userId
            if (updateModel.CompanyId == quoteModel.BuyerCompanyId || updateModel.CompanyId == quoteModel.SellerCompanyId)
            {
                try
                {
                    var StateHandler = new QuoteStateService(quoteModel);

                    // Verify this is valid action
                    StateHandler.StateMachine.Fire(QuoteEvent.Complete);

                    // Update Status on THIS model
                    quoteModel.QuoteState = QuoteState.Completed;

                    // Update status in DB
                    QuoteUpdateStatusRequest QuoteUpdate = new QuoteUpdateStatusRequest
                    {
                        QuoteId = quoteModel.QuoteId,
                        QuoteState = quoteModel.QuoteState
                    };

                    UpdateQuoteStatus(QuoteUpdate);

                    success = true;
                }
                //-Implement a catch for InvalidOperationException that sets success to false
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return success;
        }

    }
}