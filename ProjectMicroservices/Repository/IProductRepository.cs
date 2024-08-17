using ProjectMicroservices.Model;

namespace ProjectMicroservices.Repository;

public interface IProductRepository
{
    IEnumerable<Product> getProducts();
    Product GetProductByID(int productId);
    void InsertProduct(Product product);
    void DeleteProduct(int productId);
    void UpdateProduct(Product product);
    void Save();
}