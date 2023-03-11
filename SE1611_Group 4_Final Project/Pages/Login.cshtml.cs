using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SE1611_Group_4_Final_Project.Models;
using SE1611_Group_4_Final_Project.Repository;
using System.Xml.Linq;

namespace SE1611_Group_4_Final_Project.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }
        public string Msg { get; set; }
        public Repository<User> repository { get; set; }
        private readonly ILogger<LoginModel> _logger;
        public LoginModel(ILogger<LoginModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
        public IActionResult OnPost() {
            var user = repository.Find(Email,Password);

            if (user != null)
            {
                HttpContext.Session.SetString("email", Email);
                if (Request.Form["inputRememberPassword"] == "on")
                {
                    CookieOptions option = new CookieOptions();
                    option.Expires = DateTime.Now.AddDays(30);
                    //Create a Cookie with a suitable Key and add the Cookie to Browser.
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
