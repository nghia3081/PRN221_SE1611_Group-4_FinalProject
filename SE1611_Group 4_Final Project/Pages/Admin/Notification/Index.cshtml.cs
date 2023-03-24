using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Utils;

namespace SE1611_Group_4_Final_Project.Pages
{
    public class NotificationIndexModel : PageModel
    {
        private readonly IRepository<Models.Notification> _repository;
        public int TotalPage { get; private set; }
        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; }
        public bool HasPreviousPage => (PageIndex > 1);
        public bool HasNextPage => (PageIndex < TotalPage);

        private const int PageSize = 18;
        [BindProperty(SupportsGet = true)]
        public string query { get; set; }
        public IEnumerable<Models.Notification> Notifications { get; set; }
        public NotificationIndexModel(IRepository<Models.Notification> repository)
        {
            _repository = repository;
        }

        public IActionResult OnGet()
        {
            if (!Constant.IsAdmin(HttpContext)) return RedirectToPage("/Index");
            TotalPage = Constant.GetTotalPage(_repository.GetDbSet().Count(), PageSize);
            Notifications = _repository.GetDbSet().Include(notif => notif.Rooms);
            if (!query.IsNullOrEmpty()) Notifications = Notifications.Where(notif => notif.Title.Contains(query) || notif.Description.Contains(query));
            Notifications = Notifications.Skip(Constant.GetStartIndexPage(PageIndex, PageSize)).Take(PageSize);
            return Page();
        }
        public IActionResult OnGetDelete(Guid id)
        {
            if (!Constant.IsAdmin(HttpContext)) return RedirectToPage("/Index");
            _repository.Delete(id).Wait();
            OnGet();
            return Page();
        }
    }
}
