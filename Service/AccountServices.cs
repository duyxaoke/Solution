using Data.DAL;
using Data.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Service
{
    public interface IAccountServices
    {
        IEnumerable<IdentityRole> GetAllRoles();
        IEnumerable<ApplicationUser> GetAllUsers();
        IEnumerable<IdentityUserRole> GetAllUserRole();
        ApplicationUser GetUser(string id);
        ApplicationUser GetUserbyUsername(string username);
        bool AddOrUpdateUser(ApplicationUser user, IList<string> lstRole);
    }
    public class AccountServices : IAccountServices
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private readonly DatabaseContext _context = new DatabaseContext();
        public IEnumerable<IdentityRole> GetAllRoles()
        {
            return _unitOfWork.IdentityRoleRepository.GetAll();
        }
        public IEnumerable<ApplicationUser> GetAllUsers()
        {
            return _unitOfWork.ApplicationUserRepository.GetAll();
        }
        public IEnumerable<IdentityUserRole> GetAllUserRole()
        {
            return _unitOfWork.IdentityUserRoleRepository.GetAll();
        }
        public ApplicationUser GetUser(string id)
        {
            return _unitOfWork.ApplicationUserRepository.GetById(id);
        }
        public ApplicationUser GetUserbyUsername(string username)
        {
            return _unitOfWork.ApplicationUserRepository.Get(c => c.UserName == username);
        }
        /// <summary>
        /// Lưu hoặc Sửa thông tin User (demo async)
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public bool AddOrUpdateUser(ApplicationUser user, IList<string> lstRole)
        {
            bool result = false;
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            var store = new UserStore<ApplicationUser>(_context);
            var manager = new UserManager<ApplicationUser>(store);
            var existUser = userManager.Users.FirstOrDefault(aa => aa.Id == user.Id);
            if (existUser != null)
            {
                try
                {
                    //_unitOfWork.ApplicationUserRepository.Update(user);
                    existUser.Email = user.Email;
                    existUser.PhoneNumber = user.PhoneNumber;

                    manager.Update(existUser);
                    var ctx = store.Context;
                    ctx.SaveChanges();
                    if (lstRole != null && lstRole.Any())
                    {
                        var allRole = userManager.GetRoles(existUser.Id).ToArray();
                        if (allRole != null && allRole.Any())
                        {
                            using (TransactionScope scope = new TransactionScope())
                            {
                                userManager.RemoveFromRoles(existUser.Id, allRole);
                                if (lstRole.Count > 0)
                                {
                                    var roles = _unitOfWork.IdentityRoleRepository.GetMany(aa => lstRole.Contains(aa.Id));
                                    var rolesAdd = roles.Select(aa => aa.Name).ToArray();
                                    userManager.AddToRoles(existUser.Id, rolesAdd);
                                }
                                scope.Complete();
                                result = true;
                            }

                        }                        
                    }                    
                } 
                catch (Exception)
                { }
            }
            return result;
        }
    }
}
