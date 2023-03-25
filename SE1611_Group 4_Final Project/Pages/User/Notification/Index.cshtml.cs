using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Utils;

namespace SE1611_Group_4_Final_Project.Pages.User.Notification
{
    public class NotificationModel : PageModel
    {
        private readonly IRepository<Models.User> _repos;

        public IEnumerable<Models.Notification> Notification { get; set; }
        public NotificationModel(IRepository<Models.User> repos)
        {
            _repos = repos;
        }

        public IActionResult OnGet()
        {
            Notification = new List<Models.Notification>();
            string userJson = HttpContext.Session.GetString("User");
            if (userJson.IsNullOrEmpty()) return RedirectToPage("/Index");
            var user = JsonConvert.DeserializeObject<Models.User>(userJson);

            var userInfo = _repos.GetDbSet()?.Include(u => u.Invoices)?.ThenInclude(i => i.Rooms)?.ThenInclude(r => r.Notifications)?.FirstOrDefault(u => u.Id == user.Id);
            foreach (var invoice in userInfo.Invoices)
            {
                if (invoice.Status == (int)Constant.InvoiceStatus.Accepted)
                {
                    foreach (var r in invoice.Rooms)
                    {
                        foreach (var nf in r.Notifications)
                        {
                            if (nf.HideDate >= DateTime.Now && nf.IsUse)
                            {
                                Notification = Notification.Append(nf);
                            }
                        }
                    }
                }
            }
            if (Notification != null) Notification = Notification.OrderByDescending(n => n.CreatedDate ?? DateTime.Now);
            return Page();
        }
    }
}
