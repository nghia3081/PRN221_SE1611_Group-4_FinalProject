using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Models;
using SE1611_Group_4_Final_Project.Utils;

namespace SE1611_Group_4_Final_Project.Pages.User.RoomFurniture
{
    public class RoomFurnitureModel : PageModel
    {
        private readonly IRepository<Models.User> repository;
        public IEnumerable<Models.RoomFurniture> RoomFurnitures { get; set; }
        [BindProperty(SupportsGet = true)]
        public Guid? roomId { get; set; }
        public SelectList RoomSelect { get; set; }
        public RoomFurnitureModel(IRepository<Models.User> repository)
        {
            this.repository = repository;
        }

        public IActionResult OnGet()
        {
            RoomFurnitures = new List<Models.RoomFurniture>();
            List<Models.Room> lstRoom = new List<Models.Room>();
            string userJson = HttpContext.Session.GetString("User");
            if (userJson.IsNullOrEmpty()) return RedirectToPage("/Index");
            var user = JsonConvert.DeserializeObject<Models.User>(userJson);
            try
            {
                var userInfo = repository.GetDbSet().Include(u => u.Invoices).ThenInclude(i => i.Rooms).ThenInclude(r => r.RoomFurnitures).FirstOrDefault(u => u.Id == user.Id);
                foreach (var invoice in userInfo.Invoices)
                {
                    if (invoice.Status == (int)Constant.InvoiceStatus.Accepted)
                    {
                        lstRoom.AddRange(invoice.Rooms);
                        foreach (var r in invoice.Rooms)
                        {
                            foreach (var nf in r.RoomFurnitures)
                            {
                                RoomFurnitures = RoomFurnitures.Append(nf);
                            }

                        }
                    }
                }
            }
            catch (Exception)
            {
                RoomFurnitures = null;
            }
            if (roomId.HasValue && RoomFurnitures != null) RoomFurnitures = RoomFurnitures.Where(rf => rf.RoomId == roomId);
            RoomSelect = new SelectList(lstRoom, "Id", "Name", roomId);
            return Page();
        }
        public IActionResult OnGetReportCrashed(Guid id)
        {
            var rf = repository.GetDbSet<Models.RoomFurniture>().Find(id);
            rf.Status = (int)Constant.RoomFurnitureStatus.CrashReported;
            repository.GetDbSet<Models.RoomFurniture>().Update(rf).Context.SaveChanges();
            return OnGet();
        }
    }
}
