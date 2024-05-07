namespace APBD.Repository;

public interface IWarehouseRepository
{
    public bool WarehouseExists(int warehouseId);

    public int InsertIntoProductWarehouse(int productId, int warehouseId, int amount, decimal productPrice);

    public int ExecuteAddProductToWarehouseStoredProcedure(int idProduct, int idWarehouse, int amount, decimal price,
        DateTime createdAt);
}