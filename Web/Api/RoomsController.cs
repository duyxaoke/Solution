using Core.Data;
using Service;
using System.Linq;
using System.Web.Http;
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
        public IHttpActionResult List()
        {
            var result = _roomServices.GetAll();
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            var result = _roomServices.GetById(id);
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpPost]
        [Route("Create")]
        [EnableThrottling(PerSecond = 1)]
        public IHttpActionResult Post([FromBody]Room model)
        {
            var result = _roomServices.Create(model);
            _roomServices.Save();
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpPut]
        [Route("Update")]
        [EnableThrottling(PerSecond = 1)]
        public IHttpActionResult Put([FromBody]Room model)
        {
            var result = _roomServices.Update(model);
            _roomServices.Save();
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpDelete]
        [EnableThrottling(PerSecond = 1)]
        public IHttpActionResult Delete(int id)
        {
            var result = _roomServices.Delete(id);
            _roomServices.Save();
            return ApiHelper.ReturnHttpAction(result, this);
        }

    }
}
