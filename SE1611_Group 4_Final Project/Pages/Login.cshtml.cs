using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Models;

namespace SE1611_Group_4_Final_Project.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }
        public string ErrorMessage { get; set; }
        private readonly IRepository<User> _userRepository;
        private readonly ILogger<LoginModel> _logger;
        public LoginModel(ILogger<LoginModel> logger, IRepository<User> userRepository)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public void OnGet()
        {

        }
        public IActionResult OnPost() {
            var user = _userRepository.FindUserByEmailandPassword(Email, Password);

            if (user != null)
            {
                string json = JsonConvert.SerializeObject(user);
                HttpContext.Session.SetString("User", json);
                if (Request.Form["inputRememberPassword"] == "on")
                {
                    CookieOptions option = new CookieOptions();
                    option.Expires = DateTime.Now.AddDays(30);
                    Response.Cookies.Append("email", Email, option);
                }
                return RedirectToPage("Index");
            }
            else
            {
                ErrorMessage = "Email or Password Invalid";
                return Page();
            }
        }
    }
}
