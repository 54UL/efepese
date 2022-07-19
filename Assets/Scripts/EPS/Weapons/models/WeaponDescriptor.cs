using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EPS
{
    [System.Serializable]
    public class WeaponDescriptor : MonoBehaviour
    {
        public string weaponName;
        public RecoilConfig recoil;
        public Transform weaponBody;
        public bool fullAuto = true;
        public int bulletsPerSecond = 500;
        public int maganizeSize = 60;
        public int BaseDamage = 25;
    }
}
