using Microsoft.AspNetCore.Mvc.RazorPages;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Utils;

namespace SE1611_Group_4_Final_Project.Pages.User
{
    public class IndexModel : PageModel
    {
        private readonly IRepository<Models.User> _repository;
        public int TotalPage { get; set; }
        private const int PageSize = 18;
        public IEnumerable<Models.User> Users;
        public IndexModel(IRepository<Models.User> repository)
        {
            _repository = repository;
        }
        public void OnGet(string? query, int? page = 0)
        {
            TotalPage = Constant.GetTotalPage(_repository.GetDbSet().Count(), PageSize);
            Users = _repository.GetDbSet();
            if (!query.IsNullOrEmpty()) Users = Users.Where(u => u.Name.Contains(query) || u.Email.Contains(query) || u.IdentifyNumber.Contains(query) || u.Phone.Contains(query));
            Users = Users.Skip(Constant.GetTotalPage(page.Value, PageSize)).Take(PageSize);
        }
    }
}
