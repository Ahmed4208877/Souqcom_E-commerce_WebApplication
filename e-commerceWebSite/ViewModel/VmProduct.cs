using Microsoft.AspNetCore.Mvc;

namespace e_commerceWebSite.ViewModel
{
    public class VmProduct
    {
        public int Id { get; set; }
        public string SellerId { get; set; }
        public int CategoryId { get; set; }
        public string? ImagePath { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public string SellerName { get; set; }
        public int Quantity { get; set; }
        public double Total { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }



    }
}
