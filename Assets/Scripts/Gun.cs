using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    [Header("Shoot sound stuff")]
    public AudioSource source;
    public AudioClip clip;
    public AudioClip reloadstart;
    public AudioClip reloadend;
    public AudioClip pipehit;
    [Header("Recoil")]
    public GameObject Gunrecoil;
    public String animname;
    public String reloadname;

    [Header("References")]
    [SerializeField] private GunData gunData;
    [SerializeField] private Transform cam;
    public static Action shootInput;
    public static Action reloadInput;

    [SerializeField] private KeyCode reloadKey = KeyCode.R;
    
    float timeSinceLastShot;

    private void OnDisable() {
        gunData.reloading = false;
    }

    public void StartReload() {
        if (!gunData.reloading && gameObject.activeSelf){
            StartCoroutine(Reload());
            source.PlayOneShot(reloadstart);
                    StartCoroutine(StartReloadAnim());

        }
    }

    private IEnumerator Reload() {
        gunData.reloading = true;

        yield return new WaitForSeconds(gunData.reloadTime);

        gunData.currentAmmo = gunData.magSize;

        gunData.reloading = false;
        source.PlayOneShot(reloadend);
    }

    private bool CanShoot() => gameObject != null && !gunData.reloading && gameObject.activeSelf && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

    private void Shoot() {
        if (gunData.currentAmmo > 0) {
            if (CanShoot()) {
                    source.PlayOneShot(clip);
                    StartCoroutine(StartRecoil());

                if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hitInfo, gunData.maxDistance)){
                    IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    damageable?.TakeDamage(gunData.damage);
                    source.PlayOneShot(pipehit);
                }

                gunData.currentAmmo--;
                timeSinceLastShot = 0;
                OnGunShot();
            }
        }
            else if (gunData.currentAmmo == 0 && gunData.autoreload == true){
            StartReload();
        }
    }

    IEnumerator StartRecoil()
    {
        Gunrecoil.GetComponent<Animator>().Play(animname);
        yield return new WaitForSeconds(0.10f);
        Gunrecoil.GetComponent<Animator>().Play("New State");
    }
    IEnumerator StartReloadAnim()
    {
        Gunrecoil.GetComponent<Animator>().Play(reloadname);
        yield return new WaitForSeconds(2.0f);
        Gunrecoil.GetComponent<Animator>().Play("New State");
    }

    private void Update() {
        if (Input.GetMouseButton(0)) Shoot();
        if (Input.GetKeyDown(reloadKey)) StartReload();
        timeSinceLastShot += Time.deltaTime;

        if (cam != null)
        {
            Debug.DrawRay(cam.position, cam.forward * gunData.maxDistance);
        }
    }

    private void OnGunShot() { 

     }
}