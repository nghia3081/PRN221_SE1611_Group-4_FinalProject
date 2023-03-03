using SE1611_Group_4_Final_Project.Models;

namespace SE1611_Group_4_Final_Project.Repository.Interfaces
{
    public interface INotificationRepository
    {
        Task Add(Notification notification);
        Task Update(Notification notification);
        Task Delete(Notification notification);
    }
}
