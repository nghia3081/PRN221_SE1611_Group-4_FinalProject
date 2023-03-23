using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SE1611_Group_4_Final_Project.IRepository;

namespace SE1611_Group_4_Final_Project.Pages
{
    public class InvoiceServicesDetailModel : PageModel
    {
        public readonly ILogger<InvoiceServicesDetailModel> _logger;
        private readonly IRepository<Models.Invoice> _invoiceRepository;
        private readonly IRepository<Models.Room> _roomRepository;
        public Models.Invoice invoice { get; set; }
        public List<Models.Room> Rooms { get; set; }
        public InvoiceServicesDetailModel(ILogger<InvoiceServicesDetailModel> logger, IRepository<Models.Invoice> InvoiceRepository, IRepository<Models.Room> roomRepository, IRepository<Models.RoomFurniture> roomFurnitureRepository)
        {
            _invoiceRepository = InvoiceRepository;
            _roomRepository = roomRepository;
            _logger = logger;
        }
        public void OnGet(string id)
        {
            Rooms = _roomRepository.GetRoomsbyInvoice(Guid.Parse(id));
            invoice = _invoiceRepository.GetInvoice(Guid.Parse(id));
        }
    }
}
