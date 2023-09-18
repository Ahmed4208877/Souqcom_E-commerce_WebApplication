using e_commerceWebSite.Models;

namespace e_commerceWebSite.Repository
{
    public interface ICartService
    {
        string CreateCart();
        // string SaveCart(); when use session to save the cart in one time and retuen cartstutesId to finish cart
        void ChangeCartStutes(string CartStutesId);
        List<TbCart> GetAll(string UserId);
        List<TbCart> GetAll();
        void SaveInCart(string _CartId, int _ProductId, int ProductQuantity);
        TbCart GetById(string Id);
        void UpdateCartPrice(string CartId, double TotalPrice);
        TbCartProduct GetProductInCartById(string _CartId, int _ProductId);
        void DeleteProductFromCart(string _CartId, int _ProductId);
    }
}
