using UnityEngine;
using System.Collections;

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

    //RECOIL PROTOTYPE
    [Header("Recoil system")]
    //Gun aim 
    public RecoilConfig currentRecoil;
    public Vector3 startGunPivotPosition;
    public Quaternion startGunPivotRotation;
    public float kickElapsedTime = 0;
    
    [Header("Gun sway")]
    public float swaySpeed = 2;
    public float elapesedSwayTime = 0;

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
        if (kickElapsedTime > 0 ){
            kickElapsedTime = 0;
        }
        
        //procedural Recoil system
        float computedYaw = Random.Range(-currentRecoil.yawKickBackAngleRange, currentRecoil.yawKickBackAngleRange);
        float computedPitch = Random.Range(-currentRecoil.pitchKickBackAngleRange, currentRecoil.pitchKickBackAngleRange);
        
        Vector3 kickBackPos = new Vector3(0, 0, currentRecoil.kickBackDistance) + startGunPivotPosition;
        Quaternion kickBackRot = new Quaternion.Euler(computedPitch, computedYaw, 0) + startGunPivotRotation;

        gunPivot.transform.localPosition = kickBackPos;
        gunPivot.transform.localRotation = kickBackRot;

        StartCoroutine(KickBackAnimation(startPivotPosition,startGunPivotRotation));
    }
    //TODO: TEST
    private IEnumerator KickBackAnimation(Vector3 kickBackPos,Quaternion kickBackRotation)
    {
        while (kickElapsedTime < 1.0)
        {
            gunPivot.transform.localPosition = Vector.Lerp(gunPivot.localPosition, kickBackPos, kickElapsedTime);
            gunPivot.transform.localRotation = Quaternion.Slerp(gunPivot.localRotation, kickBackRotation, kickElapsedTime);
            kickElapsedTime += Time.deltaTime * currentRecoil.recoilSpringForce;
            yield return shotDuration;
        }
    }

    //TODO: TEST
    void GunSway(Vector2 lookInput)
    {
        Quaternion swayRotation = Quaternion.Euler(lookInput.x,lookInput.y,0);
        Quaternion newPivotRotation = gunPivot.transform.localRotation + swayRotation;
        gunPivot.transform.localRotation = Quaternion.Slerp(gunPivot.localRotation, to, Time.deltaTime * swaySpeed);
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
            Gizmos.DrawSphere(hitPos, 1);
    }

    private void Start()
    {
        startGunPivotPosition = gunPivot.transform.localPosition;
        startGunPivotRotation = gunPivot.transform.localRotation;
    }

    private void Update()
    {
        WeaponLogic();
    }
}