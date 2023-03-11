using SE1611_Group_4_Final_Project.Models;

namespace SE1611_Group_4_Final_Project.Repository
{
    public class UserRepository : Repository<User>
    {
        public MotelManagementContext _motelManagementContext;
        public UserRepository(MotelManagementContext motelManagementContext) : base(motelManagementContext)
        {
            _motelManagementContext = motelManagementContext;
        }
        public string GeneratePasswordResetToken(User user)
        {
            var token = Guid.NewGuid().ToString();
            _motelManagementContext.Entry<User>(user).Entity.IdentifyNumber = token;
            _motelManagementContext.Users.Update(user);
            _motelManagementContext.SaveChanges();

            return token;
        }
    }
}
