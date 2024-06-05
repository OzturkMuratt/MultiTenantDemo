using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Constants
{
    public static class SchoolAction
    {

        public const string View= nameof(View);
        public const string Create = nameof(Create);
        public const string Update = nameof(Update);
        public const string Delete = nameof(Delete);
        public const string UpgradeSubscription = nameof(UpgradeSubscription);

    }

    public static class SchoolFeature
    {
        public const string Tenants = nameof(Tenants);
        public const string Users = nameof(Users);
        public const string UserRoles = nameof(UserRoles);
        public const string Roles = nameof(Roles);
        public const string RoleClaims = nameof(RoleClaims);
        public const string Schools = nameof(Schools);
    }

    public record SchoolPermission(string Descriptioni, string Action, string Feature, bool IsBasic= false,bool IsRoot = false)
    {
        public string Name => Namefor(Action, Feature);
        public static string Namefor(string action, string feature) => $"Permission.{feature}.{action}";
    }

    public static class SchoolPermissions
    {
        private static readonly SchoolPermission[] _allPermissios =
        [
            new SchoolPermission("View Users",SchoolAction.View,SchoolFeature.Users),
            new SchoolPermission("Update Users",SchoolAction.Update,SchoolFeature.Users),
            new SchoolPermission("Create Users",SchoolAction.Create,SchoolFeature.Users),
            new SchoolPermission("Delete Users",SchoolAction.Delete,SchoolFeature.Users),


            new SchoolPermission("View User Roles",SchoolAction.View,SchoolFeature.UserRoles),
            new SchoolPermission("Update User Roles",SchoolAction.Update,SchoolFeature.UserRoles),

            new SchoolPermission("View Roles",SchoolAction.View,SchoolFeature.Roles),
            new SchoolPermission("Update Roles",SchoolAction.Update,SchoolFeature.Roles),
            new SchoolPermission("Create Roles",SchoolAction.Create,SchoolFeature.Roles),
            new SchoolPermission("Delete Roles",SchoolAction.Delete,SchoolFeature.Roles),

            new SchoolPermission("View Role Claims/Permissions",SchoolAction.View,SchoolFeature.RoleClaims),
            new SchoolPermission("Update Role Claims/Permissions",SchoolAction.Update,SchoolFeature.RoleClaims),

            new SchoolPermission("View Schools",SchoolAction.View,SchoolFeature.Schools,IsBasic:true),
            new SchoolPermission("Update Schools",SchoolAction.Update,SchoolFeature.Schools),
            new SchoolPermission("Create Schools",SchoolAction.Create,SchoolFeature.Schools),
            new SchoolPermission("Delete Schools",SchoolAction.Delete,SchoolFeature.Schools),

            new SchoolPermission("View Tenants",SchoolAction.View,SchoolFeature.Tenants,IsRoot:true),
            new SchoolPermission("Update Tenants",SchoolAction.Update,SchoolFeature.Tenants),
            new SchoolPermission("Create Tenants",SchoolAction.Create,SchoolFeature.Tenants),
            new SchoolPermission("Upgrade Tenants Subscription",SchoolAction.UpgradeSubscription,SchoolFeature.Tenants,IsRoot:true),

         ];
    }
}
