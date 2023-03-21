using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SE1611_Group_4_Final_Project.IRepository;

namespace SE1611_Group_4_Final_Project.Pages.Admin.User
{
    public class FormModel : PageModel
    {
        private readonly IRepository<Models.User> _repository;
        [BindProperty]
        public Models.User User { get; set; }
        public FormModel(IRepository<Models.User> repository)
        {
            _repository = repository;
        }
        public void OnGet(Guid? id)
        {
            if (id.HasValue)
            {
                User = _repository.Find(id);
            }
        }
        public IActionResult OnPost()
        {
            _repository.Update(User);
            return RedirectToPage("/Index");
        }
    }
}
