using Wedding.DataAccess.Context;
using Wedding.DataAccess.IRepository;
using Wedding.Model.Domain;

namespace Wedding.DataAccess.Repository;

public class EmailTemplateRepository : Repository<EmailTemplate>, IEmailTemplateRepository
{
    private readonly ApplicationDbContext _context;

    public EmailTemplateRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(EmailTemplate emailTemplate)
    {
        _context.EmailTemplates.Update(emailTemplate);
    }
}