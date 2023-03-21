using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Utils;

namespace SE1611_Group_4_Final_Project.Pages
{
    public class RoomDetailModel : PageModel
    {
        private readonly ILogger<RoomDetailModel> _logger;
        private readonly IRepository<Models.Room> _roomRepository;
        private readonly IRepository<Models.Invoice> _invoiceRepository;
        public Models.Room Room { get; set; }
        public List<Models.Room> SuggestedRooms { get; set; }
        Models.User user { get; set; }
        public RoomDetailModel(ILogger<RoomDetailModel> logger, IRepository<Models.Room> RoomRepository, IRepository<Models.Invoice> InvoiceRepository)
        {
            _roomRepository = RoomRepository;
            _invoiceRepository = InvoiceRepository;
            _logger = logger;
        }
        public void OnGet(string id)
        {
            Room = _roomRepository.Find(Guid.Parse(id));
            string json = HttpContext.Session.GetString("SuggestedRooms");
            if (!String.IsNullOrEmpty(json))
            {
                SuggestedRooms = JsonConvert.DeserializeObject<List<Models.Room>>(json);
            }
        }
        public void OnPost(string id)
        {
            string jsonUser = HttpContext.Session.GetString("User");
            if (!string.IsNullOrEmpty(jsonUser))
            {
                user = JsonConvert.DeserializeObject<Models.User>(jsonUser);
            }
            string json = HttpContext.Session.GetString("SuggestedRooms");
            if (!String.IsNullOrEmpty(json))
            {
                SuggestedRooms = JsonConvert.DeserializeObject<List<Models.Room>>(json);
            }
            Room = _roomRepository.Find(Guid.Parse(id));
            Models.Invoice invoice = _invoiceRepository.FindwithQuery(x => x.Status == ((int)Constant.InvoiceStatus.UnPaid) && x.UserId == user.Id).FirstOrDefault();
            if(invoice == null) {
                invoice = new Models.Invoice();
                invoice.UserId = user.Id;
                invoice.Id = Guid.NewGuid();
                invoice.CreatedDate= DateTime.Now;
                invoice.Status = (int)Constant.InvoiceStatus.UnPaid;
                invoice.Title = "Booking_Room"+"_"+user.Name.ToString()+"_"+DateTime.Now.ToString();
                invoice.GrandTotal = Room.Price;
                _invoiceRepository.Add(invoice);
            }
            if (Room.IsAvailable == true)
            {
                _invoiceRepository.AddRoomInvoice(Room.Id, invoice.Id);
                Room = _roomRepository.Find(Guid.Parse(id));
                Room.IsAvailable = false;
                _roomRepository.Update(Room);
            }
        }
    }
}
