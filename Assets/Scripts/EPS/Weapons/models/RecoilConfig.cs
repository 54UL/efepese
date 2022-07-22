using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EPS
{
    [System.Serializable]
    public class RecoilConfig
    {
        public AnimationCurve kickBackCurve;
        public float kickBackDistance = -1; // backwards
        public float recoilSpringForce = 1;
        public float kickInForce = 1;
        public float pitchKickBackAngleRange = 5;
        public float yawKickBackAngleRange = 5;
        public float rollKickBackAngleRange = 2;
        public Vector3 positionKickBackRange = new Vector3();
        public float recoilDeadZone = 0.1f;
        //OPTIONAL
        public Transform bolt;
        public float boltKickBackRange =-2; // backwards
    }
}
