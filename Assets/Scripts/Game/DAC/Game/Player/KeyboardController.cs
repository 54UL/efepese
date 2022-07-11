using System.Collections;
using System.Collections.Generic;
using DAC;
using UnityEngine;



//THIS IS FOR DEVELOPENT PROPUSES AND IS HARDOCDED TO BE A NETWORK PLAYER
public class KeyboardController : MonoBehaviour, IPlayerControl
{
    public Transform VehicleTransfom;
    public bool Accelerating;
    public float CurrentSteering;
    public float turnSpeed = 8;
    public int LocalPlayerId;
    public DAC.NetPlayerArgs NetClientData = new DAC.NetPlayerArgs();
    //dependencies
    //public DAC.Foundation.INetClient NetClient;

    public void Start()
    {
        //NetClient = (DAC.Foundation.INetClient)ServiceInjector.Inject("DAC.Foundation.BinaryNetClient");
        Cursor.lockState = CursorLockMode.Confined;
    }

    public float Steering()
    {
        float value = 0;
        float speedRef = 0;

        if (Input.GetKey(KeyCode.D))
            value = 1;
        else if (Input.GetKey(KeyCode.A))
            value = -1;
        else
            value = 0;

        CurrentSteering = Mathf.SmoothDamp(CurrentSteering, value, ref speedRef, turnSpeed * Time.fixedDeltaTime);
        //this.NetClientData.NS = CurrentSteering;
        return CurrentSteering;
    }

    //Must return -1 to 1, represents reverse or forward aceleration  and must be decimal
    public float Throttle()
    {
        float value = 0;

        if (Input.GetKey(KeyCode.W))
            value = 1;
        else if (Input.GetKey(KeyCode.S))
            value = -1;
        else
            value = 0;

        Accelerating = Mathf.Abs(value) > 0;

        //this.NetClientData.NT = value;
        return value;
    }

    public bool HandBrake()
    {
        var inputValue = Input.GetKey(KeyCode.Space);
        //this.NetClientData.NTHB = inputValue;
        return inputValue;
    }

    public bool RegularBrakes()
    {
        var inputValue = Input.GetKey(KeyCode.B);
        //this.NetClientData.NRB = inputValue;
        return inputValue;
    }

    public bool IsAccelerating()
    {
        return Accelerating;
    }

    public Vector2 CameraAxis()
    {
        float axisX = 0;
        float axisY = 0;
        if (Input.GetKey(KeyCode.Mouse1))
        {
            axisX = Input.GetAxis("Mouse X") * -1;
            axisY = Input.GetAxis("Mouse Y");
        }
        return new Vector2(axisX, axisY);
    }

    public int PlayerID()
    {
        return -1;
    }

    public Vector3 PlayerPosition()
    {
        throw new System.NotImplementedException();
    }

    public void SetVehicleTransform(UnityEngine.Transform transform)
    {
        this.VehicleTransfom = transform;
    }

    public void ShareDataToNetwork()
    {
        //var playerPrefix = "PLAYER_DATA#" + LocalPlayerId;
        //var playerPosition = VehicleTransfom.position;
        //this.NetClientData.NP = playerPosition;
        //this.NetClientData.NR = VehicleTransfom.rotation;
        ////var dataToSend = JsonUtility.ToJson(this.NetClientData);
        //NetClient.UppsertProperty(playerPrefix, dataToSend);
    }

    public IEnumerator sendPackets()
    {
        //ShareDataToNetwork();
        yield return new WaitForSeconds(0.016F);
    }

    private void Update()
    {
        //DESCOMENTAR ESTA LINEA PARA HABILITAR EL MP
        //StartCoroutine("sendPackets");
    }

    public PlayerType CurrentPlayerType()
    {
      return PlayerType.LOCAL;
    }
}

