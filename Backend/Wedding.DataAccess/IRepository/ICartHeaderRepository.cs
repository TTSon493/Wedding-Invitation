using Wedding.Model.Domain;

namespace Wedding.DataAccess.IRepository;

public interface ICartHeaderRepository : IRepository<CartHeader>
{
    void Update(CartHeader cartHeader);
    void UpdateRange(IEnumerable<CartHeader> cartHeaders);
}