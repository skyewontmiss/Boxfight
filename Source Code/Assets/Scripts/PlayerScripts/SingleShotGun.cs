using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SingleShotGun : Gun
{
    [SerializeField] Camera cam;
    [SerializeField] PhotonView PV;
    PhotonView myGunPV;
    [SerializeField] GameObject myPlayer, gun;
    [SerializeField] Animator crosshairAnimator;
    [SerializeField] string IGunNameable;
    [SerializeField] AudioSource reloadSound;
    [SerializeField] int maxAmmo;
    [SerializeField] float cooldown, reloadCooldown, cameraAimMinimum, cameraAimSpeed;
    [SerializeField] int myItemIndex;
    PlayerController playerController;
    [SerializeField] public TMP_Text gunText, currentAmmoText, maxAmmoText;
    public bool isAiming;
    [SerializeField] AudioClip gunSFX;
    public ParticleSystem shotParticles;

    [Header("Header")]
    public GameObject adsPos;

    Vector3 initialPosition;
    public bool FPSController;




    float timer;
    int currentAmmo;
    public float cameraAimMaximum;

    void Awake()
    {
        playerController = myPlayer.GetComponent<PlayerController>();
        myGunPV = GetComponent<PhotonView>();
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

        } else
        {
            return;
        }
    }

    void Shoot()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject != myPlayer)
        {
            crosshairAnimator.Play("ARShoot", 0, 0f);
            shotParticles.Play();
            hit.collider.gameObject.GetComponent<InteractableButton>()?.OnClicked();
            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
            myGunPV.RPC("RPC_ShootWithImpacts", RpcTarget.All, hit.point, hit.normal);
            currentAmmo = currentAmmo - 1;

        } else
        {
            crosshairAnimator.Play("ARShoot", 0, 0f);
            shotParticles.Play();
            myGunPV.RPC("RPC_ShootNoImpacts", RpcTarget.All, hit.point, hit.normal);
            currentAmmo = currentAmmo - 1;
        }
    }
    private void Update()
    {
        if (!PV.IsMine)
            return;

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
                else
                {
                    if (Input.GetKey(KeyCode.Mouse1))
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
    }            



    void Reload()
    {
        if(Input.GetKey(KeyCode.R) && currentAmmo != maxAmmo)
        {
            if(timer > reloadCooldown)
            {
                currentAmmo += 1;
                timer = 0f;
                reloadSound.Play();
            }
        }
    }


    [PunRPC]
    void RPC_ShootWithImpacts(Vector3 hitPosition, Vector3 hitNormal)
    {
        Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.4f);
        if(colliders.Length != 0)
        {
            GameObject BulletImpactObj = Instantiate(bulletImpactPrefab, hitPosition + hitNormal * 0.001f, Quaternion.LookRotation(hitNormal, Vector3.up) * bulletImpactPrefab.transform.rotation);
            Destroy(BulletImpactObj, 0.667f);
            BulletImpactObj.transform.SetParent(colliders[0].transform);
        }
        StartCoroutine(RunSoundAcrossClients());
    }

    [PunRPC]
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



}