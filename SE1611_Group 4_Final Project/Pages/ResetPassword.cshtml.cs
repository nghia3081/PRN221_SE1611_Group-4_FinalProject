using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Framework;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Models;
using SE1611_Group_4_Final_Project.Repository;
using SE1611_Group_4_Final_Project.Utils;

namespace SE1611_Group_4_Final_Project.Pages
{
    public class ResetPasswordModel : PageModel
    {
        private readonly IRepository<User> _userRepository;
        public ResetPasswordModel(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Password { get; set; }
            public string ConfirmPassword { get; set; }
        }
        public string ErrorMessage { get; set; }
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPost()
        {
            string token = HttpContext.Session.GetString(Constant.forgotTokenSessionKey);
            if (token.IsNullOrEmpty()) throw new Exception("Invalid request");
            string[] tokenSplit = token.Split(':');
            if (!_userRepository.GetDbSet().Any(u => u.Id.ToString().Equals(tokenSplit[0]))) throw new Exception("Invalid User");
            long tick = long.Parse(tokenSplit[1]);
            DateTime tokenCreatedTime = new DateTime(tick);
            if (tokenCreatedTime.AddMinutes(Constant.expireForgotTokenMinute) < DateTime.Now) throw new Exception("Token has expired");
            else
            {
                User user = _userRepository.GetDbSet().FirstOrDefault(u => u.Id.ToString().Equals(tokenSplit[0]));
                if (!Input.Password.Equals(Input.ConfirmPassword))
                {
                    ErrorMessage = "Password confirm not coincidental";
                    return Page();
                }
                else
                {
                    user.Password = Input.ConfirmPassword;
                    _userRepository.Update(user);
                    return RedirectToPage("Login");
                }

            }
        }
    }
}
