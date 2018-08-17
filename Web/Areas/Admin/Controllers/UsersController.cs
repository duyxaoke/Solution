using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Data.Models;
using Shared.Common;
using Shared.Models;
using Web.Infrastructure;

namespace Web.Areas.Admin.Controllers
{
    [ClaimsGroup(ClaimResources.Users)]
    public class UsersController : BaseController
    {
        #region Constructor
        private readonly UserManager userManager;
        private readonly ClaimedActionsProvider claimedActionsProvider;
        private readonly RoleManager roleManager;


        public UsersController(UserManager userManager, ClaimedActionsProvider claimedActionsProvider, RoleManager roleManager)
        {
            this.userManager = userManager;
            this.claimedActionsProvider = claimedActionsProvider;
            this.roleManager = roleManager;
        }

        #endregion

        #region Index - View
        [ClaimsAction(ClaimsActions.Index)]
        public ActionResult Index()
        {
            var model = new UsersIndexViewIndex()
            {
                Users = userManager.Users.ToList(),
            };
            return View(model);
        }
        [ClaimsAction(ClaimsActions.Index)]
        public async Task<ActionResult> ViewClaims(String id)
        {
            var user = await userManager.FindByIdAsync(id);

            var userRoles = await userManager.GetRolesAsync(id);
            var userclaims = new List<Claim>();
            foreach (var role in userRoles)
            {
                var roleClaims = await roleManager.GetClaimsAsync(role);

                userclaims.AddRange(roleClaims);
            }

            var claimGroups = claimedActionsProvider.GetClaimGroups();

            var viewModel = new UserClaimsViewModel()
            {
                UserName = user.UserName,
            };

            foreach (var claimGroup in claimGroups)
            {
                var claimGroupModel = new UserClaimsViewModel.ClaimGroup()
                {
                    GroupId = claimGroup.GroupId,
                    GroupName = claimGroup.GroupName,
                    GroupClaimsCheckboxes = claimGroup.Claims
                        .Select(c => new SelectListItem()
                        {
                            Value = String.Format("{0}#{1}", claimGroup.GroupId, c),
                            Text = c,
                            Selected = userclaims.Any(ac => ac.Type == claimGroup.GroupId.ToString() && ac.Value == c)
                        }).ToList()
                };
                viewModel.ClaimGroups.Add(claimGroupModel);
            }

            return PartialView("_ViewClaims", viewModel);
        }

        #endregion

        #region Add
        [HttpGet]
        [ClaimsAction(ClaimsActions.Create)]
        public ActionResult Add()
        {
            UserViewModel model = new UserViewModel();
            return PartialView("_AddUser", model);
        }

