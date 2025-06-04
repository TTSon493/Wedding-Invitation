using Wedding.DataAccess.Context;
using Wedding.DataAccess.IRepository;
using Wedding.Model.Domain;

namespace Wedding.DataAccess.Repository;

public class OrderStatusRepository : Repository<OrderStatus>, IOrderStatusRepository
{
    private readonly ApplicationDbContext _context;

    public OrderStatusRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(OrderStatus orderStatus)
    {
        _context.OrdersStatus.Update(orderStatus);
    }

    public void UpdateRange(IEnumerable<OrderStatus> ordersStatus)
    {
        _context.OrdersStatus.UpdateRange(ordersStatus);
    }
}