using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] float fireRate;
    float fireRateTimer;
    [SerializeField] bool semiAuto;
    [SerializeField] bool spread;
    public float maxSpread;

    [SerializeField] GameObject bullet;
    [SerializeField] Transform barrelPos;
    [SerializeField] float bulletVelocity;
    [SerializeField] int bulletsPerShot;
    Player_Camera aim;
    public float damage = 20;

    [SerializeField] AudioClip gunShot;
    AudioSource audioSource;
    WeaponRecoil recoil;

    Light muzzleFlashLight;
    ParticleSystem muzzleFlashParticles;
    float lightIntensity;
    [SerializeField] float lightReturnSpeed = 20;

    public Transform leftHandTarget, leftHandHint;
    WeaponClassManager weaponClass;

    private void Start()
    {
        aim = GetComponentInParent<Player_Camera>();
        fireRateTimer = fireRate;
        muzzleFlashLight = GetComponentInChildren<Light>();
        muzzleFlashParticles = GetComponentInChildren<ParticleSystem>();
        lightIntensity = muzzleFlashLight.intensity;
        muzzleFlashLight.intensity = 0;
    }

    private void OnEnable()
    {
        if(weaponClass == null)
        {
            weaponClass = GetComponentInParent<WeaponClassManager>();
            audioSource = GetComponent<AudioSource>();
            recoil = GetComponent<WeaponRecoil>();
            recoil.recoilFollowPos = weaponClass.recoilFollowPos;
        }
        weaponClass.SetCurrentWeapon(this);
    }

    private void Update()
    {
        if (GameManager.isActive)
        {
            if (ShouldFire()) Fire();
            muzzleFlashLight.intensity = Mathf.Lerp(muzzleFlashLight.intensity, 0, lightReturnSpeed * Time.deltaTime);
        }
    }

    bool ShouldFire()
    {
        fireRateTimer += Time.deltaTime;
        if (fireRateTimer < fireRate) return false;
        if (semiAuto && Input.GetKeyDown(KeyCode.Mouse0)) return true;
        if (!semiAuto && Input.GetKey(KeyCode.Mouse0)) return true;
        return false;
    }

    void Fire()
    {
        fireRateTimer = 0;
        barrelPos.LookAt(aim.aimPos);
        audioSource.PlayOneShot(gunShot);
        recoil.TriggerRecoil();
        TriggerMuzzleFlash();
        for(int i = 0; i < bulletsPerShot; i++)
        {
            GameObject currentBullet = Instantiate(bullet, barrelPos.position, barrelPos.rotation);

            Bullet bulletScript = currentBullet.GetComponent<Bullet>();
            bulletScript.weapon = this;

            Rigidbody rb = currentBullet.GetComponent<Rigidbody>();
            if (spread)
            {
                Vector3 dir = barrelPos.forward + new Vector3(Random.Range(-maxSpread, maxSpread), Random.Range(-maxSpread, maxSpread), Random.Range(-maxSpread, maxSpread));
                rb.AddForce(dir * bulletVelocity, ForceMode.Impulse);
            }
            else
            {
                rb.AddForce(barrelPos.forward * bulletVelocity, ForceMode.Impulse);
            }
        }
    }

    void TriggerMuzzleFlash()
    {
        muzzleFlashParticles.Play();
        muzzleFlashLight.intensity = lightIntensity;
    }
}
