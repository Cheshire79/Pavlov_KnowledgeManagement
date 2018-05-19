﻿using System;

using System.Threading.Tasks;
using DAL.Identity.EF;
using DAL.Identity.Entities;
using DAL.Identity.Infrastructure;
using DAL.Identity.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DAL.Identity.Repositories
{
    public class IdentityUnitOfWork : IIdentityUnitOfWork
    {
        private ApplicationContext _db;

        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;



        public IdentityUnitOfWork(ApplicationContext applicationContext, IFactoryUserManager factoryUserManager)
        {

            _db = applicationContext;
            //https://stackoverflow.com/questions/22077967/what-does-kernel-bindsometype-toself-do
            //https://github.com/ninject/ninject/wiki/Object-Scopes
            _userManager = factoryUserManager.CreateUserStore(applicationContext);

           
            _roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(applicationContext));

        }
     

        public ApplicationUserManager UserManager
        {
            get { return _userManager; }
        }


        public ApplicationRoleManager RoleManager
        {
            get { return _roleManager; }
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (_userManager != null)
                        _userManager.Dispose();
                    if (_roleManager != null)
                        _roleManager.Dispose();
                    if (_db != null)
                        _db.Dispose();
                }
                this.disposed = true;
            }
        }
    }
}

