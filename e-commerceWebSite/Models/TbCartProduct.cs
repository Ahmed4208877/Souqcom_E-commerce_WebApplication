namespace e_commerceWebSite.Models
{
    public class TbCartProduct
    {
        public string CartId { get; set; }
        public int ProductId { get; set; }
        public int Product_Quantity { get; set; }
        public DateTime InsertionData { get; set; }

        public virtual TbCart? Cart { get; set; }
        public virtual TbProduct? Product { get; set; }
    }
}