        [HttpPost]
        [ClaimsAction(ClaimsActions.Create)]
        public async Task<ActionResult> Add(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    Name = model.Name,
                    UserName = model.UserName,
                    Email = model.Email
                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return Json(new { status = true, message = Command.MessageSuccess });
                }
            }
            return Json(new { status = false, message = Command.MessageError });
        }

        #endregion

        #region Edit
        [HttpGet]
        [ClaimsAction(ClaimsActions.Edit)]
        public async Task<ActionResult> Edit(string id)
        {
            UserViewModel model = new UserViewModel();
            if (!String.IsNullOrEmpty(id))
            {
                ApplicationUser user = await userManager.FindByIdAsync(id);

                if (user != null)
                {
                    model.Name = user.Name;
                    model.Email = user.Email;
                    model.UserName = user.UserName;
                }
            }
            return PartialView("_EditUser", model);
        }

        [HttpPost]
        [ClaimsAction(ClaimsActions.Edit)]
        public async Task<ActionResult> Edit(string id, UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByIdAsync(id);
                if (user != null)
                {
                    user.Name = model.Name;
                    user.Email = model.Email;
                    user.UserName = model.UserName;
                    if (!String.IsNullOrEmpty(model.Password))
                    {
                        user.PasswordHash = userManager.PasswordHasher.HashPassword(model.Password);
                    }
                    var result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return Json(new { status = true, message = Command.MessageSuccess });
                    }
                }
            }
            return Json(new { status = false, message = Command.MessageError });
        }
        [ClaimsAction(ClaimsActions.Edit)]
        public async Task<ActionResult> EditRoles(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            var assignedRoles = await userManager.GetRolesAsync(user.Id);

            var allRoles = await roleManager.Roles.ToListAsync();

            var userRoles = allRoles.Select(r => new SelectListItem()
            {
                Value = r.Name,
                Text = r.Name,
                Selected = assignedRoles.Contains(r.Name),
            }).ToList();

            var viewModel = new UserRolesViewModel
            {
                Username = user.UserName,
                UserId = user.Id,
                UserRoles = userRoles,
            };
            return PartialView("_EditRoles", viewModel);
        }
        [HttpPost]
        [ClaimsAction(ClaimsActions.Edit)]
        public async Task<ActionResult> EditRoles(UserRolesViewModel viewModel)
        {
            try
            {
                var user = await userManager.FindByIdAsync(viewModel.UserId);
                var possibleRoles = await roleManager.Roles.ToListAsync();
                var userRoles = await userManager.GetRolesAsync(user.Id);

                var submittedRoles = viewModel.SelectedRoles;

                var shouldUpdateSecurityStamp = false;

                foreach (var submittedRole in submittedRoles)
                {
                    var hasRole = await userManager.IsInRoleAsync(user.Id, submittedRole);
                    if (!hasRole)
                    {
                        shouldUpdateSecurityStamp = true;
                        await userManager.AddToRoleAsync(user.Id, submittedRole);
                    }
                }

                foreach (var removedRole in possibleRoles.Select(r => r.Name).Except(submittedRoles))
                {
                    shouldUpdateSecurityStamp = true;
                    await userManager.RemoveFromRoleAsync(user.Id, removedRole);
                }

                if (shouldUpdateSecurityStamp)
                {
                    await userManager.UpdateSecurityStampAsync(user.Id);
                }
                return Json(new { status = true, message = Command.MessageSuccess });
            }
            catch
            {
                return Json(new { status = false, message = Command.MessageError });
            }
            
        }
        [ClaimsAction(ClaimsActions.Edit)]
        public async Task<ActionResult> UpdateSecurityStamp(String userId)
        {
            await userManager.UpdateSecurityStampAsync(userId);

            return RedirectToAction("Index");
        }

        #endregion

        #region Delete
        [HttpGet]
        [ClaimsAction(ClaimsActions.Delete)]
        public async Task<ActionResult> Delete(string id)
        {
            var applicationUser = new ApplicationUser();
            if (!String.IsNullOrEmpty(id))
            {
                applicationUser = await userManager.FindByIdAsync(id);
            }
            return PartialView("_DeleteUser", applicationUser);
        }

        [HttpPost]
        [ClaimsAction(ClaimsActions.Delete)]
        public async Task<ActionResult> Delete(string id, FormCollection form)
        {
            if (!String.IsNullOrEmpty(id))
            {
                ApplicationUser applicationUser = await userManager.FindByIdAsync(id);
                if (applicationUser != null)
                {
                    var result = await userManager.DeleteAsync(applicationUser);
                    if (result.Succeeded)
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
        //    var request = new DataTablesRequest<ApplicationUser>(Request.Params);
        //    request.Columns[p => p.UserName]
        //        .GlobalSearchPredicate = (p) => p.Name.ToUpper().Contains(request.GlobalSearchValue.ToUpper());
        //    var model = userManager.Users;
        //    var data = model.ToPagedList(request);
        //    return JsonDataTable(data);
        //}
        #endregion
    }


    public class UsersIndexViewIndex
    {
        public List<ApplicationUser> Users { get; set; }
    }


    public class UserClaimsViewModel
    {
        public UserClaimsViewModel()
        {
            ClaimGroups = new List<ClaimGroup>();

            SelectedClaims = new List<String>();
        }

        public String UserName { get; set; }

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



    public class UserRolesViewModel
    {
        public UserRolesViewModel()
        {
            UserRoles = new List<SelectListItem>();
            SelectedRoles = new List<String>();
        }

        public String UserId { get; set; }
        public String Username { get; set; }
        public List<SelectListItem> UserRoles { get; set; }
        public List<String> SelectedRoles { get; set; }
    }
}