﻿using AutoMapper;
using Core.Data;
using Data.Models;
using DataTablesDotNet;
using DataTablesDotNet.Models;
using Service;
using System.Linq;
using System.Web.Mvc;
using Web.Helpers;
using Web.Infrastructure;

namespace Web.Areas.Admin.Controllers
{
    [MvcAuthorizeAttribute]
    public class RoomsController : Controller
    {
        private readonly IRoomServices _roomServices;

        public RoomsController(IRoomServices roomServices)
        {
            _roomServices = roomServices;
        }

        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Data(DataTablesRequest model)
        {
            var data = _roomServices.GetAll().Data.AsQueryable();
            var dataTableParser = new DataTablesParser<Room>(model, data);
            var formattedList = dataTableParser.Process();
            return Json(formattedList, JsonRequestBehavior.AllowGet);
        }

    }
}