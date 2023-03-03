using SE1611_Group_4_Final_Project.Models;
using SE1611_Group_4_Final_Project.Repository.Interfaces;

namespace SE1611_Group_4_Final_Project.Repository
{
    public class RoomRepository : IRoomRepository
    {
        private readonly MotelManagementContext context = new MotelManagementContext();

        public async Task Add(Room room)
        {
            context.Rooms.Add(room);
            context.SaveChanges();
        }

        public async Task Update(Room room)
        {
            context.Rooms.Find(room.Id.GetType());
            context.Rooms.Update(room);
            context.SaveChanges();
        }

        public async Task Delete(Room room)
        {
            context.Rooms.Find(room.Id.GetType());
            context.Rooms.Remove(room);
            context.SaveChanges();
        }
    }
}
