using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Utils;

namespace SE1611_Group_4_Final_Project.Pages.Invoice
{
    public class IndexModel : PageModel
    {
        private readonly IRepository<Models.Invoice> _repository;
        public int TotalPage { get; set; }
        private const int PageSize = 18;
        public IEnumerable<Models.Invoice> Invoices { get; set; }
        public IndexModel(IRepository<Models.Invoice> repository)
        {
            _repository = repository;
        }
        public IActionResult OnGet(string query, int? page = 0)
        {
            TotalPage = Constant.GetTotalPage(_repository.GetDbSet().Count(), PageSize);
            Invoices = _repository.GetDbSet();
            if (!query.IsNullOrEmpty()) Invoices = Invoices.Where(i => i.Title.Contains(query) || i.Description.Contains(query) || i.GrandTotal.ToString().Contains(query));
            Invoices = Invoices.Skip(Constant.GetStartIndexPage(page.Value, PageSize)).Take(PageSize);
            return Page();
        }
        public IActionResult OnGetConfirmBooking(Guid id)
        {
            var invoice = _repository.Find(id);
            invoice.Status = (int)Constant.InvoiceStatus.Approved;
            _repository.Update(invoice);
            return OnGet("");
        }
        public IActionResult OnGetConfirmPaid(Guid id)
        {
            var invoice = _repository.Find(id);
            _repository.Update(invoice);
            return OnGet("");
        }
    }
}
