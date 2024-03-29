using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Models;

namespace SE1611_Group_4_Final_Project.Pages
{
    public class UserBillingModel : PageModel
    {

        private readonly ILogger<UserBillingModel> _logger;
        private readonly IRepository<Invoice> _invoiceRepository;
        private readonly IRepository<Room> _roomRepository;
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
        }
        public int month { get; set; }
        public int year { get; set; }

        public SelectList FilterMonth { get; set; }
        public SelectList FilterYear { get; set; }

        public List<Invoice> Invoices { get; set; }
        public List<Room> Rooms { get; set; }

        public UserBillingModel(ILogger<UserBillingModel> logger, IRepository<Invoice> InvoiceRepository, IRepository<Room> RoomRepository)
        {
            _invoiceRepository = InvoiceRepository;
            _roomRepository = RoomRepository;
            _logger = logger;
        }

        public void OnGet(string filterMonth, string filterYear, string id)
        {
            var ListYear = _invoiceRepository.SelectField(p => p.CreatedDate.Year).Distinct();
            var ListMonth = _invoiceRepository.SelectField(p => p.CreatedDate.Month).Distinct();
            if (ListYear != null && ListMonth != null)
            {
                FilterMonth = new SelectList(ListMonth);
                FilterYear = new SelectList(ListYear);
                if (filterMonth != null) { month = Int32.Parse(filterMonth); } else { month = -1; }
                if (filterYear != null) { year = Int32.Parse(filterYear); } else { year = -1; }
                Invoices = _invoiceRepository.FilterInvoices(month, year);
                if (id != null) { Rooms = _roomRepository.GetRoomsbyInvoice(Guid.Parse(id)); }
            }
        }
    }
}
