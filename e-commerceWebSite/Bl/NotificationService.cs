using e_commerceWebSite.Models;
using e_commerceWebSite.Repository;

namespace e_commerceWebSite.Bl
{
    public class NotificationService : INotificationService
    {
        private readonly e_commerceStoreContext _context;

        public NotificationService(e_commerceStoreContext context)
        {
            _context = context;
        }
        public List<TbNotification> GetAll()
        {
            return _context.TbNotifications.ToList();
        }
        public void Insert(TbNotification notification)
        {
            _context.TbNotifications.Add(notification);
            _context.SaveChanges();
        }
        public void Delete(int Id)
        {
            TbNotification notification = _context.TbNotifications.FirstOrDefault(i => i.Id == Id);
            _context.Remove(notification);
            _context.SaveChanges();
        }
    }
}
