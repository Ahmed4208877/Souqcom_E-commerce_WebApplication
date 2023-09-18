using Castle.Core.Internal;
using e_commerceWebSite.Helper;
using e_commerceWebSite.Models;
using e_commerceWebSite.Repository;
using e_commerceWebSite.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace e_commerceWebSite.Bl
{
    public class CartService : ICartService
    {
        private readonly e_commerceStoreContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor; // this to get HttpContext to get userId And Sesion
     

        public CartService(e_commerceStoreContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;            
        }
        public string CreateCart()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            IEnumerable<string> StatusIds = _context.TbCartStatuses.Where(s => s.Status == "ISERTION").Select(i => i.Id).ToList();
            TbCart? CurrentCart = _context.TbCarts.FirstOrDefault(u => u.UserId == userId && StatusIds.Contains(u.CartStatusId));
            if (CurrentCart == null)
            {
                TbCartStatus NewcartStatus = new TbCartStatus() { Id = Guid.NewGuid().ToString(), Status = "ISERTION" };
                _context.TbCartStatuses.Add(NewcartStatus);
                _context.SaveChanges();
                CurrentCart = new TbCart()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    CartStatusId = NewcartStatus.Id,
                    Total_Price = 0
                };
                _context.TbCarts.Add(CurrentCart);
                _context.SaveChanges();
                return CurrentCart.Id;
            }
            else
            {
                return CurrentCart.Id;
            }
        }
        public void ChangeCartStutes(string CartStutesId)
        {
            TbCartStatus cartStatus = _context.TbCartStatuses.FirstOrDefault(i => i.Id == CartStutesId);
            cartStatus.Id = CartStutesId;
            cartStatus.Status = "FINISHED";
            _context.TbCartStatuses.Update(cartStatus);
            _context.SaveChanges();
        }
        public List<TbCart> GetAll(string UserId)
        {
            IEnumerable<string> StatusIds = _context.TbCartStatuses.Where(s => s.Status == "FINISHED").Select(i => i.Id).ToList();
            return _context.TbCarts.Where(u => u.UserId == UserId && StatusIds.Contains(u.CartStatusId)).ToList();
        }
        public List<TbCart> GetAll()
        {
            return _context.TbCarts.ToList();

        }
        public TbCart GetById(string Id)
        {
            return _context.TbCarts.FirstOrDefault(i => i.Id == Id);
        }
        public void UpdateCartPrice(string CartId, double TotalPrice)
        {
            TbCart cart = GetById(CartId);
            cart.Total_Price = TotalPrice;
            cart.Id = CartId;
            _context.Entry(cart).Property(p => p.UserId).IsModified = false;
            _context.Entry(cart).Property(p => p.InsertionData).IsModified = false;
            _context.Entry(cart).Property(p => p.CartStatusId).IsModified = false;
            _context.TbCarts.Update(cart);
            _context.SaveChanges();
        }
        public TbCartProduct GetProductInCartById(string _CartId, int _ProductId)
        {
            return _context.TbCartProducts.FirstOrDefault(i => i.CartId == _CartId && i.ProductId == _ProductId);
        }
        public void SaveInCart(string _CartId, int _ProductId, int ProductQuantity)
        {
            TbCartProduct CartProduct = GetProductInCartById(_CartId, _ProductId);
            if (CartProduct != null)
            {
                CartProduct.Product_Quantity = ProductQuantity;
                CartProduct.CartId = _CartId;
                CartProduct.ProductId = _ProductId;
                _context.Entry(CartProduct).Property(p => p.InsertionData).IsModified = false;
                _context.TbCartProducts.Update(CartProduct);
                _context.SaveChanges();
            }
            else
            {
                CartProduct = new TbCartProduct()
                {
                    CartId = _CartId,
                    ProductId = _ProductId,
                    Product_Quantity = ProductQuantity,
                    InsertionData = DateTime.Now
                };
                _context.TbCartProducts.Add(CartProduct);
                _context.SaveChanges();
            }

        }
        public void DeleteProductFromCart(string _CartId, int _ProductId)
        {
            TbCartProduct ProductInCart = _context.TbCartProducts.FirstOrDefault(i => i.CartId == _CartId && i.ProductId == _ProductId);
            TbProduct RealProduct = _context.TbProducts.IgnoreQueryFilters().FirstOrDefault(i => i.Id == _ProductId);
            RealProduct.StockQuantity = RealProduct.StockQuantity + ProductInCart.Product_Quantity;
            RealProduct.Id = _ProductId;
            if (!RealProduct.IsActive)
            {
                RealProduct.IsActive = true;
            }
            else
            {
                _context.Entry(RealProduct).Property(p => p.IsActive).IsModified = false;

            }
            _context.Entry(RealProduct).Property(p => p.Name).IsModified = false;
            _context.Entry(RealProduct).Property(p => p.IsDeleted).IsModified = false;
            _context.Entry(RealProduct).Property(p => p.SellerId).IsModified = false;
            _context.Entry(RealProduct).Property(p => p.Price).IsModified = false;
            _context.Entry(RealProduct).Property(p => p.CategoryId).IsModified = false;
            _context.Entry(RealProduct).Property(p => p.Description).IsModified = false;
            _context.Entry(RealProduct).Property(p => p.ImagePath).IsModified = false;
            _context.Entry(RealProduct).Property(p => p.ExipirationDate).IsModified = false;
            _context.TbProducts.Update(RealProduct);
            _context.TbCartProducts.Remove(ProductInCart);
            _context.SaveChanges();
        }

        // when use session to save the cart in one time and retuen cartstutesId to finish cart
        //public string SaveCart()
        //{
        //    VmShopingCart shopingCart = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<VmShopingCart>("Cart");
        //    if (shopingCart == null)
        //    {
        //        shopingCart = new VmShopingCart();
        //    }
        //    if (!string.IsNullOrEmpty(shopingCart.CartId) && shopingCart.ListShopingCartProducts.Count() != 0)
        //    {
        //        TbCart cart = _context.TbCarts.FirstOrDefault(i => i.Id == shopingCart.CartId);
        //        cart.Id = shopingCart.CartId;
        //        cart.Total_Price = shopingCart.TotalPrice;
        //        cart.InsertionData = DateTime.Now;
        //        _context.Entry(cart).Property(p => p.UserId).IsModified = false;
        //        _context.Entry(cart).Property(p => p.CartStatusId).IsModified = false;
        //        _context.TbCarts.Update(cart);
        //        foreach (var Product in shopingCart.ListShopingCartProducts)
        //        {
        //            _context.TbCartProducts.Add(new TbCartProduct
        //            {
        //                CartId = shopingCart.CartId,
        //                ProductId = Product.Id,
        //                Product_Quantity = Product.Quantity,
        //                InsertionData = DateTime.Now
        //            });
        //        }
        //        _context.SaveChanges();
        //        shopingCart = new VmShopingCart();
        //        _httpContextAccessor.HttpContext.Session.SetObjectAsJson("Cart", shopingCart);
        //        return cart.CartStatusId;
        //    }
        //    return string.Empty;
        //}
    }
}
