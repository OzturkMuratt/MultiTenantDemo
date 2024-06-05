﻿using Finbuckle.MultiTenant;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Contexts
{
    public abstract class BaseDbContext :
        MultiTenantIdentityDbContext<ApplicationUser
            , ApplicaitonRole
            , string, IdentityUserClaim<string>
            , IdentityUserRole<string>
            , IdentityUserLogin<string>
            , IdentityRoleClaim<string>
            , IdentityUserToken<string>
            >
    {
        protected BaseDbContext
            (
                ITenantInfo tenantInfo,
                DbContextOptions options
            ) : base(tenantInfo, options)
        {



        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}