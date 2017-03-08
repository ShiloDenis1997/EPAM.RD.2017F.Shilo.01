using System.Configuration;

namespace UserServiceConfigSection
{
    public class ServerElement : ConfigurationElement
    {
        [ConfigurationProperty("type", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Type
        {
            get { return (string)base["type"]; }
            set { base["type"] = value; }
        }

        [ConfigurationProperty("ipAddress", DefaultValue = "", IsRequired = false)]
        public string IpAddress
        {
            get { return (string)base["ipAddress"]; }
            set { base["type"] = value; }
        }

        [ConfigurationProperty("port", DefaultValue = "", IsRequired = false)]
        public string Port
        {
            get { return (string)base["port"]; }
            set { base["port"] = value; }
        }

        [ConfigurationProperty("domain", DefaultValue = "UserServiceDomain", IsRequired = false)]
        public string DomainName
        {
            get { return (string)base["domain"]; }
            set { base["domain"] = value; }
        }
    }
}