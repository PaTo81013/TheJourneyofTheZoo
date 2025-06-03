using System;
using Scenes;
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
    private Rigidbody playerRB = default;
    private CapsuleCollider capsuleCollider = default;
    private CharacterController playerCharacterController = default;
    private bool forcedAiming = false;
    private float lastShotTime = 0f;
    private float lastHitBTime = 0f;
    private float hitBTimeStunTime = 2f;
    private bool hitStunned = false;
    //Player Modifiers
    private float weaponCoolDownTime = 0.3f;
    private int currentHitPoints = 100;
    private int currentShieldPoints = 0;
    private int maxWeaponAmmo = 45;
    private int currentWeaponAmmo = 45;
    private float reloadTime = 1.5f;
    private float reloadLastTime = 0f;
    private float knockbackTime = 0.65f;
    private bool beingLaunchedByKnockback = false;
    private bool playerIsAlive = true;
    private bool reloading = false;

    public Canvas LoseCanvas;

    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        playerRB = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        playerCharacterController = GetComponent<CharacterController>();
        //Can be removed once the top down mode is implemented!
        aimCrosshair.SetActive(false);
        normalCrosshair.SetActive(true);
        animator = GetComponent<Animator>();
        hitTransform = null;
    }

    private void Start()
    {
        SetFirstStatValues();
    }

    private void Update()
    {
        if (playerIsAlive)
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

            if ((starterAssetsInputs.aim || forcedAiming || reloading) && !Pause.IsPaused && !hitStunned)
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

            if (starterAssetsInputs.shoot && !Pause.IsPaused && !hitStunned)
            {
                forcedAiming = true;
                if (hitTransform != null && Time.time >= lastShotTime + weaponCoolDownTime)
                {
                    Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
                    reachedGameObjectWithRaycastHit = hitTransform.root.gameObject;
                    if (hitTransform.gameObject.CompareTag("MissHit") && currentWeaponAmmo != 0 && !reloading)
                    {
                        PlayerPoolManager.Instance.InstantiateBulletForShoot(spawnBulletPosition.position, aimDir,
                            mouseWorldPosition, false, false, reachedGameObjectWithRaycastHit);
                    }
                    else if (hitTransform.gameObject.CompareTag("CriticalHit") && currentWeaponAmmo != 0 && !reloading)
                    {
                        PlayerPoolManager.Instance.InstantiateBulletForShoot(spawnBulletPosition.position, aimDir,
                            mouseWorldPosition, true, true, reachedGameObjectWithRaycastHit);
                    }
                    else if (hitTransform.gameObject.CompareTag("NormalHit") && currentWeaponAmmo != 0 && !reloading)
                    {
                        PlayerPoolManager.Instance.InstantiateBulletForShoot(spawnBulletPosition.position, aimDir,
                            mouseWorldPosition, false, true, reachedGameObjectWithRaycastHit);
                    }

                    lastShotTime = Time.time;
                    if (currentWeaponAmmo == 0 && !reloading)
                    {
                        BeginReloadSequence();
                    }
                    else
                    {
                        if (!reloading)
                        {
                            currentWeaponAmmo--;
                        }
                    }
                    AmmoUIManager.Instance.UpdateNewAmmoValue(currentWeaponAmmo);
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
                //animator.SetBool("GettingHitB", false);
                hitStunned = false;
                //this.transform.position = playerRB.transform.position;
                ResetRigidbodyAfterKnockbackAndEnableCharacterController();
                thirdPersonController.SetMovementState(!hitStunned);
            }

            if (Time.time >= reloadLastTime + reloadTime && reloading)
            {
                ReloadWeapon();
            }

            if (Input.GetKeyDown(KeyCode.R) && !Pause.IsPaused && !hitStunned && !reloading)
            {
                BeginReloadSequence();
            }
        }
        else
        {
            thirdPersonController.SetMovementState(false);
        }

        if (beingLaunchedByKnockback && hitStunned && Time.time >= lastHitBTime + knockbackTime)
        {
            KnockBackMovementStop();
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

    public void TakingDamageFromEnemies(int damageTaken, Vector3 hitPositionSource)
    {
        if (!playerIsAlive)
        {
            return;
        }
        ManageDamageTakenIntoCurrentHitPointsAndShieldPoints(damageTaken);
        ScoreManager.Instance.PlayerHasBeenHit();
        TriggerBackwardHitAnimation();
        this.transform.LookAt(hitPositionSource);
        //Preparing for knockback
        hitPositionSource = new Vector3(hitPositionSource.x, 0f, hitPositionSource.z);
        Vector3 playerPositionInXandZ = new Vector3(this.transform.position.x, 0f, this.transform.position.z);
        Vector3 throwBackDirection = (playerPositionInXandZ - hitPositionSource).normalized;
        TriggerKnockbackSequenceAndDisableCharacterController(throwBackDirection * 500f);
        //this.transform.rotation = Quaternion.LookRotation(hitPositionSource, Vector3.up);
    }

    private void TriggerBackwardHitAnimation()
    {
        lastHitBTime = Time.time;
        animator.SetTrigger("GettingHitB");
        hitStunned = true;
        thirdPersonController.SetMovementState(!hitStunned);
    }

    private void TriggerKnockbackSequenceAndDisableCharacterController(Vector3 force)
    {
        playerCharacterController.enabled = false;
        playerRB.isKinematic = false;
        capsuleCollider.enabled = true;
        beingLaunchedByKnockback = true;
        playerRB.AddForce(force);
    }

    private void ResetRigidbodyAfterKnockbackAndEnableCharacterController()
    {
        playerRB.isKinematic = true;
        capsuleCollider.enabled = false;
        playerCharacterController.enabled = true;
    }

    private void KnockBackMovementStop()
    {
        playerRB.linearVelocity = Vector3.zero;
        beingLaunchedByKnockback = false;
    }

    private void SetFirstStatValues()
    {
        playerIsAlive = true;
        currentHitPoints = 100;
        currentShieldPoints = 0;
        currentWeaponAmmo = 45;
        hitStunned = false;
        reloading = false;
        thirdPersonController.SetMovementState(true);
        animator.SetBool("Death", false);
    }

    private void ManageDamageTakenIntoCurrentHitPointsAndShieldPoints(int damageTaken)
    {
        if (currentShieldPoints < 0)
        {
            currentShieldPoints = currentShieldPoints - damageTaken;
            if (currentShieldPoints < 0)
            {
                int residualDamage = Mathf.Abs(currentShieldPoints);
                currentHitPoints = currentHitPoints - residualDamage;
            }
        }
        else
        {
            currentHitPoints = currentHitPoints - damageTaken;
        }
        UpdateHitpointsAndShieldPointsInUI();
        CheckIfPlayerIsAlive();
    }

    private void CheckIfPlayerIsAlive()
    {
        if (currentHitPoints <= 0)
        {
            playerIsAlive = false;
            currentHitPoints = 0;
            hitStunned = true;
            thirdPersonController.SetMovementState(false);
            animator.SetBool("Death", true);
            LoseCanvas.enabled=true;
            //CONDICION DE DERROTA
        }
    }

    private void BeginReloadSequence()
    {
        reloading = true;
        animator.SetTrigger("Reload");
        reloadLastTime = Time.time;
        animator.SetLayerWeight(1,1);
    }

    private void ReloadWeapon()
    {
        currentWeaponAmmo = maxWeaponAmmo;
        AmmoUIManager.Instance.UpdateNewAmmoValue(currentWeaponAmmo);
        reloading = false;
    }

    private void UpdateHitpointsAndShieldPointsInUI()
    {
        HitpointsUIManager.Instance.UpdateNewHPValue(currentHitPoints);
        HitpointsUIManager.Instance.UpdateNewShieldValue(currentShieldPoints);
    }
}
