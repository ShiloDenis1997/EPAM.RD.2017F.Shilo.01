using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserServiceConfigSection
{
    public class UserServiceSettingsConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("slaves")]
        public SlavesCollection SlaveItems => (SlavesCollection)base["slaves"];

        [ConfigurationProperty("server", IsRequired = true)]
        public ServerElement Server => (ServerElement)base["server"];

        [ConfigurationProperty("storage", IsRequired = false)]
        public StorageElement Storage => (StorageElement)base["storage"];
    }
}