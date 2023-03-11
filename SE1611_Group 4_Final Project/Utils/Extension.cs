using SE1611_Group_4_Final_Project.IRepository;
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
        public static bool IsNullOrEmpty(this string valueStr) => string.IsNullOrEmpty(valueStr);

        public static IServiceCollection RegisterMyServices(this IServiceCollection services) => services
                    .AddTransient<MotelManagementContext>()
                    .AddScoped(typeof(IRepository<>), typeof(Repository<>))
                    .AddSingleton<IEmailSender, EmailSender>();


    }
}
