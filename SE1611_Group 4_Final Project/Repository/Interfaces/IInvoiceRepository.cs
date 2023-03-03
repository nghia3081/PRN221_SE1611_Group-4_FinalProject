using SE1611_Group_4_Final_Project.Models;

namespace SE1611_Group_4_Final_Project.Repository.Interfaces
{
    public interface IInvoiceRepository
    {
        Task Add(Invoice invoice);
        Task Update(Invoice invoice);
        Task Delete(Invoice invoice);
    }
}
