using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Models;

namespace SE1611_Group_4_Final_Project.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IRepository<Invoice> repository;
        private readonly ILogger<IndexModel> _logger;
        [BindProperty]
        public string Email { get; set; }
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            Email = HttpContext.Session.GetString("email");
        }
        public IActionResult OnGetLogout()
        {
            HttpContext.Session.Remove("email");
            return RedirectToPage("Login");
        }
        public IActionResult OnGetLogin()
        {
            return RedirectToPage("Login");
        }
    }
}