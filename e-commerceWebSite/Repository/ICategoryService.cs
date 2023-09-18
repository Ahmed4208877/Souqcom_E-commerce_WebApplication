using e_commerceWebSite.Models;

namespace e_commerceWebSite.Repository
{
    public interface ICategoryService
    {
        List<TbCategory> GetAll();
        TbCategory GetById(int Id);
        void Insert(TbCategory product);
        void Update(TbCategory product);
        void Delete(int Id);
    }
}
