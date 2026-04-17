namespace eShop.Interfaces;

public interface IUnitOfWork
{
    IProductRepository ProductRepository{ get; }
    ISupplierRepository SupplierRepository{ get; }
    Task<bool> Complete();
}
