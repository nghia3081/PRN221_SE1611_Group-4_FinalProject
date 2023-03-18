using Microsoft.AspNetCore.Mvc.RazorPages;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Models;
using SE1611_Group_4_Final_Project.Utils;

namespace SE1611_Group_4_Final_Project.Pages
{
    public class ForgotPasswordConfirmationModel : PageModel
    {
        private readonly IRepository<User> _userRepository;
        public ForgotPasswordConfirmationModel(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }
        public void OnGet()
        {
            string token = HttpContext.Session.GetString(Constant.forgotTokenSessionKey);
            if (token.IsNullOrEmpty()) throw new Exception("Invalid request");
            string[] tokenSplit = token.Split(':');
            if (!_userRepository.GetDbSet().Any(u => u.Id.ToString().Equals(tokenSplit[0]))) throw new Exception("Invalid User");
            long tick = long.Parse(tokenSplit[1]);
            DateTime tokenCreatedTime = new DateTime(tick);
            if (tokenCreatedTime.AddMinutes(Constant.expireForgotTokenMinute) < DateTime.Now) throw new Exception("Token has expired");

        }
    }
}
