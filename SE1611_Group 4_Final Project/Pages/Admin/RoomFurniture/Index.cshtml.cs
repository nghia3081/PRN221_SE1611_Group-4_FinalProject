using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Utils;

namespace SE1611_Group_4_Final_Project.Pages.RoomFurniture
{
    public class IndexModel : PageModel
    {
        private readonly IRepository<Models.RoomFurniture> _repository;
        public IEnumerable<Models.RoomFurniture> RoomFurnitures { get; set; }
        public int TotalPage { get; private set; }
        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; }
        public bool HasPreviousPage => (PageIndex > 1);
        public bool HasNextPage => (PageIndex < TotalPage);

        public Dictionary<int, string> listStatus { get; set; }

        private const int PageSize = 18;
        [BindProperty(SupportsGet = true)]
        public string query { get; set; }
        public IndexModel(IRepository<Models.RoomFurniture> repository)
        {
            _repository = repository;
            listStatus  = Enum.GetValues(typeof(Constant.RoomFurnitureStatus)).Cast<Constant.RoomFurnitureStatus>().ToDictionary(t => (int)t, t => t.ToString());
        }
        public void OnGet()
        {
            RoomFurnitures = _repository.GetDbSet().Include(r => r.Room);
            if (!query.IsNullOrEmpty()) RoomFurnitures.Where(rf => rf.Name.Contains(query));
            TotalPage = Constant.GetTotalPage(RoomFurnitures.Count(), PageSize);
            RoomFurnitures = RoomFurnitures.Skip(Constant.GetStartIndexPage(PageIndex, PageSize)).Take(PageSize);
        }
        public IActionResult OnGetCrashedConfirm(Guid id)
        {
            var rf = _repository.Find(id);
            rf.Status = (int)Constant.RoomFurnitureStatus.Crashed;
            _repository.Update(rf);
             OnGet();
            return RedirectToPage("/Admin/RoomFurniture/Index");
        }
        public IActionResult OnGetReplaced(Guid id)
        {
            var rf = _repository.Find(id);
            rf.Status = (int)Constant.RoomFurnitureStatus.Normal;
            _repository.Update(rf);
            OnGet();
            return RedirectToPage("/Admin/RoomFurniture/Index");
        }

    }
}
