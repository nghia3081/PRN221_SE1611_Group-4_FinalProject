using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
        [BindProperty]
        public decimal GrandTotal { get; set; }
        Models.User user { get; set; }
        public BookingListModel(ILogger<BookingListModel> logger, IRepository<Models.Room> RoomRepository, IRepository<Models.Invoice> InvoiceRepository)
        {
            _roomRepository = RoomRepository;
            _invoiceRepository = InvoiceRepository;
            _logger = logger;
        }
        private void CalGrandTotal()
        {
            GrandTotal = Rooms.Sum(r => r.Price);
        }
        public void OnGet(string id)
        {
            string jsonUser = HttpContext.Session.GetString("User");
            if (!string.IsNullOrEmpty(jsonUser))
            {
                user = JsonConvert.DeserializeObject<Models.User>(jsonUser);
            }
            Invoices = _invoiceRepository.FindwithQuery(x => x.Status == (int)Constant.InvoiceStatus.Waiting && x.UserId == user.Id).ToList();
            Rooms = _invoiceRepository.GetRoomInvoice(Invoices);
            CalGrandTotal();
        }
        public IActionResult OnGetRemove(string id)
        {
            string jsonUser = HttpContext.Session.GetString("User");
            if (!string.IsNullOrEmpty(jsonUser))
            {
                user = JsonConvert.DeserializeObject<Models.User>(jsonUser);
            }
            var InvoiceId = _invoiceRepository.FindwithQuery(x => x.Status == (int)Constant.InvoiceStatus.Waiting && x.UserId == user.Id).Select(x => x.Id).First();
            Models.Invoice invoice = _invoiceRepository.GetDbSet().Where(x => x.Id == InvoiceId).Include(x => x.Rooms).FirstOrDefault();
            var room = _roomRepository.FindwithQuery(x => x.Id == Guid.Parse(id)).First();
            invoice.Rooms.Remove(room);
            _invoiceRepository.Update(invoice).Wait();
            room.IsAvailable = true;
            CalGrandTotal();
            return RedirectToPage("/User/Room/BookingList");
        }
        public IActionResult Onpost(string from, string to)
        {
            if (Rooms != null && Rooms.Count() != 0 ){
                string jsonUser = HttpContext.Session.GetString("User");
                if (!string.IsNullOrEmpty(jsonUser))
                {
                    user = JsonConvert.DeserializeObject<Models.User>(jsonUser);
                }
                var InvoiceId = _invoiceRepository.FindwithQuery(x => x.Status == (int)Constant.InvoiceStatus.Waiting && x.UserId == user.Id).Select(x => x.Id).First();
                if (InvoiceId != null)
                {
                    Models.Invoice invoice = _invoiceRepository.GetDbSet().Where(x => x.Id == InvoiceId).Include(x => x.Rooms).FirstOrDefault();
                    invoice.Status = (int)Constant.InvoiceStatus.Booked;
                    invoice.From = DateTime.Parse(from);
                    invoice.To = DateTime.Parse(to);
                    invoice.GrandTotal = GrandTotal;
                    _invoiceRepository.Update(invoice).Wait();
                }
                return RedirectToPage("/User/Room/BookingList");
            }
            return Page();
        }

    }
}
