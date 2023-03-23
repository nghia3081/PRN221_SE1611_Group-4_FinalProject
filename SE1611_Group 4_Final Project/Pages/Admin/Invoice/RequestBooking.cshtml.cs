using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Utils;

namespace SE1611_Group_4_Final_Project.Pages.Admin.Invoice
{
    public class RequestBookingModel : PageModel
    {
        private readonly IRepository<Models.Invoice> repository;
        public IEnumerable<Models.Invoice> Invoices { get; set; }
        [BindProperty]
        public string query { get; set; }
        public RequestBookingModel(IRepository<Models.Invoice> repository)
        {
            this.repository = repository;
        }
        public void OnGet()
        {
            Invoices = repository.GetDbSet().Include(i => i.User).Include(i => i.Rooms).Where(i => i.Status == (int)Constant.InvoiceStatus.CheckOut || i.Status == (int)Constant.InvoiceStatus.Processing);
            if (!query.IsNullOrEmpty()) Invoices = Invoices.Where(i => i.User.Name.Contains(query) || i.Title.Contains(query));
        }
        public void OnGetDone(Guid Id)
        {
            var inv = repository.Find(Id);
            inv.Status = (int)Constant.InvoiceStatus.Done;
            repository.Update(inv);
            OnGet();
        }
    }
}
