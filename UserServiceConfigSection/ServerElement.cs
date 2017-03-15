using System;
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
        [ConfigurationProperty("type", DefaultValue = "Master", IsKey = false, IsRequired = true)]
        [RegexStringValidator("(Slave)|(Master)")]
        public string Type
        {
            get { return (string)base["type"]; }
            set { base["type"] = value; }
        }

        public ServiceType Ttype
        {
            get
            {
                switch (Type)
                {
                    case "Slave":
                        return ServiceType.Slave;
                    case "Master":
                        return ServiceType.Master;
                    default:
                        throw new ConfigurationErrorsException("Unknown service type");
                }
            }
        }

        [ConfigurationProperty("ipAddress", DefaultValue = "127.0.0.1", IsRequired = false)]
        [RegexStringValidator("(2(5[0-5]|[0-4]\\d)|1\\d{2}|[1-9]\\d{0,1})(\\.(2(5[0-5]|[0-4]\\d)|1\\d{2}|[1-9]\\d|\\d)){3}")]
        public string IpAddress
        {
            get { return (string)base["ipAddress"]; }
            set { base["ipAddress"] = value; }
        }

        public IPAddress TipAddress => IPAddress.Parse(IpAddress);

        [ConfigurationProperty("port", DefaultValue = "8080", IsRequired = false)]
        [RegexStringValidator("^([0-9]{1,4}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]|6553[0-5])$")]
        public string Port
        {
            get { return (string)base["port"]; }
            set { base["port"] = value; }
        }

        public int Tport => int.Parse(Port);

        [ConfigurationProperty("domain", DefaultValue = "UserServiceDomain", IsRequired = false)]
        [StringValidator(MinLength = 1)]
        public string DomainName
        {
            get { return (string)base["domain"]; }
            set { base["domain"] = value; }
        }

        [ConfigurationProperty("slaves")]
        public SlavesCollection SlaveItems => (SlavesCollection)base["slaves"];
    }
}