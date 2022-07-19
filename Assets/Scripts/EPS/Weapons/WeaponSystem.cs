using UnityEngine;
using System.Collections;
using EPS.Core.Services.Implementations;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

public class WeaponSystem : MonoBehaviour
{
    //Scene objects and prefabs
    public Transform  gunEnd;
    public GameObject muzzleFlash;
    public GameObject impactPoint;
    public Transform  aimPoint;
    public Transform  gunPivot;
    public Transform gunModel;
    public InputSystem.PlayerActions _input;

    //RECOIL PROTOTYPE
    [Header("Recoil system")]
    //Gun aim 
    public EPS.RecoilConfig currentRecoil;
    public Vector3 startGunPivotPosition;
    public Quaternion startGunPivotRotation;
    public Quaternion startMoodelRotation;
    public float kickElapsedTime = 0;
    
    [Header("Gun sway")]
    public float swaySpeed = 10;

    [Header("Gun Aim")]
    public float aimSpeed = 10;

    [Header("Weapon descriptor")]
    //Weapon descriptor
    public int gunDamage = 25;
    public float fireRate = 0.25f;                                     
    public float weaponRange = 50f;                                 
    public float hitForce = 100f;                                                             

    //Private members
    private float nextFire;                   
    private Vector3 hitPos;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);    
    private AudioSource gunAudio;   
   
    //EVENTS
    public delegate void BulletHit(GameObject target, float damage);
    public event BulletHit OnBulletHit;

    void WeaponLogic()
    {
        // Check if the player has pressed the fire button and if enough time has elapsed since they last fired
        if (Mouse.current.leftButton.isPressed && Time.time > nextFire)
        {
            // Update the time when our player can fire next
            nextFire = Time.time + fireRate;
            // Start our ShotEffect coroutine to turn our laser line on and off
            // StartCoroutine(ShotEffect());
            RaycastHit hit;

            // Check if our raycast has hit anything
            if (Physics.Raycast(gunEnd.transform.position, gunEnd.transform.forward, out hit, weaponRange))
            {
                OnShoot(); //XUL TODO: TEST THIS FIRST...
                StartCoroutine(ShotEffect());
                // Set the end position for our laser line 
                //impactPoint.SetActive(true);
                hitPos = hit.point;
                //impactPoint.transform.position = hit.point;
                if (OnBulletHit != null)
                    OnBulletHit(hit.transform.gameObject, gunDamage);

                // Check if the object we hit has a rigidbody attached
                if (hit.rigidbody != null)
                    // Add force to the rigidbody we hit, in the direction from which it was hit
                    hit.rigidbody.AddForce(-hit.normal * hitForce);
            }
            else
            {
                hitPos = Vector3.zero;
                //impactPoint.SetActive(false);
            }
        }
    }

    void OnShoot()
    {
        //If already shooting restart
        if (kickElapsedTime > 0)
        {
            kickElapsedTime = 0;
        }

        //procedural Recoil system
        float computedYaw = Random.Range(-currentRecoil.yawKickBackAngleRange, currentRecoil.yawKickBackAngleRange);
        float computedPitch = Random.Range(-currentRecoil.pitchKickBackAngleRange, currentRecoil.pitchKickBackAngleRange);
        
        Vector3 kickBackPos = new Vector3(0, 0, currentRecoil.kickBackDistance) + startGunPivotPosition;
        Quaternion kickBackRot = Quaternion.Euler(computedPitch, computedYaw, 0) * startGunPivotRotation;

        gunPivot.transform.localPosition = kickBackPos;
        gunPivot.transform.localRotation = kickBackRot;

        StartCoroutine(KickBackAnimation(startGunPivotPosition, startGunPivotRotation, currentRecoil.recoilSpringForce));
    }

    void KickInInterpolation(Vector3 kickBackPosition, Quaternion kickBackRotation )
    {
        gunPivot.transform.localPosition = Vector3.Lerp(gunPivot.localPosition, kickBackPosition, Time.smoothDeltaTime * swaySpeed);
        gunPivot.transform.localRotation = Quaternion.Slerp(gunPivot.localRotation, kickBackRotation, Time.smoothDeltaTime * swaySpeed);
    }

    private IEnumerator KickBackAnimation(Vector3 kickBackPos,Quaternion kickBackRotation, float force)
    {
        while (kickElapsedTime < 1.0)
        {
            gunPivot.transform.localPosition = Vector3.Lerp(gunPivot.localPosition, kickBackPos, kickElapsedTime);
            gunPivot.transform.localRotation = Quaternion.Slerp(gunPivot.localRotation, kickBackRotation, kickElapsedTime);
            kickElapsedTime += Time.smoothDeltaTime * force;
            yield return shotDuration;
        }
        //kickElapsedTime = 0;
    }


    //TODO: TEST
    
    
    void GunSway(Vector2 lookInput)
    {
        Vector3 swayRotation = startMoodelRotation.eulerAngles;
        Quaternion newPivotRotation = Quaternion.Euler(swayRotation.x + lookInput.y, swayRotation.y + lookInput.x, 0); ;
        gunModel.localRotation = Quaternion.Slerp(gunModel.localRotation,newPivotRotation, Time.smoothDeltaTime * swaySpeed);
    }

    void GunAim(bool aim)
    {
        Vector3 target;

        if (aim)
            target = aimPoint.localPosition;
        else
            target = startGunPivotPosition;

            gunPivot.transform.localPosition = Vector3.Lerp(gunPivot.localPosition, target, aimSpeed * Time.smoothDeltaTime);
    }


    // private IEnumerator SwayAnimation(Quaternion to)
    // {
    //     while (elapesedSwayTime < 1.0)
    //     {
    //         yield return new WaitForSeconds(0.16);
    //     }
    //     elapesedSwayTime = 0;
    // }

    void WeaponAnimation()
    {
        
    }

    void WeaponAim()
    {
        
        if (Mouse.current.rightButton.isPressed)
        {

        }
    }

    private IEnumerator ShotEffect()
    {
        // Play the shooting sound effect
        //gunAudio.Play();

        // Turn on our line renderer
        muzzleFlash.SetActive(true);
        //impactPoint.SetActive(true);
        //Wait for .07 seconds
        yield return shotDuration;
        // Deactivate our line renderer after waiting
        muzzleFlash.SetActive(false);
        //impactPoint.SetActive(false);
    }
    
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (hitPos != Vector3.zero)
            Gizmos.DrawSphere(hitPos, 0.1f);
    }

    private void Start()
    {
        var inputService = EPS.ServiceInjector.getSingleton<InputService>();
        _input = inputService.GetInputActions();
        startGunPivotPosition = gunPivot.transform.localPosition;
        startGunPivotRotation = gunPivot.transform.localRotation;
        startMoodelRotation = gunModel.localRotation; 
    }


    public Vector2 lookAcceleration;
    private void LateUpdate()
    {
        var aim = Mouse.current.rightButton.isPressed;
        var aimRelased = Mouse.current.rightButton.wasReleasedThisFrame;

        if (aim)
            GunAim(true);

        if (aimRelased)
            GunAim(false);//Start corrutine instead!
        
        GunSway(_input.look);
        WeaponLogic();
    }
}