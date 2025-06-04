using Wedding.Model.Domain;
using Wedding.Utility.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Wedding.DataAccess.Seeding;

public static class ApplicationDbContextSeed
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void SeedEmailTemplate(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmailTemplate>().HasData(
            new
            {
                Id = Guid.NewGuid(),
                TemplateName = "WelcomeEmail",
                SenderName = "Wedding Team",
                SenderEmail = "cursusservicetts@gmail.com",
                Category = "Welcome",
                SubjectLine = "Welcome to Wedding!",
                PreHeaderText = "Thank you for signing up!",
                PersonalizationTags = "{FirstName}, {LastName}",
                BodyContent =
                    "Dear [UserFullName],<br><br>Welcome to Wedding! We are excited to have you join our learning community.",
                FooterContent = "<p>Contact us at cursusservicetts@gmail.com</p>",
                CallToAction = "<a href=\"https://weddinginvations.web.app/user/sign-in\">Login</a>",
                Language = "English",
                RecipientType = "Customer",
                CreateBy = "System",
                CreateTime = DateTime.Now,
                UpdateBy = "Admin",
                UpdateTime = DateTime.Now,
                Status = 1
            },
            new
            {
                Id = Guid.NewGuid(),
                TemplateName = "ForgotPasswordEmail",
                SenderName = "Wedding Team",
                SenderEmail = "cursusservicetts@gmail.com",
                Category = "Security",
                SubjectLine = "Reset Your Password",
                PreHeaderText = "Reset your password to regain access",
                PersonalizationTags = "[UserFullName], [ResetPasswordLink]",
                BodyContent =
                    "Hi [UserFullName],<br><br>We received a request to reset your password. Click the link below to reset your password.",
                FooterContent = "If you did not request a password reset, please ignore this email.",
                CallToAction =
                    $"https://weddinginvations.web.app/sign-in/verify-email?userId=user.Id&token=Uri.EscapeDataString(token)",
                Language = "English",
                RecipientType = "Customer",
                CreateBy = "System",
                CreateTime = DateTime.Now,
                UpdateBy = "Admin",
                UpdateTime = DateTime.Now,
                Status = 1
            },
            new
            {
                Id = Guid.NewGuid(),
                TemplateName = "SendVerifyEmail",
                SenderName = "Wedding Team",
                SenderEmail = "cursusservicetts@gmail.com",
                Category = "Verify",
                SubjectLine = "Wedding Verify Email",
                PreHeaderText = "User Account Verified!",
                PersonalizationTags = "{FirstName}, {LinkLogin}",
                BodyContent = "<p>Thank you for registering your Wedding account. Click here to go back the page</p>",
                FooterContent = "<p>Contact us at cursusservicetts@gmail.com</p>",
                CallToAction = "<a href=\"{{Login}}\">Login now</a>",
                Language = "English",
                RecipientType = "Customer",
                CreateBy = "System",
                CreateTime = DateTime.Now,
                UpdateBy = "Admin",
                UpdateTime = DateTime.Now,
                Status = 1
            },
            new
            {
                Id = Guid.NewGuid(),
                TemplateName = "ChangePassword",
                SenderName = "Wedding Team",
                SenderEmail = "cursusservicetts@gmail.com",
                Category = "Security",
                SubjectLine = "Reset Your Password",
                PreHeaderText = "Reset your password to regain access.",
                PersonalizationTags = "{FirstName}, {ResetLink}",
                BodyContent =
                    "<p>Hello {FirstName},</p><p>Click <a href=\"{ResetLink}\">here</a> to reset your password.</p>",
                FooterContent = "<p>Contact us at cursusservicetts@gmail.com</p>",
                CallToAction = "<a href=\"{{ResetLink}}\">Reset Password</a>",
                Language = "English",
                RecipientType = "Customer",
                CreateBy = "System",
                CreateTime = DateTime.Now,
                UpdateBy = "Admin",
                UpdateTime = DateTime.Now,
                Status = 1
            },
            new
            {
                Id = Guid.NewGuid(),
                TemplateName = "RemindDeleteAccount",
                SenderName = "Wedding Team",
                SenderEmail = "cursusservicetts@gmail.com",
                Category = "Remind Account",
                SubjectLine = "Remind Delete Account!",
                PreHeaderText = "Hello!",
                PersonalizationTags = "{FirstName}, {LastName}",
                BodyContent =
                    "Dear [UserFullName],<br><br>Your account will be deleted after 14 days.",
                FooterContent = "<p>Contact us at cursusservicetts@gmail.com</p>",
                CallToAction = "<a href=\"https://weddinginvations.web.app/user/sign-in\">Login</a>",
                Language = "English",
                RecipientType = "Customer",
                CreateBy = "System",
                CreateTime = DateTime.Now,
                UpdateBy = "Admin",
                UpdateTime = DateTime.Now,
                Status = 1
            },
            new
            {
                Id = Guid.NewGuid(),
                TemplateName = "CustomerCompleteCourse",
                SenderName = "Wedding Team",
                SenderEmail = "cursusservicetts@gmail.com",
                Category = "Course completed",
                SubjectLine = "Congratulations on completing the course!",
                PreHeaderText = "Hello!",
                PersonalizationTags = "{FirstName}, {LastName}",
                BodyContent =
                    "Dear [UserFullName],<br><br>You have completed our course program, you can take new courses to increase your knowledge and skills.",
                FooterContent = "<p>Contact us at cursusservicetts@gmail.com</p>",
                CallToAction = "<a href=\"https://weddinginvations.web.app/user/sign-in\">Login</a>",
                Language = "English",
                RecipientType = "Customer",
                CreateBy = "System",
                CreateTime = DateTime.Now,
                UpdateBy = "Admin",
                UpdateTime = DateTime.Now,
                Status = 1
            },
            new
            {
                Id = Guid.NewGuid(),
                TemplateName = "DeleteAccount",
                SenderName = "Wedding Team",
                SenderEmail = "cursusservicetts@gmail.com",
                Category = "Delete Account",
                SubjectLine = "Delete Account!",
                PreHeaderText = "Hello!",
                PersonalizationTags = "{FirstName}, {LastName}",
                BodyContent =
                    "Dear [UserFullName],<br><br>Your account has been deleted.",
                FooterContent = "<p>Contact us at cursusservicetts@gmail.com</p>",
                CallToAction = "<a href=\"https://weddinginvations.web.app/user/sign-in\">Login</a>",
                Language = "English",
                RecipientType = "Customer",
                CreateBy = "System",
                CreateTime = DateTime.Now,
                UpdateBy = "Admin",
                UpdateTime = DateTime.Now,
                Status = 1
            },
            new
            {
                Id = Guid.NewGuid(),
                TemplateName = "NotifyCustomerPaymentReceived",
                SenderName = "Wedding Team",
                SenderEmail = "cursusservicetts@gmail.com",
                Category = "Payout",
                SubjectLine = "Your Wedding Payout is Complete!",
                PreHeaderText = "Payout Successful!",
                PersonalizationTags = "{FirstName}, {LastName}, {PayoutAmount}, {TransactionDate}",
                BodyContent =
                    @"Dear {FirstName} {LastName},<br><br>

                    This email confirms that your payout request has been processed successfully.
                    <br>
                    <strong>Payout Details:</strong>
                    <ul>
                    <li>Amount: {PayoutAmount}</li>
                    <li>Transaction Date: {TransactionDate}</li> 
                    </ul>
                    <br>
                    You can view your payout history in your customer dashboard. 
                    <br> 
                    Thank you for being a valued Wedding customer!
                    <br>",
                FooterContent = "<p>Contact us at cursusservicetts@gmail.com</p>",
                CallToAction = "<a href=\"https://weddinginvations.web.app/user/sign-in\">Login</a>",
                Language = "English",
                RecipientType = "Customer",
                CreateBy = "System",
                CreateTime = DateTime.Now,
                UpdateBy = "Admin",
                UpdateTime = DateTime.Now,
                Status = 1
            }
        );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void SeedAdminAccount(ModelBuilder modelBuilder)
    {
        var studentRoleId = "8fa7c7bb-b4dc-480d-a660-e07a90855d5d";
        var adminRoleId = "8fa7c7bb-daa5-a660-bf02-82301a5eb327"; // Add admin role

        var roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Id = studentRoleId,
                ConcurrencyStamp = StaticUserRoles.Customer,
                Name = StaticUserRoles.Customer,
                NormalizedName = StaticUserRoles.Customer,
            },
            new IdentityRole
            {
                Id = adminRoleId,
                ConcurrencyStamp = StaticUserRoles.Admin,
                Name = StaticUserRoles.Admin,
                NormalizedName = StaticUserRoles.Admin,
            }
        };

        modelBuilder.Entity<IdentityRole>().HasData(roles);

        // Seeding admin user
        var adminUserId = "TranThaiSon493";
        var hasher = new PasswordHasher<ApplicationUser>();
        var adminUser = new ApplicationUser
        {
            Id = adminUserId,
            Gender = "Male", // Set appropriate value
            FullName = "Admin User",
            BirthDate = new DateTime(1990, 1, 1), // Set appropriate value
            AvatarUrl = "https://example.com/avatar.png", // Set appropriate value
            Country = "Country", // Set appropriate value
            Address = "123 Admin St",
            TaxNumber = "123456789",
            UserName = "admin@gmail.com",
            NormalizedUserName = "ADMIN@GMAIL.COM",
            Email = "admin@gmail.com",
            NormalizedEmail = "ADMIN@GMAIL.COM",
            EmailConfirmed = true,
            PasswordHash = hasher.HashPassword(null, "Admin123!"),
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            PhoneNumber = "1234567890",
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = false,
            LockoutEnd = null,
            LockoutEnabled = true,
            AccessFailedCount = 0,
            UpdateTime = new DateTime(2003, 1, 12)
        };

        modelBuilder.Entity<ApplicationUser>().HasData(adminUser);

        // Assigning the admin role to the admin user
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = adminRoleId,
            UserId = adminUserId
        });
    }

    public static void SeedCompany(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>().HasData(
            new Company
            {
                Id = Guid.NewGuid(),
                Name = "ABC Corp",
                Address = "123 Main St",
                City = "Hometown",
                State = "State",
                Country = "Country",
                PostalCode = "12345",
                Phone = "123-456-7890",
                Email = "contact@abccorp.com",
                Website = "http://www.abccorp.com",
                FoundedDate = new DateTime(2000, 1, 1),
                LogoUrl = "http://www.abccorp.com/logo.png",
                Description = "ABC Corp is a leading company in XYZ industry."
            }
        );
    }

    public static void SeedPrivacy(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Privacy>().HasData(
            new Privacy()
            {
                Id = Guid.NewGuid(),
                Title = "Privacy",
                Content = "These are the privacy for our service.",
                LastUpdated = DateTime.UtcNow,
                IsActive = true
            }
        );
    }

    public static void SeedTermOfUse(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TermOfUse>().HasData(
            new TermOfUse
            {
                Id = Guid.NewGuid(),
                Title = "Terms of Use",
                Content = "These are the terms of use for our service.",
                LastUpdated = DateTime.UtcNow,
                IsActive = true
            }
        );
    }
}