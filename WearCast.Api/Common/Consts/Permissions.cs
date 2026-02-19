namespace WearCast.Api.Common.Consts
{
    //permissions for all endpoints
    public static class Permissions
    {
        public static string Type { get; } = "permissions";

        public const string GetCategorys = "categorys:read";//name of module or entity: name of the operation
        public const string AddCategorys = "categorys:add";
        public const string UpdateCategorys = "categorys:update";
        public const string DeleteCategorys = "categorys:delete";

        public static IList<string> GetAllPermissions()
        {
            return typeof(Permissions)
                .GetFields()
                .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string))
                .Select(f => (string)f.GetValue(null)!)
                .ToList();
        }

        public static readonly List<string> CustomerPermissions =
        [

        ];

        public static readonly List<string> SellerPermissions =
        [

        ];

        public static readonly List<string> FactoryPermissions =
        [

        ];

        public static readonly List<string> ShippingCompanyPermissions =
        [

        ];

        public static readonly List<string> DriverPermissions =
        [

        ];
    }
}
