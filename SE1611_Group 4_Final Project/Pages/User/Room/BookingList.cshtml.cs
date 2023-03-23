using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Utils;

namespace SE1611_Group_4_Final_Project.Pages.User.Room
{
    public class BookingListModel : PageModel
    {
        public readonly ILogger<BookingListModel> _logger;
        private readonly IRepository<Models.Room> _roomRepository;
        private readonly IRepository<Models.Invoice> _invoiceRepository;
        public List<Models.Invoice> Invoices { get; set; }
        public List<Models.Room> Rooms { get; set; }
        Models.User user { get; set; }
        public BookingListModel(ILogger<BookingListModel> logger, IRepository<Models.Room> RoomRepository, IRepository<Models.Invoice> InvoiceRepository)
        {
            _roomRepository = RoomRepository;
            _invoiceRepository = InvoiceRepository;
            _logger = logger;
        }
        public void OnGet(string id)
        {
            string jsonUser = HttpContext.Session.GetString("User");
            if (!string.IsNullOrEmpty(jsonUser))
            {
                user = JsonConvert.DeserializeObject<Models.User>(jsonUser);
            }
            Invoices = _invoiceRepository.FindwithQuery(x => x.Status == (int)Constant.InvoiceStatus.Created && x.UserId == user.Id).ToList();
            Rooms = _invoiceRepository.GetRoomInvoice(Invoices);
        }
        public IActionResult OnGetRemove(string id)
        {
            string jsonUser = HttpContext.Session.GetString("User");
            if (!string.IsNullOrEmpty(jsonUser))
            {
                user = JsonConvert.DeserializeObject<Models.User>(jsonUser);
            }
            var Invoice = _invoiceRepository.FindwithQuery(x => x.Status == (int)Constant.InvoiceStatus.Created && x.UserId == user.Id).Select(x => x.Id).First();

            _invoiceRepository.RemoveRoomfromInvoice(Guid.Parse(id), Invoice);
            return RedirectToPage("/User/Room/BookingList");
        }

    }
}
