using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DAC;
public interface IPlayerControl
{
    //For vehicles
    //Must return -1 to 1, represents left or right and must be decimal
    float Steering();
    //Must return -1 to 1, represents reverse or forward aceleration  and must be decimal
    float Throttle();
    bool IsAccelerating();
    bool HandBrake();
    bool RegularBrakes();
    //For cameras
    Vector2 CameraAxis();
    int PlayerID();
    PlayerType CurrentPlayerType();
    void SetVehicleTransform(UnityEngine.Transform transform);
}



