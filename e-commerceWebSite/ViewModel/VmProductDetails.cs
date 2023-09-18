using e_commerceWebSite.Models;

namespace e_commerceWebSite.ViewModel
{
    public class VmProductDetails
    {
        public TbProduct Product { get; set; }
        public List<TbProduct> RelatedProducts { get; set; }
    }
}
