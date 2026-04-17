using eShop.DTOs.Suppliers;

namespace eShop.Interfaces;

public interface ISupplierRepository
{
    public Task<List<GetSuppliersDto>> ListAllSuppliers();
    public Task<GetSupplierDto> FindSupplier(int Id);
    public Task<int> AddSupplier(PostSupplierDto supplier);
}
