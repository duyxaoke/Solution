using System.Collections.Generic;
using Core.Data;

using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using Service.CacheService;
using System.Linq;
using Shared.Models;
using System;

namespace Service
{
    public interface IMenuInRolesServices
    {
        MenuInRoles Get(int id);
        IEnumerable<MenuInRoles> GetAll();
        void Add(MenuInRoles model);
        void Update(MenuInRoles model);
        void Delete(int id);
        void Save();
        void Dispose();
        bool AddOrUpdateMenuInRoles(Guid roleId, List<int> menuIds);
        IEnumerable<MenuInRoles> GetMenuByRoleId(Guid roleId);
    }
    public class MenuInRolesServices : IMenuInRolesServices
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        public IEnumerable<MenuInRoles> GetMenuByRoleId(Guid roleId)
        {
            return _unitOfWork.MenuInRolesRepository.GetMany(c => c.RoleId == roleId);
        }

        public MenuInRoles Get(int id)
        {
            return _unitOfWork.MenuInRolesRepository.GetById(id);
        }
        public IEnumerable<MenuInRoles> GetAll()
        {
            return _unitOfWork.MenuInRolesRepository.GetAll();
        }
        public void Add(MenuInRoles model)
        {
            _unitOfWork.MenuInRolesRepository.Insert(model);

        }
        public void Update(MenuInRoles model)
        {
            _unitOfWork.MenuInRolesRepository.Update(model);

        }
        public void Delete(int id)
        {
            _unitOfWork.MenuInRolesRepository.Delete(id);

        }
        public bool AddOrUpdateMenuInRoles(Guid roleId, List<int> menuIds)
        {
            bool result = false;
            var existRole = _unitOfWork.MenuInRolesRepository.GetMany(c => c.RoleId == roleId);
            if (existRole != null)
            {
                try
                {
                    //xóa hết dữ liệu trong MenuInRoles theo RoleId
                    foreach (var item in existRole)
                    {
                        _unitOfWork.MenuInRolesRepository.Delete(item.Id);
                    }
                    if (menuIds.Count > 0)
                    {
                        //thêm dữ liệu mới
                        foreach (var item in menuIds)
                        {
                            var model = new MenuInRoles();
                            model.MenuId = item;
                            model.RoleId = roleId;
                            _unitOfWork.MenuInRolesRepository.Insert(model);
                        }
                    }
                    _unitOfWork.MenuInRolesRepository.Save();
                    result = true;
                }
                catch (Exception)
                { }
            }
            return result;
        }

        public void Save()
        {
            _unitOfWork.Save();
        }
        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
