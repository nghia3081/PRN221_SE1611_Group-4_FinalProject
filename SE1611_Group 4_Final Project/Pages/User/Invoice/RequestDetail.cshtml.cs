using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Models;

namespace SE1611_Group_4_Final_Project.Pages.User.Invoice
{
    public class RequestDetailModel : PageModel
    {
        public readonly ILogger<RequestDetailModel> _logger;
        private readonly IRepository<Models.Invoice> _invoiceRepository;
        public Models.Invoice invoice { get; set; }
        public RequestDetailModel(ILogger<RequestDetailModel> logger, IRepository<Models.Invoice> InvoiceRepository)
        {
            _invoiceRepository = InvoiceRepository;
            _logger = logger;
        }
        public void OnGet(string id)
        {
            invoice = _invoiceRepository.Find(Guid.Parse(id));
        }
    }
}
