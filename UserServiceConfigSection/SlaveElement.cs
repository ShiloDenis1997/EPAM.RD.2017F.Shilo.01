using System.Configuration;

namespace UserServiceConfigSection
{
    public class SlaveElement : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string SlaveName
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("ipAddress", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string IpAddress
        {
            get { return (string)base["ipAddress"]; }
            set { base["ipAddress"] = value; }
        }

        [ConfigurationProperty("port", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Port
        {
            get { return (string)base["port"]; }
            set { base["port"] = value; }
        }
    }
}