using Microsoft.EntityFrameworkCore;
using SE1611_Group_4_Final_Project.IRepository;
using SE1611_Group_4_Final_Project.Models;
using SE1611_Group_4_Final_Project.Utils;
using SendGrid.Helpers.Mail;
using System.Data;
using System.Linq.Expressions;

namespace SE1611_Group_4_Final_Project.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly MotelManagementContext _motelManagementContext;
        private readonly DbSet<T> _entities;
        public Repository(MotelManagementContext motelManagementContext)
        {
            _motelManagementContext = motelManagementContext;
            _entities = _motelManagementContext.Set<T>();
        }
        public DbSet<T> GetDbSet() => _entities;
        public DbSet<Y> GetDbSet<Y>() where Y : class
        {
            return _motelManagementContext.Set<Y>();
        }
        public T Find(params object?[]? key)
        {
            var findResult = _entities.Find(key);
            return findResult ?? throw new NullReferenceException("Record not found");
        }
        public virtual IEnumerable<T> FindwithQuery(Expression<Func<T, bool>> predicate)
        {
            return _entities.Where(predicate);
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
        public async Task Delete(params object?[]? key)
        {
            var foundRecord = Find(key);
            _entities.Remove(foundRecord);
            await _motelManagementContext.SaveChangesAsync();

        }
        public string GeneratePasswordResetToken(User user)
        {
            //if (typeof(T) != typeof(User)) throw new Exception("Only for user");
            User u = _entities.Find(user.Id) as User ?? throw new Exception("Not found user");
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
        public IEnumerable<TResult> SelectField<TResult>(Expression<Func<T, TResult>> selector)
        {
            return _entities
                .Select(selector)
                .ToList();
        }
        public List<Room> FilterRooms(int minPrice, int maxPrice, int minArea, int maxArea, string address, int pageIndex, int pageSize)
        {
            var result = _motelManagementContext.Rooms.AsQueryable();

            if (minPrice != -1 && maxPrice != -1)
            {
                result = result.Where(r => r.Price >= minPrice && r.Price <= maxPrice);
            }

            if (minArea != -1 && maxArea != -1)
            {
                result = result.Where(r => r.Area >= minArea && r.Area <= maxArea);
            }

            bool includeAddress = !string.IsNullOrEmpty(address) && address != "All";
            if (includeAddress)
            {
                result = result.Where(r => r.Address.Contains(address));
            }

            return result.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public List<Invoice> FilterRoomInvoices(int month, int year, Guid userID)
        {
            var result = _motelManagementContext.Invoices.Include(i => i.Rooms).Where(x => x.UserId == userID
            && x.Type == (int)Constant.InvoiceType.Living
            && x.Status == (int)Constant.InvoiceStatus.Approved
            && x.GrandTotal != 0).AsQueryable();

            if (month != -1)
            {
                result = result.Where(i => i.CreatedDate.Month == month);
            }

            if (year != -1)
            {
                result = result.Where(i => i.CreatedDate.Year == year);
            }

            return result.ToList();
        }
        public List<Invoice> FilterServiceInvoices(int month, int year, Guid userID)
        {
            var result = _motelManagementContext.Invoices.Include(i => i.Rooms).Where(x => x.UserId == userID 
            && x.Type == (int)Constant.InvoiceType.Electricity
            || x.Type == (int)Constant.InvoiceType.Water 
            || x.Type == (int)Constant.InvoiceType.Internet 
            && x.Status == (int)Constant.InvoiceStatus.Approved 
            && x.GrandTotal != 0).AsQueryable();

            if (month != -1)
            {
                result = result.Where(i => i.CreatedDate.Month == month);
            }

            if (year != -1)
            {
                result = result.Where(i => i.CreatedDate.Year == year);
            }

            return result.ToList();
        }
        public List<Invoice> FilterConfirmInvoices(int month, int year, Guid userID)
        {
            var result = _motelManagementContext.Invoices.Include(i => i.Rooms).Where(x => x.UserId == userID
           && x.Status == (int)Constant.InvoiceStatus.RequirePaid
           && x.GrandTotal != 0).AsQueryable();

            if (month != -1)
            {
                result = result.Where(i => i.CreatedDate.Month == month);
            }

            if (year != -1)
            {
                result = result.Where(i => i.CreatedDate.Year == year);
            }

            return result.ToList();
        }
        public List<Room> GetRoomsbyInvoice(Guid id)
        {
            var roomIdList = new List<Guid>();
            using (var command = _motelManagementContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = $"SELECT RoomId FROM RoomInvoice WHERE InvoiceId = '{id.ToString()}'";
                command.CommandType = CommandType.Text;

                _motelManagementContext.Database.OpenConnection();

                using (var excute = command.ExecuteReader())
                {
                    while (excute.Read())
                    {
                        var roomId = excute.GetGuid(0);
                        roomIdList.Add(roomId);
                    }
                }
            }
            List<Room> result = new List<Room>();
            foreach (var room in roomIdList)
            {
                var rooms = _motelManagementContext.Rooms.Where(x => x.Id == room).FirstOrDefault();
                result.Add(rooms);
            }
            return result;
        }
        public Invoice GetInvoice(Guid id)
        {
            return _motelManagementContext.Invoices.Include(x => x.User).Include(x => x.Rooms).Where(x => x.Id == id).FirstOrDefault();
        }
        public void AddRoomInvoice(Guid roomId, Guid invoiceId)
        {
            var roomIdList = new List<Guid>();
            using (var command = _motelManagementContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = $"INSERT INTO RoomInvoice (RoomId,InvoiceId) VALUES ('{roomId.ToString()}', '{invoiceId.ToString()}');";
                command.CommandType = CommandType.Text;

                _motelManagementContext.Database.OpenConnectionAsync();

                command.ExecuteNonQueryAsync();
            }
        }
        public List<Room> GetRoomInvoice(List<Invoice> invoice)
        {
            var rooms = new List<Room>();
            foreach (var item in invoice)
            {
                var room = _motelManagementContext.Rooms.Where(x => x.Invoices.Contains(item)).ToList();
                foreach (var itemRoom in room)
                {
                    rooms.Add(itemRoom);
                }
            }
            return rooms.Distinct().ToList();
        }
    }
}