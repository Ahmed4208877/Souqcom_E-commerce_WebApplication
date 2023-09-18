using e_commerceWebSite.Models;

namespace e_commerceWebSite.Repository
{
    public interface INotificationService
    {
        List<TbNotification> GetAll();
        void Insert(TbNotification notification);
        void Delete(int Id);
    }
}
