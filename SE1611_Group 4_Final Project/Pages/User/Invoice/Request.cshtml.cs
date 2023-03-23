using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SE1611_Group_4_Final_Project.IRepository;

namespace SE1611_Group_4_Final_Project.Pages.User.Invoice
{
    public class RequestModel : PageModel
    {
        public readonly ILogger<RequestModel> _logger;
        private readonly IRepository<Models.Invoice> _invoiceRepository;
        public List<Models.Invoice> invoices { get; set; }
        public Models.User user { get; set; }
        public RequestModel(ILogger<RequestModel> logger, IRepository<Models.Invoice> InvoiceRepository)
        {
            _invoiceRepository = InvoiceRepository;
            _logger = logger;
        }
        public void OnGet()
        {
            string jsonUser = HttpContext.Session.GetString("User");
            if (!string.IsNullOrEmpty(jsonUser))
            {
                user = JsonConvert.DeserializeObject<Models.User>(jsonUser);
            }
            invoices = _invoiceRepository.FindwithQuery(x => x.GrandTotal == 0 && x.UserId == user.Id).ToList();
        }
    }
}
