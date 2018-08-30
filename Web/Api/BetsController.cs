using Core.Data;
using Service;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using Web.Helpers;
using Web.Infrastructure;
using WebApiThrottle;

namespace Web.Api
{
    [ApiAuthorizeAttribute]
    [RoutePrefix("api/Bets")]
    public class BetsController : ApiControllerBase
    {
        private readonly IBetServices _betServices;

        public BetsController(IBetServices betServices)
        {
            _betServices = betServices;
        }
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List()
        {
            var result = _betServices.GetAll();
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            var result = _betServices.GetById(id);
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpGet]
        [Route("{code:guid}")]
        public IHttpActionResult GetByCode(Guid code)
        {
            var result = _betServices.GetByCode(code);
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpPost]
        [Route("Create")]
        [EnableThrottling(PerSecond = 1)]
        public IHttpActionResult Post([FromBody]Bet model)
        {
            var result = _betServices.Create(model);
            _betServices.Save();
            return ApiHelper.ReturnHttpAction(result, this);
        }
        [HttpPut]
        [Route("Update")]
        [EnableThrottling(PerSecond = 1)]
        public IHttpActionResult Put([FromBody]Bet model)
        {
            var result = _betServices.Update(model);
            _betServices.Save();
            return ApiHelper.ReturnHttpAction(result, this);
        }

    }
}
