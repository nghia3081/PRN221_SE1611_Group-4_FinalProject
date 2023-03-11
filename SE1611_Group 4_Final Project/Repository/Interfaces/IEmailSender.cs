
namespace SE1611_Group_4_Final_Project.Repository.Interfaces
{
    public interface IEmailSender
    {
       Task SendEmailAsync(string email, string subject, string message);
    }
}
