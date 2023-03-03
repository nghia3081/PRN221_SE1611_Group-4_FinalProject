using SE1611_Group_4_Final_Project.Models;

namespace SE1611_Group_4_Final_Project.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task Add(User user);
        Task Update(User user);
        Task Delete(User user);
    }
}
