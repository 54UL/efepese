using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EPS
{
    [System.Serializable]
    public class WeaponDescriptor : MonoBehaviour
    {
        [System.Serializable]
        public string name;
        [System.Serializable]
        public RecoilConfig recoil;
        [System.Serializable]
        public Transform weaponBody;
        [System.Serializable]
        public bool fullAuto = true;
        [System.Serializable]
        public int bulletsPerSecond = 500;
        [System.Serializable]
        public int maganizeSize = 60;
        [System.Serializable]
        public int BaseDamage = 25;
    }
}
