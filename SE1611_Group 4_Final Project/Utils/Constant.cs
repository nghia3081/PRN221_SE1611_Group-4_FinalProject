using Newtonsoft.Json;
using SE1611_Group_4_Final_Project.Models;

namespace SE1611_Group_4_Final_Project.Utils
{
    public class Constant
    {
        public const string forgotTokenSessionKey = "forgotToken";
        public const int expireForgotTokenMinute = 15;
        public static int GetStartIndexPage(int page, int pageSize)
        {
            page = page == 0 ? 0 : page - 1;
            return page * pageSize;
        }
        public static int GetTotalPage(int totalRow, int pageSize)
        {
            var remain = totalRow % pageSize;
            var divide = totalRow / pageSize;
            return remain != 0 ? divide : divide + 1;
        }
        public enum InvoiceStatus
        {
            Booked = 1,
            Waiting,
            Accepted,
            Rejected,
        }
        public enum InvoiceType
        {
            Living = 1,
            Electricity,
            Water,
            Internet,
            Other
        }
        public enum RoomFurnitureStatus
        {
            Normal = 1,
            CrashReported,
            Crashed,
        }
        public string Content { get; set; }
        public static bool IsAdmin(HttpContext context)
        {
            if (!context.Session.Keys.Contains("User")) return false;
            string userJson = context.Session.GetString("User");
            var user = JsonConvert.DeserializeObject<User>(userJson);
            if (user == null) return false;
            return user.IsAdmin ?? false;
        }
    }
}
