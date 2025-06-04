using Wedding.DataAccess.Context;
using Wedding.DataAccess.IRepository;
using Wedding.Model.Domain;
using Microsoft.AspNetCore.Identity;

namespace Wedding.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IUserManagerRepository UserManagerRepository { get; }
        public ICustomerRepository CustomerRepository { get; set; }
        public IEmailTemplateRepository EmailTemplateRepository { get; }
        public ICartHeaderRepository CartHeaderRepository { get; }
        public ICartDetailsRepository CartDetailsRepository { get; }
        public ICardRepository CardRepository { get; set; }
        public IBalanceRepository BalanceRepository { get; }
        public ITransactionRepository TransactionRepository { get; }
        public IOrderDetailsRepository OrderDetailsRepository { get; }
        public IOrderHeaderRepository OrderHeaderRepository { get; }
        public IOrderStatusRepository OrderStatusRepository { get; }
        public ITermOfUseRepository TermOfUseRepository { get; }
        public ICompanyRepository CompanyRepository { get; }
        public IPrivacyRepository PrivacyRepository { get; }
        public IActivityLogRepository ActivityLogRepository { get; }
        public ICardManagementRepository CardManagementRepository { get; }
        public IInvitationRepository InvitationRepository { get; }
        public IInvitationTemplateRepository InvitationTemplateRepository { get; }
        public IWeddingRepository WeddingRepository { get; }
        public IEventRepository EventRepository { get; }
        public IEventPhotoRepository EventPhotoRepository { get; }
        public IGuestRepository GuestRepository { get; }
        public IGuestListRepository GuestListRepository { get; }
        public IInvitationHtmlRepository InvitationHtmlRepository { get; }

        public async Task<int> SaveAync()
        {
            return await _context.SaveChangesAsync();
        }

        public UnitOfWork(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            UserManagerRepository = new UserManagerRepository(userManager);
            CustomerRepository = new CustomerRepository(_context);
            EmailTemplateRepository = new EmailTemplateRepository(_context);
            CartHeaderRepository = new CartHeaderRepository(_context);
            CartDetailsRepository = new CartDetailsRepository(_context);
            CardRepository = new CardRepository(_context);
            BalanceRepository = new BalanceRepository(_context);
            TransactionRepository = new TransactionRepository(_context);
            OrderDetailsRepository = new OrderDetailsRepository(_context);
            OrderHeaderRepository = new OrderHeaderRepository(_context);
            OrderStatusRepository = new OrderStatusRepository(_context);
            TermOfUseRepository = new TermOfUseRepository(_context);
            CompanyRepository = new CompanyRepository(_context);
            PrivacyRepository = new PrivacyRepository(_context);
            ActivityLogRepository = new ActivityLogRepository(_context);
            CardManagementRepository = new CardManagementRepository(_context);
            InvitationRepository = new InvitationRepository(_context);
            InvitationTemplateRepository = new InvitationTemplateRepository(_context);
            WeddingRepository = new WeddingRepository(_context);
            EventRepository = new EventRepository(_context);
            EventPhotoRepository = new EventPhotoRepository(_context);
            GuestRepository = new GuestRepository(_context);
            GuestListRepository = new GuestListRepository(_context);
            InvitationHtmlRepository = new InvitationHtmlRepository(_context);
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
