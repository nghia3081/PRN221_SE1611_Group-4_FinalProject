using Microsoft.AspNetCore.Mvc.RazorPages;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Utils;

namespace SE1611_Group_4_Final_Project.Pages.Room
{
    public class IndexModel : PageModel
    {
        private readonly IRepository<Models.Room> _repository;
        public int TotalPage { get; set; }
        private const int PageSize = 18;
        public IEnumerable<Models.Room> Rooms;
        public IndexModel(IRepository<Models.Room> repository)
        {
            _repository = repository;
        }
        public async Task OnGet(string query, int? page = 0)
        {
            TotalPage = Constant.GetTotalPage(_repository.GetDbSet().Count(), PageSize);
            Rooms = _repository.GetDbSet();
            if (!query.IsNullOrEmpty()) Rooms = Rooms.Where(r => r.Name.Contains(query) || r.Address.Contains(query) || r.Area.ToString().Contains(query));
            Rooms = Rooms.Skip(Constant.GetStartIndexPage(page.Value, PageSize)).Take(PageSize);
        }
    }
}
