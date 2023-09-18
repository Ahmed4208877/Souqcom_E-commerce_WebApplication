using e_commerceWebSite.Models;
using e_commerceWebSite.Repository;

namespace e_commerceWebSite.Bl
{
    public class CategoryService : ICategoryService
    {
        private readonly e_commerceStoreContext _context;

        public CategoryService(e_commerceStoreContext context)
        {
            _context = context;
        }
        public void Delete(int Id)
        {
            TbCategory category = GetById(Id);
            category.IsDeleted = true;
            category.Id = Id;
            _context.Entry(category).Property(p => p.Name).IsModified = false;
            _context.Entry(category).Property(p => p.InsertionDate).IsModified = false;
            _context.Entry(category).Property(p => p.ModifiedDate).IsModified = false;
            _context.Update(category);
            _context.SaveChanges();
            ChangeProductsCategory(Id);

        }
        public void ChangeProductsCategory(int DeletedCategory)
        {
            TbCategory OthersproCategory = _context.TbCategories.FirstOrDefault(n => n.Name == "Others");
            if (OthersproCategory == null)
            {
                OthersproCategory = new TbCategory()
                {
                    Name = "Others",
                    IsDeleted = false
                };
                Insert(OthersproCategory);
            }
            List<TbProduct> products = _context.TbProducts.Where(c => c.CategoryId == DeletedCategory).ToList();
            foreach (var product in products)
            {
                product.Id = product.Id;
                product.CategoryId = OthersproCategory.Id;
                _context.Entry(product).Property(p => p.Name).IsModified = false;
                _context.Entry(product).Property(p => p.StockQuantity).IsModified = false;
                _context.Entry(product).Property(p => p.SellerId).IsModified = false;
                _context.Entry(product).Property(p => p.Price).IsModified = false;
                _context.Entry(product).Property(p => p.IsDeleted).IsModified = false;
                _context.Entry(product).Property(p => p.IsActive).IsModified = false;
                _context.Entry(product).Property(p => p.Description).IsModified = false;
                _context.Entry(product).Property(p => p.ImagePath).IsModified = false;
                _context.Entry(product).Property(p => p.ExipirationDate).IsModified = false;
                _context.TbProducts.Update(product);
                _context.SaveChanges();
            }
        }
        public List<TbCategory> GetAll()
        {
            return _context.TbCategories.ToList();
        }


        public TbCategory GetById(int Id)
        {
            return _context.TbCategories.FirstOrDefault(i => i.Id == Id);
        }

        public void Insert(TbCategory category)
        {
            category.InsertionDate = DateTime.Now;
            category.ModifiedDate = DateTime.Now;
            _context.Add(category);
            _context.SaveChanges();
        }

        public void Update(TbCategory category)
        {
            category.ModifiedDate = DateTime.Now;
            _context.Entry(category).Property(p => p.InsertionDate).IsModified = false;
            _context.Entry(category).Property(p => p.IsDeleted).IsModified = false;
            _context.Update(category);
            _context.SaveChanges();
        
        }
    }
}
