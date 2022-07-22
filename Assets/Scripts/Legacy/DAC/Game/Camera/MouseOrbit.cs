using UnityEngine;
using System.Collections;
//Saul Aceves - 2015/12/30
//Traditional Mouse Orbit 
public class MouseOrbit : MonoBehaviour
{
    public IPlayerControl currentController;
    public Camera CameraInstance;
    public Transform Target;

    [Header("Mouse Orbit")]
    public float LookSpeed;
    public float LookLerpSpeed;
    public float Distance;

    public Vector3 targetOffsetPosition;
    public Vector2 RawAxis;
    public Vector2 LerpAxis;


    [Header("Camera Follow")]
    public float RawTargetY;
    public float LerpTargetY;

    void Start()
    {
        CameraInstance = this.gameObject.GetComponent<Camera>();
        /*
        //TEST BED 
        currentController = GameObject.Find("TEST_PLAYER").GetComponent<PhysicalPlayerController>() as IPlayerControl;

        if (currentController == null)
            Debug.Log("PLAYER NOT FOUND camera");
        else
            Debug.Log("PLAYER FOUND!! camera");
            */
    }

    bool FirstRun = true;
    //Methods
    void FixedUpdate()
    {
        //Raw values
        if (FirstRun)
        {
            Vector3 tmpRoot = this.transform.rotation.eulerAngles;
            RawAxis.x = tmpRoot.x;
            RawAxis.y = tmpRoot.y;
            FirstRun = false;
        }
        else
        {
            RawAxis.x += currentController.CameraAxis().y * LookSpeed;
            RawAxis.y -= currentController.CameraAxis().x * LookSpeed;
        }
        RawTargetY = Target.rotation.eulerAngles.y;


        //Lerped values
        LerpTargetY = Mathf.LerpAngle(LerpTargetY, RawTargetY, LookLerpSpeed * Time.fixedDeltaTime);
        LerpAxis = Vector2.Lerp(LerpAxis, RawAxis, LookLerpSpeed * Time.fixedDeltaTime);

        Quaternion Rotation = Quaternion.Euler(new Vector3(LerpAxis.x, LerpAxis.y + LerpTargetY, 0));
        Vector3 Position = Rotation * (new Vector3(0, 0, -Distance)) + (Target.position + targetOffsetPosition);

        this.transform.position = Position;
        this.transform.rotation = Rotation;
    }
}