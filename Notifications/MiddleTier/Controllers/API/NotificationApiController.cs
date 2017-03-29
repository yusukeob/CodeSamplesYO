using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sabio.Web.Controllers.Api
{
    [RoutePrefix("api/notification")]
    public class NotificationApiController : ApiController
    {

        [Dependency]
        public INotificationService _NotificationOptionService { get; set; }

        // ++++++++++++++++++++++++++++++++++++++++++++++++
        // Send company a notification that another company has bid on their quote request
        [Route("receivingCompany/{BidId:int}"), HttpPost]
        public HttpResponseMessage NotifyReceivingCompanyOfNewBid([FromUri]NotificationInsertRequest model)
        {
            if (!ModelState.IsValid)
            {
                Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            bool success = false;

            _NotificationOptionService.NotifyCompanyOfNewBidOnQuoteRequestItem(model.BidId);

            success = true;

            ItemResponse<bool> response = new ItemResponse<bool>();

            response.Item = success;

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }


        [Route("{userId}"), HttpGet]

        public HttpResponseMessage NotificationsGetByUserId(string userId)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            List<NotificationDomain> notificationList = _NotificationOptionService.Notifications_GetByUserId(userId);

            ItemsResponse<NotificationDomain> response = new ItemsResponse<NotificationDomain>();

            response.Items = notificationList;

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }


        [Route("unread/{userId}"), HttpGet]

        public HttpResponseMessage NotificationsUnreadGetByUserId(string userId)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            List<NotificationDomain> notificationList = _NotificationOptionService.Notifications_Unread_GetByUserId(userId);

            ItemsResponse<NotificationDomain> response = new ItemsResponse<NotificationDomain>();

            response.Items = notificationList;

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }


        [Route("{Id}"), HttpDelete]
        public HttpResponseMessage NotificationDelete(int Id)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            bool isSuccessful = _NotificationOptionService.NotificationDelete(Id);

            ItemResponse<bool> response = new ItemResponse<bool>();

            response.Item = isSuccessful;

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        [Route("{Id}"), HttpPut]
        public HttpResponseMessage NotificationUpdateAsRead(int Id)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            bool isSuccessful = _NotificationOptionService.NotificationUpdateAsRead(Id);

            ItemResponse<bool> response = new ItemResponse<bool>();

            response.Item = isSuccessful;

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        [Route("time/{userId}"), HttpGet]

        public HttpResponseMessage NotificationsUnreadGetByTimeAndUserId(string userId)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            DateTime time = DateTime.Now;
            DateTime tMinus10 = time.AddSeconds(-10);

            List<NotificationDomain> notificationList = _NotificationOptionService.Notifications_GetByTimeAndUserId(tMinus10, userId);

            ItemsResponse<NotificationDomain> response = new ItemsResponse<NotificationDomain>();

            response.Items = notificationList;

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }


        [Route("quoteSubmitted/{quoteId}"), HttpPost]
        public HttpResponseMessage NotifyBuyerCompanyOfSubmittedQuote(int quoteId)
        {
            if (!ModelState.IsValid)
            {
                Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            bool success = false;

            _NotificationOptionService.NotifyBuyerCompanyOfSubmittedQuote(quoteId);

            success = true;

            ItemResponse<bool> response = new ItemResponse<bool>();

            response.Item = success;

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }


        [Route("quoteAccepted/{quoteId}"), HttpPost]
        public HttpResponseMessage NotifySellerCompanyOfAcceptedQuote(int quoteId)
        {
            if (!ModelState.IsValid)
            {
                Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            bool success = false;

            _NotificationOptionService.NotifySellerCompanyOfAcceptedQuote(quoteId);

            success = true;

            ItemResponse<bool> response = new ItemResponse<bool>();

            response.Item = success;

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }



        [Route("bidAccepted/{bidId}"), HttpPost]
        public HttpResponseMessage NotifySellerCompanyOfAcceptedBid(int bidId)
        {
            if (!ModelState.IsValid)
            {
                Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            bool success = false;

            _NotificationOptionService.NotifySellerCompanyOfAcceptedBid(bidId);

            success = true;

            ItemResponse<bool> response = new ItemResponse<bool>();

            response.Item = success;

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }


        [Route("contractApproved/{quoteId}"), HttpPost]
        public HttpResponseMessage NotifyBuyerCompanyOfApprovedContract(int quoteId)
        {
            if (!ModelState.IsValid)
            {
                Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            bool success = false;

            _NotificationOptionService.NotifyBuyerCompanyOfApprovedContract(quoteId);

            success = true;

            ItemResponse<bool> response = new ItemResponse<bool>();

            response.Item = success;

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }


        [Route("contractSubmitted/{quoteId}"), HttpPost]
        public HttpResponseMessage NotifySellerCompanyOfSubmittedContract(int quoteId)
        {
            if (!ModelState.IsValid)
            {
                Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            bool success = false;

            _NotificationOptionService.NotifySellerCompanyOfSubmittedContract(quoteId);

            success = true;

            ItemResponse<bool> response = new ItemResponse<bool>();

            response.Item = success;

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }
    }
}
