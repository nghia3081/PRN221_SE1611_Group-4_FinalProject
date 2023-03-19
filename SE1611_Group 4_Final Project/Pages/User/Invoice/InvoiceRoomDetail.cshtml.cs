using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Models;

namespace SE1611_Group_4_Final_Project.Pages
{
    public class InvoiceRoomDetailModel : PageModel
    {
		private readonly ILogger<InvoiceRoomDetailModel> _logger;
		private readonly IRepository<Models.Invoice> _invoiceRepository;
		private readonly IRepository<Models.Room> _roomRepository;
		private readonly IRepository<Models.RoomFurniture> _roomFurnitureRepository;
		public Models.Invoice invoice { get; set; }
		public List<Models.Room> Rooms { get; set; }
		public decimal Total { get; set; }
		public List<Models.RoomFurniture> RoomFurnitures { get; set; }
		public InvoiceRoomDetailModel(ILogger<InvoiceRoomDetailModel> logger, IRepository<Models.Invoice> InvoiceRepository, IRepository<Models.Room> roomRepository, IRepository<Models.RoomFurniture> roomFurnitureRepository)
		{
			_invoiceRepository = InvoiceRepository;
			_roomRepository = roomRepository;
			_roomFurnitureRepository = roomFurnitureRepository;
			_logger = logger;
		}
		public void OnGet(string id)
		{
			Rooms = _roomRepository.GetRoomsbyInvoice(Guid.Parse(id));
			invoice = _invoiceRepository.GetInvoice(Guid.Parse(id));

			RoomFurnitures = new List<Models.RoomFurniture>();
			foreach (Models.Room room in Rooms)
			{
				var furnitures = _roomFurnitureRepository.FindwithQuery(x => x.Room.Id == room.Id).ToList();
				foreach(Models.RoomFurniture item in furnitures)
				{
					RoomFurnitures.Add(item);
				}
			}
			foreach(var item in Rooms)
			{
				Total += item.Price;
			}
			Total = ((int)Total);
		}
	}
}
