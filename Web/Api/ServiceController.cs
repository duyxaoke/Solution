using Microsoft.AspNet.Identity;
using Service;
using Shared.Models;
using SignalRAngularDemo.Hubs;
using System.Web.Http;
using Web.Helpers;
using Web.Hubs;
using Web.Infrastructure;
using WebApiThrottle;

namespace Web.Api
{
    [ApiAuthorizeAttribute]
    [RoutePrefix("api/Services")]
    public class ServicesController : ApiControllerBase //SignalRBase<BetHub>
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
        public IHttpActionResult CreateBet([FromBody]CreateBetInsertReq model)
        {
            model.UserId = User.Identity.GetUserId();
            var result = _transactionServices.Create(model);
            // notify all connected clients
            //if(result.Data)
            //    Hub.Clients.All.newBet(model);
            return ApiHelper.ReturnHttpAction(result, this);
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("GetInfoRooms")]
        [EnableThrottling(PerSecond = 3)]
        public IHttpActionResult GetInfoRooms()
        {
            var result = _roomServices.GetInfoRooms();
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetInfoChartsByRoom")]
        [EnableThrottling(PerSecond = 3)]
        public IHttpActionResult GetInfoChartsByRoom(int roomId)
        {
            var result = _transactionServices.GetInfoChartsByRoom(roomId);
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ResultBet")]
        [EnableThrottling(PerSecond = 1)]
        public IHttpActionResult ResultBet(int betId)
        {
            var result = _betServices.GetResultBet(betId);
            return ApiHelper.ReturnHttpAction(result, this);
        }

    }

}
