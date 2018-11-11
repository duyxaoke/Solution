using AutoMapper;
using Core.Data;
using Data.Models;
using DataTablesDotNet;
using DataTablesDotNet.Models;
using Service;
using Shared.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web.Helpers;
using Web.Infrastructure;

namespace Web.Areas.Admin.Controllers
{
    [MvcAuthorizeAttribute]
    public class BetsController : Controller
    {
        private readonly IBetServices _betServices;

        public BetsController(IBetServices betServices)
        {
            _betServices = betServices;
        }

        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Data(DataTablesRequest model)
        {
            var data = _betServices.List().Data.AsQueryable();
            var dataTableParser = new DataTablesParser<BetRes>(model, data);
            var formattedList = dataTableParser.Process();
            return Json(formattedList, JsonRequestBehavior.AllowGet);
        }

    }
}