using SE1611_Group_4_Final_Project.Models;

namespace SE1611_Group_4_Final_Project.Utils
{
    public class Constant
    {
        public const string forgotTokenSessionKey = "forgotToken";
        public const int expireForgotTokenMinute = 15;
        public static int GetStartIndexPage(int page, int pageSize)
        {
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
            Created = 1,
            Approved,
            UserConfirmedPaid,
            ManagerConfirmedPaid
        }
        public enum InvoiceType
        {
            Living = 1,
            Electricity,
            Water,
            Internet,
        }
        public enum RoomFurnitureStatus
        {
            Normal = 1,
            CrashReported,
            Crashed,
        }
        public string Content { get;set; }
    }
}
