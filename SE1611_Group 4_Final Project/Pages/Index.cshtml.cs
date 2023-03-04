using Microsoft.AspNetCore.Mvc.RazorPages;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Models;

namespace SE1611_Group_4_Final_Project.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IRepository<Invoice> repository;
        private readonly ILogger<IndexModel> _logger;
        public IndexModel(ILogger<IndexModel> logger, IRepository<Invoice> repository)
        {
            _logger = logger;
        }

    }
}