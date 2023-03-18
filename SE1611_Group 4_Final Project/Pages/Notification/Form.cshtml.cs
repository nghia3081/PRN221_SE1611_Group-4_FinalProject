using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Models;

namespace SE1611_Group_4_Final_Project.Pages.Notification
{
    public class NotificationFormModel : PageModel
    {
        private readonly IRepository<Models.Notification> _repository;
        [BindProperty]
        public Models.Notification Notification { get; set; }
        public MultiSelectList RoomSelect { get; set; }
        [BindProperty]
        public List<Guid> RoomSelected { get; set; }
        public NotificationFormModel(IRepository<Models.Notification> repository)
        {
            _repository = repository;
            Notification = new();
        }
        public void OnGet(Guid? notificationId)
        {
            RoomSelect = new MultiSelectList(_repository.GetDbSet<Room>().OrderBy(r => r.Name).AsEnumerable(), "Id", "Name");
            if (notificationId.HasValue)
            {
                Notification = _repository.Find(notificationId);
            }
        }
        public void OnPost()
        {
            Notification.Id = Guid.NewGuid();
            Notification.CreatedDate = DateTime.Now;
            Notification.Rooms = _repository.GetDbSet<Room>().Where(room => RoomSelected.Contains(room.Id)).ToList();
            _repository.Add(Notification);
        }
        public void OnPut()
        {
            _repository.Update(Notification);
        }
    }
}
