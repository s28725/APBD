using APBD.Model;

namespace APBD.Repository;

public interface IProductRepository
{
    public Product ProductExists(int productId);
}