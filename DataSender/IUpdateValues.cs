using System.Collections.Generic;
using DataModels;
using Windows.Web.Http;
using System;
using DataOperations;

namespace PIClient
{
    public interface IUpdateValues
    {
        IHttpContent GetHttpContent(UserContext userContext, IList<EventItem> items);
        Uri GetUri();
    }
}