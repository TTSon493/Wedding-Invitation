using Wedding.Model.Domain;

namespace Wedding.DataAccess.IRepository;

public interface ICartDetailsRepository : IRepository<CartDetails>
{
    void Update(CartDetails cartDetails);
    void UpdateRange(IEnumerable<CartDetails> cartsDetails);
}