using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Rewired;


//THIS IS FOR DEVELOPENT PROPUSES
namespace DAC
{
    [System.Serializable]
    public class NetPlayerArgs
    {
        public NetPlayerArgs()
        {
            NP = new Vector3();
        }
        public float NS;
        public float NT;
        public bool NTHB;
        public bool NRB;
        public int Id;
        public Vector3 NP;
        public Quaternion NR;
    }
}