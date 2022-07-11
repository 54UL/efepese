
using System.Collections.Generic;

namespace DAC
{
    [System.Serializable]
    public class PersistentData
    {
        public List<RaceDescriptor> SavedRutes;
        [System.NonSerialized]
        public string JsonData = "null";
        public PersistentData()
        {
            this.SavedRutes = new List<RaceDescriptor>();
        }
    }
}