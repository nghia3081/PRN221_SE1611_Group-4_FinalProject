using SE1611_Group_4_Final_Project.Models;

namespace SE1611_Group_4_Final_Project.Repository.Interfaces
{
    public interface IRoomFunitureRepository
    {
        Task Add(RoomFurniture roomFuniture);
        Task Update(RoomFurniture roomFuniture);
        Task Delete(RoomFurniture roomFuniture);
    }
}
