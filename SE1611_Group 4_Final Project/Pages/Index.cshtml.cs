using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Models;
using SE1611_Group_4_Final_Project.Repository;
using SE1611_Group_4_Final_Project.Services;
using SE1611_Group_4_Final_Project.Utils;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace SE1611_Group_4_Final_Project.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IRepository<Models.Invoice> repository;
        public readonly ILogger<IndexModel> _logger;
        private readonly IRepository<Models.Room> _roomRepository;
        [BindProperty]
        public Models.User user { get; set; }
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

        public IndexModel(ILogger<IndexModel> logger, IRepository<Models.Room> RoomRepository)
        {
            _roomRepository = RoomRepository;
            _logger = logger;
        }

        public void OnGet()
        {
            Rooms = _roomRepository.GetAll().Result.Take(5).ToList();
            SuggestedRooms = _roomRepository.GetAll().Result.Take(5).ToList();

            string jsonUser = HttpContext.Session.GetString("User");
            if (!string.IsNullOrEmpty(jsonUser))
            {
                user = JsonConvert.DeserializeObject<Models.User>(jsonUser);
            }
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