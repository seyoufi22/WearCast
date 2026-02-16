using Hangfire;

namespace WearCast.Api.Common.Email
{
    public class EmailHelper(IEmailSender emailSender)
    {
        private readonly IEmailSender _emailSender = emailSender;

        public async Task SendConfirmationEmail(ApplicationUser user, string code)
        {
            var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",
           templateModel: new Dictionary<string, string>
           {
            { "{{name}}", $"{user.FirstName} {user.LastName}" },
            { "{{code}}", code }
           }
       );

            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(
                user.Email!,
                "🔐 WearCast App: Email Confirmation",
                emailBody
            ));

            await Task.CompletedTask;
        }
    }
}
