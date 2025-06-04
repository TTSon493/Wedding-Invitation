using Wedding.DataAccess.Context;
using Wedding.DataAccess.IRepository;
using Wedding.Model.Domain;

namespace Wedding.DataAccess.Repository;

public class CartDetailsRepository : Repository<CartDetails>, ICartDetailsRepository
{
    private readonly ApplicationDbContext _context;

    public CartDetailsRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(CartDetails cartDetails)
    {
        _context.CartDetails.Update(cartDetails);
    }

    public void UpdateRange(IEnumerable<CartDetails> cartsDetails)
    {
        _context.CartDetails.UpdateRange(cartsDetails);
    }
}