using Wedding.DataAccess.Context;
using Wedding.DataAccess.IRepository;
using Wedding.DataAccess.Repository;
using Wedding.Model.Domain;
using Wedding.Service.IService;
using Wedding.Service.Service;
using Microsoft.AspNetCore.Identity;

namespace Wedding.API.Extentions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        // Registering IUnitOfWork with its implementation UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        // Registering IBaseService with its implementation BaseService
        services.AddScoped<IBaseService, BaseService>();
        // Registering IAuthService with its implementation AuthService
        services.AddScoped<IAuthService, AuthService>();
        // Registering IEmailService with its implementation EmailService
        services.AddScoped<IEmailService, EmailService>();
        // Registering ITokenService with its implementation TokenService
        services.AddScoped<ITokenService, TokenService>();
        // Registering IUserManagerRepository its implementation UserManagerRepository
        services.AddScoped<IUserManagerRepository, UserManagerRepository>();
        // Registering IEmailSender its implementation EmailSender
        services.AddScoped<IEmailSender, EmailSender>();
        // Registering ICartService its implementation CartService
        services.AddScoped<ICartService, CartService>();
        // Registering IStudentsService its implementation StudentsService
        services.AddScoped<ICustomersService, CustomerService>();
        // Registering IRedisService with its implementation RedisService
        services.AddScoped<IRedisService, RedisService>();
        // Registering IClosedXMLService its implementation ClosedXMLService
        services.AddScoped<IClosedXMLService, ClosedXMLService>();
        // Registering IStripeService its implementation StripeService
        services.AddScoped<IStripeService, StripeService>();
        // Registering IBalanceService its implementation BalanceService
        services.AddScoped<IBalanceService, BalanceService>();
        // Registering IPaymentService its implementation PaymentService
        services.AddScoped<IPaymentService, PaymentService>();
        // Registering ITransactionService its implementation TransactionService
        services.AddScoped<ITransactionService, TransactionService>();
        // Registering IOrderService its implementation OrderService
        services.AddScoped<IOrderService, OrderService>();
        // Registering IOrderStatusService its implementation OrderStatusService
        services.AddScoped<IOrderStatusService, OrderStatusService>();
        // Registering ICompanyService its implementation CompanyService
        services.AddScoped<ICompanyService, CompanyService>();
        // Registering ITermOfUseService its implementation TermOfUseService
        services.AddScoped<ITermOfUseService, TermOfUseService>();
        // Registering IPrivacyService its implementation PrivacyService
        services.AddScoped<IPrivacyService, PrivacyService>();
        // Registering IActivityLogService its implementation ActivityLogService
        services.AddScoped<IActivityLogService, ActivityLogService>();
        // Registering ICardManagementService its implementation CardManagementService
        services.AddScoped<ICardManagementService, CardManagementService>();
        // Registering IInvitationService its implementation InvitationService
        services.AddScoped<IInvitationService, InvitationService>();
        // Registering IInvitationService its implementation IInvitationService
        services.AddScoped<IInvitationTemplateService, InvitationTemplateService>();
        // Registering IWeddingService its implementation WeddingService
        services.AddScoped<IWeddingService, WeddingService>();
        // Registering IEventService its implementation EventService
        services.AddScoped<IEventService, EventService>();
        // Registering IEventPhotoService its implementation EventPhotoService
        services.AddScoped<IEventPhotoService, EventPhotoService>();
        // Registering IGuestService its implementation GuestService
        services.AddScoped<IGuestService, GuestService>();
        // Registering IGuestListService its implementation GuestListService
        services.AddScoped<IGuestListService, GuestListService>();
        // Registering IVnPayService its implementation VnPayService
        services.AddScoped<IVnPayService, VnPayService>();

        // Register the Identity services with default configuration
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}




