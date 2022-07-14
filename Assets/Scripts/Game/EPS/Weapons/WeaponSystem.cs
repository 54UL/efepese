using UnityEngine;
using System.Collections;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

//This was made in 2015 don't blame me 
public class WeaponSystem : MonoBehaviour
{
    //PUBLIC VARS
    public Transform gunEnd;
    public GameObject muzzleFlash;
    public GameObject impactPoint;    
    public PlayerInput _playerInput;
    public int gunDamage = 1;
    public float fireRate = 0.25f;                                     
    public float weaponRange = 50f;                                 
    public float hitForce = 100f;                                                             
    public bool mouselookenabled;
    //public InputAction fireAction;

    //PRIVATE VARS
    private float nextFire;                   
    private Vector3 hitPos;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);    
    private AudioSource gunAudio;   
   
    //EVENTS
    public delegate void BulletHit(GameObject target);
    public event BulletHit OnBulletHit;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (hitPos != Vector3.zero)
            Gizmos.DrawSphere(hitPos,1);
    }

    void WeaponLogic()
    {
        // Check if the player has pressed the fire button and if enough time has elapsed since they last fired
        if (Mouse.current.leftButton.isPressed && Time.time > nextFire)
        {
            // Update the time when our player can fire next
            nextFire = Time.time + fireRate;
            // Start our ShotEffect coroutine to turn our laser line on and off
            StartCoroutine(ShotEffect());
            RaycastHit hit;

            // Check if our raycast has hit anything
            if (Physics.Raycast(gunEnd.transform.position, gunEnd.transform.forward, out hit, weaponRange))
            {
                // Set the end position for our laser line 
                //impactPoint.SetActive(true);
                hitPos = hit.point;
                //impactPoint.transform.position = hit.point;
                if (OnBulletHit != null)
                    OnBulletHit(hit.transform.gameObject);

                // Check if the object we hit has a rigidbody attached
                if (hit.rigidbody != null)
                {
                    // Add force to the rigidbody we hit, in the direction from which it was hit
                    hit.rigidbody.AddForce(-hit.normal * hitForce);
                }
            }
            else
            {
                hitPos = Vector3.zero;
                //impactPoint.SetActive(false);
            }
        }
    }

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

    private void Start()
    {
   
    }

    private void Update()
    {
        WeaponLogic();
    }
}