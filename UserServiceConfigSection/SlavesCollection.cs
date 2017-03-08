using System.Configuration;

namespace UserServiceConfigSection
{
    [ConfigurationCollection(typeof(SlaveElement), AddItemName = "slave")]
    public class SlavesCollection : ConfigurationElementCollection
    {
        public SlaveElement this[int idx]
        {
            get { return (SlaveElement)BaseGet(idx); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new SlaveElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SlaveElement)element).SlaveName;
        }
    }
}