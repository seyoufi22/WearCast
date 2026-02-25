namespace WearCast.Api.Common.Consts
{
    public static class RegexPatterns
    {
        public const string Password = "(?=.*[0-9])(?=.*[\\!@#$%^&*()\\\\\\\\[\\]{}\\-_+=\\~`|:;\\\"'<>,./?])(?=.*[a-z])(?=.*[A-Z])(?=.*).{8,}";

        public const string EgyptianPhoneNumber = @"^01[0125][0-9]{8}$";

        // السجل التجاري (أرقام فقط، من 6 إلى 20 رقم)
        public const string CommercialRegisterNumber = @"^\d{6,20}$";

        // البطاقة الضريبية (9 أرقام بالضبط)
        public const string TaxIdNumber = @"^\d{9}$";
    }
}
