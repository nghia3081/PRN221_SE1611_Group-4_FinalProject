using SE1611_Group_4_Final_Project.Models;
using SE1611_Group_4_Final_Project.Repository.Interfaces;

namespace SE1611_Group_4_Final_Project.Repository
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly MotelManagementContext context = new MotelManagementContext();
        public async Task Add(Invoice invoice)
        {
            context.Invoices.Add(invoice);
            context.SaveChanges();
        }

        public async Task Update(Invoice invoice)
        {
            context.Invoices.Find(invoice.Id.GetType());
            context.Invoices.Update(invoice);
            context.SaveChanges();
        }

        public async Task Delete(Invoice invoice)
        {
            context.Invoices.Find(invoice.Id.GetType());
            context.Invoices.Remove(invoice);
            context.SaveChanges();
        }
    }
}
