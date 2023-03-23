using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Models;
using SE1611_Group_4_Final_Project.Utils;
using SendGrid.Helpers.Mail;
using System.IO;

namespace SE1611_Group_4_Final_Project.Pages
{
    public class EditProfileModel : PageModel
    {
        public readonly ILogger<EditProfileModel> _logger;
        private readonly IRepository<Models.User> _userRepository;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        [BindProperty]
        public Models.User user { get; set; }
        [BindProperty]
        public string constant { get; set; }
        public EditProfileModel(ILogger<EditProfileModel> logger, IRepository<Models.User> UserRepository, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _userRepository = UserRepository;
            _logger = logger;
            _environment = environment;
        }
        public void OnGet()
        {
            string jsonUser = HttpContext.Session.GetString("User");
            if (!string.IsNullOrEmpty(jsonUser))
            {
                user = JsonConvert.DeserializeObject<Models.User>(jsonUser);
            }
            JObject jsonStr = JObject.Parse(System.IO.File.ReadAllText("TempData.json"));
            if (jsonStr != null)
            {

                if (jsonStr.ContainsKey(user.Id.ToString()))
                {
                    constant = jsonStr[user.Id.ToString()].ToString();
                }
                else
                {
                    constant = "";
                }
            }
            else
            {
                constant = "";
            }

        }
        public IActionResult OnPost()
        {
            JObject json = JObject.Parse(System.IO.File.ReadAllText("TempData.json"));
            if (json.ContainsKey(user.Id.ToString()))
            {
                json.Remove(user.Id.ToString());
            }
            json[user.Id.ToString()] = constant;
            System.IO.File.WriteAllText("TempData.json", json.ToString());
            _userRepository.Update(user);
            return RedirectToPage("/User/Profile/UserProfile");
        }
    }
}
