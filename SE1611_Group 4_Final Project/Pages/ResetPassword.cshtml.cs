using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SE1611_Group_4_Final_Project.Pages
{
    public class ResetPasswordModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Password { get; set; }
            public string ConfirmPassword { get; set; }
        }
        public void OnGet()
        {
        }
    }
}
