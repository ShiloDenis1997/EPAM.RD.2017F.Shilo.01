using System.Configuration;

namespace UserServiceConfigSection
{
    public class ServerElement : ConfigurationElement
    {
        [ConfigurationProperty("type", DefaultValue = "", IsKey = false, IsRequired = true)]
        [RegexStringValidator("(Slave)|(Master)")]
        public string Type
        {
            get { return (string)base["type"]; }
            set { base["type"] = value; }
        }

        [ConfigurationProperty("ipAddress", DefaultValue = "", IsRequired = false)]
        [RegexStringValidator("(2(5[0-5]|[0-4]\\d)|1\\d{2}|[1-9]\\d{0,1})(\\.(2(5[0-5]|[0-4]\\d)|1\\d{2}|[1-9]\\d|\\d)){3}")]
        public string IpAddress
        {
            get { return (string)base["ipAddress"]; }
            set { base["type"] = value; }
        }

        [ConfigurationProperty("port", DefaultValue = "", IsRequired = false)]
        [RegexStringValidator("^([0-9]{1,4}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]|6553[0-5])$")]
        public string Port
        {
            get { return (string)base["port"]; }
            set { base["port"] = value; }
        }

        [ConfigurationProperty("domain", DefaultValue = "UserServiceDomain", IsRequired = false)]
        [StringValidator(MinLength = 1)]
        public string DomainName
        {
            get { return (string)base["domain"]; }
            set { base["domain"] = value; }
        }
    }
}