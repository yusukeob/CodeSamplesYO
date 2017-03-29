using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    [RoutePrefix("quote/manage")]
    public class QuoteController : BaseController
    {
        // GET: Quote
        public ActionResult Index()
        {
            return View();
        }


        [Route("{QuoteId}")]

        public ActionResult ManageQuote(int QuoteId)
        {
            QuoteViewModel model = new QuoteViewModel();

            model._QuoteId = QuoteId;

            return View(model);
        }
    }
}