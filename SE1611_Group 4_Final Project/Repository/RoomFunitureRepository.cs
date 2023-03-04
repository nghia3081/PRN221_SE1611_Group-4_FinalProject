using SE1611_Group_4_Final_Project.Models;
using SE1611_Group_4_Final_Project.Repository.Interfaces;

namespace SE1611_Group_4_Final_Project.Repository
{
    public class RoomFunitureRepository : IRoomFunitureRepository
    {
        private readonly MotelManagementContext context = new MotelManagementContext();

        public async Task Add(RoomFurniture roomFurniture)
        {
            context.RoomFurnitures.Add(roomFurniture);
            context.SaveChanges();
        }

        public async Task Update(RoomFurniture roomFurniture)
        {
            context.RoomFurnitures.Find(roomFurniture.Id.GetType());
            context.RoomFurnitures.Update(roomFurniture);
            context.SaveChanges();
        }

        public async Task Delete(RoomFurniture roomFurniture)
        {
            context.RoomFurnitures.Find(roomFurniture.Id.GetType());
            context.RoomFurnitures.Remove(roomFurniture);
            context.SaveChanges();
        }
    }
}
