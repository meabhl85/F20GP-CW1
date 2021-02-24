using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.EnemyBehaviour;

public class Gun : MonoBehaviour
{

    //Gun variables
    public float damage = 100f;
    public float range = 3;
    public float impactForce = 10f;
    public float fireRate = 15f; 
    private float nextTimeToFire = 0f;
    public int maxAmmo = 10;
    private int currentAmmo;
    public float reloadTime = 1.5f;

    public bool gunEnabled = true;
    private bool isReloading = false;

    //References to other components
    public Camera fpsCam;
    public ParticleSystem mussleFlash;
    public GameObject impactEffect;
    public AudioSource gunFireSource;
    public Animator animator;

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {

        if (gunEnabled)
        {
            if (isReloading) return;

            if (currentAmmo <= 0)
            {
                StartCoroutine(Reload());
                return;
            }

            //Check for mouse click and timer
            if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                StartCoroutine(ShootAnimation());
                Shoot();

            }

            
        }
        


    }

    IEnumerator ShootAnimation()
    {
        animator.SetBool("Shoot", true);
        yield return new WaitForSeconds(.25f);
        animator.SetBool("Shoot", false);
    }

    IEnumerator Reload()
    {
        isReloading = true;

        animator.SetBool("Reload", true);
        yield return new WaitForSeconds(reloadTime - .25f);
        animator.SetBool("Reload", false);
        yield return new WaitForSeconds(.30f);

        currentAmmo = maxAmmo;
        isReloading = false;
    }

    void Shoot()
    {
        
        mussleFlash.Play();
        gunFireSource.Play();

        currentAmmo--;

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {

            if (hit.transform.tag == "Enemy")
            {
                EnemyHealth health = hit.transform.GetComponent<EnemyHealth>();
                if (health != null)
                {
                    health.TakeDamage(1);
                }
            } else
            {
                Target target = hit.transform.GetComponent<Target>();
                if (target != null)
                {
                    target.TakeDamage(damage);
                }
            }

            if (hit.rigidbody != null)
            {                
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);

        }

    }
   
}
