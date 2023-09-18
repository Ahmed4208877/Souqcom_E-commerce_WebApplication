using Castle.Core.Internal;
using e_commerceWebSite.Helper;
using e_commerceWebSite.Models;
using e_commerceWebSite.Repository;
using e_commerceWebSite.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Linq;
using System.Security.Claims;

namespace e_commerceWebSite.Controllers
{
    [Authorize(Roles = "User")]
    public class CartController : Controller
    {

        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly IToastNotification _toastNotification;

        public CartController(IProductService productService, ICartService cartService,IToastNotification toastNotification)
        {
            _productService = productService;
            _cartService = cartService;
            _toastNotification = toastNotification;
        }
       
        public IActionResult AddToCart(int Id)
        {
            TbProduct product = _productService.GetById(Id);
            string CartId = _cartService.CreateCart();
            TbCart cart = _cartService.GetById(CartId);
            TbCartProduct CurrentProduct = _cartService.GetProductInCartById(CartId, product.Id);
            if (CurrentProduct != null)
            {
                CurrentProduct.Product_Quantity++;
                bool IsInStock = _productService.ChangeStockQuantity(Id, 1);
                if (IsInStock)
                {
                    List<VmProduct> products = _productService.GetProductsPerCart(CartId);
                    cart.Total_Price = products.Sum(t => t.Total);
                    _cartService.UpdateCartPrice(CartId, cart.Total_Price);
                }
                else
                {
                    _toastNotification.AddErrorToastMessage("Stock is Empty");
                    return RedirectToAction("Index", "Home");
                }                
            }
            else
            {
                int ProductQuantity = 1;
                _productService.ChangeStockQuantity(Id, ProductQuantity);
                cart.Total_Price = cart.Total_Price + (product.Price * ProductQuantity);
                _cartService.UpdateCartPrice(CartId, cart.Total_Price);
                _cartService.SaveInCart(CartId, product.Id, ProductQuantity);
            }            
            _toastNotification.AddSuccessToastMessage("Added To Cart Successfully");
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Index()
        {
            VmShopingCart shopingCart = new VmShopingCart();
            List<VmProduct> products = _productService.GetProductsPerCart(_cartService.CreateCart());
            shopingCart.ListShopingCartProducts.AddRange(products);
            shopingCart.TotalPrice = shopingCart.ListShopingCartProducts.Sum(t => t.Total);
            if (shopingCart == null)
            {
                shopingCart = new VmShopingCart();
            }
            return View(shopingCart);
        }
      
        public IActionResult DeleteFromCart(int id)
        {
            string CartId = _cartService.CreateCart();
            _cartService.DeleteProductFromCart(CartId, id);
            List<VmProduct> products = _productService.GetProductsPerCart(_cartService.CreateCart());
            double Total = products.Sum(t => t.Total);
            _cartService.UpdateCartPrice(CartId, Total);
            return RedirectToAction("Index");
        }
       
        public  IActionResult ChangeTotalAndQuantity(int ProductId, int Quantity)
        {
            string CartId = _cartService.CreateCart();
            TbCartProduct ProductInCart = _cartService.GetProductInCartById(CartId, ProductId);
            int QuntityResult = Quantity - ProductInCart.Product_Quantity;
            if (QuntityResult >0)
            {
                bool IsInStock = _productService.ChangeStockQuantity(ProductId, QuntityResult);
                if (IsInStock)
                {
                    _cartService.SaveInCart(CartId, ProductId, Quantity);
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            QuntityResult = ProductInCart.Product_Quantity - Quantity;
            _productService.IncreaseStockQuantity(ProductId, QuntityResult);
            _cartService.SaveInCart(CartId, ProductId, Quantity);
            return Json(true);

        }
        
        public IActionResult FinishCartAndSave()
        {
            string CartId = _cartService.CreateCart();
            TbCart cart = _cartService.GetById(CartId);
            List<VmProduct> products = _productService.GetProductsPerCart(CartId);
            if (products.Count()!=0)
            {
                double Total = products.Sum(t => t.Total);
                _cartService.UpdateCartPrice(CartId, Total);
                _cartService.ChangeCartStutes(cart.CartStatusId);
                _toastNotification.AddSuccessToastMessage("Oreder Sent Successfully");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                _toastNotification.AddErrorToastMessage("Cart Is Empty");
                return RedirectToAction("Index", "Cart");
            }            
        }
        public IActionResult HistoryOfShopping()
        {
            var userId =HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            List<TbCart> model = _cartService.GetAll(userId);
            if (model ==null)
            {
                model = new List<TbCart>();
            }
            return View(model);
        }
        public IActionResult CartDetails(string CartId)
        {
            List<VmProduct> model = _productService.GetProductsPerCart(CartId);
            return View(model);
        }
      
    }
}
