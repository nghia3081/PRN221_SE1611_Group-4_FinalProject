using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Utils;

namespace SE1611_Group_4_Final_Project.Pages.Room
{
    public class IndexModel : PageModel
    {
        private readonly IRepository<Models.Room> _repository;
        public int TotalPage { get; private set; }
        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool? IsAvailable { get; set; }
        public bool HasPreviousPage => (PageIndex > 1);
        public bool HasNextPage => (PageIndex < TotalPage);

        private const int PageSize = 18;
        [BindProperty(SupportsGet = true)]
        public string query { get; set; }
        public IEnumerable<Models.Room> Rooms;
        public IndexModel(IRepository<Models.Room> repository)
        {
            _repository = repository;
        }
        public async Task OnGet()
        {
            Rooms = _repository.GetDbSet();
            if (IsAvailable.HasValue)
            {
                Rooms = Rooms.Where(r => r.IsAvailable == IsAvailable);
            }

            if (!query.IsNullOrEmpty()) Rooms = Rooms.Where(r => r.Name.Contains(query) || r.Address.Contains(query) || r.Area.ToString().Contains(query));

            TotalPage = Constant.GetTotalPage(Rooms.Count(), PageSize);
            Rooms = Rooms.Skip(Constant.GetStartIndexPage(PageIndex, PageSize)).Take(PageSize);

        }
    }
}
