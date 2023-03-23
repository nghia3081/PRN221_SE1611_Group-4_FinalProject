using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Utils;

namespace SE1611_Group_4_Final_Project.Pages.Admin.Invoice
{
    public class BookingInvoiceModel : PageModel
    {
        private readonly IRepository<Models.Invoice> _repos;
        public IEnumerable<Models.Invoice> Invoices { get; set; }
        [BindProperty]
        public string query { get; set; }

        public void OnGet()
        {
            Invoices = _repos.GetDbSet().Include(i => i.User).Include(i => i.Rooms).Where(i => i.Status == (int)Constant.InvoiceStatus.UnPaid || i.Status == (int)Constant.InvoiceStatus.Paid);
            if (!query.IsNullOrEmpty()) Invoices = Invoices.Where(i => i.User.Name.Contains(query) || i.Title.Contains(query));
        }
        public void OnGetRequestPaid(Guid id)
        {
            var invoice = _repos.Find(id);
            invoice.Status = (int)Constant.InvoiceStatus.RequirePaid;
            _repos.Update(invoice);
            OnGet();
        }
        public void OnGetConfirmPaid(Guid id)
        {
            var invoice = _repos.Find(id);
            invoice.Status = (int)Constant.InvoiceStatus.Approved;
            _repos.Update(invoice);
            OnGet();
        }
    }
}
