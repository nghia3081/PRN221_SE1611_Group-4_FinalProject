using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SE1611_Group_4_Final_Project.Models;
using SE1611_Group_4_Final_Project.Repository.Interfaces;
using System.Text.Encodings.Web;
using System.ComponentModel.DataAnnotations;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Utils;

namespace SE1611_Group_4_Final_Project.Pages
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly ILogger<ForgotPasswordModel> _logger;
        private IRepository<User> UserRepository;
        private readonly IEmailSender _emailSender;
        public ForgotPasswordModel(ILogger<ForgotPasswordModel> logger, IEmailSender emailSender, IRepository<User> repository)
        {
            UserRepository = repository;    
            _logger = logger; 
            _emailSender = emailSender;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public string ErrorMessage { get; set; }
        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {

                var user = UserRepository.FindUserByEmail(Input.Email);
                if (user == null)
                {
                    // Nếu email không tồn tại, hiển thị thông báo lỗi
                    ErrorMessage = "Email not found";
                    return Page();
                }

                // Nếu email tồn tại, gửi email thông báo đặt lại mật khẩu
                var token = UserRepository.GeneratePasswordResetToken(user);
                HttpContext.Session.SetString(Constant.forgotTokenSessionKey, token);
                // Gửi email thông báo đặt lại mật khẩu
                await SendPasswordResetEmail(token);

                // Hiển thị trang thông báo rằng email đã được gửi đi thành công
                return RedirectToPage("ForgotPasswordConfirmation");

            }

            return Page();
        }
        private async Task SendPasswordResetEmail(string token)
        {
            var callbackUrl = Url.Page(
                "ResetPassword",
                pageHandler: null,
                values: new { token },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(
                Input.Email,
                "Reset Password",
                $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
        }
    }
}
