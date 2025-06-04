using Wedding.Model.Domain;

namespace Wedding.DataAccess.IRepository;

public interface IOrderDetailsRepository : IRepository<OrderDetails>
{
    void Update(OrderDetails orderDetails);
    void UpdateRange(IEnumerable<OrderDetails> ordersDetails);
}