using Wedding.Model.Domain;

namespace Wedding.DataAccess.IRepository;

public interface IEmailTemplateRepository : IRepository<EmailTemplate>
{
    void Update(EmailTemplate emailTemplate);
}
