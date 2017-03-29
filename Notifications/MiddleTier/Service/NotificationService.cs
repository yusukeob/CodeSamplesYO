using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace Sabio.Web.Services
{
    public class NotificationService : BaseService, INotificationService
    {
        [Dependency]
        public ICompanyService _CompanyService { get; set; }

        [Dependency]
        public IUserEmailService _UserEmailService { get; set; }

        [Dependency]
        public IBidService _BidService { get; set; }

        [Dependency]
        public IQuoteService _QuoteService { get; set; }

        [Dependency]
        public IUserProfileService _UserProfileService { get; set; }

        // ++++++++++++++++++++++++++++++++++++++++++++++++
        // Send company a notification that another company has bid on their quote request

        public void NotifyCompanyOfNewBidOnQuoteRequestItem(int bidId)
        {
            BidDomain bid = _BidService.BidGetById(bidId);

            CompanyDomain company = _CompanyService.GetByIdCompany(bid.ReceivingCompanyId);
            CompanyDomain company2 = _CompanyService.GetByIdCompany(bid.SubmittingCompanyId);
            string BidAmount = String.Format("{0:0.00}", bid.Amount);


            if (company != null)
            {
                try
                {
                    EmailRequest notification = new EmailRequest();

                    notification.UserEmail = company.Email;
                    notification.Subject = "New Bid On Your Quote Request";
                    notification.Content = "There was a new bid on your quote request: " + bid.QrName + ". " + company2.Name + " submitted a bid of $" + BidAmount + " on the quote request item " + bid.QriName +
                                           ". The provided shipping address was " + bid.Address1 + ", " + bid.City + ", " + bid.State + " " + bid.ZipCode + ". Log into your QuoteMule account(http://quotemule.dev/) to view and take action.";


                    _UserEmailService.SendEmail(notification);


                    NotificationInsertRequest notificationModel = new NotificationInsertRequest();

                    notificationModel.UserId = company.OwnerId;
                    notificationModel.Category = "New Bid On Your Quote Request";
                    notificationModel.Message1 = company2.Name + " submitted a bid on your quote request: " + bid.QrName + ". Click ";
                    notificationModel.Link = "/quoterequest/manage/" + bid.QuoteRequestId;
                    notificationModel.Message2 = "here";
                    notificationModel.Message3 = " to view it.";
                    notificationModel.Is_Read = false;

                    NotificationInsert(notificationModel);


                    NotificationInsertRequest toastrModel = new NotificationInsertRequest();

                    toastrModel.UserId = company.OwnerId;
                    toastrModel.Category = "New Bid On Your Quote Request";
                    toastrModel.Message1 = "<a href='/quoterequest/manage/" + bid.QuoteRequestId + "#/active'>" + company2.Name + " submitted a bid on your quote request: " + bid.QrName + ".</a>";
                    toastrModel.Is_Read = false;


                    UserProfileService UserProfile = new UserProfileService();

                    List<CompanyEmployeeDomain> notificationList = UserProfile.GetAllEmployees(bid.ReceivingCompanyId);


                    NotifyAllCompanyUsers(toastrModel, notificationList);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }




        public bool NotifySellerCompanyOfAcceptedBid(int bidId)
        {
            bool Success = false;

            BidDomain bid = _BidService.BidGetById(bidId);
            int QuoteRequestId = bid.QuoteRequestId;

            CompanyDomain buyerCompany = _CompanyService.GetByIdCompany(bid.ReceivingCompanyId);
            CompanyDomain supplierCompany = _CompanyService.GetByIdCompany(bid.SubmittingCompanyId);
            string link = "/quoterequest/manage/" + QuoteRequestId;

            try
            {
                NotificationInsertRequest model = new NotificationInsertRequest();

                model.UserId = supplierCompany.OwnerId;
                model.Category = "Bid Accepted";
                model.Message1 = buyerCompany.Name + " has accepted your bid! ";
                model.Link = link;
                model.Message2 = "here";
                model.Message3 = " to convert to quote.";
                model.Is_Read = false;

                NotificationInsert(model);


                NotificationInsertRequest toastrModel = new NotificationInsertRequest();

                toastrModel.UserId = supplierCompany.OwnerId;
                toastrModel.Category = "Bid Accepted";
                toastrModel.Message1 = "<a href='/quoterequest/manage/" + bid.QuoteRequestId + "#/pending'><div>" + buyerCompany.Name + " has accepted your bid! Click here!</div></a>";

                UserProfileService UserProfile = new UserProfileService();

                List<CompanyEmployeeDomain> notificationList = UserProfile.GetAllEmployees(bid.SubmittingCompanyId);

                NotifyAllCompanyUsers(toastrModel, notificationList);

                Success = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Success;
        }


        public bool NotifySellerCompanyOfSubmittedContract(int quoteId)
        {
            bool Success = false;

            QuoteDomain quote = _QuoteService.QuoteGetByQuoteId(quoteId);
            CompanyDomain buyerCompany = _CompanyService.GetByIdCompany(quote.BuyerCompanyId);
            CompanyDomain sellerCompany = _CompanyService.GetByIdCompany(quote.SellerCompanyId);
            string link = "/quote/manage/" + quote.QuoteId;

            try
            {
                NotificationInsertRequest model = new NotificationInsertRequest();

                model.UserId = sellerCompany.OwnerId;
                model.Category = "Review Quote";
                model.Message1 = buyerCompany.Name + " submitted a contract for your quote. Click ";
                model.Link = link;
                model.Message2 = "here";
                model.Message3 = " to view.";
                model.Is_Read = false;

                NotificationInsert(model);


                NotificationInsertRequest toastrModel = new NotificationInsertRequest();

                toastrModel.UserId = sellerCompany.OwnerId;
                toastrModel.Category = "Contract Submitted";
                toastrModel.Message1 = "<a href='/quote/manage/" + quote.QuoteId + "'><div>" + buyerCompany.Name + " has submitted a contract for your quote! Click here to view!</div></a>";

                UserProfileService UserProfile = new UserProfileService();

                List<CompanyEmployeeDomain> notificationList = UserProfile.GetAllEmployees(quote.SellerCompanyId);

                NotifyAllCompanyUsers(toastrModel, notificationList);


                Success = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Success;
        }



        public bool NotifyBuyerCompanyOfApprovedContract(int quoteId)
        {
            bool Success = false;

            QuoteDomain quote = _QuoteService.QuoteGetByQuoteId(quoteId);
            CompanyDomain buyerCompany = _CompanyService.GetByIdCompany(quote.BuyerCompanyId);
            CompanyDomain sellerCompany = _CompanyService.GetByIdCompany(quote.SellerCompanyId);

            try
            {
                NotificationInsertRequest model = new NotificationInsertRequest();

                model.UserId = buyerCompany.OwnerId;
                model.Category = "Quote Completed";
                model.Message1 = sellerCompany.Name + " accepted the contract. Quote is completed!";
                model.Is_Read = false;

                NotificationInsert(model);


                NotificationInsertRequest toastrModel = new NotificationInsertRequest();

                toastrModel.UserId = buyerCompany.OwnerId;
                toastrModel.Category = "Contract Approved";
                toastrModel.Message1 = sellerCompany.Name + " has accepted the contract. Quote is completed!</div></a>";

                UserProfileService UserProfile = new UserProfileService();

                List<CompanyEmployeeDomain> notificationList = UserProfile.GetAllEmployees(quote.BuyerCompanyId);

                NotifyAllCompanyUsers(toastrModel, notificationList);



                Success = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Success;
        }


        public bool NotifyBuyerCompanyOfSubmittedQuote(int quoteId)
        {
            bool Success = false;

            QuoteDomain quote = _QuoteService.QuoteGetByQuoteId(quoteId);
            CompanyDomain buyerCompany = _CompanyService.GetByIdCompany(quote.BuyerCompanyId);
            CompanyDomain sellerCompany = _CompanyService.GetByIdCompany(quote.SellerCompanyId);
            string link = "/quote/manage/" + quote.QuoteId;

            try
            {
                NotificationInsertRequest model = new NotificationInsertRequest();

                model.UserId = buyerCompany.OwnerId;
                model.Category = "Quote Submitted";
                model.Message1 = sellerCompany.Name + " submitted a quote. Click ";
                model.Link = link;
                model.Message2 = "here";
                model.Message3 = " to review.";
                model.Is_Read = false;

                NotificationInsert(model);


                NotificationInsertRequest toastrModel = new NotificationInsertRequest();

                toastrModel.UserId = buyerCompany.OwnerId;
                toastrModel.Category = "Quote Submitted";
                toastrModel.Message1 = "<a href='/quote/manage/" + quote.QuoteId + "'><div>" + sellerCompany.Name + " has submitted a quote! Click here!</div></a>";

                UserProfileService UserProfile = new UserProfileService();

                List<CompanyEmployeeDomain> notificationList = UserProfile.GetAllEmployees(quote.BuyerCompanyId);

                NotifyAllCompanyUsers(toastrModel, notificationList);


                Success = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Success;
        }



        public bool NotifySellerCompanyOfAcceptedQuote(int quoteId)
        {
            bool Success = false;

            QuoteDomain quote = _QuoteService.QuoteGetByQuoteId(quoteId);
            CompanyDomain buyerCompany = _CompanyService.GetByIdCompany(quote.BuyerCompanyId);
            CompanyDomain sellerCompany = _CompanyService.GetByIdCompany(quote.SellerCompanyId);

            try
            {
                NotificationInsertRequest model = new NotificationInsertRequest();

                model.UserId = sellerCompany.OwnerId;
                model.Category = "Quote Accepted";
                model.Message1 = buyerCompany.Name + " has accepted your quote!";
                model.Is_Read = false;

                NotificationInsert(model);


                NotificationInsertRequest toastrModel = new NotificationInsertRequest();

                toastrModel.UserId = sellerCompany.OwnerId;
                toastrModel.Category = "Quote Accepted";
                toastrModel.Message1 = buyerCompany.Name + " has accepted your quote!";

                UserProfileService UserProfile = new UserProfileService();

                List<CompanyEmployeeDomain> notificationList = UserProfile.GetAllEmployees(quote.SellerCompanyId);

                NotifyAllCompanyUsers(toastrModel, notificationList);


                Success = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Success;
        }



        public int NotificationInsert(NotificationInsertRequest model)
        {
            int id = 0;

            try
            {
                DataProvider.ExecuteNonQuery(GetConnection, "dbo.Notification_Insert"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                    {
                        paramCollection.AddWithValue("@userId", model.UserId);
                        paramCollection.AddWithValue("@category", model.Category);
                        paramCollection.AddWithValue("@message1", model.Message1);
                        paramCollection.AddWithValue("@is_read", model.Is_Read);
                        paramCollection.AddWithValue("@link", model.Link);
                        paramCollection.AddWithValue("@message2", model.Message2);
                        paramCollection.AddWithValue("@message3", model.Message3);


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


        public void NotifyAllCompanyUsers(NotificationInsertRequest model, List<CompanyEmployeeDomain> NotificationList)
        {
            //Instantialize a new NotifySmsRequest
            NotifySMSRequest companyUser = new NotifySMSRequest();
            EmployeeProfileRequest Employee = new EmployeeProfileRequest();
            Employee.companyId = model.CompanyId;

            //List<CompanyEmployeeDomain> notificationList = _UserProfileService.GetEmployeesByCompanyId(Employee.companyId);


            foreach (CompanyEmployeeDomain employee in NotificationList)
            {
                model.UserId = employee.UserId;
                model.CompanyId = employee.CompanyId;



                companyUser.Phone = employee.PhoneNumber;

                //Send out text message
                try
                {
                    NotifySMSService.SendBidNotification(companyUser);
                }

                catch (ArgumentException)
                {
                    companyUser.Phone = "";
                }

                catch (Exception)
                {

                }
                
                SignalRHub.SendNotification(model);

            }

        }



        public List<NotificationDomain> Notifications_GetByUserId(string userId)
        {
            List<NotificationDomain> notificationList = null;
            NotificationDomain notification = null;

            try
            {
                DataProvider.ExecuteCmd(GetConnection, "dbo.Notifications_GetByUserId"
                    , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                    {
                        paramCollection.AddWithValue("@userId", userId);
                    }, map: delegate (IDataReader reader, short set)
                    {
                        int startingIndex = 0;

                        notification = new NotificationDomain();

                        notification.NotificationId = reader.GetSafeInt32(startingIndex++);
                        notification.UserId = reader.GetSafeString(startingIndex++);
                        notification.DateCreated = reader.GetSafeDateTime(startingIndex++);
                        notification.DateUpdated = reader.GetSafeDateTime(startingIndex++);
                        notification.Category = reader.GetSafeString(startingIndex++);
                        notification.Message1 = reader.GetSafeString(startingIndex++);
                        notification.Is_Read = reader.GetSafeBool(startingIndex++);
                        notification.Link = reader.GetSafeString(startingIndex++);
                        notification.Message2 = reader.GetSafeString(startingIndex++);
                        notification.Message3 = reader.GetSafeString(startingIndex++);

                        if (notificationList == null)
                        {
                            notificationList = new List<NotificationDomain>();
                        }

                        notificationList.Add(notification);
                    });
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return notificationList;
        }


        public List<NotificationDomain> Notifications_Unread_GetByUserId(string userId)
        {
            List<NotificationDomain> notificationList = null;
            NotificationDomain notification = null;

            try
            {
                DataProvider.ExecuteCmd(GetConnection, "dbo.Notifications_Unread_GetByUserId"
                    , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                    {
                        paramCollection.AddWithValue("@userId", userId);
                    }, map: delegate (IDataReader reader, short set)
                    {
                        int startingIndex = 0;

                        notification = new NotificationDomain();

                        notification.NotificationId = reader.GetSafeInt32(startingIndex++);
                        notification.UserId = reader.GetSafeString(startingIndex++);
                        notification.DateCreated = reader.GetSafeDateTime(startingIndex++);
                        notification.DateUpdated = reader.GetSafeDateTime(startingIndex++);
                        notification.Category = reader.GetSafeString(startingIndex++);
                        notification.Message1 = reader.GetSafeString(startingIndex++);
                        notification.Is_Read = reader.GetSafeBool(startingIndex++);
                        notification.Link = reader.GetSafeString(startingIndex++);
                        notification.Message2 = reader.GetSafeString(startingIndex++);
                        notification.Message3 = reader.GetSafeString(startingIndex++);

                        if (notificationList == null)
                        {
                            notificationList = new List<NotificationDomain>();
                        }

                        notificationList.Add(notification);
                    });
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return notificationList;
        }


        public bool NotificationDelete(int Id)
        {
            bool success = false;

            try
            {
                DataProvider.ExecuteNonQuery(GetConnection, "dbo.Notification_Delete"
                    , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                    {
                        paramCollection.AddWithValue("@id", Id);

                        success = true;
                    });

                return success;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool NotificationUpdateAsRead(int Id)
        {
            bool result = false;

            try
            {
                DataProvider.ExecuteNonQuery(GetConnection, "dbo.Notification_Update_MarkAsRead"
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@id", Id);

                    result = true;
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }



        public List<NotificationDomain> Notifications_GetByTimeAndUserId(DateTime time, string userId)
        {
            List<NotificationDomain> notificationList = null;
            NotificationDomain notification = null;

            try
            {
                DataProvider.ExecuteCmd(GetConnection, "dbo.Notifications_GetByTimeAndUserId"
                    , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                    {
                        paramCollection.AddWithValue("@time", time);
                        paramCollection.AddWithValue("@userId", userId);
                    }, map: delegate (IDataReader reader, short set)
                    {
                        int startingIndex = 0;

                        notification = new NotificationDomain();

                        notification.NotificationId = reader.GetSafeInt32(startingIndex++);
                        notification.UserId = reader.GetSafeString(startingIndex++);
                        notification.DateCreated = reader.GetSafeDateTime(startingIndex++);
                        notification.DateUpdated = reader.GetSafeDateTime(startingIndex++);
                        notification.Category = reader.GetSafeString(startingIndex++);
                        notification.Message1 = reader.GetSafeString(startingIndex++);
                        notification.Is_Read = reader.GetSafeBool(startingIndex++);
                        notification.Link = reader.GetSafeString(startingIndex++);
                        notification.Message2 = reader.GetSafeString(startingIndex++);
                        notification.Message3 = reader.GetSafeString(startingIndex++);

                        if (notificationList == null)
                        {
                            notificationList = new List<NotificationDomain>();
                        }

                        notificationList.Add(notification);
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                notificationList = null;
            }

            return notificationList;
        }

    }

}