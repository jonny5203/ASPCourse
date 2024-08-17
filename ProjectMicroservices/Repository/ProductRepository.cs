using ProjectMicroservices.Model;

namespace ProjectMicroservices.Repository;

public class ProductRepository : IProductRepository
{
    private readonly ProductDbContext _context;

    public ProductRepository(ProductDbContext context)
    {
        _context = context;
    }
    
    public IEnumerable<Product> getProducts()
    {
        return _context.Products.ToList();
    }

    public Product GetProductByID(int productId)
    {
        return _context.Products.Find(productId);
    }

    public void InsertProduct(Product product)
    {
        _context.Products.Add(product);
    }

    public void DeleteProduct(int productId)
    {
        _context.Products.Remove(GetProductByID(productId));
    }

    public void UpdateProduct(Product product)
    {
        _context.Products.Update(product);
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}