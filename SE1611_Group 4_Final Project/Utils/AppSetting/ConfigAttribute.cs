namespace SE1611_Group_4_Final_Project.Utils.AppSetting.AppSetting
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class ConfigAttribute : Attribute
    {
        public string Key { get; private set; }
        public object DefaultValue { get; private set; }
        public string Section { get; private set; }
        public bool ThrowExceptionIfSourceNotExist { get; private set; }
        public ConfigAttribute(string key)
        {
            Key = key;
            Section = "AppSettings";
            DefaultValue = string.Empty;
            ThrowExceptionIfSourceNotExist = true;
        }
        public ConfigAttribute(AttributeSection section, string key)
        {
            this.Section = section.ToString();
            this.Key = key;
            this.DefaultValue = string.Empty;
            this.ThrowExceptionIfSourceNotExist = true;
        }

        public ConfigAttribute(AttributeSection propertyType, string key, object defaultValue, bool throwExceptionIfSourceNotExist)
        {
            this.Key = key;
            this.DefaultValue = defaultValue;
            this.Section = propertyType.ToString();
            this.ThrowExceptionIfSourceNotExist = throwExceptionIfSourceNotExist;
        }
        public enum AttributeSection
        {
            ConnectionStrings,
            AppSettings,
            SendGrid
        }
    }
}
