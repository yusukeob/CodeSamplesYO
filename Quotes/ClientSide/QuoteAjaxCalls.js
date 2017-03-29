quotemule.services = quotemule.services || {};
quotemule.services.quotes = quotemule.services.quotes || {};

//-------------------------------------------------------------------

quotemule.services.quotes.getBidInfoByBidId = function (bidId, onAjaxSuccess, onAjaxError) {

    var url = "/api/quote/" + bidId;

    var settings = {
        cache: false,
        dataType: "json",
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        type: "GET",
        success: onAjaxSuccess,
        error: onAjaxError
    };

    $.ajax(url, settings);

};



quotemule.services.quotes.insertQuote = function (payload, onAjaxSuccess, onAjaxError) {

    var url = "/api/quote/insert";

    var settings = {
        cache: false,
        data: payload,
        dataType: "json",
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        type: "POST",
        success: onAjaxSuccess,
        error: onAjaxError
    };

    $.ajax(url, settings);

};


quotemule.services.quotes.insertQuoteItem = function (payload, onAjaxSuccess, onAjaxError) {

    var url = "/api/quote/insert/item";

    var settings = {
        cache: false,
        data: payload,
        dataType: "json",
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        type: "POST",
        success: onAjaxSuccess,
        error: onAjaxError
    };

    $.ajax(url, settings);

};


quotemule.services.quotes.getQuoteItemsByQuoteId = function (quoteId, onAjaxSuccess, onAjaxError) {

    var url = "/api/quote/quoteitems/" + quoteId;

    var settings = {
        cache: false,
        dataType: "json",
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        type: "GET",
        success: onAjaxSuccess,
        error: onAjaxError
    };

    $.ajax(url, settings);

};


quotemule.services.quotes.deleteQuoteItem = function (quoteItemId, onAjaxSuccess, onAjaxError) {

    var url = "/api/quote/quoteitems/" + quoteItemId;

    var settings = {
        cache: false,
        dataType: "json",
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        type: "DELETE",
        success: onAjaxSuccess,
        error: onAjaxError
    };

    $.ajax(url, settings);

};


quotemule.services.quotes.getQuoteInfoForQuoteReviewByQuoteId = function (quoteId, onAjaxSuccess, onAjaxError) {

    var url = "/api/quote/quotereview/" + quoteId;

    var settings = {
        cache: false,
        dataType: "json",
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        type: "GET",
        success: onAjaxSuccess,
        error: onAjaxError
    };

    $.ajax(url, settings);

};


quotemule.services.quotes.getQuotesByBuyerCompanyId = function (companyId, onSuccess, onError){
    var url = "/api/companies/" + companyId + "/buyerquotes";

    var settings = {
        cache: false,
        dataType: "json",
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        type: "GET",
        success: onSuccess,
        error: onError
    };

    $.ajax(url, settings);
}