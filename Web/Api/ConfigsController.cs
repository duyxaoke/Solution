using Core.Data;
using Service;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using Web.Helpers;
using Web.Infrastructure;

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
        [Route("list")]
        [ApiAuthorizeAttribute(Roles = "User")]
        public IHttpActionResult List()
        {
            var result = _configServices.GetAll();
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var result = _configServices.Get(id);
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpPost]
        [Route("create")]
        public IHttpActionResult Post([FromBody]Config model)
        {
            var result = _configServices.Create(model);
            _configServices.Save();
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpPut]
        [Route("update")]
        public IHttpActionResult Put([FromBody]Config model)
        {
            var result = _configServices.Update(model);
            _configServices.Save();
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var result = _configServices.Delete(id);
            _configServices.Save();
            return ApiHelper.ReturnHttpAction(result, this);
        }

    }
}
