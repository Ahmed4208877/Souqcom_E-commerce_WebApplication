namespace e_commerceWebSite.Helper
{
    public class DraftUseingSession
    {
        // CartService AddToCart ==>
        //public IActionResult AddToCart(int Id)
        //{
        //    TbProduct product = _productService.GetById(Id);
        //    VmShopingCart shopingCart = HttpContext.Session.GetObjectFromJson<VmShopingCart>("Cart");
        //    if (shopingCart == null)
        //    {
        //        shopingCart = new VmShopingCart();
        //    }
        //    VmProduct CurrentProduct = shopingCart.ListShopingCartProducts.Where(p => p.Id == Id).FirstOrDefault();
        //    if (CurrentProduct != null)
        //    {
        //        CurrentProduct.Quantity++;
        //        CurrentProduct.Total = CurrentProduct.Price * CurrentProduct.Quantity;
        //    }
        //    else
        //    {
        //        shopingCart.ListShopingCartProducts.Add(new VmProduct()
        //        {
        //            Id = product.Id,
        //            Name = product.Name,
        //            CategoryName = product.Category.Name,
        //            Description = product.Description,
        //            ImagePath = product.ImagePath,
        //            Price = product.Price,
        //            SellerName = product.Seller.UserName,
        //            SellerId = product.SellerId,
        //            Quantity = 1,
        //            Total = product.Price
        //        });
        //    }
        //    shopingCart.TotalPrice = shopingCart.ListShopingCartProducts.Sum(a => a.Total);
        //    shopingCart.CartId = _cartService.CreateCart();
        //    HttpContext.Session.SetObjectAsJson("Cart", shopingCart);
        //    _toastNotification.AddSuccessToastMessage("Added To Cart Successfully");
        //    return RedirectToAction("Index", "Home");
        //}

        // CartService DeleteFromCart ==>
        //public IActionResult DeleteFromCart(int id)
        //{
        //    VmShopingCart shopingCart = HttpContext.Session.GetObjectFromJson<VmShopingCart>("Cart");
        //    shopingCart.ListShopingCartProducts.Remove(shopingCart.ListShopingCartProducts.FirstOrDefault(i => i.Id == id));
        //    shopingCart.TotalPrice = shopingCart.ListShopingCartProducts.Sum(t => t.Total);
        //    HttpContext.Session.SetObjectAsJson("Cart", shopingCart);
        //    return RedirectToAction("Index");
        //}

        // CartService ChangeTotalAndQuantity ==>
        //public IActionResult ChangeTotalAndQuantity(int ProductId, int Quantity, double Total)
        //{
        //    VmShopingCart shopingCart = HttpContext.Session.GetObjectFromJson<VmShopingCart>("Cart");
        //    shopingCart.ListShopingCartProducts.FirstOrDefault(i => i.Id == ProductId).Quantity = Quantity;
        //    shopingCart.ListShopingCartProducts.FirstOrDefault(i => i.Id == ProductId).Total = (shopingCart.ListShopingCartProducts.FirstOrDefault(i => i.Id == ProductId).Quantity * shopingCart.ListShopingCartProducts.FirstOrDefault(i => i.Id == ProductId).Price);
        //    shopingCart.TotalPrice = shopingCart.ListShopingCartProducts.Sum(t => t.Total);
        //    HttpContext.Session.SetObjectAsJson("Cart", shopingCart);
        //    return Json(true);
        //}

        // CartService FinishCartAndSave ==>
        //public IActionResult FinishCartAndSave()
        //{
        //    string CartStutesId = _cartService.SaveCart();
        //    if (!string.IsNullOrEmpty(CartStutesId))
        //    {
        //        _cartService.ChangeCartStutes(CartStutesId);
        //        _toastNotification.AddSuccessToastMessage("Oreder Sent Successfully");
        //        return RedirectToAction("Index", "Home");
        //    }
        //    _toastNotification.AddErrorToastMessage("Cart Is Empty");
        //    return RedirectToAction("Index", "Cart");
        //}



    }
}
