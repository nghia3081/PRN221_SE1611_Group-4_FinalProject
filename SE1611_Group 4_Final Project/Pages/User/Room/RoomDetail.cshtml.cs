using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Models;

namespace SE1611_Group_4_Final_Project.Pages
{
    public class RoomDetailModel : PageModel
    {
        private readonly ILogger<RoomDetailModel> _logger;
        private readonly IRepository<Models.Room> _roomRepository;
        public Models.Room Room { get; set; }
        public List<Models.Room> SuggestedRooms { get; set; }
        public RoomDetailModel(ILogger<RoomDetailModel> logger, IRepository<Models.Room> RoomRepository)
        {
            _roomRepository = RoomRepository;
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
    }
}
