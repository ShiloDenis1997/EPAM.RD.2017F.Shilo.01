using System.Configuration;

namespace UserServiceConfigSection
{
    public class StorageElement : ConfigurationElement
    {
        [ConfigurationProperty("filename", IsRequired = true, DefaultValue = "")]
        public string Filename
        {
            get { return (string)base["filename"]; }
            set { base["filename"] = value; }
        }
    }
}