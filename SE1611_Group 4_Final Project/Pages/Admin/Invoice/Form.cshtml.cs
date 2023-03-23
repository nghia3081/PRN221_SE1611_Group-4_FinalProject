using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SE1611_Group_4_Final_Project.IRepository;

namespace SE1611_Group_4_Final_Project.Pages.Invoice
{
    public class FormModel : PageModel
    {
        private readonly IRepository<Models.Invoice> _repository;
        [BindProperty]
        public Models.Invoice Invoice { get; set; }
        public MultiSelectList RoomSelectList { get; set; }
        [BindProperty]
        public List<Guid> SelectedRoom { get; set; }
        public FormModel(IRepository<Models.Invoice> repository)
        {
            _repository = repository;
        }
        public void OnGet(Guid? id)
        {
            if (id.HasValue) Invoice = _repository.Find(id);
            var roomDbSet = _repository.GetDbSet<Models.Room>();
            RoomSelectList = new MultiSelectList(roomDbSet.OrderBy(r => r.Name), "Id", "Name", Invoice?.Rooms.Select(r => r.Id));
        }
        public IActionResult OnPost()
        {
            Invoice.Rooms = _repository.GetDbSet<Models.Room>().Where(room => SelectedRoom.Contains(room.Id)).ToList();
            if (Invoice.Id == Guid.Empty)
            {
                Invoice.Id = Guid.NewGuid();
                _repository.Add(Invoice);
            }
            else
            {
                var currentInvoice = _repository.GetDbSet().Include(i => i.Rooms).FirstOrDefault(i => i.Id == Invoice.Id);
                currentInvoice.Rooms.Clear();
                currentInvoice.Rooms = Invoice.Rooms;
                _repository.Update(currentInvoice);
            }
            return RedirectToPage("/Admin/Invoice/Index");
        }
    }
}
