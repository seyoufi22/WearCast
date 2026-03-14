namespace WearCast.Api.Common.Consts
{
    public static class RegexPatterns
    {
        public const string Password = "(?=.*[0-9])(?=.*[\\!@#$%^&*()\\\\\\\\[\\]{}\\-_+=\\~`|:;\\\"'<>,./?])(?=.*[a-z])(?=.*[A-Z])(?=.*).{8,}";

        public const string EgyptianPhoneNumber = @"^01[0125][0-9]{8}$";

        public const string CommercialRegisterNumber = @"^\d{6,20}$";

        public const string TaxIdNumber = @"^\d{9}$";

        public const string HexColorCode = @"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$";
    }
}
