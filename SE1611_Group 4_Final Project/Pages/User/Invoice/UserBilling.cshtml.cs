using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SE1611_Group_4_Final_Project.IRepository;

namespace SE1611_Group_4_Final_Project.Pages
{
    public class UserBillingModel : PageModel
    {

        public readonly ILogger<UserBillingModel> _logger;
        private readonly IRepository<Models.Invoice> _invoiceRepository;
        private readonly IRepository<Models.Room> _roomRepository;
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
        }
        public int month { get; set; }
        public int year { get; set; }

        public SelectList FilterMonth { get; set; }
        public SelectList FilterYear { get; set; }

        public List<Models.Invoice> RoomInvoices { get; set; }
        public List<Models.Invoice> ServiceInvoices { get; set; }
        public List<Models.Room> Rooms { get; set; }
        public Models.User user { get; set; }

        public UserBillingModel(ILogger<UserBillingModel> logger, IRepository<Models.Invoice> InvoiceRepository, IRepository<Models.Room> RoomRepository)
        {
            _invoiceRepository = InvoiceRepository;
            _roomRepository = RoomRepository;
            _logger = logger;
        }

        public void OnGet(string filterMonth, string filterYear, string id)
        {
            string jsonUser = HttpContext.Session.GetString("User");
            if (!string.IsNullOrEmpty(jsonUser))
            {
                user = JsonConvert.DeserializeObject<Models.User>(jsonUser);
            }
            var ListYear = _invoiceRepository.SelectField(p => p.CreatedDate.Year).Distinct();
            var ListMonth = _invoiceRepository.SelectField(p => p.CreatedDate.Month).Distinct();
            if (ListYear != null && ListMonth != null)
            {
                FilterMonth = new SelectList(ListMonth);
                FilterYear = new SelectList(ListYear);
                if (filterMonth != null) { month = Int32.Parse(filterMonth); } else { month = -1; }
                if (filterYear != null) { year = Int32.Parse(filterYear); } else { year = -1; }
                RoomInvoices = _invoiceRepository.FilterRoomInvoices(month, year, user.Id);
                ServiceInvoices = _invoiceRepository.FilterServiceInvoices(month, year, user.Id);
                if (id != null) { Rooms = _roomRepository.GetRoomsbyInvoice(Guid.Parse(id)); }
            }
        }
    }
}
