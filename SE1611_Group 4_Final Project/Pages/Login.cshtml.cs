using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        private readonly ILogger<LoginModel> _logger;
        public LoginModel(ILogger<LoginModel> logger, IRepository<Models.User> userRepository)
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
                    CookieOptions option = new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(30)
                    };
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
