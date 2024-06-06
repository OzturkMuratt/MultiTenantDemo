using Infrastructure.Tenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.DbInitializers
{
    internal class TenatDbInitializer : ITenatDbInitializer
    {
        private readonly TenantDbContext _tenantDbContext;
        public TenatDbInitializer(TenantDbContext tenantDbContext)
        {
            _tenantDbContext = tenantDbContext;
        }
        public async Task InitializeApplicationDbForTenantAsync(ABCSchoolTenantInfo tenant, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task InitializeDatabaseWithTenantAsync(CancellationToken cancellationToken)
        {
            //Check if root tenant already exist
            if (await _tenantDbContext.TenantInfo.FindAsync([TenancyConstants.Root.Id],cancellationToken:cancellationToken)
                is null)
            {
                   //Create root tenant
                   var rootTenant=new ABCSchoolTenantInfo
                   {
                       Id=TenancyConstants.Root.Id,
                       Identifier=TenancyConstants.Root.Name,
                       Name=TenancyConstants.Root.Name,
                       AdminEmail=TenancyConstants.Root.Email,
                        IsActive=true,
                       ValidUpTo=DateTime.UtcNow.AddYears(5)
                    };
                await _tenantDbContext.TenantInfo.AddAsync(rootTenant);
                await _tenantDbContext.SaveChangesAsync(cancellationToken);
            }
         //--> skip
         
            
        }
    }
}
