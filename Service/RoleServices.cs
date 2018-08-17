using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.AspNet.Identity;
using Data.DAL;

namespace Service
{
    public interface IRoleServices
    {
        IEnumerable<IdentityRole> GetAllRoles();
        IdentityRole GetRoleById(string id);
        IdentityRole GetRoleByName(string name);
        bool AddOrUpdateRole(IdentityRole role);
        IEnumerable<IdentityUserRole> GetRoleByUser(string UserId);
    }
    public class RoleServices : IRoleServices
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private readonly DatabaseContext _context = new DatabaseContext();

        public IEnumerable<IdentityRole> GetAllRoles()
        {
            return _unitOfWork.IdentityRoleRepository.GetAll();
        }
        public IdentityRole GetRoleByName(string name)
        {
            return _unitOfWork.IdentityRoleRepository.Get(c=>c.Name == name);
        }
        public IdentityRole GetRoleById(string id)
        {
            return _unitOfWork.IdentityRoleRepository.GetById(id);
        }
        public IEnumerable<IdentityUserRole> GetRoleByUser(string UserId)
        {
            return _unitOfWork.IdentityUserRoleRepository.GetMany(c=>c.UserId == UserId);
        }
        public bool AddOrUpdateRole(IdentityRole role)
        {
            var result = false;
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));
            var existRole = _unitOfWork.IdentityRoleRepository.GetById(role.Id);
            if (existRole != null)
            {
                existRole.Name = role.Name;
                roleManager.Update(existRole);
                //result = _unitOfWork.IdentityRoleRepository.Update(role);
            }
            else
            {                
                var tmp = roleManager.Create(role);
                result = tmp.Succeeded;
            }

            return result;
        }        
    }
}
