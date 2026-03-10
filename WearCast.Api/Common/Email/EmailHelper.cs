using Hangfire;

namespace WearCast.Api.Common.Email
{
    public class EmailHelper(IEmailSender emailSender, IWebHostEnvironment webHostEnvironment)
    {
        private readonly IEmailSender _emailSender = emailSender;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        public async Task SendConfirmationEmail(ApplicationUser user, string code)
        {
            var emailBody = EmailBodyBuilder.GenerateEmailBody(
                "EmailConfirmation",
                _webHostEnvironment.WebRootPath,
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
            var emailBody = EmailBodyBuilder.GenerateEmailBody(
                "ForgetPassword",
                _webHostEnvironment.WebRootPath,
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

        public async Task SendConfirmationEmailForSellerManager(SellerApplication app, string code)
        {
            var emailBody = EmailBodyBuilder.GenerateEmailBody(
                "EmailConfirmationForSellerManager",
                _webHostEnvironment.WebRootPath,
                templateModel: new Dictionary<string, string>
                {
                    { "{{managerName}}", app.ManagerFirstName },
                    { "{{sellerName}}", app.SellerName },
                    { "{{code}}", code }
                }
            );

            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(
                app.ManagerEmail!,
                "🔐 WearCast App: Email Confirmation",
                emailBody
            ));

            await Task.CompletedTask;
        }

        public async Task SendSellerApplicationApprovedEmail(SellerApplication app)
        {
            var emailBody = EmailBodyBuilder.GenerateEmailBody(
                "SellerApplicationApproved",
                _webHostEnvironment.WebRootPath,
                templateModel: new Dictionary<string, string>
                {
                    { "{{managerName}}", app.ManagerFirstName },
                    { "{{sellerName}}", app.SellerName }
                }
            );

            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(
                app.ManagerEmail!,
                "🎉 Congratulations! Your WearCast Seller Application is Approved",
                emailBody
            ));

            await Task.CompletedTask;
        }

        public async Task SendSellerApplicationRejectedEmail(SellerApplication app)
        {
            var emailBody = EmailBodyBuilder.GenerateEmailBody(
                "SellerApplicationRejected",
                _webHostEnvironment.WebRootPath,
                templateModel: new Dictionary<string, string>
                {
                    { "{{managerName}}", app.ManagerFirstName },
                    { "{{sellerName}}", app.SellerName },
                    { "{{reason}}", app.RejectionReason ?? "No specific reason was provided. Please contact support for more details." }
                }
            );

            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(
                app.ManagerEmail!,
                "⚠️ WearCast: Update on Your Seller Application",
                emailBody
            ));

            await Task.CompletedTask;
        }
    }
}