using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Utils;

namespace SE1611_Group_4_Final_Project.Pages.RoomFurniture
{
    public class IndexModel : PageModel
    {
        private readonly IRepository<Models.RoomFurniture> _repository;
        public IEnumerable<Models.RoomFurniture> RoomFurnitures { get; set; }
        public IndexModel(IRepository<Models.RoomFurniture> repository)
        {
            _repository = repository;
        }
        public void OnGet(string? query, int? skip = 0, int? take = 18)
        {
            RoomFurnitures = _repository.GetDbSet();
            if (!query.IsNullOrEmpty()) RoomFurnitures.Where(rf => rf.Name.Contains(query));
            RoomFurnitures = RoomFurnitures.Skip(skip.Value).Take(take.Value);
        }
        public IActionResult OnGetCrashedConfirm(Guid id)
        {
            var rf = _repository.Find(id);
            rf.Status = (int)Constant.RoomFurnitureStatus.Crashed;
            _repository.Update(rf);
             OnGet("");
            return RedirectToPage("/Index");
        }
        public IActionResult OnGetReplaced(Guid id)
        {
            var rf = _repository.Find(id);
            rf.Status = (int)Constant.RoomFurnitureStatus.Normal;
            _repository.Update(rf);
            OnGet("");
            return RedirectToPage("/Index");
        }

    }
}
