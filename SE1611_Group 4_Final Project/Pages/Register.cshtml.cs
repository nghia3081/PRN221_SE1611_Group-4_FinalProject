using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SE1611_Group_4_Final_Project.IRepository;
using System.ComponentModel.DataAnnotations;

namespace SE1611_Group_4_Final_Project.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IRepository<Models.User> _userRepository;
        private readonly ILogger<LoginModel> _logger;
        public RegisterModel(ILogger<LoginModel> logger, IRepository<Models.User> userRepository)
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
            Models.User user = _userRepository.FindUserByEmail(Input.Email);
            if (user != null){
                ErrorMessage = "Email exist! Please choose another Email";
                return Page();
            }
            else
            {
                Models.User newUser = new()
                {
                    Id = Guid.NewGuid(),
                    Email = Input.Email,
                    Name = Input.Name,
                    Password = Input.Password,
                    Address = Input.Address,
                    Phone = Input.PhoneNumber
                };
                _userRepository.Add(newUser);
                return RedirectToPage("Login");
            }
        }
    }
}
