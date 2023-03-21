using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SE1611_Group_4_Final_Project.IRepository;

namespace SE1611_Group_4_Final_Project.Pages
{
    public class UserProfileModel : PageModel
    {
        private readonly ILogger<UserProfileModel> _logger;
        private readonly IRepository<Models.User> _userRepository;
        public Models.User user { get; set; }
        public string content { get; set; }
        public UserProfileModel(ILogger<UserProfileModel> logger, IRepository<Models.User> UserRepository)
        {
            _userRepository = UserRepository;
            _logger = logger;
        }
        public void OnGet()
        {
            string jsonUser = HttpContext.Session.GetString("User");
            if (!string.IsNullOrEmpty(jsonUser))
            {
                user = JsonConvert.DeserializeObject<Models.User>(jsonUser);
            }
            JObject jsonStr = JObject.Parse(System.IO.File.ReadAllText("TempData.json"));
            if(jsonStr != null) {

                if (jsonStr.ContainsKey(user.Id.ToString()))
                {
                    content = jsonStr[user.Id.ToString()].ToString();
                }
                else
                {
                    content = "";
                }
            }
            else
            {
                content = "";
            }
            
        }
    }
}
