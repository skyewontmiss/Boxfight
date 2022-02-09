using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    public Camera plcam;
    private GameObject bullettoaddforce;
    [Header("BulletSeetings")]
    public float ShotForce;
    public GameObject bullet;
    public Transform bulletSpawnPoint;
    public float timebwshot;
    public float spreadNOTWORKINGFORNOW;
    public float reloadtime;
    public float bulletsleft;
    public float magazinesize;
    public bool autogun;
    public bool retoshot;
    public bool test;
    public bool shoting;
    void Awake()
    {
        magazinesize = 16;
    }
    void Update()
    {
        if (autogun) { shoting = Input.GetMouseButton(0); } else { shoting = Input.GetMouseButtonDown(0); }



        Debug.DrawRay(rayy.origin, rayy.direction * 10, Color.red);
        MonoBehaviour.print(tarpoint);
        Inputs();
    }
    private Ray rayy;
    private RaycastHit hit;
    private Vector3 tarpoint;
    public void shot()
    {
        rayy = plcam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        bullettoaddforce = Instantiate(bullet, bulletSpawnPoint.position, Quaternion.identity);
        if(Physics.Raycast(rayy, out hit)) { tarpoint = hit.point; }
        else { tarpoint = rayy.GetPoint(200); }
        //bullettoaddforce.transform.forward = 
        Vector3 dir = tarpoint - bulletSpawnPoint.position;
        bullettoaddforce.GetComponent<Rigidbody>().AddForce(dir.normalized * ShotForce, ForceMode.Impulse);
    }
    public void Inputs()
    {
        if (Input.GetMouseButtonDown(0) && shoting)
        {
            shot();
        }
    }
}
