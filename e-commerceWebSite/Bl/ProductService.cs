using e_commerceWebSite.Models;
using e_commerceWebSite.Repository;
using e_commerceWebSite.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace e_commerceWebSite.Bl
{
    public class ProductService : IProductService
    {
        private readonly e_commerceStoreContext _context;

        public ProductService(e_commerceStoreContext context)
        {
            _context = context;
        }
        public void IncreaseStockQuantity(int _ProductId, int IncreaseQuantity)
        {
            TbProduct product = _context.TbProducts.IgnoreQueryFilters().FirstOrDefault(i => i.Id == _ProductId);
            product.Id = _ProductId;
            product.StockQuantity = product.StockQuantity + IncreaseQuantity;
            _context.Entry(product).Property(p => p.Name).IsModified = false;
            _context.Entry(product).Property(p => p.IsDeleted).IsModified = false;
            _context.Entry(product).Property(p => p.SellerId).IsModified = false;
            _context.Entry(product).Property(p => p.Price).IsModified = false;
            _context.Entry(product).Property(p => p.CategoryId).IsModified = false;
            _context.Entry(product).Property(p => p.Description).IsModified = false;
            _context.Entry(product).Property(p => p.ImagePath).IsModified = false;
            _context.Entry(product).Property(p => p.ExipirationDate).IsModified = false;
            if (!product.IsActive)
            {
                product.IsActive = true;
            }
            else
            {
                _context.Entry(product).Property(p => p.IsActive).IsModified = false;                    
            }
            _context.Update(product);
            _context.SaveChanges();
        }
        public bool ChangeStockQuantity(int _ProductId, int ProductQuantity)
        {
            TbProduct product = _context.TbProducts.FirstOrDefault(i => i.Id == _ProductId);
            int result = product.StockQuantity - ProductQuantity;
            if (result >= 0)
            {
                product.Id = _ProductId;
                product.StockQuantity = result;
                _context.Entry(product).Property(p => p.Name).IsModified = false;
                _context.Entry(product).Property(p => p.IsDeleted).IsModified = false;
                _context.Entry(product).Property(p => p.SellerId).IsModified = false;
                _context.Entry(product).Property(p => p.Price).IsModified = false;
                _context.Entry(product).Property(p => p.CategoryId).IsModified = false;
                _context.Entry(product).Property(p => p.Description).IsModified = false;
                _context.Entry(product).Property(p => p.ImagePath).IsModified = false;
                _context.Entry(product).Property(p => p.ExipirationDate).IsModified = false;
                if (result > 0)
                {
                    _context.Entry(product).Property(p => p.IsActive).IsModified = false;
                    _context.Update(product);
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    product.IsActive = false;
                    _context.Update(product);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public void Delete(int Id)
        {
            TbProduct product = _context.TbProducts.FirstOrDefault(i => i.Id == Id);
            product.Id = Id;
            product.IsDeleted = true;
            _context.Entry(product).Property(p => p.Name).IsModified = false;
            _context.Entry(product).Property(p => p.StockQuantity).IsModified = false;
            _context.Entry(product).Property(p => p.SellerId).IsModified = false;
            _context.Entry(product).Property(p => p.Price).IsModified = false;
            _context.Entry(product).Property(p => p.CategoryId).IsModified = false;
            _context.Entry(product).Property(p => p.IsActive).IsModified = false;
            _context.Entry(product).Property(p => p.Description).IsModified = false;
            _context.Entry(product).Property(p => p.ImagePath).IsModified = false;
            _context.Entry(product).Property(p => p.ExipirationDate).IsModified = false;
            _context.Update(product);

            _context.SaveChanges();
        }

        public List<TbProduct> GetAll()
        {
           List<TbProduct> products = _context.TbProducts.ToList();
            List<TbProduct> ReturnProducts = new List<TbProduct>();
            foreach (var product in products)
            {
                if (product.ExipirationDate != null)
                {
                    if (product.ExipirationDate > DateTime.Now)
                    {
                        ReturnProducts.Add(product);
                    }
                }
                else
                {
                    ReturnProducts.Add(product);
                }

            }
            return ReturnProducts;
        }

        public List<TbProduct> GetAllParCategory(int CategoryId)
        {
            return _context.TbProducts.Where(c => c.CategoryId == CategoryId).ToList();
        }

        public List<TbProduct> GetAllParSeller(string SellerId)
        {
            return _context.TbProducts.IgnoreQueryFilters().Where(a => a.SellerId == SellerId && !a.IsDeleted).ToList();
        }

        public List<VmProduct> GetAllVmProduct()
        {
            var query = from i in _context.TbProducts
                        join ii in _context.TbCategories
                        on i.CategoryId equals ii.Id
                        join s in _context.Users
                        on i.SellerId equals s.Id
                        where i.ExipirationDate == null || i.ExipirationDate > DateTime.Now
                        select new VmProduct
                        {
                            Id = i.Id,
                            Name = i.Name,
                            Price = i.Price,
                            ImagePath = i.ImagePath,
                            CategoryId = ii.Id,
                            SellerId = s.Id,
                            CategoryName = ii.Name,
                            Description = i.Description,
                            SellerName = s.UserName

                        };

            return query.ToList();
        }

        public TbProduct GetById(int Id)
        {
            return _context.TbProducts.IgnoreQueryFilters().FirstOrDefault(a => a.Id == Id);
        }

        public void Insert(TbProduct product)
        {
            _context.TbProducts.Add(product);
            _context.SaveChanges();
        }

        public void Update(TbProduct product)
        {
            TbProduct updateProduct = product;
            _context.Entry(updateProduct).Property(p => p.IsDeleted).IsModified = false;
            _context.Update(updateProduct);
            _context.SaveChanges();
        }
        public List<VmProduct> GetProductsPerCart(string CartId)
        {
            List<VmProduct> Products = new List<VmProduct>();
            List<TbCartProduct> CartProducts = _context.TbCartProducts.Where(ci => ci.CartId == CartId).ToList();
            foreach (var product in CartProducts)
            {
                TbProduct Realproduct = GetById(product.ProductId);
                Products.Add(new VmProduct()
                {
                    Id = product.ProductId,
                    Quantity = product.Product_Quantity,
                    Total = Realproduct.Price * product.Product_Quantity,
                    ImagePath = Realproduct.ImagePath,
                    Price = Realproduct.Price,
                    Name = Realproduct.Name,
                    CategoryName = Realproduct.Category.Name,
                });
            }
            return Products;
        }
    }
}
