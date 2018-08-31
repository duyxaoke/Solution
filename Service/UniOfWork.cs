using Core.Data;
using Data.DAL;

using Data.Models;
using Shared.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;






namespace Service
{
    public class UnitOfWork : IDisposable
    {
        #region Fields

        #region System
        public readonly DatabaseContext Context = new DatabaseContext();
        private BaseRepository<Message> _messageRepository;
        private BaseRepository<Config> _configRepository;
        private BaseRepository<Menu> _menuRepository;
        private BaseRepository<MenuInRoles> _menuInRolesRepository;
        private BaseRepository<MenuViewModel> _menuViewRepository;
        private BaseRepository<UserViewModel> _userViewRepository;
        private BaseRepository<ApplicationRole> _identityRoleRepository;
        private BaseRepository<ApplicationUser> _applicationUserRepository;
        private BaseRepository<IdentityUserRole> _identityUserRoleRepository;
        private BaseRepository<DB_LOG> _dbLogRepository;
        private BaseRepository<Room> _roomRepository;
        private BaseRepository<Bet> _betRepository;
        private BaseRepository<Transaction> _transactionRepository;
        #endregion

        #endregion

        #region Constructors and Destructors

        #region System

        public BaseRepository<DB_LOG> dbLogRepository
        {
            get
            {
                if (_dbLogRepository == null)
                    _dbLogRepository = new BaseRepository<DB_LOG>(Context);
                return _dbLogRepository;
            }
        }
        public BaseRepository<ApplicationRole> IdentityRoleRepository
        {
            get
            {
                if (this._identityRoleRepository == null)
                    this._identityRoleRepository = new BaseRepository<ApplicationRole>(Context);
                return _identityRoleRepository;
            }
        }
        public BaseRepository<MenuViewModel> MenuViewRepository
        {
            get
            {
                if (_menuViewRepository == null)
                    _menuViewRepository = new BaseRepository<MenuViewModel>(Context);
                return _menuViewRepository;
            }
        }
        public BaseRepository<MenuInRoles> MenuInRolesRepository
        {
            get
            {
                if (_menuInRolesRepository == null)
                    _menuInRolesRepository = new BaseRepository<MenuInRoles>(Context);
                return _menuInRolesRepository;
            }
        }
        public BaseRepository<Menu> MenuRepository
        {
            get
            {
                if (_menuRepository == null)
                    _menuRepository = new BaseRepository<Menu>(Context);
                return _menuRepository;
            }
        }
        public BaseRepository<Config> ConfigRepository
        {
            get
            {
                if (_configRepository == null)
                    _configRepository = new BaseRepository<Config>(Context);
                return _configRepository;
            }
        }
        public BaseRepository<ApplicationUser> ApplicationUserRepository
        {
            get
            {
                if (_applicationUserRepository == null)
                    _applicationUserRepository = new BaseRepository<ApplicationUser>(Context);
                return _applicationUserRepository;
            }
        }
        public BaseRepository<IdentityUserRole> IdentityUserRoleRepository
        {
            get
            {
                if (_identityUserRoleRepository == null)
                    _identityUserRoleRepository = new BaseRepository<IdentityUserRole>(Context);
                return _identityUserRoleRepository;
            }
        }
        public BaseRepository<Message> MessageRepository
        {
            get
            {
                if (_messageRepository == null)
                    _messageRepository = new BaseRepository<Message>(Context);
                return _messageRepository;
            }
        }
        public BaseRepository<UserViewModel> UserViewRepository
        {
            get
            {
                if (_userViewRepository == null)
                    _userViewRepository = new BaseRepository<UserViewModel>(Context);
                return _userViewRepository;
            }
        }
        #endregion

        #region Bussiness
        public BaseRepository<Transaction> TransactionRepository
        {
            get
            {
                if (_transactionRepository == null)
                    _transactionRepository = new BaseRepository<Transaction>(Context);
                return _transactionRepository;
            }
        }
        public BaseRepository<Bet> BetRepository
        {
            get
            {
                if (_betRepository == null)
                    _betRepository = new BaseRepository<Bet>(Context);
                return _betRepository;
            }
        }
        public BaseRepository<Room> RoomRepository
        {
            get
            {
                if (_roomRepository == null)
                    _roomRepository = new BaseRepository<Room>(Context);
                return _roomRepository;
            }
        }

        #endregion

        #endregion

        #region Public Methods and Operators

        public void Save()
        {
            Context.SaveChanges();
        }
        #endregion

        #region Disposed

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
        #endregion
    }
}
