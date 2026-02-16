namespace WearCast.Api.Common.Consts
{
    public static class RegexPatterns
    {
        public const string Password = "(?=.*[0-9])(?=.*[\\!@#$%^&*()\\\\\\\\[\\]{}\\-_+=\\~`|:;\\\"'<>,./?])(?=.*[a-z])(?=.*[A-Z])(?=.*).{8,}";

        public const string EgyptianPhoneNumber = @"^01[0125][0-9]{8}$";
    }
}
