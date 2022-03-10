using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TargetShooter : Gun
{
    [SerializeField] Camera cam;
    [SerializeField] GameObject myPlayer;
    [SerializeField] Animator crosshairAnimator;
    [SerializeField] string IGunNameable;
    [SerializeField] AudioSource reloadSound;
    [SerializeField] float cooldown, reloadCooldown, cameraAimMinimum, cameraAimSpeed;
    [SerializeField] public TMP_Text gunText, currentAmmoText, maxAmmoText;
    int currentAmmo;
    public int maxAmmo;
    [SerializeField] int myItemIndex;
    public bool isAiming;
    [SerializeField] AudioClip gunSFX;
    LocalController playerController;
    public int damageIGive;



    float timer;
    public float cameraAimMaximum;

    public GameObject gun, adsPos;
    Vector3 initialPosition;
    public bool FPSController;

    void Awake()
    {
        playerController = myPlayer.GetComponent<LocalController>();
        currentAmmo = maxAmmo;
    }

    public void RefreshCamera(int FOV)
    {
        cameraAimMaximum = FOV;
    }

    private void Start()
    {
        RefreshCamera(PlayerPrefs.GetInt("FOV"));
        initialPosition = gun.transform.localPosition;
    }

    public override void Use()
    {
        if (timer >= cooldown && currentAmmo > 0)
        {
            Shoot();
            timer = 0f;

        }
        else
        {
            return;
        }
    }

    private void Update()
    {

        timer += Time.deltaTime;



        if (myItemIndex == playerController.globalIndex)
        {
            gunText.text = IGunNameable;
            maxAmmoText.text = maxAmmo.ToString();
            currentAmmoText.text = currentAmmo.ToString();
            Reload();
            if (!playerController.paused)
            {
                if (FPSController == true)
                {
                    if (Input.GetKey(KeyCode.Mouse1))
                    {
                        gun.transform.localPosition = Vector3.Lerp(gun.transform.localPosition, adsPos.transform.localPosition, 0.5f);
                        
                    }
                    else
                    {

                        gun.transform.localPosition = Vector3.Lerp(gun.transform.localPosition, initialPosition, 0.5f);

                    }
                }

                    if (Input.GetKey(KeyCode.Mouse1) && !FPSController)
                {
                    if (cam.fieldOfView > cameraAimMinimum)
                    {
                        cam.fieldOfView -= cameraAimSpeed;
                        playerController.isAiming = true;

                    }

                }
                else if (cam.fieldOfView < cameraAimMaximum)
                {
                    cam.fieldOfView += cameraAimSpeed;
                    playerController.isAiming = false;
                }
            }

        }


    }

    void Reload()
    {
        if (Input.GetKey(KeyCode.R) && currentAmmo != maxAmmo)
        {
            if (timer > reloadCooldown)
            {
                currentAmmo += 1;
                timer = 0f;
                reloadSound.Play();
            }
        }
    }


    void RPC_ShootWithImpacts(Vector3 hitPosition, Vector3 hitNormal)
    {
        Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.4f);
        if (colliders.Length != 0)
        {
            GameObject BulletImpactObj = Instantiate(bulletImpactPrefab, hitPosition + hitNormal * 0.001f, Quaternion.LookRotation(hitNormal, Vector3.up) * bulletImpactPrefab.transform.rotation);
            Destroy(BulletImpactObj, 0.667f);
            BulletImpactObj.transform.SetParent(colliders[0].transform);
        }
        StartCoroutine(RunSoundAcrossClients());
    }


    void RPC_ShootNoImpacts(Vector3 hitPosition, Vector3 hitNormal)
    {
        StartCoroutine(RunSoundAcrossClients());
    }

    IEnumerator RunSoundAcrossClients()
    {
        AudioSource audioRPC = gameObject.AddComponent<AudioSource>();
        audioRPC.clip = gunSFX;
        audioRPC.spatialBlend = 1;
        audioRPC.minDistance = 25;
        audioRPC.maxDistance = 100;
        audioRPC.Play();
        yield return new WaitForSeconds(gunSFX.length);
        Destroy(audioRPC);
    }

    void Shoot()
    {
        Debug.Log("Count: " + PlayerPrefs.GetInt("AssaultRifleShotsCount"));
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject != myPlayer)
        {
            if (PlayerPrefs.GetInt("AssaultRifleShotsCount") >= 900)
            {
                Debug.Log("getting achievement");
                AchievementManager.instance.AchievementGet("Rifle Enthusiast");
            }

            Debug.Log("We hit an object! That object was: " + hit.collider.gameObject.name);

            EnemyMovement Enemy = hit.collider.gameObject.GetComponent<EnemyMovement>();
            if (Enemy != null)
            {
                crosshairAnimator.Play("ARShoot", 0, 0f);
                Enemy.IWillDieable(damageIGive);
                RPC_ShootWithImpacts(hit.point, hit.normal);
                currentAmmo = currentAmmo - 1;

                if(PlayerPrefs.HasKey("AssaultRifleShotsCount"))
                {
                    PlayerPrefs.SetInt("AssaultRifleShotsCount", PlayerPrefs.GetInt("AssaultRifleShotsCount") + 1);
                    PlayerPrefs.Save();

                } else
                {
                    PlayerPrefs.SetInt("AssaultRifleShotsCount", 0 + 1);
                    PlayerPrefs.Save();
                }
                

            }
            else
            {

                if (PlayerPrefs.GetInt("AssaultRifleShotsCount") >= 900)
                {
   
                    AchievementManager.instance.AchievementGet("Rifle Enthusiast");
                }

                crosshairAnimator.Play("ARShoot", 0, 0f);
                RPC_ShootNoImpacts(hit.point, hit.normal);
                currentAmmo = currentAmmo - 1;

                if (PlayerPrefs.HasKey("AssaultRifleShotsCount"))
                {
                    PlayerPrefs.SetInt("AssaultRifleShotsCount", PlayerPrefs.GetInt("AssaultRifleShotsCount") + 1);
                    PlayerPrefs.Save();

                }
                else
                {
                    PlayerPrefs.SetInt("AssaultRifleShotsCount", 0 + 1);
                    PlayerPrefs.Save();
                }
            }
        }
    }



}