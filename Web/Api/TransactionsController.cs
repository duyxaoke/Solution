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
    [RoutePrefix("api/Transactions")]
    public class TransactionsController : ApiControllerBase
    {
        private readonly ITransactionServices _transactionServices;

        public TransactionsController(ITransactionServices transactionServices)
        {
            _transactionServices = transactionServices;
        }
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List()
        {
            var result = _transactionServices.GetAll();
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            var result = _transactionServices.GetById(id);
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpGet]
        [Route("GetByBet/{betId:int}")]
        public IHttpActionResult GetByBet(int betId)
        {
            var result = _transactionServices.GetByBet(betId);
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpPost]
        [Route("Create")]
        [EnableThrottling(PerSecond = 1)]
        public IHttpActionResult Post([FromBody]Transaction model)
        {
            var result = _transactionServices.Create(model);
            _transactionServices.Save();
            return ApiHelper.ReturnHttpAction(result, this);
        }
    }
}
