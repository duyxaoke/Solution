using System.Collections.Generic;
using Core.Data;

using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using Service.CacheService;
using System.Linq;
using Core.DTO.Response;

namespace Service
{
    public interface IConfigServices
    {
        CRUDResult<IEnumerable<Config>> GetAll();
        CRUDResult<Config> Get(int id);
        CRUDResult<bool> Create(Config model);
        CRUDResult<bool> Update(Config model);
        CRUDResult<bool> Delete(int id);
        void Save();
        void Dispose();

    }
    public class ConfigServices : IConfigServices
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public CRUDResult<IEnumerable<Config>> GetAll()
        {
            var result = _unitOfWork.ConfigRepository.GetAll();
            return new CRUDResult<IEnumerable<Config>> { StatusCode = CRUDStatusCodeRes.Success, Data = result };
        }
        public CRUDResult<Config> Get(int id)
        {
            var result = _unitOfWork.ConfigRepository.GetById(id);
            return new CRUDResult<Config> { StatusCode = CRUDStatusCodeRes.Success, Data = result };
        }
        public CRUDResult<bool> Create(Config model)
        {
            var result = _unitOfWork.ConfigRepository.Insert(model);
            if (result)
                return new CRUDResult<bool> { StatusCode = CRUDStatusCodeRes.Success, Data = true };
            return new CRUDResult<bool> { StatusCode = CRUDStatusCodeRes.ResetContent, Data = false };
        }
        public CRUDResult<bool> Update(Config model)
        {
            var result = _unitOfWork.ConfigRepository.Update(model);
            if (result)
                return new CRUDResult<bool> { StatusCode = CRUDStatusCodeRes.Success, Data = true };
            return new CRUDResult<bool> { StatusCode = CRUDStatusCodeRes.ResetContent, Data = false };
        }
        public CRUDResult<bool> Delete(int id)
        {
            var result = _unitOfWork.ConfigRepository.Delete(id);
            if (result)
                return new CRUDResult<bool> { StatusCode = CRUDStatusCodeRes.Success, Data = true };
            return new CRUDResult<bool> { StatusCode = CRUDStatusCodeRes.ResetContent, Data = false };
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
