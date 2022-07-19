using UnityEngine;
using System.Collections;


[System.Serializable]
public class VehicleSettings
{
    public Transform Chasis;
    public Transform CenterOfMass;

    [Header("Wheels parameters")]
    public float MaxSuspensionHeight=1;
    public float MaxForwardStiffnes=1;
    public float MaxSidewaysStiffnes=1;
    public WheelFrictionCurve ForwardFriction;
    public WheelFrictionCurve SidewaysFriction;
    public AnimationCurve ForwardStiffnesCurve;
    public AnimationCurve SidewaysStiffnesCurve;
    public AnimationCurve SuspensionCurve;
    public Wheel[] Wheels;
    [Header("Steering")]
    public float TurnAngle;
    public AnimationCurve TurnAngleCurve;
    [Header("Engine")]
    public AnimationCurve AccelerationCurve;
    public AnimationCurve TorqueCurve;
    public int MaxTorque=1200;
    public float ReverseMultiplier;
    public int MaxSpeed=30;
    public int BrakeForce=1500;
    public int HandBrakeForce = 4000;

    [Header("Audio")]
    public AudioSource CarEngineAudioSoruce;
    
};