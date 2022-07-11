using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TO DOS: REFACTOR Simulate()
public class CarDynamics : MonoBehaviour
{
    public IPlayerControl currentVehicleController;
    public VehicleSettings vehicleSettings;
    public VehicleSensors vehicleSensors;

    public bool Drivable = true;
    [Header("DEBUGGING INTERNALS")]
    public float CurrentSpeed;
    public float CurrentTorque;
    public float CurrentSuspension;
    public float CurrentForwardFriction;
    public float CurrentSidewaysFriction;
    public float normalizedSpeed;
    public float CurrentSpeedInKMH;
    public float CurrentSteeringAngle;

    // Use this for initialization
    void Start()
    {
        //Initializations
        //vehicleSettings.Chasis.GetComponent<Rigidbody>().centerOfMass = vehicleSettings.CenterOfMass.position;
        //TEST BED 
        // currentVehicleController = GameObject.Find("TEST_PLAYER").GetComponent<PhysicalPlayerController>() as IPlayerControl;
    }

    public void SetPlayerControl(IPlayerControl control)
    {
        this.currentVehicleController = control;
    }


    //Contador de aceleracion
    public float MaxAcelerationTime = 5;
    public float AcelerationTime = 0.0f;
    public float GearChange = 2.0f;

    public float SoundLerpTime = 1.0f;

    public bool GearChanged = false;
    public float GearChangedColdDown = 3.0f;
    public float GearColdDownTime = 0.0f;

    public float MinPitch = -1;

    public float MaxPitch = 6;


     public bool ReachedTopCycle = false;
    public float ReachedTopCycleTimeColdDown = 1.0f;
    public float ReachedTopCycleTime = 0.0f;

    public void PlayAudioCar(bool isAcelerating, float Throttle)
    {

        if ((AcelerationTime % GearChange) <= 0.1F && !GearChanged)
        {
            GearChanged = true;
            AcelerationTime -= 0.8f;
        }

        if (AcelerationTime >= MaxPitch && !ReachedTopCycle)
        {
            ReachedTopCycle = true;
            AcelerationTime -= 0.8f;
        }

        if (ReachedTopCycle)
        {
            ReachedTopCycleTime += Time.fixedDeltaTime;
            if (ReachedTopCycleTime >= ReachedTopCycleTimeColdDown)
            {
                ReachedTopCycleTime = 0;
                ReachedTopCycle = false;
            }
        }

        if (GearChanged)
        {
            GearColdDownTime += Time.fixedDeltaTime;
            if (GearColdDownTime >= GearChangedColdDown)
            {
                GearColdDownTime = 0;
                GearChanged = false;
            }
        }

        if (isAcelerating && Throttle > -1)
        {
            AcelerationTime += Throttle * Time.deltaTime * 0.5f;
        }
        else
        {
            AcelerationTime = Mathf.Lerp(AcelerationTime, 1.3f, Time.fixedDeltaTime * SoundLerpTime);
        }


        AcelerationTime = Mathf.Clamp(AcelerationTime, MinPitch, MaxPitch);
        vehicleSettings.CarEngineAudioSoruce.pitch = AcelerationTime;
    }

    //FACTORIZAME =(
    void Simulate()
    {
        CurrentSpeed = vehicleSettings.Chasis.GetComponent<Rigidbody>().velocity.magnitude;
        normalizedSpeed = vehicleSettings.AccelerationCurve.Evaluate(CurrentSpeed / vehicleSettings.MaxSpeed);
        CurrentSpeedInKMH = CurrentSpeed * 3.6f;
        foreach (Wheel w in vehicleSettings.Wheels)
        {
            w.WheelCollider.GetWorldPose(out Vector3 tmpPos, out Quaternion tmpRoot);
            w.WheelMesh.position = tmpPos;
            w.WheelMesh.rotation = tmpRoot; 

            if (w.Stering)
            {
                if (currentVehicleController != null)
                    CurrentSteeringAngle = w.BaseSteeringAngle + currentVehicleController.Steering() * vehicleSettings.TurnAngle * vehicleSettings.TurnAngleCurve.Evaluate(normalizedSpeed);

                w.WheelCollider.steerAngle = CurrentSteeringAngle;
            }
            else
                w.WheelCollider.steerAngle = w.BaseSteeringAngle;

            if (w.MotorPowered)
            {
                float currentThrottle = currentVehicleController.Throttle();
                float currentReverseMultiplier = currentThrottle >= 0 ? 1 : vehicleSettings.ReverseMultiplier;
                CurrentTorque = vehicleSettings.TorqueCurve.Evaluate(normalizedSpeed) * currentThrottle * vehicleSettings.MaxTorque * currentReverseMultiplier;
                w.WheelCollider.motorTorque = (w.InvertMotorPower ? -1 * CurrentTorque : CurrentTorque);
                PlayAudioCar(currentVehicleController.IsAccelerating(), currentThrottle); ;
            }

            if (w.HasBrakes)
            {
                if (currentVehicleController.HandBrake())
                    w.WheelCollider.brakeTorque = vehicleSettings.HandBrakeForce;
                else if (currentVehicleController.RegularBrakes())
                    w.WheelCollider.brakeTorque = vehicleSettings.BrakeForce;
                else
                    w.WheelCollider.brakeTorque = 0;
            }

            //Frictions curves due to the actual speed
            // w.WheelCollider.sidewaysFriction.stiffness = vehicleSettings.SidewaysStiffnesCurve.Evaluate(normalizedSpeed)*vehicleSettings.MaxSidewaysStiffnes;
            //  w.WheelCollider.ConfigureVehicleSubsteps
            //w.WheelCollider.sidewaysFriction.stiffness = vehicleSettings.SidewaysStiffnesCurve.Evaluate(normalizedSpeed)*vehicleSettings.MaxSidewaysStiffnes;
            // w.WheelCollider.suspensionDistance = CurrentSuspension;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Drivable)
            Simulate();
    }
}
