using System.Configuration;
using System.Net;

namespace UserServiceConfigSection
{
    public class SlaveElement : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = "Slave1", IsKey = true, IsRequired = true)]
        [StringValidator(MinLength = 1)]
        public string SlaveName
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("ipAddress", DefaultValue = "127.0.0.1", IsKey = false, IsRequired = true)]
        [RegexStringValidator("(2(5[0-5]|[0-4]\\d)|1\\d{2}|[1-9]\\d{0,1})(\\.(2(5[0-5]|[0-4]\\d)|1\\d{2}|[1-9]\\d|\\d)){3}")]
        public string IpAddress
        {
            get { return (string)base["ipAddress"]; }
            set { base["ipAddress"] = value; }
        }

        /// <summary>
        /// Returns typed ip address of slave element
        /// </summary>
        public IPAddress TipAddress => IPAddress.Parse(IpAddress);

        [ConfigurationProperty("port", DefaultValue = "8080", IsKey = false, IsRequired = true)]
        [RegexStringValidator("^([0-9]{1,4}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]|6553[0-5])$")]
        public string Port
        {
            get { return (string)base["port"]; }
            set { base["port"] = value; }
        }

        /// <summary>
        /// Returns typed port number of slave element
        /// </summary>
        public int Tport => int.Parse(Port);

        [ConfigurationProperty("domain", DefaultValue = "UserServiceDomain", IsRequired = false)]
        [StringValidator(MinLength = 1)]
        public string DomainName
        {
            get { return (string)base["domain"]; }
            set { base["domain"] = value; }
        }
    }
}