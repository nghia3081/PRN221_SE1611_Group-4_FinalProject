using SE1611_Group_4_Final_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace SE1611_Group_4_Final_Project.IRepository
{
    public interface IRepository<T> where T : class
    {
        public DbSet<T> GetDbSet();
        public DbSet<Y> GetDbSet<Y>() where Y : class;
        public Task<IEnumerable<T>> GetAll();
        public T Find(params object?[]? objects);
        public Task Add(T entity);
        public Task Update(T entity);
        public Task Delete(T entity);
        public Task Delete(params object?[]? key);
        public String GeneratePasswordResetToken(User user);
        public User FindUserByEmail(string email);
        public User FindUserByEmailandPassword(string email, string password);
    }
}


