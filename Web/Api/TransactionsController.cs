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
        [ResponseType(typeof(IEnumerable<TransactionRes>))]
        public IHttpActionResult List()
        {
            var result = _transactionServices.List();
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpGet]
        [Route("{id:int}")]
        [ResponseType(typeof(TransactionRes))]
        public IHttpActionResult GetById(int id)
        {
            var result = _transactionServices.GetById(id);
            return ApiHelper.ReturnHttpAction(result, this);
        }

        [HttpGet]
        [Route("GetByBet/{betId:int}")]
        [ResponseType(typeof(IEnumerable<TransactionRes>))]
        public IHttpActionResult GetByBet(int betId)
        {
            var result = _transactionServices.GetByBet(betId);
            return ApiHelper.ReturnHttpAction(result, this);
        }
        [HttpPut]
        [Route("Update")]
        [ResponseType(typeof(bool))]
        [EnableThrottling(PerSecond = 1)]
        public IHttpActionResult Put([FromBody]TransactionUpdateReq model)
        {
            var result = _transactionServices.Update(model);
            return ApiHelper.ReturnHttpAction(result, this);
        }

    }
}
