using Infrastructure.Identity.Constants;
using Infrastructure.Identity.Models;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Tenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.DbInitializers
{
    internal class ApplicationDbInitializer
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ABCSchoolTenantInfo _tenant;
        public ApplicationDbInitializer(ABCSchoolTenantInfo tenant,
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _tenant = tenant;
        }
        private async Task InitializeDatabaseAsync(ApplicationDbContext applicationDbContext, CancellationToken cancellationToken)
        {
            await InitializeDefaultRolesAsync(applicationDbContext, cancellationToken);
            await InitializeAdminUser();
        }
        private async Task InitializeDefaultRolesAsync(ApplicationDbContext applicationDbContext, CancellationToken cancellationToken)
        {

            foreach (string rolename in RoleConstants.DefaultRoles)
            {
                if (await _roleManager.Roles.SingleOrDefaultAsync(role => role.Name == rolename)
                    is not ApplicationRole incomingrole)
                {
                    incomingrole = new ApplicationRole
                    {
                        Name = rolename,
                        Description = $"{rolename} Role"
                    };

                    await _roleManager.CreateAsync(incomingrole);
                }

                if (rolename == RoleConstants.Basic)
                {
                    await AssignPermissionsToRole(applicationDbContext,
                    SchoolPermissions.Basic,
                    incomingrole,
                    cancellationToken);
                }
                else if (rolename == RoleConstants.Admin)
                {
                    await AssignPermissionsToRole(applicationDbContext,
                  SchoolPermissions.Admin,
                  incomingrole,
                  cancellationToken);
                }

            }


        }

        private async Task InitializeAdminUser()
        {
            if (string.IsNullOrEmpty(_tenant.AdminEmail))
            {
                return;
            }
            if (await _userManager.Users.FirstOrDefaultAsync(x=> x.Email==_tenant.AdminEmail) is not ApplicationUser adminUser)
            {
 
                adminUser = new ApplicationUser
                {
                    FirstName = TenancyConstants.FirstName,
                    LastName = TenancyConstants.LasrName,
                    Email = _tenant.AdminEmail,
                    UserName = _tenant.AdminEmail,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    NormalizedEmail=_tenant.AdminEmail.ToUpperInvariant(),
                    NormalizedUserName=_tenant.AdminEmail.ToUpperInvariant(),
                    IsActive = true,
                };

                var password = new PasswordHasher<ApplicationUser>();
                adminUser.PasswordHash = password.HashPassword(adminUser, TenancyConstants.DefaultPassword);
                await _userManager.CreateAsync(adminUser);
            }

            if (!await _userManager.IsInRoleAsync(adminUser,RoleConstants.Admin))
            {
                await _userManager.AddToRoleAsync(adminUser, RoleConstants.Admin);

            }

        }

        private async Task AssignPermissionsToRole(
            ApplicationDbContext applicationDbContext,
            IReadOnlyList<SchoolPermission> rolePermissions,
            ApplicationRole currenRole,
            CancellationToken cancellationToken)
        {
            var currentClaims = await _roleManager.GetClaimsAsync(currenRole);
            foreach (var schoolPermission in rolePermissions)
            {
                if (!currentClaims.Any(c => c.Type == ClaimConstants.Permission && c.Value == schoolPermission.Name))
                {
                    await applicationDbContext.RoleClaims.AddAsync(new IdentityRoleClaim<string>
                    {
                        RoleId = currenRole.Id,
                        ClaimType = ClaimConstants.Permission,
                        ClaimValue = schoolPermission.Name
                    }, cancellationToken
                     );
                }
                await applicationDbContext.SaveChangesAsync(cancellationToken);
            }
        }

    }
}
