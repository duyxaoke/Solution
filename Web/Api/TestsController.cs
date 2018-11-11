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
    [RoutePrefix("api/Tests")]
    public class TestController : ApiControllerBase
    {
        private readonly ITestServices _TestService;

        public TestController(ITestServices TestService)
        {
            _TestService = TestService;
        }

        /// <summary>
        /// List
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<TestRes>))]
        public IHttpActionResult List()
        {
            var result = _TestService.List();
            return ApiHelper.ReturnHttpAction(result, this);
        }

        /// <summary>
        /// ReadByID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(TestRes))]
        public IHttpActionResult ReadByID(int id)
        {
            var result = _TestService.ReadById(id);
            return ApiHelper.ReturnHttpAction(result, this);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(int))]
        public IHttpActionResult Create(TestInsertReq obj)
        {
            var result = _TestService.Create(obj);
            return ApiHelper.ReturnHttpAction(result, this);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        [HttpPut]
        [ResponseType(typeof(bool))]
        public IHttpActionResult Update(TestUpdateReq Obj)
        {
            var result = _TestService.Update(Obj);
            return ApiHelper.ReturnHttpAction(result, this);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [ResponseType(typeof(bool))]
        public IHttpActionResult Delete(int id)
        {
            var result = _TestService.Delete(id);
            return ApiHelper.ReturnHttpAction(result, this);
        }
    }
}
