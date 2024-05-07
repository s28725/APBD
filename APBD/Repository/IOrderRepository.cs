using APBD.Model;

namespace APBD.Repository;

public interface IOrderRepository
{
    public Order HasValidPurchaseOrder(int productId, int amount, DateTime createdAt);

    public bool IsOrderCompleted(int orderId);

    public void UpdateOrderFulfilledDate(int orderId);
}