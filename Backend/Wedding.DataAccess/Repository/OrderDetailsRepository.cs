using Wedding.DataAccess.Context;
using Wedding.DataAccess.IRepository;
using Wedding.Model.Domain;

namespace Wedding.DataAccess.Repository;

public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
{
    private readonly ApplicationDbContext _context;

    public OrderDetailsRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(OrderDetails orderDetails)
    {
        _context.OrderDetails.Update(orderDetails);
    }

    public void UpdateRange(IEnumerable<OrderDetails> ordersDetails)
    {
        _context.OrderDetails.UpdateRange(ordersDetails);
    }
}