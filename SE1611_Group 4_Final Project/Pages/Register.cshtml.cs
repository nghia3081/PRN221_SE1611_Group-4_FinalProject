using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Models;
using System.ComponentModel.DataAnnotations;

namespace SE1611_Group_4_Final_Project.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IRepository<User> _userRepository;
        private readonly ILogger<LoginModel> _logger;
        public RegisterModel(ILogger<LoginModel> logger, IRepository<User> userRepository)
        {
            _userRepository = userRepository;
            _logger = logger;
        }


        [BindProperty]
        public InputModel Input { get; set; }
        public string ErrorMessage { get; set; }
        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            [Required]
            public string Name { get; set; }
            [Required]
            public string Password { get; set; }
            [Required]
            public string PasswordConfirm { get; set; }
            [Required]
            public string Address { get; set; }
            [Required]
            public string PhoneNumber { get; set; }

        }
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            User user = _userRepository.FindUserByEmail(Input.Email);
            if (user != null){
                ErrorMessage = "Email exist! Please choose another Email";
                return Page();
            }
            else
            {
                User newUser = new();
                newUser.Id = Guid.NewGuid();
                newUser.Email = Input.Email;
                newUser.Name = Input.Name;
                newUser.Password = Input.Password;
                newUser.Address = Input.Address;
                newUser.Phone = Input.PhoneNumber;
                _userRepository.Add(newUser);
                return RedirectToPage("Login");
            }
        }
    }
}
