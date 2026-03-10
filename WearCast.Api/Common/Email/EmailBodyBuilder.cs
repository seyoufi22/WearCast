namespace WearCast.Api.Common.Email
{
    public static class EmailBodyBuilder
    {
        public static string GenerateEmailBody(string template, string webRootPath, Dictionary<string, string> templateModel)
        {
            var templatePath = Path.Combine(webRootPath, "Templates", $"{template}.html");

            using var streamReader = new StreamReader(templatePath);
            var body = streamReader.ReadToEnd();

            foreach (var item in templateModel)
            {
                body = body.Replace(item.Key, item.Value);
            }

            return body;
        }
    }
}