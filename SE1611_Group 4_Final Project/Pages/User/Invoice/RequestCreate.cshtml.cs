using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Models;
using SE1611_Group_4_Final_Project.Utils;

namespace SE1611_Group_4_Final_Project.Pages.User.Invoice
{
    public class RequestCreateModel : PageModel
    {

        public readonly ILogger<RequestCreateModel> _logger;
        private readonly IRepository<Models.Invoice> _invoiceRepository;
        private readonly IRepository<Models.Room> _roomRepository;
        public Models.Invoice invoice { get; set; }
        public Models.User user { get; set; }
        public SelectList Type { get; set; }
        public SelectList Room { get; set; }
        public RequestCreateModel(ILogger<RequestCreateModel> logger, IRepository<Models.Invoice> InvoiceRepository, IRepository<Models.Room> RoomRepository)
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
            var type = Enum.GetNames(typeof(Constant.InvoiceType)).Cast<string>().ToList();
            Type = new SelectList(type);
            var invoices = _invoiceRepository.FindwithQuery(x => x.UserId == user.Id).ToList();
            var room = _roomRepository.GetRoomInvoice(invoices);
            Room = new SelectList(room.Select(x => x.Name));
        }
        public IActionResult OnPost(string type, string title, string room)
        {
            string jsonUser = HttpContext.Session.GetString("User");
            if (!string.IsNullOrEmpty(jsonUser))
            {
                user = JsonConvert.DeserializeObject<Models.User>(jsonUser);
            }
            List<Models.Room> rooms = new();
            rooms.Add(_roomRepository.FindwithQuery(x => x.Name.Equals(room)).First());
            Models.Invoice invoice = new Models.Invoice();
            invoice.Id = Guid.NewGuid();
            invoice.CreatedDate= DateTime.Now;
            invoice.UserId = user.Id;
            invoice.GrandTotal = 0;
            invoice.User = user;
            invoice.Rooms = rooms;
            invoice.Title = "Request-" + room +"-"+ type;
            invoice.Description = title;
            invoice.Type = (int)Enum.Parse(typeof(Constant.InvoiceType), type);
            invoice.Status = (int)Constant.InvoiceStatus.Processing;
            _invoiceRepository.Add(invoice);
            return RedirectToPage("/User/Invoice/Request");
        }
    }
}
