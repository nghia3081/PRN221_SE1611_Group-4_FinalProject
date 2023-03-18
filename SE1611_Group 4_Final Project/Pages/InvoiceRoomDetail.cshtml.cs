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
		private readonly IRepository<Invoice> _invoiceRepository;
		private readonly IRepository<Room> _roomRepository;
		private readonly IRepository<RoomFurniture> _roomFurnitureRepository;
		public Invoice invoice { get; set; }
		public List<Room> Rooms { get; set; }
		public decimal Total { get; set; }
		public List<RoomFurniture> RoomFurnitures { get; set; }
		public InvoiceRoomDetailModel(ILogger<InvoiceRoomDetailModel> logger, IRepository<Invoice> InvoiceRepository, IRepository<Room> roomRepository, IRepository<RoomFurniture> roomFurnitureRepository)
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

			RoomFurnitures = new List<RoomFurniture>();
			foreach (Room room in Rooms)
			{
				var furnitures = _roomFurnitureRepository.FindwithQuery(x => x.Room.Id == room.Id).ToList();
				foreach(RoomFurniture item in furnitures)
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
