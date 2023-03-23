using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Utils;

namespace SE1611_Group_4_Final_Project.Pages.Invoice
{
    public class IndexModel : PageModel
    {
        private readonly IRepository<Models.Invoice> _repository;
        public int TotalPage { get; private set; }
        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; }
        public bool HasPreviousPage => (PageIndex > 1);
        public bool HasNextPage => (PageIndex < TotalPage);

        private const int PageSize = 18;
        [BindProperty(SupportsGet = true)]
        public string query { get; set; }
        public IEnumerable<Models.Invoice> Invoices { get; set; }
        public IndexModel(IRepository<Models.Invoice> repository)
        {
            _repository = repository;
        }
        public void OnGet()
        {
            TotalPage = Constant.GetTotalPage(_repository.GetDbSet().Count(), PageSize);
            Invoices = _repository.GetDbSet();
            if (!query.IsNullOrEmpty()) Invoices = Invoices.Where(i => i.Title.Contains(query) || i.Description.Contains(query) || i.GrandTotal.ToString().Contains(query));
            Invoices = Invoices.Skip(Constant.GetStartIndexPage(PageIndex, PageSize)).Take(PageSize);
        }
        public IActionResult OnGetConfirmBooking(Guid id)
        {
            var invoice = _repository.Find(id);
            invoice.Status = (int)Constant.InvoiceStatus.Approved;
            _repository.Update(invoice);
            OnGet();
            return RedirectToPage("/Index");
        }
        public IActionResult OnGetConfirmPaid(Guid id)
        {
            var invoice = _repository.Find(id);
            invoice.Status = (int)Constant.InvoiceStatus.ManagerConfirmedPaid;
            _repository.Update(invoice);
            OnGet();
            return RedirectToPage("/Index");
        }
    }
}
