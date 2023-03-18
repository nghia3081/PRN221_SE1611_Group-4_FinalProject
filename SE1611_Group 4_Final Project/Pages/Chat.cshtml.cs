using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SE1611_Group_4_Final_Project.Pages
{
    public class ChatModel : PageModel
    {
        public void OnGet()
        {
            // Get user ID from session or create new ID
            string userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                userId = System.Guid.NewGuid().ToString();
                HttpContext.Session.SetString("UserId", userId);
            }

            ViewData["UserId"] = userId;
        }
    }
}
