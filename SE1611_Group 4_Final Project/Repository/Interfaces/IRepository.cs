using SE1611_Group_4_Final_Project.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using SendGrid.Helpers.Mail;
using MimeKit.Encodings;

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
        public IEnumerable<T> FindwithQuery(Expression<Func<T, bool>> predicate);
        public IEnumerable<TResult> SelectField<TResult>(Expression<Func<T, TResult>> selector);
        public List<Room> FilterRooms(int minPrice, int maxPrice, int minArea, int maxArea, string address, int pageIndex, int pageSize);
        public List<Invoice> FilterInvoices(int month, int year);
        public List<Room> GetRoomsbyInvoice(Guid id);
        public Invoice GetInvoice(Guid id);
	}
}