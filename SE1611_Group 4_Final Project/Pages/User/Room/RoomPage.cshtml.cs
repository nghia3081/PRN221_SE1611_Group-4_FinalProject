using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Models;

namespace SE1611_Group_4_Final_Project.Pages
{
    public class RoomPageModel : PageModel
    {

        public readonly ILogger<RoomPageModel> _logger;
        private readonly IRepository<Models.Room> _roomRepository;
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
        }
        public int minPrice { get; set; }
        public int maxPrice { get; set; }
        public int minArea { get; set; }
        public int maxArea { get; set; }
        public string address { get; set; }
        public List<Models.Room> Rooms { get; set; }
        public SelectList FilterAddress { get; set; }
        public SelectList FilterArea { get; set; }
        public SelectList FilterPrice { get; set; }
        public List<Models.Room> SuggestedRooms { get; set; }
        private const int PageSize = 10;

        public int TotalPages { get; private set; }
        public int PageIndex { get; private set; }
        public bool HasPreviousPage => (PageIndex > 1);
        public bool HasNextPage => (PageIndex < TotalPages);

        public RoomPageModel(ILogger<RoomPageModel> logger, IRepository<Models.Room> RoomRepository)
        {
            _roomRepository = RoomRepository;
            _logger = logger;
        }

        public void OnGet(string filterAddress, string filterArea, string filterPrice, int pageIndex = 1)
        {
            var ListAddress = _roomRepository.SelectField(p => p.Address).Distinct();
            FilterAddress = new SelectList(ListAddress);
            switch (filterArea)
            {
                case null:
                    minArea = -1; maxArea = -1;
                    break;
                case "1":
                    minArea = 0; maxArea = 40;
                    break;
                case "2":
                    minArea = 40; maxArea = 100;
                    break;
            }

            switch (filterPrice)
            {
                case null:
                    minPrice = -1; maxPrice = -1;
                    break;
                case "1":
                    minPrice = 0; maxPrice = 2000000;
                    break;
                case "2":
                    minPrice = 2000000; maxPrice = 10000000;
                    break;
            }
            address = filterAddress;
            PageIndex = pageIndex;
            Rooms = _roomRepository.FilterRooms(minPrice, maxPrice, minArea, maxArea, address, PageIndex, PageSize).Where(x => x.IsAvailable == true).ToList();
            TotalPages = _roomRepository.GetDbSet().Count() / PageSize + (_roomRepository.GetDbSet().Count() % PageSize > 0 ? 1 : 0);
            SuggestedRooms = _roomRepository.FilterRooms(minPrice, maxPrice, minArea, maxArea, address, pageIndex, PageSize).Take(5).ToList();
            string json = JsonConvert.SerializeObject(SuggestedRooms);
            HttpContext.Session.SetString("SuggestedRooms", json);
        }

        public IActionResult OnGetLogout()
        {
            HttpContext.Session.Remove("User");
            return RedirectToPage("/Index");
        }

        public IActionResult OnGetLogin()
        {
            return RedirectToPage("/User/Auth/Login");
        }
    }
}
