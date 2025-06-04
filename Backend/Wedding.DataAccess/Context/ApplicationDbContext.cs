using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Wedding.Model.Domain;
using Wedding.DataAccess.Seeding;


namespace Wedding.DataAccess.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ApplicationDbContextSeed.SeedAdminAccount(modelBuilder);
            ApplicationDbContextSeed.SeedEmailTemplate(modelBuilder);
            ApplicationDbContextSeed.SeedCompany(modelBuilder);
            ApplicationDbContextSeed.SeedPrivacy(modelBuilder);
            ApplicationDbContextSeed.SeedTermOfUse(modelBuilder);
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<CartDetails> CartDetails { get; set; }
        public DbSet<CartHeader> CartHeaders { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Balance> Balances { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderStatus> OrdersStatus { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Privacy> Privacies { get; set; }
        public DbSet<TermOfUse> TermOfUses { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<CardManagement> CardManagements { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<InvitationTemplate> InvitationTemplates { get; set; }
        public DbSet<Model.Domain.Wedding> Weddings { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventPhoto> EventPhotos { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<GuestList> GuestLists { get; set; }
        public DbSet<InvitationHtml> InvitationHtmls { get; set; }
    }
}
