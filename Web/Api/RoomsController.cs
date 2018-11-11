using Core.Data;
using Service;
using Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using Web.Helpers;
using Web.Infrastructure;
using WebApiThrottle;

namespace Web.Api
{
    [ApiAuthorizeAttribute]
    [RoutePrefix("api/Rooms")]
    public class RoomsController : ApiControllerBase
    {
        private readonly IRoomServices _roomServices;

        public RoomsController(IRoomServices roomServices)
        {
            _roomServices = roomServices;
        }
        [HttpGet]
        [Route("List")]
        [ResponseType(typeof(IEnumerable<RoomRes>))]
        public IHttpActionResult List()
        {
            var result = _roomServices.List();
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpGet]
        [Route("{id:int}")]
        [ResponseType(typeof(RoomRes))]
        public IHttpActionResult GetById(int id)
        {
            var result = _roomServices.GetById(id);
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpPost]
        [Route("Create")]
        [ResponseType(typeof(int))]
        [EnableThrottling(PerSecond = 1)]
        public IHttpActionResult Post([FromBody]RoomInsertReq model)
        {
            var result = _roomServices.Create(model);
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpPut]
        [Route("Update")]
        [ResponseType(typeof(bool))]
        [EnableThrottling(PerSecond = 1)]
        public IHttpActionResult Put([FromBody]RoomUpdateReq model)
        {
            var result = _roomServices.Update(model);
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpDelete]
        [ResponseType(typeof(bool))]
        [EnableThrottling(PerSecond = 1)]
        public IHttpActionResult Delete(int id)
        {
            var result = _roomServices.Delete(id);
            return ApiHelper.ReturnHttpAction(result, this);
        }

    }
}
