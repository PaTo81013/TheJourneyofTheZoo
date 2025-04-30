using System;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

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

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private Animator animator;
    private Transform hitTransform;

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
        if (starterAssetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);
            AimCrosshairController(true);
            //animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));

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
            //animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
        }

        if (starterAssetsInputs.shoot)
        {
            if (hitTransform != null)
            {
                Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
                if (hitTransform.gameObject.CompareTag("CriticalHit"))
                {
                    PlayerPoolManager.Instance.InstantiateBulletForShoot(spawnBulletPosition.position, aimDir, mouseWorldPosition, true);
                } else
                {
                    PlayerPoolManager.Instance.InstantiateBulletForShoot(spawnBulletPosition.position, aimDir, mouseWorldPosition, false);
                }
            }
            
            //if (hitTransform.gameObject.CompareTag("NormalHit"))
            //PlayerPoolManager.Instance.InstantiateBulletForShoot(spawnBulletPosition.position, aimDir, mouseWorldPosition);
            //Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
            starterAssetsInputs.shoot = false;
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
}
