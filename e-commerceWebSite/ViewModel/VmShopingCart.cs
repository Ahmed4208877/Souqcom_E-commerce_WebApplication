using e_commerceWebSite.Models;

namespace e_commerceWebSite.ViewModel
{
    public class VmShopingCart
    {
        public VmShopingCart()
        {
            ListShopingCartProducts = new List<VmProduct>();
            CartId = "";
            TotalPrice = 0;

        }
        public List<VmProduct> ListShopingCartProducts { get; set; }
        public double TotalPrice { get; set; }
        public string CartId { get; set; }
    }
}
