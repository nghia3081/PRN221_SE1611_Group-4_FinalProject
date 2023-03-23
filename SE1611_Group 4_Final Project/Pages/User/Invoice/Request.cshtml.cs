using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Models;

namespace SE1611_Group_4_Final_Project.Pages.User.Invoice
{
    public class RequestModel : PageModel
    {
        public readonly ILogger<RequestModel> _logger;
        private readonly IRepository<Models.Invoice> _invoiceRepository;
        private readonly IRepository<Models.Room> _roomRepository;
        public List<Models.Invoice> invoices { get; set; }
        public Models.User user { get; set; }
        public RequestModel(ILogger<RequestModel> logger, IRepository<Models.Invoice> InvoiceRepository, IRepository<Models.Room> RoomRepository)
        {
            _invoiceRepository = InvoiceRepository;
            _roomRepository = RoomRepository;
            _logger = logger;
        }
        public void OnGet()
        {
            string jsonUser = HttpContext.Session.GetString("User");
            if (!string.IsNullOrEmpty(jsonUser))
            {
                user = JsonConvert.DeserializeObject<Models.User>(jsonUser);
            }
            invoices = _invoiceRepository.GetDbSet().Include(x => x.Rooms).Where(x => x.GrandTotal == 0 && x.UserId == user.Id).ToList();
        }
        public IActionResult OnGetDelete(Guid id)
        {
            Models.Invoice invoice = _invoiceRepository.Find(id);
            invoice.Rooms.Clear();
            _invoiceRepository.Delete(invoice);
            return RedirectToPage("/User/Invoice/Request");
        }
    }
}
