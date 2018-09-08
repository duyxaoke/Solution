using Core.Data;
using Service;
using Shared.Models;
using SignalRAngularDemo.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web.Helpers;
using Web.Hubs;
using WebApiThrottle;

namespace Web.Api
{
    [RoutePrefix("api/Services")]
    public class ServicesController : SignalRBase<BetHub>
    {
        private readonly ITransactionServices _transactionServices;
        private readonly IBetServices _betServices;
        private readonly IRoomServices _roomServices;

        public ServicesController(ITransactionServices transactionServices, IBetServices betServices, IRoomServices roomServices)
        {
            _transactionServices = transactionServices;
            _betServices = betServices;
            _roomServices = roomServices;
        }

        [HttpPost]
        [Route("CreateBet")]
        [EnableThrottling(PerSecond = 1)]
        public IHttpActionResult CreateBet([FromBody]CreateBetViewModel model)
        {
            var result = _transactionServices.Create(model);
            // notify all connected clients
            var bet = _roomServices.GetAll().Data.Select(c=> new {
                RoomName = c.Name,
                TotalUser = 3,
                TotalAmount = _betServices.GetByRoomAvailable(c.Id).Data?.TotalBet ?? 0
            });
            if(result.Data)
                Hub.Clients.All.newBet(bet);
            return ApiHelper.ReturnHttpAction(result, this);
        }


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
            Hub.Clients.All.newBet(item);

            // return the item inside of a 201 response
            return Request.CreateResponse(HttpStatusCode.Created, item);
        }
    }

}
