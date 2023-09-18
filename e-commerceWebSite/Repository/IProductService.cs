using e_commerceWebSite.Models;
using e_commerceWebSite.ViewModel;

namespace e_commerceWebSite.Repository
{
    public interface IProductService
    {
        List<TbProduct> GetAll();
        List<TbProduct> GetAllParSeller(string SellerId);
        List<TbProduct> GetAllParCategory(int CategoryId);
        List<VmProduct> GetProductsPerCart(string CartId);
        List<VmProduct> GetAllVmProduct();
        TbProduct GetById(int Id);
        void Insert(TbProduct product);
        void Update(TbProduct product);
        void Delete(int Id);
        bool ChangeStockQuantity(int _ProductId, int ProductQuantity);
        void IncreaseStockQuantity(int _ProductId, int IncreaseQuantity);
    }
}
