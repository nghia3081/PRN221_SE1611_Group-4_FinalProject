using Microsoft.EntityFrameworkCore;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Models;
using System.Linq.Expressions;

namespace SE1611_Group_4_Final_Project.Repository
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private MotelManagementContext _motelManagementContext;
        public Repository(MotelManagementContext motelManagementContext)
        {
            _motelManagementContext = motelManagementContext;
        }
        public MotelManagementContext GetContext() => _motelManagementContext;
        public T Find(params object?[]? key)
        {
            var findResult = _motelManagementContext.Find<T>(key);
            return findResult ?? throw new NullReferenceException("Record not found");
        }
        public async Task Add(T entity)
        {
            _motelManagementContext.Entry<T>(entity).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            await _motelManagementContext.SaveChangesAsync();
        }
        public async Task Update(T entity)
        {
            if (_motelManagementContext.Entry<T>(entity) == null) throw new NullReferenceException("Record not found");
            _motelManagementContext.Entry<T>(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _motelManagementContext.SaveChangesAsync();
        }
        public async Task Delete(T entity)
        {
            if (_motelManagementContext.Entry<T>(entity) == null) throw new NullReferenceException("Record not found");
            _motelManagementContext.Entry<T>(entity).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            await _motelManagementContext.SaveChangesAsync();
        }
    }
}
