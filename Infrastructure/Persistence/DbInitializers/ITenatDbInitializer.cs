using Infrastructure.Tenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.DbInitializers
{
    internal interface ITenatDbInitializer
    {
        Task InitializeDatabaseWithTenantAsync(CancellationToken cancellationToken);
        Task InitializeApplicationDbForTenantAsync(ABCSchoolTenantInfo tenant,CancellationToken cancellationToken);

    }
}
