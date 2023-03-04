using Microsoft.AspNetCore.DataProtection.KeyManagement;
using SE1611_Group_4_Final_Project.Models;
using SE1611_Group_4_Final_Project.Repository;
using SE1611_Group_4_Final_Project.Repository.Interfaces;

namespace SE1611_Group_4_Final_Project.Utils
{
    public static class Extension
    {
        public static object ConvertDataToType(this string valueStr, Type propertyType)
        {
            object value;
            if (valueStr != null && typeof(string) != propertyType)
            {
                Type underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
                value = Convert.ChangeType(valueStr, underlyingType);
            }
            else
            {
                value = valueStr;
            }

            return value;
        }
        public static bool IsNullOrWhiteSpace(this string value) => string.IsNullOrWhiteSpace(value);
        public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);


        public static IServiceCollection RegisterMyServices(this IServiceCollection services) => services
                    .AddTransient<IInvoiceRepository, InvoiceRepository>()
                    .AddTransient<INotificationRepository, NotificationRepository>()
                    .AddTransient<IRoomFunitureRepository, RoomFunitureRepository>()
                    .AddTransient<IRoomRepository, RoomRepository>()
                    .AddTransient<IUserRepository, UserRepository>()
                    .AddTransient <MotelManagementContext>()
            ;
    }
}
