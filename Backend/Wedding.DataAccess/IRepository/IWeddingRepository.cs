using Wedding.Model.Domain;

namespace Wedding.DataAccess.IRepository;

public interface IWeddingRepository : IRepository<Model.Domain.Wedding>
{
    void Update(Model.Domain.Wedding wedding);
    void UpdateRange(IEnumerable<Model.Domain.Wedding> weddings);
    Task<Model.Domain.Wedding?> GetById(Guid id);
    Task<Model.Domain.Wedding> AddAsync(Model.Domain.Wedding wedding);
}