using AutoMapper;
using Core.Data;
using Data.Models;
using DataTablesDotNet;
using DataTablesDotNet.Models;
using Service;
using Shared.Models;
using System.Linq;
using System.Web.Mvc;
using Web.Helpers;
using Web.Infrastructure;

namespace Web.Areas.Admin.Controllers
{
    [MvcAuthorizeAttribute]
    public class ConfigsController : Controller
    {
        private readonly IConfigServices _configServices;

        public ConfigsController(IConfigServices configServices)
        {
            _configServices = configServices;
        }

        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Data(DataTablesRequest model)
        {
            var data = _configServices.List().Data.AsQueryable();
            var dataTableParser = new DataTablesParser<ConfigRes>(model, data);
            var formattedList = dataTableParser.Process();
            return Json(formattedList, JsonRequestBehavior.AllowGet);
        }

    }
}