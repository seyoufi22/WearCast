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
                _webHostEnvironment.ContentRootPath,
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
        public async Task SendCustomerConfirmationEmail(ApplicationUser user, string code)
        {
            var emailBody = EmailBodyBuilder.GenerateEmailBody(
                "CustomerEmailConfirmation",
                _webHostEnvironment.ContentRootPath,
                templateModel: new Dictionary<string, string>
                {
                { "{{name}}", $"{user.FirstName} {user.LastName}" },
                { "{{code}}", code }
                }
            );

            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(
                user.Email!,
                "Welcome to WearCast! Confirm your email",
                emailBody
            ));

            await Task.CompletedTask;
        }
        public async Task SendDriverConfirmationEmail(ApplicationUser user, string code)
        {
            var emailBody = EmailBodyBuilder.GenerateEmailBody(
                "DriverEmailConfirmation",
                _webHostEnvironment.ContentRootPath,
                templateModel: new Dictionary<string, string>
                {
                { "{{name}}", $"{user.FirstName} {user.LastName}" },
                { "{{code}}", code }
                }
            );

            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(
                user.Email!,
                "🚗 WearCast Delivery: Activate Your Account",
                emailBody
            ));

            await Task.CompletedTask;
        }

        public async Task SendFactoryManagerConfirmationEmail(ApplicationUser user, string code)
        {
            var emailBody = EmailBodyBuilder.GenerateEmailBody(
                "FactoryManagerEmailConfirmation",
                _webHostEnvironment.ContentRootPath,
                templateModel: new Dictionary<string, string>
                {
                { "{{name}}", $"{user.FirstName} {user.LastName}" },
                { "{{code}}", code }
                }
            );

            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(
                user.Email!,
                "🏭 WearCast Partners: Verify Factory Portal",
                emailBody
            ));

            await Task.CompletedTask;
        }
        public async Task SendShippingCompanyManagerConfirmationEmail(ApplicationUser user, string code)
        {
            var emailBody = EmailBodyBuilder.GenerateEmailBody(
                "ShippingCompanyManagerEmailConfirmation",
                _webHostEnvironment.ContentRootPath,
                templateModel: new Dictionary<string, string>
                {
                { "{{name}}", $"{user.FirstName} {user.LastName}" },
                { "{{code}}", code }
                }
            );

            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(
                user.Email!,
                "📦 WearCast Logistics: Verify Your Account",
                emailBody
            ));

            await Task.CompletedTask;
        }

        public async Task SendResetPasswordEmail(ApplicationUser user, string code)
        {
            var emailBody = EmailBodyBuilder.GenerateEmailBody(
                "ForgetPassword",
                _webHostEnvironment.ContentRootPath,
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

        public Task SendConfirmationEmailForSellerManager(SellerApplication app, string code)
        {
            var emailBody = EmailBodyBuilder.GenerateEmailBody(
                "EmailConfirmationForSellerManager",
                _webHostEnvironment.ContentRootPath,
                templateModel: new Dictionary<string, string>
                {
            { "{{managerName}}", app.ManagerFirstName },
            { "{{sellerName}}", app.SellerName },
            { "{{code}}", code }
                }
            );

            BackgroundJob.Enqueue<IEmailSender>(sender => sender.SendEmailAsync(
                app.ManagerEmail!,
                "🔐 WearCast App: Email Confirmation",
                emailBody
            ));

            return Task.CompletedTask;
        }
        public Task SendSellerManagerConfirmationEmail(ApplicationUser user, string code)
        {
            var emailBody = EmailBodyBuilder.GenerateEmailBody(
                "EmailConfirmationForNewManager",
                _webHostEnvironment.ContentRootPath,
                templateModel: new Dictionary<string, string>
                {
            { "{{managerName}}", user.FirstName },
            { "{{code}}", code }
                }
            );

            BackgroundJob.Enqueue<IEmailSender>(sender => sender.SendEmailAsync(
                user.Email!,
                "🔐 WearCast App: Welcome to your Manager Account",
                emailBody
            ));

            return Task.CompletedTask;
        }

        public async Task SendSellerApplicationApprovedEmail(SellerApplication app)
        {
            var emailBody = EmailBodyBuilder.GenerateEmailBody(
                "SellerApplicationApproved",
                _webHostEnvironment.ContentRootPath,
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
                _webHostEnvironment.ContentRootPath,
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
        public async Task SendAccountDeletedEmail(string email, string fullName, string reason)
        {
            var emailBody = EmailBodyBuilder.GenerateEmailBody(
                "AccountDeleted",
                _webHostEnvironment.ContentRootPath,
                templateModel: new Dictionary<string, string>
                {
            { "{{name}}", fullName },
            { "{{reason}}", reason }
                }
            );

            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(
                email,
                "⚠️ WearCast: Your Account Has Been Deleted",
                emailBody
            ));

            await Task.CompletedTask;
        }
    }
}