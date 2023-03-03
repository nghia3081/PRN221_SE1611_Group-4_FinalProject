using SE1611_Group_4_Final_Project.Models;
using SE1611_Group_4_Final_Project.Repository.Interfaces;

namespace SE1611_Group_4_Final_Project.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly MotelManagementContext context = new MotelManagementContext();

        public async Task Add(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
        }

        public async Task Update(User user)
        {
            context.Users.Find(user.Id.GetType());
            context.Users.Update(user);
            context.SaveChanges();
        }

        public async Task Delete(User user)
        {
            context.Users.Find(user.Id.GetType());
            context.Users.Remove(user);
            context.SaveChanges();
        }
    }
}
