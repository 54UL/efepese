using UnityEngine;
using System.Collections;


[System.Serializable]
public class Wheel 
{
    public WheelCollider WheelCollider;
    public Transform WheelMesh;
    public float BaseSteeringAngle=-90;
    [Header("Features")]
    public bool Stering;
    public bool InvertSteering;
    public bool MotorPowered;
    public bool InvertMotorPower;
    public bool HasBrakes;
}