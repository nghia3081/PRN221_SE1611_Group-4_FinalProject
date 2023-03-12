using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Protocol.Core.Types;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Models;
using SE1611_Group_4_Final_Project.Repository;

namespace SE1611_Group_4_Final_Project.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }
        public string Msg { get; set; }
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
                HttpContext.Session.SetString("email", Email);
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
                Msg = "Email or Password Invalid";
                return Page();
            }
        }
    }
}
