using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Utils;

namespace SE1611_Group_4_Final_Project.Pages.User
{
    public class IndexModel : PageModel
    {
        private readonly IRepository<Models.User> _repository;
        public int TotalPage { get; private set; }
        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; private set; }
        public bool HasPreviousPage => (PageIndex > 1);
        public bool HasNextPage => (PageIndex < TotalPage);

        private const int PageSize = 18;
        [BindProperty(SupportsGet = true)]
        public string query { get; set; }
        public IEnumerable<Models.User> Users;
        public IndexModel(IRepository<Models.User> repository)
        {
            _repository = repository;
        }
        public void OnGet()
        {
          
            Users = _repository.GetDbSet();
            if (!query.IsNullOrEmpty()) Users = Users.Where(u => u.Name.Contains(query) || u.Email.Contains(query) || u.IdentifyNumber.Contains(query) || u.Phone.Contains(query));
            TotalPage = Constant.GetTotalPage(Users.Count(), PageSize);
            Users = Users.Skip(Constant.GetStartIndexPage(PageIndex, PageSize)).Take(PageSize);
        }
    }
}
