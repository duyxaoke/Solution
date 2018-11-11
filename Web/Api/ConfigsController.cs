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
    [RoutePrefix("api/configs")]
    public class ConfigsController : ApiControllerBase
    {
        private readonly IConfigServices _configServices;

        public ConfigsController(IConfigServices configServices)
        {
            _configServices = configServices;
        }
        [HttpGet]
        [Route("List")]
        [ResponseType(typeof(IEnumerable<ConfigRes>))]
        public IHttpActionResult List()
        {
            var result = _configServices.List();
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpGet]
        [Route("{id:int}")]
        [ResponseType(typeof(ConfigRes))]
        public IHttpActionResult GetById(int id)
        {
            var result = _configServices.GetById(id);
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpPost]
        [Route("Create")]
        [ResponseType(typeof(int))]
        [EnableThrottling(PerSecond = 1)]
        public IHttpActionResult Post([FromBody]ConfigInsertReq model)
        {
            var result = _configServices.Create(model);
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpPut]
        [Route("Update")]
        [ResponseType(typeof(bool))]
        [EnableThrottling(PerSecond = 1)]
        public IHttpActionResult Put([FromBody]ConfigUpdateReq model)
        {
            var result = _configServices.Update(model);
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpDelete]
        [ResponseType(typeof(bool))]
        [EnableThrottling(PerSecond = 1)]
        public IHttpActionResult Delete(int id)
        {
            var result = _configServices.Delete(id);
            return ApiHelper.ReturnHttpAction(result, this);
        }

    }
}
