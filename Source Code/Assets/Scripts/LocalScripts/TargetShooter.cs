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
    [SerializeField] int myItemIndex;
    public bool isAiming;
    [SerializeField] AudioClip gunSFX;



    float timer;
    public float cameraAimMaximum;

    void Awake()
    {
    }

    public void RefreshCamera(int FOV)
    {
        cameraAimMaximum = FOV;
    }

    private void Start()
    {
        RefreshCamera(PlayerPrefs.GetInt("FOV"));
    }

    public override void Use()
    {
        if (timer >= cooldown)
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



        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (cam.fieldOfView > cameraAimMinimum)
            {
                cam.fieldOfView -= cameraAimSpeed;

            }

        }
        else if (cam.fieldOfView < cameraAimMaximum)
        {
            cam.fieldOfView += cameraAimSpeed;
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
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject != myPlayer)
        {
            Debug.Log("We hit an object! That object was: " + hit.collider.gameObject.name);

            EnemyMovement Enemy = hit.collider.gameObject.GetComponent<EnemyMovement>();
            if (Enemy != null)
            {
                crosshairAnimator.Play("ARShoot", 0, 0f);
                Enemy.IWillDieable();
                RPC_ShootWithImpacts(hit.point, hit.normal);
            }
            else
            {
                crosshairAnimator.Play("ARShoot", 0, 0f);
                RPC_ShootNoImpacts(hit.point, hit.normal);
            }
        }
    }



}