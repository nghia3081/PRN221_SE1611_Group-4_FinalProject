using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SE1611_Group_4_Final_Project.IRepository;

namespace SE1611_Group_4_Final_Project.Pages
{
    public class NotificationIndexModel : PageModel
    {
        private readonly IRepository<Models.Notification> _repository;
        public List<Models.Notification> Notifications { get; set; }
        public NotificationIndexModel(IRepository<Models.Notification> repository)
        {
            _repository = repository;
        }

        public void OnGet()
        {
            Notifications = _repository.GetDbSet().Include(notif => notif.Rooms) .ToList();
        }
        public void OnDelete(Guid notificationId)
        {
            _repository.Delete(notificationId);
        }
    }
}
