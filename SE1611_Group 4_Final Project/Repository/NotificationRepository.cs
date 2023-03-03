using SE1611_Group_4_Final_Project.Models;
using SE1611_Group_4_Final_Project.Repository.Interfaces;

namespace SE1611_Group_4_Final_Project.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly MotelManagementContext context = new MotelManagementContext();

        public async Task Add(Notification notification)
        {
            context.Notifications.Add(notification);
            context.SaveChanges();
        }

        public async Task Update(Notification notification)
        {
            context.Notifications.Find(notification.Id.GetType());
            context.Notifications.Update(notification);
            context.SaveChanges();
        }

        public async Task Delete(Notification notification)
        {
            context.Notifications.Find(notification.Id.GetType());
            context.Notifications.Remove(notification);
            context.SaveChanges();
        }
    }
}
