using Wedding.Model.Domain;

namespace Wedding.DataAccess.IRepository;

public interface ICardManagementRepository : IRepository<CardManagement>
{
    void Update(CardManagement cardManagement);
    void UpdateRange(IEnumerable<CardManagement> cardManagements);
    Task<CardManagement?> GetById(Guid id);
    Task<CardManagement> AddAsync(CardManagement cardManagement);
}