using SE1611_Group_4_Final_Project.Models;

namespace SE1611_Group_4_Final_Project.Repository.Interfaces
{
    public interface IRoomRepository
    {
        Task Add(Room room);
        Task Update(Room room);
        Task Delete(Room room);
    }
}
