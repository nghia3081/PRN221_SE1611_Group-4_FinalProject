using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Utils;

namespace SE1611_Group_4_Final_Project.Pages.Invoice
{
    public class FormModel : PageModel
    {
        private readonly IRepository<Models.Invoice> _repository;
        [BindProperty]
        public Models.Invoice Invoice { get; set; }
        public SelectList InvoiceStatus { get; set; }
        public SelectList InvoiceType { get; set; }
        public MultiSelectList RoomSelectList { get; set; }
        [BindProperty]
        public List<Guid> SelectedRoom { get; set; }
        public Dictionary<int, string> listStatus { get; set; }
        public Dictionary<int, string> listType { get; set; }
        public FormModel(IRepository<Models.Invoice> repository)
        {
            _repository = repository;
        }
        public void OnGet(Guid? id)
        {
            var roomDbSet = _repository.GetDbSet<Models.Room>();

            listStatus = Enum.GetValues(typeof(Constant.InvoiceStatus)).Cast<Constant.InvoiceStatus>().ToDictionary(t => (int)t, t => t.ToString());
            listType = Enum.GetValues(typeof(Constant.InvoiceType)).Cast<Constant.InvoiceType>().ToDictionary(t => (int)t, t => t.ToString());
            InvoiceStatus = new SelectList(listStatus, "Key", "Value");
            InvoiceType = new SelectList(listType, "Key", "Value");
            RoomSelectList = new MultiSelectList(roomDbSet.OrderBy(r => r.Name), "Id", "Name");

            if (id.HasValue)
            {
                Invoice = _repository.Find(id);
                RoomSelectList = new MultiSelectList(roomDbSet.OrderBy(r => r.Name), "Id", "Name", Invoice?.Rooms.Select(r => r.Id));
                InvoiceStatus = new SelectList(listStatus, "Key", "Value", Invoice.Status);
                InvoiceType = new SelectList(listType, "Key", "Value", Invoice.Type);
            }
        }
        public IActionResult OnPost()
        {
            Invoice.Rooms = _repository.GetDbSet<Models.Room>().Where(room => SelectedRoom.Contains(room.Id)).ToList();
            if (Invoice.Id == Guid.Empty)
            {
                Invoice.Id = Guid.NewGuid();
                Invoice.CreatedDate = DateTime.Now;
                _repository.Add(Invoice);
            }
            else
            {
                var currentInvoice = _repository.GetDbSet().Include(i => i.Rooms).FirstOrDefault(i => i.Id == Invoice.Id);
                currentInvoice.Title = Invoice.Title;
                currentInvoice.Description = Invoice.Description;
                currentInvoice.From = Invoice.From;
                currentInvoice.To = Invoice.To;
                currentInvoice.Type = Invoice.Type;
                currentInvoice.UserId = JsonConvert.DeserializeObject<Models.User>(HttpContext.Session.GetString("User")).Id;
                currentInvoice.GrandTotal = Invoice.GrandTotal;
                currentInvoice.Rooms.Clear();
                currentInvoice.Status = Invoice.Status;
                currentInvoice.Type = Invoice.Type;
                currentInvoice.Rooms = Invoice.Rooms;
                _repository.Update(currentInvoice);
            }
            return RedirectToPage("/Admin/Invoice/Index");
        }
    }
}
