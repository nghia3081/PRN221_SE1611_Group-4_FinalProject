using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SE1611_Group_4_Final_Project.IRepository;
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
            Room = new SelectList(room, "Id", "Name");
        }
        //public IActionResult OnPost(string type, string title, Guid roomId)
        //{
        //    string jsonUser = HttpContext.Session.GetString("User");
        //    if (!string.IsNullOrEmpty(jsonUser))
        //    {
        //        user = JsonConvert.DeserializeObject<Models.User>(jsonUser);
        //    }

        //    var roomRef = _roomRepository.GetDbSet().Include(r => r.Invoices).FirstOrDefault(r => r.Id == roomId);

        //    Models.Invoice invoice = new Models.Invoice();
        //    invoice.Id = Guid.NewGuid();
        //    invoice.CreatedDate= DateTime.Now;
        //    invoice.UserId = user.Id;
        //    invoice.GrandTotal = 0;
           
            
        //    invoice.Title = "Request_" + roomRef.Name +"_"+ type;
        //    invoice.Description = title;
        //    invoice.Type = (int)Enum.Parse(typeof(Constant.InvoiceType), type);
        //    invoice.Status = (int)Constant.InvoiceStatus.Processing;
        //    roomRef.Invoices.Add(invoice);
        //    invoice.Rooms.Add(roomRef);
        //    _roomRepository.Update(roomRef).Wait();
        //    return RedirectToPage("/User/Invoice/Request");
        //}
    }
}
