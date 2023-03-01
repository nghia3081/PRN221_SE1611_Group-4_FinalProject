using SE1611_Group_4_Final_Project.Utils.AppSetting.AppSetting;
using System.Reflection;
namespace SE1611_Group_4_Final_Project.Utils.AppSetting
{
    public class ApplicationSetting
    {
        private readonly IConfigurationRoot _configurationRoot;
        [Config(propertyType: ConfigAttribute.PropertyType.ConnectionStrings, "MotelManagementConnection")]
        public string ConnectionString { get; private set; }
        public static ApplicationSetting Instance
        {
            get
            {
                return instance;
            }
        }
        private ApplicationSetting()
        {
            _configurationRoot = new ConfigurationManager().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", false, true).Build();
            LoadConfig();
        }
        private static readonly ApplicationSetting instance = new();
        private void LoadConfig()
        {
            bool throwIfNotExist = true;
            try
            {
                var configProperties = typeof(ApplicationSetting).GetProperties();
                foreach (var property in configProperties)
                {
                    var attr = property.GetCustomAttribute<ConfigAttribute>();
                    if (attr == null) continue;
                    var configKey = attr.Key;
                    var configSection = attr.Section;
                    throwIfNotExist = attr.ThrowExceptionIfSourceNotExist;
                    var propertyType = property.PropertyType;
                    property.SetValue(this, _configurationRoot.GetSection(configSection).GetValue(propertyType, configKey));
                }
            }
            catch (Exception)
            {
                if (throwIfNotExist) throw;
            }

        }
    }
}
