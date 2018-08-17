using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Data.Models;
using Service;
using Shared.Common;
using Shared.Models;
using Web.Infrastructure;

namespace Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RolesController : BaseController
    {
        private readonly RoleManager roleManager;
        private readonly ClaimedActionsProvider claimedActionsProvider;
        private IMenuServices _menuAppService = new MenuServices();
        private IMenuInRolesServices _menuInRolesAppService = new MenuInRolesServices();

        public RolesController(RoleManager roleManager, ClaimedActionsProvider claimedActionsProvider)
        {
            this.roleManager = roleManager;
            this.claimedActionsProvider = claimedActionsProvider;
        }

        #region Constructor

        #endregion

        #region Index - View
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult MenuInRoles(Guid? roleId)
        {
            var lsmodel = new List<MenuViewModel>();
            var menus = _menuAppService.GetParent().OrderBy(l => l.Order);
            foreach (var item in menus)
            {
                MenuViewModel model = new MenuViewModel();
                model.Id = item.Id;
                model.Name = item.Name;
                model.Url = item.Url;
                model.Icon = item.Icon;
                model.Order = item.Order;
                model.IsActive = item.IsActive;
                model.ParentId = item.ParentId;
                model.RoleId = roleId;
                model.Children = GetChildren(item.Id, roleId);
                if (roleId.HasValue)
                {
                    var menu = _menuInRolesAppService.GetMenuByRoleId(roleId.Value).Any(c => c.MenuId == item.Id);
                    if (menu)
                        model.Checked = "checked";
                    else
                        model.Checked = "";
                }
                lsmodel.Add(model);
            }
            return PartialView("_MenuInRoles", lsmodel);
        }

        [HttpPost]
        public ActionResult MenuInRoles(Guid? roleId, List<int> menuIds)
        {
            try
            {
                _menuInRolesAppService.AddOrUpdateMenuInRoles(roleId.Value, menuIds);
                return Json(new { status = true, message = Command.MessageSuccess });
            }
            catch
            {
                return Json(new { status = false, message = Command.MessageError });
            }
        }

        #endregion

        #region Add
        public ActionResult Create()
        {
            CreateRoleViewModel model = new CreateRoleViewModel();
            return PartialView("_Create", model);
        }
        [HttpPost]
        public async Task<ActionResult> Create(CreateRoleViewModel viewModel)
        {
            try
            {
                var newRole = new ApplicationRole()
                {
                    Name = viewModel.Name,
                };
                await roleManager.CreateAsync(newRole);
                return Json(new { status = true, message = Command.MessageSuccess });
            }
            catch
            {
                return Json(new { status = false, message = Command.MessageError });

            }
        }
        #endregion

        #region Edit
        public async Task<ActionResult> EditClaims(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            var claimGroups = claimedActionsProvider.GetClaimGroups();

            var assignedClaims = await roleManager.GetClaimsAsync(role.Name);

            var viewModel = new RoleClaimsViewModel()
            {
                RoleId = role.Id,
                RoleName = role.Name,
            };

            foreach (var claimGroup in claimGroups)
            {
                var claimGroupModel = new RoleClaimsViewModel.ClaimGroup()
                {
                    GroupId = claimGroup.GroupId,
                    GroupName = claimGroup.GroupName,
                    GroupClaimsCheckboxes = claimGroup.Claims
                        .Select(c => new SelectListItem()
                        {
                            Value = String.Format("{0}#{1}", claimGroup.GroupId, c),
                            Text = c,
                            Selected = assignedClaims.Any(ac => ac.Type == claimGroup.GroupId.ToString() && ac.Value == c)
                        }).ToList()
                };
                viewModel.ClaimGroups.Add(claimGroupModel);
            }

            return PartialView("_EditClaims", viewModel);
        }


        [HttpPost]
        public async Task<ActionResult> EditClaims(RoleClaimsViewModel viewModel)
        {
            try
            {
                var role = await roleManager.FindByIdAsync(viewModel.RoleId);
                role.Name = viewModel.RoleName;
                var roleResult = await roleManager.UpdateAsync(role);
                var roleClaims = await roleManager.GetClaimsAsync(role.Name);
                // this is ugly. Deletes all the claims and adds them back in.
                // can be done in a better fashion
                foreach (var removedClaim in roleClaims)
                {
                    await roleManager.RemoveClaimAsync(role.Id, removedClaim);
                }

                var submittedClaims = viewModel
                    .SelectedClaims
                    .Select(s =>
                    {
                        var tokens = s.Split('#');
                        if (tokens.Count() != 2)
                        {
                            throw new Exception(String.Format("Claim {0} can't be processed because it is in incorrect format", s));
                        }
                        return new Claim(tokens[0], tokens[1]);
                    }).ToList();


                roleClaims = await roleManager.GetClaimsAsync(role.Name);

                foreach (var submittedClaim in submittedClaims)
                {
                    var hasClaim = roleClaims.Any(c => c.Value == submittedClaim.Value && c.Type == submittedClaim.Type);
                    if (!hasClaim)
                    {
                        await roleManager.AddClaimAsync(role.Id, submittedClaim);
                    }
                }

                roleClaims = await roleManager.GetClaimsAsync(role.Name);

                var cacheKey = ApplicationRole.GetCacheKey(role.Name);
                System.Web.HttpContext.Current.Cache.Remove(cacheKey);
                return Json(new { status = true, message = Command.MessageSuccess });
            }
            catch
            {
                return Json(new { status = false, message = Command.MessageError });
            }

        }
        #endregion

        #region Delete
        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            var role = new ApplicationRole();
            if (!String.IsNullOrEmpty(id))
            {
                role = await roleManager.FindByIdAsync(id);
            }
            return PartialView("_Delete", role);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id, FormCollection form)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var role = await roleManager.FindByIdAsync(id);
                if (role != null)
                {
                    var roleClaims = await roleManager.GetClaimsAsync(role.Name);
                    // this is ugly. Deletes all the claims and adds them back in.
                    // can be done in a better fashion
                    foreach (var removedClaim in roleClaims)
                    {
                        await roleManager.RemoveClaimAsync(role.Id, removedClaim);
                    }
                    var roleRuslt = roleManager.DeleteAsync(role).Result;
                    if (roleRuslt.Succeeded)
                    {
                        return Json(new { status = true, message = Command.MessageSuccess });
                    }
                }
            }
            return Json(new { status = false, message = Command.MessageError });
        }
        #endregion

        #region Function
        //public JsonResult GetDataTable()
        //{
        //    var request = new DataTablesRequest<ApplicationRole>(Request.Params);
        //    request.Columns[p => p.Name]
        //        .GlobalSearchPredicate = (p) => p.Name.ToUpper().Contains(request.GlobalSearchValue.ToUpper());
        //    var model = roleManager.Roles;
        //    var data = model.ToPagedList(request);
        //    return JsonDataTable(data);
        //}
        private List<MenuViewModel> GetChildren(int parentId, Guid? roleId)
        {
            var lsmodel = new List<MenuViewModel>();
            var menus = _menuAppService.GetChildren(parentId).OrderBy(l => l.Order);
            foreach (var item in menus)
            {
                MenuViewModel model = new MenuViewModel();
                model.Id = item.Id;
                model.Name = item.Name;
                model.Url = item.Url;
                model.Icon = item.Icon;
                model.Order = item.Order;
                model.IsActive = item.IsActive;
                model.ParentId = item.ParentId;
                if (roleId.HasValue)
                {
                    var menu = _menuInRolesAppService.GetMenuByRoleId(roleId.Value).Any(c => c.MenuId == item.Id);
                    if (menu)
                        model.Checked = "checked";
                    else
                        model.Checked = "";
                }
                lsmodel.Add(model);
            }
            return lsmodel;
        }

        #endregion





    }


    public class CreateRoleViewModel
    {
        [Required]
        public String Name { get; set; }
    }


    public class RoleClaimsViewModel
    {
        public RoleClaimsViewModel()
        {
            ClaimGroups = new List<ClaimGroup>();

            SelectedClaims = new List<String>();
        }

        public String RoleId { get; set; }

        public String RoleName { get; set; }

        public List<ClaimGroup> ClaimGroups { get; set; }

        public IEnumerable<String> SelectedClaims { get; set; }


        public class ClaimGroup
        {
            public ClaimGroup()
            {
                GroupClaimsCheckboxes = new List<SelectListItem>();
            }
            public String GroupName { get; set; }

            public int GroupId { get; set; }

            public List<SelectListItem> GroupClaimsCheckboxes { get; set; }
        }
    }
}