using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Rewired;

using DAC;

public class PhysicalPlayerController : MonoBehaviour 
    //,IPlayerControl
{
    //public Player controlOwner; // rewired player
    //public bool Accelerating;
    //public float CurrentSteering;
    //public float turnSpeed = 8;

    ////Testing 
    //public void Start()
    //{
    //    //This must be created by the game session and setted by the player manager
    //    //controlOwner = ReInput.players.GetPlayer("Player0");
    //}

    //public float Steering()
    //{
    //    float value = controlOwner.GetAxis("Steer");
    //    float speedRef = 0;
    //    CurrentSteering = Mathf.SmoothDamp(CurrentSteering, value, ref speedRef, turnSpeed * Time.fixedDeltaTime);
    //    return CurrentSteering;
    //}


    ////Must return -1 to 1, represents reverse or forward aceleration  and must be decimal
    //public float Throttle()
    //{
    //    float value = controlOwner.GetAxis("Throttle");
    //    Accelerating = Mathf.Abs(value) > 0;
    //    return value;
    //}
    //public bool HandBrake()
    //{
    //    return controlOwner.GetButton("HandBrake");
    //}
    //public bool RegularBrakes()
    //{
    //    return controlOwner.GetButton("Brake");
    //}

    //public bool IsAccelerating()
    //{
    //    return Accelerating;
    //}

    //public Vector2 CameraAxis()
    //{
    //    float axisX = controlOwner.GetAxis("CameraAxisX");
    //    float axisY = controlOwner.GetAxis("CameraAxisY");
    //    return new Vector2(axisX, axisY);
    //}

    //public int PlayerID()
    //{
    //    return controlOwner.id;
    //}

    //public Vector3 PlayerPosition()
    //{
    //    throw new System.NotImplementedException();
    //}

    //public void SetVehicleTransform(UnityEngine.Transform transform)
    //{
    //    Debug.Log("Physical player does not need this yeet");
    //}

    //public PlayerType CurrentPlayerType()
    //{
    //    return PlayerType.LOCAL;
    //}
}

