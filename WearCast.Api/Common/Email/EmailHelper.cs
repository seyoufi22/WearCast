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
        public async Task SendResetPasswordEmail(ApplicationUser user, string code)
        {
            var emailBody = EmailBodyBuilder.GenerateEmailBody("ForgetPassword",
                templateModel: new Dictionary<string, string>
                {
            { "{{name}}", $"{user.FirstName} {user.LastName}" },
            { "{{code}}", code }
                }
            );

            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(
                user.Email!,
                "🔐 WearCast App: Reset Password",
                emailBody
            ));

            await Task.CompletedTask;
        }
        public async Task SendConfirmationEmailForSellerApplication(SellerApplication app, string code)
        {
            var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmationForSellerApplication",
           templateModel: new Dictionary<string, string>
           {
            { "{{name}}", app.SellerName },
            { "{{code}}", code }
           }
        );

            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(
                app.Email!,
                "🔐 WearCast App: Email Confirmation",
                emailBody
            ));

            await Task.CompletedTask;
        }
        public async Task SendSellerApplicationApprovedEmail(SellerApplication app)
        {
            var emailBody = EmailBodyBuilder.GenerateEmailBody("SellerApplicationApproved",
                templateModel: new Dictionary<string, string>
                {
                    { "{{name}}", app.SellerName }
                }
            );

            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(
                app.Email!,
                "🎉 WearCast: Your Seller Application is Approved!",
                emailBody
            ));

            await Task.CompletedTask;
        }

        public async Task SendSellerApplicationRejectedEmail(SellerApplication app)
        {
            var emailBody = EmailBodyBuilder.GenerateEmailBody("SellerApplicationRejected",
                templateModel: new Dictionary<string, string>
                {
                    { "{{name}}", app.SellerName },
                    { "{{reason}}", app.RejectionReason ?? "No specific reason was provided." }
                }
            );

            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(
                app.Email!,
                "⚠️ WearCast: Update on Your Seller Application",
                emailBody
            ));

            await Task.CompletedTask;
        }
    }
}
