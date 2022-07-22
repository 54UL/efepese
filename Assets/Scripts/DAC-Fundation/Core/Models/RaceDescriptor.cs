using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DAC
{
    [System.Serializable]
    public class RaceDescriptor
    {
        public RaceDescriptor(string name,List<string> racePath)
        {
            this.RaceName = name;
            this.RacePath = racePath;
        }

        public RaceDescriptor()
        {
            
        }
        public string RaceName;
        public List<string> RacePath;
        [System.NonSerialized]
        public List<Intersection> Intersections;
        public float BestTime;
    }
}