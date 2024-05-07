using APBD.Model;
using Microsoft.Data.SqlClient;

namespace APBD.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly IConfiguration _configuration;

    public OrderRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Order HasValidPurchaseOrder(int productId, int amount, DateTime createdAt)
    {
        string connectionString = _configuration.GetConnectionString("ConnectionStrings:DefaultConnection");

        using SqlConnection connection = new SqlConnection(connectionString);
        
        string query = @"SELECT COUNT(*) 
                         FROM [Order] 
                         WHERE IdProduct = @ProductId 
                         AND Amount = @Amount 
                         AND CreatedAt < @CreatedAt";

        using SqlCommand command = new SqlCommand(query, connection);
        
        command.Parameters.AddWithValue("@ProductId", productId);
        command.Parameters.AddWithValue("@Amount", amount);
        command.Parameters.AddWithValue("@CreatedAt", createdAt);

        connection.Open();
        var dr = command.ExecuteReader();
        if (!dr.Read())
        {
            return null;
        }

        Order order = new Order();
        order.IdOrder = (int)dr["IdOrder"];
        order.IdProduct = (int)dr["IdProduct"];
        order.Amount = (int)dr["Amount"];
        order.CreatedAt = Convert.ToDateTime(dr["CreatedAt"]);
        order.FulfilledAt = Convert.ToDateTime(dr["FulfilledAt"]);

        return order;
    }

    public bool IsOrderCompleted(int orderId)
    {
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("ConnectionStrings:DefaultConnection"));

        string query = "SELECT COUNT(*) FROM Product_Warehouse WHERE IdOrder = @OrderId";

        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@OrderId", orderId);

        connection.Open();
        int count = (int)command.ExecuteScalar();
        connection.Close();

        return count == 0; // If count is 0, order is not completed
    }
    
    public void UpdateOrderFulfilledDate(int orderId)
    {
        string connectionString = _configuration.GetConnectionString(_configuration.GetConnectionString("ConnectionStrings:DefaultConnection"));

        using SqlConnection connection = new SqlConnection(connectionString);
        
        string query = @"UPDATE [Order] 
                         SET FulfilledAt = @CurrentDateTime 
                         WHERE IdOrder = @OrderId";

        using SqlCommand command = new SqlCommand(query, connection);
        
        command.Parameters.AddWithValue("@OrderId", orderId);
        command.Parameters.AddWithValue("@CurrentDateTime", DateTime.Now);

        connection.Open();
        command.ExecuteNonQuery();
    }
}