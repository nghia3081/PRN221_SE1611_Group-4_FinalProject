using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult OnGet()
        {
            if (!Constant.IsAdmin(HttpContext)) return RedirectToPage("/Index");
            TotalPage = Constant.GetTotalPage(_repository.GetDbSet().Count(), PageSize);
            Invoices = _repository.GetDbSet().Include(i => i.Rooms).Include(i => i.User);
            if (!query.IsNullOrEmpty()) Invoices = Invoices.Where(i => i.Title.Contains(query) || i.Description.Contains(query) || i.GrandTotal.ToString().Contains(query));
            Invoices = Invoices.OrderByDescending(i => i.CreatedDate);
            Invoices = Invoices.Skip(Constant.GetStartIndexPage(PageIndex, PageSize)).Take(PageSize);
            return Page();
        }
        public IActionResult OnGetAccept(Guid id)
        {
            if (!Constant.IsAdmin(HttpContext)) return RedirectToPage("/Index");
            var invoice = _repository.GetDbSet().Include(i => i.Rooms).FirstOrDefault(i => i.Id == id);
            invoice.Status = (int)Constant.InvoiceStatus.Accepted;
            foreach (var r in invoice.Rooms)
            {
                r.IsAvailable = false;
            }
            _repository.GetDbSet<Models.Room>().UpdateRange(invoice.Rooms);
            _repository.GetDbSet().Update(invoice).Context.SaveChanges();
            
            return OnGet();
        }
        public IActionResult OnGetReject(Guid id)
        {
            if (!Constant.IsAdmin(HttpContext)) return RedirectToPage("/Index");

            var invoice = _repository.GetDbSet().Include(i => i.Rooms).FirstOrDefault(i => i.Id == id);
            invoice.Status = (int)Constant.InvoiceStatus.Rejected;
            foreach (var r in invoice.Rooms)
            {
                r.IsAvailable = true;
            }
            _repository.GetDbSet<Models.Room>().UpdateRange(invoice.Rooms);
            _repository.GetDbSet().Update(invoice).Context.SaveChanges();
            OnGet();
            return OnGet();
        }
    }
}
