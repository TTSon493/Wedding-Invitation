using System.Security.Claims;
using Wedding.Model.DTO;
using Wedding.Model.Domain;

namespace Wedding.Service.IService;

public interface IEmailService
{
    Task<ResponseDTO> GetAll
    (
        ClaimsPrincipal User,
        string? filterOn,
        string? filterQuery,
        string? sortBy,
        bool? isAscending,
        int pageNumber,
        int pageSize
    );

    Task<bool> SendEmailAsync(string toEmail, string subject, string body);
    Task<bool> SendEmailResetAsync(string toEmail, string subject, ApplicationUser user, string currentDate, string resetLink, string operatingSystem, string browser, string ip, string region, string city, string country);
    Task<bool> SendVerifyEmail(string toMail, string confirmationLink);
    Task<bool> SendEmailRemindDeleteAccount(string toMail);
    Task<bool> SendEmailDeleteAccount(string toMail);
    Task<bool> SendEmailToCustomerAfterPayout(string toMail, double PayoutAmount, DateTime TransactionDate);
}
