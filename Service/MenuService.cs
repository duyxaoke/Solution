using System.Collections.Generic;
using Core.Data;

using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using Service.CacheService;
using System.Linq;
using Shared.Models;

namespace Service
{
    public interface IMenuServices
    {
        Menu Get(int id);
        IEnumerable<Menu> GetAll();
        IEnumerable<Menu> GetParent();
        IEnumerable<Menu> GetChildren(int parentId);
        void Add(Menu model);
        void Update(Menu model);
        void Delete(int id);
        void Save();
        void Dispose();

    }
    public class MenuServices : IMenuServices
    {
        readonly ICacheProviderService _cacheProviderService = new CacheProviderService();
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        public IEnumerable<Menu> GetDataCache()
        {
            // First, check the cache
            var trackingdata = _cacheProviderService.Get("CacheMenu") as IDictionary<int, Menu>;

            // If it's not in the cache, we need to read it from the repository
            if (trackingdata == null)
            {
                //kiem tra thong bao trong ngay
                trackingdata = _unitOfWork.MenuRepository.GetAll().ToDictionary(v => v.Id);

                if (trackingdata.Any())
                {
                    // Put this data into the cache for 300 minutes
                    _cacheProviderService.Set("CacheMenu", trackingdata); // 300 minute
                }
            }
            return trackingdata.Values;
        }

        public Menu Get(int id)
        {
            return GetDataCache().FirstOrDefault(c=> c.Id == id);
        }
        public IEnumerable<Menu> GetAll()
        {
            return GetDataCache();
        }
        public IEnumerable<Menu> GetParent()
        {
            return GetDataCache().Where(c => c.ParentId == null && c.IsActive == true);
        }
        public IEnumerable<Menu> GetChildren(int parentId)
        {
            return GetDataCache().Where(c => c.ParentId == parentId && c.IsActive == true);
        }

        public void Add(Menu model)
        {
            _unitOfWork.MenuRepository.Insert(model);
            #region Clear cache when insert or update
            _cacheProviderService.Invalidate("CacheMenu");
            #endregion

        }
        public void Update(Menu model)
        {
            _unitOfWork.MenuRepository.Update(model);
            #region Clear cache when insert or update
            _cacheProviderService.Invalidate("CacheMenu");
            #endregion
        }
        public void Delete(int id)
        {
            _unitOfWork.MenuRepository.Delete(id);
            #region Clear cache when insert or update
            _cacheProviderService.Invalidate("CacheMenu");
            #endregion
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
