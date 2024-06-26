﻿using Application.Features.Tenancy;
using Finbuckle.MultiTenant;
using Infrastructure.Identity.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tenancy
{
    internal static class TenancyServiceExtensions
    {

        internal static IServiceCollection AddMultiTenancyServices(
         this IServiceCollection services,
         IConfiguration configuration)
        {

            return services
                .AddDbContext<TenantDbContext>(options => options
                    .UseSqlServer(configuration.GetConnectionString("DefaultConnection")))
                .AddMultiTenant<ABCSchoolTenantInfo>()
                    .WithHeaderStrategy(TenancyConstants.TenantIdName)
                    .WithClaimStrategy(ClaimConstants.Tenant)
                    .WithCustomQueryStringStrategy(TenancyConstants.TenantIdName)
                    .WithEFCoreStore<TenantDbContext, ABCSchoolTenantInfo>()
                    .Services
                    .AddScoped<ITenantService,TenantService>();
        }

        private static FinbuckleMultiTenantBuilder<ABCSchoolTenantInfo> WithCustomQueryStringStrategy
            (this FinbuckleMultiTenantBuilder<ABCSchoolTenantInfo> builder, string customQueryStringStrategy)
        {
            return builder
                .WithDelegateStrategy(context =>
                {
                    if (context is not HttpContext httpContext)
                    {
                        return Task.FromResult((string)null);
                    }
                    httpContext.Request.Query.TryGetValue(customQueryStringStrategy, out StringValues tenantIdParam);

                    return Task.FromResult(tenantIdParam.ToString());
                });
        }
    }
}
