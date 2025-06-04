using Wedding.Model.Domain;

namespace Wedding.DataAccess.IRepository;

public interface IOrderStatusRepository : IRepository<OrderStatus>
{
    void Update(OrderStatus orderStatus);
    void UpdateRange(IEnumerable<OrderStatus> ordersStatus);
}