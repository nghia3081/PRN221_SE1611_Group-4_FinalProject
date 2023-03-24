using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json;
using SE1611_Group_4_Final_Project.IRepository;

namespace SE1611_Group_4_Final_Project.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }
        public string ErrorMessage { get; set; }
        private readonly IRepository<Models.User> _userRepository;
        private readonly IRepository<Models.Invoice> _invoiceRepository;
        private readonly IRepository<Models.Room> _roomRepository;
        private readonly IRepository<Models.Notification> _notificationRepository;
        public readonly ILogger<LoginModel> _logger;
        public LoginModel(ILogger<LoginModel> logger, IRepository<Models.User> userRepository, IRepository<Models.Invoice> invoiceRepository, IRepository<Models.Room> roomRepository, IRepository<Models.Notification> notificationRepository)
        {
            _userRepository = userRepository;
            _invoiceRepository = invoiceRepository;
            _roomRepository = roomRepository;
            _logger = logger;
            _notificationRepository = notificationRepository;
        }

        public void OnGet()
        {

        }
        public IActionResult OnPost()
        {
            var user = _userRepository.FindUserByEmailandPassword(Email, Password);

            if (user != null)
            {
                string json = JsonConvert.SerializeObject(user);
                HttpContext.Session.SetString("User", json);
                if (Request.Form["inputRememberPassword"] == "on")
                {
                    CookieOptions option = new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(30)
                    };
                    Response.Cookies.Append("email", Email, option);
                }
                return RedirectToPage("/Index");
            }
            else
            {
                ErrorMessage = "Email or Password Invalid";
                return Page();
            }
        }
    }
}
