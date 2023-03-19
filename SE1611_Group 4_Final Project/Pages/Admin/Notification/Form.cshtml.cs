using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using SE1611_Group_4_Final_Project.IRepository;

namespace SE1611_Group_4_Final_Project.Pages.Notification
{
    public class NotificationFormModel : PageModel
    {
        private readonly IRepository<Models.Notification> _repository;
        private readonly DbSet<Models.Room> roomDbSet;
        [BindProperty]
        public Models.Notification Notification { get; set; }
        public MultiSelectList RoomSelect { get; set; }
        [BindProperty]
        public List<Guid> RoomSelected { get; set; }
        public NotificationFormModel(IRepository<Models.Notification> repository)
        {
            _repository = repository;
            roomDbSet = _repository.GetDbSet<Models.Room>();
            Notification = new();
        }
        public void OnGet(Guid? id)
        {

            if (id.HasValue)
            {
                Notification = _repository.GetDbSet().Include(notif => notif.Rooms).FirstOrDefault(notif => notif.Id.Equals(id));
            }
            RoomSelect = new MultiSelectList(roomDbSet.OrderBy(r => r.Name).AsEnumerable(), "Id", "Name", Notification?.Rooms.Select(r => r.Id));
        }
        public IActionResult OnPost()
        {
            Notification.Rooms = _repository.GetDbSet<Models.Room>().Where(room => RoomSelected.Contains(room.Id)).ToList();
            if (Notification.Id == Guid.Empty)
            {
                Notification.Id = Guid.NewGuid();
                Notification.CreatedDate = DateTime.Now;
                _repository.Add(Notification).Wait();
            }
            else
            {
                var foundNotif = _repository.GetDbSet().Include(notif => notif.Rooms).FirstOrDefault(notif => notif.Id == Notification.Id);
                foundNotif.Rooms.Clear();
                foundNotif.Rooms.AddRange(Notification.Rooms);
                _repository.Update(foundNotif);
            }
            return RedirectToPage("/Index");
        }
    }
}
