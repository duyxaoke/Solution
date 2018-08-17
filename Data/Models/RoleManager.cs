﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Data.DAL;

namespace Data.Models
{
    public class RoleManager : RoleManager<ApplicationRole>
    {
        private readonly DatabaseContext context;


        public RoleManager(DatabaseContext context)
            : base(new RoleStore<ApplicationRole>(context))
        {
            this.context = context;
        }


        public async Task<List<Claim>> GetClaimsAsync(string roleName)
        {
            var roleClaims = await context.RoleClaims
                .Where(rc => rc.Role.Name == roleName)
                .ToListAsync();

            var claims = roleClaims
                .Select(rc => new Claim(rc.ClaimType, rc.ClaimValue))
                .ToList();

            return claims;
        }


        public async Task AddClaimAsync(string roleId, Claim claim)
        {
            var roleClaim = new RoleClaim()
            {
                RoleId = roleId,
                ClaimType = claim.Type,
                ClaimValue = claim.Value,
            };

            context.RoleClaims.Add(roleClaim);

            await context.SaveChangesAsync();
        }


        public async Task RemoveClaimAsync(string roleId, Claim claim)
        {
            var toRemoveClaim = await context.RoleClaims
                .FirstOrDefaultAsync(rc => rc.RoleId == roleId && rc.ClaimType == claim.Type && rc.ClaimValue == claim.Value);

            if (toRemoveClaim != null)
            {
                context.RoleClaims.Remove(toRemoveClaim);
                await context.SaveChangesAsync();
            }
        }
    }
}