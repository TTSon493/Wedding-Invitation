using Wedding.Model.Domain;

namespace Wedding.DataAccess.IRepository;

public interface IInvitationTemplateRepository : IRepository<InvitationTemplate>
{
    void Update(InvitationTemplate invitationTemplate);
    void UpdateRange(IEnumerable<InvitationTemplate> invitationTemplates);
    Task<InvitationTemplate?> GetById(Guid id);
    Task<InvitationTemplate> AddAsync(InvitationTemplate invitationTemplate);
}