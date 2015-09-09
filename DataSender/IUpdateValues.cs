using System.Collections.Generic;
using DataModels;
using Windows.Web.Http;
using System;

namespace DataSender
{
    public interface IUpdateValues
    {
        IHttpContent GetHttpContent(IList<EventItem> items);
        Uri GetUri();
    }
}