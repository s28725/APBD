using APBD.Model;
using Microsoft.Data.SqlClient;

namespace APBD.Repository;

public class ProductRepository : IProductRepository
{
    private readonly IConfiguration _configuration;

    public ProductRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Product ProductExists(int productId)
    {
        string connectionString = _configuration.GetConnectionString("ConnectionStrings:DefaultConnection");

        using SqlConnection connection = new SqlConnection(connectionString);
        
        string query = "SELECT COUNT(*) FROM Product WHERE Id = @ProductId";

        using SqlCommand command = new SqlCommand(query, connection);
            
        command.Parameters.AddWithValue("@ProductId", productId);

        connection.Open();
        var dr = command.ExecuteReader();
        if (!dr.Read())
        {
            return null;
        }
        
        Product product = new Product();
        product.IdProduct = (int)dr["IdProduct"];
        product.Description = dr["Description"].ToString();
        product.Name = dr["Name"].ToString();
        product.Price = (decimal) dr["Price"];
        
        return product;
    }
}