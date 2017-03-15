using System.Configuration;
using System.Net;

namespace UserServiceConfigSection
{
    public enum ServiceType
    {
        Master = 0,
        Slave = 1
    }

    public class ServerElement : ConfigurationElement
    {
        [ConfigurationProperty("type", DefaultValue = "", IsKey = false, IsRequired = true)]
        [RegexStringValidator("(Slave)|(Master)")]
        public ServiceType Type
        {
            get
            {
                switch ((string) base["type"])
                {
                    case "Slave":
                        return ServiceType.Slave;
                    case "Master":
                        return ServiceType.Master;
                    default:
                        throw new ConfigurationErrorsException("Unknown service type");
                }
            }
            set
            {
                switch (value)
                {
                    case ServiceType.Slave:
                        base["type"] = "Slave";
                        break;
                    case ServiceType.Master:
                        base["type"] = "Master";
                        break;
                    default:
                        throw new ConfigurationErrorsException("Unknown service type");
                }
            }
        }

        [ConfigurationProperty("ipAddress", DefaultValue = "", IsRequired = false)]
        [RegexStringValidator("(2(5[0-5]|[0-4]\\d)|1\\d{2}|[1-9]\\d{0,1})(\\.(2(5[0-5]|[0-4]\\d)|1\\d{2}|[1-9]\\d|\\d)){3}")]
        public IPAddress IpAddress
        {
            get { return IPAddress.Parse((string)base["ipAddress"]); }
            set { base["ipAddress"] = value.ToString(); }
        }

        [ConfigurationProperty("port", DefaultValue = "", IsRequired = false)]
        [RegexStringValidator("^([0-9]{1,4}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]|6553[0-5])$")]
        public int Port
        {
            get { return int.Parse((string)base["port"]); }
            set { base["port"] = value.ToString(); }
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