using Core.Data;
using DataTablesDotNet;
using DataTablesDotNet.Models;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Web.Helpers;
using Web.Infrastructure;

namespace Web.Api
{
    public class ApiRolesController : Controller
    {
        private readonly IConfigServices _configServices = new ConfigServices();


        [ApiAuthorize]
        // GET: api/ApiRoles
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}
