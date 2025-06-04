using Wedding.DataAccess.IRepository;

namespace Wedding.DataAccess.IRepository;

public interface IUnitOfWork
{
    ICustomerRepository CustomerRepository { get; }
    IUserManagerRepository UserManagerRepository { get; }
    IEmailTemplateRepository EmailTemplateRepository { get; }
    ICartHeaderRepository CartHeaderRepository { get; }
    ICartDetailsRepository CartDetailsRepository { get; }
    ICardRepository CardRepository { get; }
    IBalanceRepository BalanceRepository { get; }
    ITransactionRepository TransactionRepository { get; }
    IOrderDetailsRepository OrderDetailsRepository { get; }
    IOrderHeaderRepository OrderHeaderRepository { get; }
    IOrderStatusRepository OrderStatusRepository { get; }
    ITermOfUseRepository TermOfUseRepository { get; }
    ICompanyRepository CompanyRepository { get; }
    IPrivacyRepository PrivacyRepository { get; }
    IActivityLogRepository ActivityLogRepository { get; }
    ICardManagementRepository CardManagementRepository { get; }
    IInvitationRepository InvitationRepository { get; }
    IInvitationTemplateRepository InvitationTemplateRepository { get; }
    IWeddingRepository WeddingRepository { get; }
    IEventRepository EventRepository { get; }
    IEventPhotoRepository EventPhotoRepository { get; }
    IGuestRepository GuestRepository { get; }
    IGuestListRepository GuestListRepository { get; }
    IInvitationHtmlRepository InvitationHtmlRepository { get; }

    Task<int> SaveAsync();
}
