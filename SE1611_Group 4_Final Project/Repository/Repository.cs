using Microsoft.EntityFrameworkCore;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Models;

namespace SE1611_Group_4_Final_Project.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private MotelManagementContext _motelManagementContext;
        private DbSet<T> _entities;
        public Repository(MotelManagementContext motelManagementContext)
        {
            _motelManagementContext = motelManagementContext;
            _entities = _motelManagementContext.Set<T>();
        }
        public DbSet<T> GetDbSet() => _entities;
        public T Find(params object?[]? key)
        {
            var findResult = _entities.Find(key);
            return findResult ?? throw new NullReferenceException("Record not found");
        }
        public async Task<IEnumerable<T>> GetAll()
        {
            var Results = await _entities.ToListAsync();
            return Results;
        }
        public async Task Add(T entity)
        {
            _entities.Add(entity);
            await _motelManagementContext.SaveChangesAsync();
        }
        public async Task Update(T entity)
        {
            if (_motelManagementContext.Entry<T>(entity) == null) throw new NullReferenceException("Record not found");
            _entities.Update(entity);
            await _motelManagementContext.SaveChangesAsync();
        }
        public async Task Delete(T entity)
        {
            if (_motelManagementContext.Entry<T>(entity) == null) throw new NullReferenceException("Record not found");
            _entities.Remove(entity);
            await _motelManagementContext.SaveChangesAsync();
        }
        public string GeneratePasswordResetToken(User user)
        {
            if (typeof(T) != typeof(User)) throw new Exception("Only for user");
            User u = _entities.Find(user.Id) as User;
            if (u is null) throw new Exception("Not found user");
            string token = u.Id.ToString();
            token += $":{DateTime.Now.Ticks}:0";
            return token;
        }
        public User FindUserByEmail(string email)
        {
            return _motelManagementContext.Users.FirstOrDefault(x => x.Email == email);
        }

        public User FindUserByEmailandPassword(string email, string password)
        {
            return _motelManagementContext.Users.FirstOrDefault(x => x.Email == email && x.Password == password);
        }

    }
}