using System.Data;
using Microsoft.Data.SqlClient;

namespace APBD.Repository;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly IConfiguration _configuration;

    public WarehouseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public bool WarehouseExists(int warehouseId)
    {
        string connectionString = _configuration.GetConnectionString("ConnectionStrings:DefaultConnection");

        SqlConnection connection = new SqlConnection(connectionString);
        
        string query = "SELECT COUNT(*) FROM Warehouse WHERE Id = @WarehouseId";

        using SqlCommand command = new SqlCommand(query, connection);
        
        command.Parameters.AddWithValue("@WarehouseId", warehouseId);

        connection.Open();
        int count = (int) command.ExecuteScalar();
        connection.Close();

        return count > 0;
    }
    
    public int InsertIntoProductWarehouse(int productId, int warehouseId, int amount, decimal productPrice)
    {
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        
        string query = @"INSERT INTO Product_Warehouse (IdProduct, IdWarehouse, Amount, Price, CreatedAt) 
                         VALUES (@ProductId, @WarehouseId, @Amount, @Price, @CreatedAt)";

        using SqlCommand command = new SqlCommand(query, connection);
        
        command.Parameters.AddWithValue("@ProductId", productId);
        command.Parameters.AddWithValue("@WarehouseId", warehouseId);
        command.Parameters.AddWithValue("@Amount", amount);
        command.Parameters.AddWithValue("@Price", amount * productPrice); // Calculate the price
        command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

        connection.Open();
        var generatedId = (int)command.ExecuteScalar();

        return generatedId;
    }
    
    public int ExecuteAddProductToWarehouseStoredProcedure(int idProduct, int idWarehouse, int amount, decimal price, DateTime createdAt)
    {
        int generatedId = 0;

        string connectionString = _configuration.GetConnectionString("DefaultConnection");
    
        using SqlConnection connection = new SqlConnection(connectionString);
        using SqlCommand command = new SqlCommand("AddProductToWarehouse", connection);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@IdProduct", idProduct);
        command.Parameters.AddWithValue("@IdWarehouse", idWarehouse);
        command.Parameters.AddWithValue("@Amount", amount);
        command.Parameters.AddWithValue("@CreatedAt", createdAt);

        connection.Open();
        SqlDataReader reader = command.ExecuteReader();
        if (reader.Read())
        {
            generatedId = Convert.ToInt32(reader["NewId"]);
        }
        connection.Close();

        return generatedId;
    }
}