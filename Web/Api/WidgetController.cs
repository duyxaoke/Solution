using SignalRAngularDemo.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web.Hubs;

namespace Web.Api
{
    [RoutePrefix("api/widget")]
    public class WidgetController : SignalRBase<WidgetHub>
    {
        [HttpPost]
        // POST api/<controller>
        public HttpResponseMessage Post(Widget item)
        {
            if (item == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            // validate and add to database in a real app

            // notify all connected clients
            Hub.Clients.All.newWidget(item);

            // return the item inside of a 201 response
            return Request.CreateResponse(HttpStatusCode.Created, item);
        }
    }
    public class Widget
    {
        public int ID { get; set; }
        public string Color { get; set; }
        public string Shape { get; set; }
    }

}
