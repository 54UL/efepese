using UnityEngine;
using System.Collections;
using EPS.Core.Services.Implementations;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace EPS
{
    //TODO: MOVER ESTO A LA API
    public enum WeaponStateFlags {idle, shooting, reloading, swapingWeapons, dropingWeapon}
    public enum PlayerSateFlags {idle, walking, running, crouch, jumping, leaning}
}

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
    public bool runningAnimation = false;
    public bool antiBounce = false;

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
    public float inverseRotationInfluence = 2;
    public float movementInfluence = 2;

    [Header("Gun Aim")]
    public float aimSpeed = 10;
    public float aimElapsedTime = 0;

    [Header("Weapon descriptor")]
    //Weapon descriptor
    public int gunDamage = 25;
    public float fireRate = 0.25f;                                     
    public float weaponRange = 50f;                                 
    public float hitForce = 100f;

    [Header("Gun Audio")]
    public AudioSource gunAudio;
    public AudioClip[] gunShoots;

    [Header("Particle system")]
    public GameObject bulletImpact;
    public ParticleSystem muzzleFlashParticles;


    //Private members
    private float nextFire;                   
    private Vector3 hitPos;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.09f);    

    //EVENTS
    public delegate void BulletHit(GameObject target, float damage);
    public event BulletHit OnBulletHit;

    void WeaponLogic(bool isAiming)
    {
        // Check if the player has pressed the fire button and if enough time has elapsed since they last fired
        if (Mouse.current.leftButton.isPressed && Time.time > nextFire)
        {
            // Update the time when our player can fire next
            nextFire = Time.time + fireRate;
            OnShoot(isAiming);
            StartCoroutine(ShotEffect());
            if (Physics.Raycast(gunEnd.transform.position, gunEnd.transform.forward, out RaycastHit hit, weaponRange))
            {
                hitPos = hit.point;
                if (OnBulletHit != null)
                    OnBulletHit(hit.transform.gameObject, gunDamage);

                if (hit.rigidbody != null)
                    hit.rigidbody.AddForce(-hit.normal * hitForce);
            }
            else
            {
                hitPos = Vector3.zero;
            }
        }
    }

  
    void OnShoot(bool IsAiming)
    {
        //procedural Recoil system
        float computedYaw = Random.Range(-currentRecoil.yawKickBackAngleRange, currentRecoil.yawKickBackAngleRange);
        float computedPitch = Random.Range(-currentRecoil.pitchKickBackAngleRange, (currentRecoil.pitchKickBackAngleRange + currentRecoil.pitchKickBackAngleRange) * -1.0f);
        float computedRoll = Random.Range(-currentRecoil.rollKickBackAngleRange, currentRecoil.rollKickBackAngleRange);
        
        Vector3 computedPos = new Vector3 (
            Random.Range(-currentRecoil.positionKickBackRange.x, currentRecoil.positionKickBackRange.x), 
            Random.Range(-currentRecoil.positionKickBackRange.y, currentRecoil.positionKickBackRange.y), 
            Random.Range(-currentRecoil.positionKickBackRange.z, currentRecoil.positionKickBackRange.z)); // refactor this shit out

        Vector3 aimPos = IsAiming ? aimPoint.localPosition : startGunPivotPosition;
        Vector3 kickBackPos =  aimPos + computedPos;
        Quaternion kickBackRot = Quaternion.Euler(computedPitch, computedYaw, computedRoll) * startGunPivotRotation;

        if (!runningAnimation)
        {
            runningAnimation = true;
            StartCoroutine(KickBackAnimation(kickBackPos, kickBackRot, currentRecoil.recoilSpringForce));
        }
    }

    private IEnumerator KickBackAnimation(Vector3 kickBackPos,Quaternion kickBackRotation, float force)
    {
        while (kickElapsedTime < 1.0)
        {
            gunPivot.transform.localPosition = Vector3.Lerp(gunPivot.localPosition, kickBackPos, kickElapsedTime);
            gunPivot.transform.localRotation = Quaternion.Slerp(gunPivot.localRotation, kickBackRotation, kickElapsedTime);
            kickElapsedTime += Time.deltaTime * force;

            yield return null;
        }
        kickElapsedTime = 0;
     
        runningAnimation = false;
    }

    void GunSway(Vector2 lookInput)
    {
        Vector3 swayRotation = startMoodelRotation.eulerAngles;
        Quaternion newPivotRotation = Quaternion.Euler(
            swayRotation.x + (lookInput.y * movementInfluence),
            swayRotation.y + (lookInput.x * movementInfluence), 
            lookInput.x * inverseRotationInfluence); 
        gunModel.localRotation = Quaternion.Slerp(gunModel.localRotation,newPivotRotation, Time.deltaTime * swaySpeed);
    }

    private void AimAnimation(bool aimIn)
    {
        Vector3 target;

        if (!antiBounce) 
        {
            antiBounce = true;
            aimElapsedTime = 0;
        }

        if (aimElapsedTime < 1.0)
        {
            if (aimIn)
            {
                target = aimPoint.localPosition;
            }
            else
            {
                target = startGunPivotPosition;
            }

            gunPivot.transform.localPosition = Vector3.Lerp(gunPivot.localPosition, target, aimElapsedTime);
            gunPivot.transform.localRotation = Quaternion.Slerp(gunPivot.localRotation, startGunPivotRotation, aimElapsedTime);
            aimElapsedTime += Time.deltaTime * aimSpeed;
        }
        else
        {
            aimElapsedTime = 0;
        }
    }

    public int gunAudioIndex = 0;

    private float GunShootAudio()
    {
        var currentClip = gunShoots[gunAudioIndex];
        gunAudio.clip = currentClip;
        gunAudio.Play();

        if (gunAudioIndex >= gunShoots.Length-1)
            gunAudioIndex = 0;
        else
            gunAudioIndex++;

        return currentClip.length;
    }

    private IEnumerator ShotEffect()
    {
        //Add camera recoil
        if (_input.look.y < 0.99f) _input.look.y -= currentRecoil.upsideRecoil * Time.deltaTime; //TODO:REPAIR THIS THING

        //float gunShootDuration = GunShootAudio();
        muzzleFlash.SetActive(true);
        //muzzleFlashParticles.Play();
        yield return new WaitForSeconds(0.07f);
      
        //muzzleFlashParticles.Stop();
        muzzleFlash.SetActive(false);
        _input.look.y = 0;
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

    private IEnumerator TickSystem()
    {
        var aim = Mouse.current.rightButton.isPressed;
        var wasRelased = Mouse.current.rightButton.wasReleasedThisFrame;

        if (wasRelased)
            antiBounce = true;

        if (aim)
        {
            AimAnimation(true);
        }
        else if (!runningAnimation)
        {

            AimAnimation(false);
        }

        GunSway(_input.look);
        WeaponLogic(aim);
        yield return new WaitForSeconds(0.016f);
    }

    private void Update()
    {
        StartCoroutine(TickSystem());
    }
}