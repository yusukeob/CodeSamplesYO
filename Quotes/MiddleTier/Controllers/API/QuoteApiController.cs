using Microsoft.Practices.Unity;
using Sabio.Web.Domain;
using Sabio.Web.Models.Requests;
using Sabio.Web.Models.Responses;
using Sabio.Web.Services;
using Sabio.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace Sabio.Web.Controllers.Api
{
    [RoutePrefix("api/quote")]
    public class QuoteApiController : ApiController
    {
        [Dependency]
        public IQuoteService _QuoteService { get; set; }


        [Route("{BidId}"), HttpGet]
        public HttpResponseMessage GetInfoByBidId(int BidId)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            QRInfoAndBidDomain bidInfo = _QuoteService.InfoGetByBidId(BidId);

            ItemResponse<QRInfoAndBidDomain> response = new ItemResponse<QRInfoAndBidDomain>();

            response.Item = bidInfo;

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }



        [Route("insert"), HttpPost]
        public HttpResponseMessage QuoteInsert(QuoteInsertRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            ItemResponse<int> response = new ItemResponse<int>();

            response.Item = _QuoteService.QuoteInsert(model);

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }


        [Route("insert/item"), HttpPost]
        public HttpResponseMessage QuoteItemInsert(QuoteItemInsertRequest model)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            ItemResponse<int> response = new ItemResponse<int>();

            response.Item = _QuoteService.QuoteItemInsert(model);

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }


        [Route("quoteitems/{quoteId}"), HttpGet]

        public HttpResponseMessage QuoteItemsGetByQuoteId(int quoteId)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            List<QuoteItemDomain> quoteItemList = _QuoteService.QuoteItemsGetByQuoteId(quoteId);

            ItemsResponse<QuoteItemDomain> response = new ItemsResponse<QuoteItemDomain>();

            response.Items = quoteItemList;

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }


        [Route("quoteitems/{QuoteItemId}"), HttpDelete]
        public HttpResponseMessage QuoteItemDelete(int QuoteItemId)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            bool isSuccessful = _QuoteService.QuoteItemDelete(QuoteItemId);

            ItemResponse<bool> response = new ItemResponse<bool>();

            response.Item = isSuccessful;

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        [Route("quotereview/{quoteId}"), HttpGet]
        public HttpResponseMessage QuoteInfoForQuoteReviewGetByQuoteId(int quoteId)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            QuoteDomain quoteInfo = _QuoteService.QuoteInfoForQuoteReviewGetByQuoteId(quoteId);

            ItemResponse<QuoteDomain> response = new ItemResponse<QuoteDomain>();

            response.Item = quoteInfo;

            return Request.CreateResponse(HttpStatusCode.OK, response);

        }

    }
}
