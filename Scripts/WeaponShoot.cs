using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponShoot : MonoBehaviour
{
    [Header("References")]
    public Camera shootCamera;
    public Animator armsAnimator;
    public GameObject bulletPrefab;        // NEW: assign bullet prefab

    [Header("Shooting")]
    public float range = 500f;
    public float fireRate = 8f;
    float nextFireTime;

    [Header("Ammo")]
    public int magazineSize = 12;
    public int currentAmmo;
    public int reserveAmmo = 999;
    public float reloadTime = 1.4f;
    bool isReloading;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip shootClip;
    public AudioClip reloadClip;

    void Start()
    {
        currentAmmo = magazineSize;
    }

    void Update()
    {
        if (Mouse.current == null) return;

        if (isReloading) return;

        if (Keyboard.current != null &&
            Keyboard.current.rKey.wasPressedThisFrame &&
            currentAmmo < magazineSize &&
            reserveAmmo > 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Mouse.current.leftButton.isPressed && Time.time >= nextFireTime)
        {
            if (currentAmmo > 0)
            {
                nextFireTime = Time.time + 1f / fireRate;
                Fire();
            }
            else if (reserveAmmo > 0)
            {
                StartCoroutine(Reload());
            }
        }
    }

    void Fire()
    {
        currentAmmo--;

        // Play animation
        if (armsAnimator != null)
        {
            armsAnimator.ResetTrigger("Shoot");
            armsAnimator.Play("Pistol Shoot", 0, 0f);
        }

        // Play sound
        if (audioSource != null && shootClip != null)
            audioSource.PlayOneShot(shootClip);

        // Spawn bullet with trail
        if (bulletPrefab != null)
        {
            Vector3 firePos = shootCamera.transform.position + shootCamera.transform.forward * 0.5f;
            GameObject bulletObj = Instantiate(bulletPrefab, firePos, Quaternion.identity);

            RaycastBullet bullet = bulletObj.GetComponent<RaycastBullet>();
            if (bullet != null)
            {
                bullet.Fire(firePos, shootCamera.transform.forward, 20f);
            }
        }
    }

    System.Collections.IEnumerator Reload()
    {
        isReloading = true;

        if (armsAnimator != null)
        {
            armsAnimator.ResetTrigger("Reload");
            armsAnimator.SetTrigger("Reload");
        }

        if (audioSource != null && reloadClip != null)
            audioSource.PlayOneShot(reloadClip);

        yield return new WaitForSeconds(reloadTime);

        int needed = magazineSize - currentAmmo;
        int bulletsToLoad = Mathf.Min(needed, reserveAmmo);

        currentAmmo += bulletsToLoad;
        reserveAmmo -= bulletsToLoad;

        isReloading = false;
    }
}
