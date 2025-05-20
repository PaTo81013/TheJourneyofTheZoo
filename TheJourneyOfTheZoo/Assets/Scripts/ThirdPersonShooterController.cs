using System;
using UnityEngine;
using StarterAssets;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.Animations.Rigging;
using Random = UnityEngine.Random;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private GameObject normalCrosshair;
    [SerializeField] private GameObject aimCrosshair;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    //[SerializeField] private Transform pfBulletProjectile;
    [SerializeField] private Transform spawnBulletPosition;
    
    [SerializeField] private Rig fullBodyAimingRig;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private Animator animator;
    private Transform hitTransform;
    private GameObject reachedGameObjectWithRaycastHit = default;
    private bool forcedAiming = false;
    private float lastShotTime = 0f;
    private float lastHitBTime = 0f;
    private float hitBTimeStunTime = 2f;
    private bool hitStunned = false;
    //Player Modifiers
    private float weaponCoolDownTime = 0.3f;
    private int maxHitPoints = 100;
    private int currentHitPoints = 100;
    private int damagePerShot = 10;
    private int critMultiplier = 2;
    private int maxWeaponAmmo = 45;
    private int currentWeaponAmmo = 45;
    private float reloadTime = 1.5f;

    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        //Can be removed once the top down mode is implemented!
        aimCrosshair.SetActive(false);
        normalCrosshair.SetActive(true);
        animator = GetComponent<Animator>();
        hitTransform = null;
    }

    private void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
            hitTransform = raycastHit.transform;
        }
        if ((starterAssetsInputs.aim || forcedAiming) && !Pause.IsPaused)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);
            AimCrosshairController(true);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));
            ToggleAimingRig(true);

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            
            //Rotate character to direction
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
            AimCrosshairController(false);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
            ToggleAimingRig(false);
        }

        if (starterAssetsInputs.shoot && !Pause.IsPaused)
        {
            forcedAiming = true;
            if (hitTransform != null && Time.time >= lastShotTime + weaponCoolDownTime)
            {
                Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
                reachedGameObjectWithRaycastHit = hitTransform.root.gameObject;
                if (hitTransform.gameObject.CompareTag("MissHit"))
                {
                    PlayerPoolManager.Instance.InstantiateBulletForShoot(spawnBulletPosition.position, aimDir, mouseWorldPosition, false, false, reachedGameObjectWithRaycastHit);
                }
                else if (hitTransform.gameObject.CompareTag("CriticalHit"))
                {
                    PlayerPoolManager.Instance.InstantiateBulletForShoot(spawnBulletPosition.position, aimDir, mouseWorldPosition, true, true, reachedGameObjectWithRaycastHit);
                } 
                else if (hitTransform.gameObject.CompareTag("NormalHit"))
                {
                    PlayerPoolManager.Instance.InstantiateBulletForShoot(spawnBulletPosition.position, aimDir, mouseWorldPosition, false, true, reachedGameObjectWithRaycastHit);
                }
                lastShotTime = Time.time;
            }
            
            //if (hitTransform.gameObject.CompareTag("NormalHit"))
            //PlayerPoolManager.Instance.InstantiateBulletForShoot(spawnBulletPosition.position, aimDir, mouseWorldPosition);
            //Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
            //starterAssetsInputs.shoot = false;
        }
        else
        {
            forcedAiming = false;
        }

        if (hitStunned && Time.time >= lastHitBTime + hitBTimeStunTime)
        {
            animator.SetBool("GettingHitB", false);
            hitStunned = false;
            thirdPersonController.SetMovementState(!hitStunned);
        }
        
    }

    private void AimCrosshairController(bool aimingNow)
    {
        if (aimingNow)
        {
            //normalCrosshair.SetActive(false);
            aimCrosshair.SetActive(true);
        }
        else
        {
            aimCrosshair.SetActive(false);
            //normalCrosshair.SetActive(true);
        }
    }

    private void ToggleAimingRig(bool rigOption)
    {
        fullBodyAimingRig.weight = rigOption ? 1 : 0;
    }

    public void TakingDamageFromEnemies(int damageTaken)
    {
        currentHitPoints -= damageTaken;
        TriggerBackwardHitAnimation();
    }

    private void TriggerBackwardHitAnimation()
    {
        lastHitBTime = Time.time;
        animator.SetBool("GettingHitB", true);
        hitStunned = true;
        thirdPersonController.SetMovementState(!hitStunned);
    }
}
