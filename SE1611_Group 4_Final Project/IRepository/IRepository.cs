using Microsoft.EntityFrameworkCore;
using SE1611_Group_4_Final_Project.Models;
using System.Linq.Expressions;

namespace SE1611_Group_4_Final_Project.IRepository
{
    public interface IRepository<T> where T : Entity
    {
        public MotelManagementContext GetContext();
        public T Find(params object?[]? objects);
        public Task Add(T entity);
        public Task Update(T entity);
        public Task Delete(T entity);
    }
}
