using System.Configuration;
using System.Net;

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
        public IPAddress IpAddress
        {
            get { return IPAddress.Parse((string)base["ipAddress"]); }
            set { base["ipAddress"] = value.ToString(); }
        }

        [ConfigurationProperty("port", DefaultValue = "", IsKey = false, IsRequired = true)]
        public int Port
        {
            get { return int.Parse((string)base["port"]); }
            set { base["port"] = value.ToString(); }
        }
    }
}