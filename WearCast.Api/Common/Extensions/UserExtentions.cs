using System.Security.Claims;

namespace WearCast.Api.Common.Extensions
{
    public static class UserExtensions
    {
        #region Basic User Info

        public static string? GetUserId(this ClaimsPrincipal user) =>
            user.FindFirstValue(ClaimTypes.NameIdentifier);

        public static string? GetUserRole(this ClaimsPrincipal user) =>
            user.FindFirstValue(ClaimTypes.Role);

        #endregion

        #region Role Checkers

        public static bool IsSuperAdmin(this ClaimsPrincipal user) =>
            user.IsInRole(DefaultRoles.SuperAdmin);

        public static bool IsFactoryManager(this ClaimsPrincipal user) =>
            user.IsInRole(DefaultRoles.FactoryManager);

        public static bool IsSellerManager(this ClaimsPrincipal user) =>
            user.IsInRole(DefaultRoles.SellerManager);

        public static bool IsCustomer(this ClaimsPrincipal user) =>
            user.IsInRole(DefaultRoles.Customer);

        public static bool IsDriver(this ClaimsPrincipal user) =>
            user.IsInRole(DefaultRoles.Driver);

        public static bool IsShippingCompanyManager(this ClaimsPrincipal user) =>
            user.IsInRole(DefaultRoles.ShippingCompanyManager);

        #endregion

        #region Business Actor IDs

        public static int? GetCustomerId(this ClaimsPrincipal user)
        {
            var idString = user.FindFirstValue("CustomerId");
            return int.TryParse(idString, out var id) ? id : null;
        }

        public static int? GetDriverId(this ClaimsPrincipal user)
        {
            var idString = user.FindFirstValue("DriverId");
            return int.TryParse(idString, out var id) ? id : null;
        }

        public static int? GetFactoryManagerId(this ClaimsPrincipal user)
        {
            var idString = user.FindFirstValue("FactoryManagerId");
            return int.TryParse(idString, out var id) ? id : null;
        }

        public static int? GetSellerManagerId(this ClaimsPrincipal user)
        {
            var idString = user.FindFirstValue("SellerManagerId");
            return int.TryParse(idString, out var id) ? id : null;
        }

        public static int? GetShippingCompanyManagerId(this ClaimsPrincipal user)
        {
            var idString = user.FindFirstValue("ShippingCompanyManagerId");
            return int.TryParse(idString, out var id) ? id : null;
        }

        public static string? GetAdminId(this ClaimsPrincipal user) =>
            user.GetUserId();

        public static int? GetFactoryId(this ClaimsPrincipal user)
        {
            var idString = user.FindFirstValue("FactoryId");
            return int.TryParse(idString, out var id) ? id : null;
        }

        public static int? GetSellerId(this ClaimsPrincipal user)
        {
            var idString = user.FindFirstValue("SellerId");
            return int.TryParse(idString, out var id) ? id : null;
        }

        public static int? GetShippingCompanyId(this ClaimsPrincipal user)
        {
            var idString = user.FindFirstValue("ShippingCompanyId");
            return int.TryParse(idString, out var id) ? id : null;
        }

        #endregion
    }
}