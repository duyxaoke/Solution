using Core.Data;
using Service;
using Shared.Models;
using System;
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
        [ResponseType(typeof(IEnumerable<BetRes>))]
        public IHttpActionResult List()
        {
            var result = _betServices.List();
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpGet]
        [Route("{id:int}")]
        [ResponseType(typeof(BetRes))]
        public IHttpActionResult GetById(int id)
        {
            var result = _betServices.GetById(id);
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpGet]
        [Route("{code:guid}")]
        [ResponseType(typeof(BetRes))]
        public IHttpActionResult GetByCode(Guid code)
        {
            var result = _betServices.GetByCode(code);
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpPost]
        [Route("Create")]
        [ResponseType(typeof(int))]
        [EnableThrottling(PerSecond = 1)]
        public IHttpActionResult Post([FromBody]BetInsertReq model)
        {
            var result = _betServices.Create(model);
            return ApiHelper.ReturnHttpAction(result, this);
        }
        [HttpPut]
        [Route("Update")]
        [ResponseType(typeof(bool))]
        [EnableThrottling(PerSecond = 1)]
        public IHttpActionResult Put([FromBody]BetUpdateReq model)
        {
            var result = _betServices.Update(model);
            return ApiHelper.ReturnHttpAction(result, this);
        }

    }
}
