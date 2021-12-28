using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//carefully ported to Single Player by Kaylerr.

public class LocalController : MonoBehaviour
{
    [SerializeField] GameObject[] crosshairs;
    [SerializeField] Image healthbarImage;
    [SerializeField] TMP_Text healthText;
    [SerializeField] GameObject cameraHolder;
    [SerializeField] Animator pauseMenuAnimator, hitAnimator;
    GameObject postProcessing;
    [SerializeField] Camera myCamera;
    bool animating;



    [SerializeField] float sprintSpeed, walkSpeed, jumpForce, smoothTime;
    [SerializeField] [HideInInspector] public float mouseSensitivity;

    [SerializeField] Item[] items;
    [SerializeField] TargetShooter[] mySingleShotguns;
    [SerializeField] WeaponSway[] myWeaponSways;

    int itemIndex;
    [SerializeField] [HideInInspector] public int globalIndex;

    [HideInInspector] [SerializeField] public bool isAiming, paused;
    int previousItemIndex = -1;

    float verticalLookRotation;
    bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    const float maxHealth = 100f;
    public float currentHealth;

    Rigidbody rb;

    [HideInInspector] [SerializeField] public float globalDamage;
    float timer;

    public Material[] allMaterials;

    public GameObject fpsCounter;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        postProcessing = GameObject.FindGameObjectWithTag("PostProcessingObject");


    }


    void Start()
    {
        foreach (GameObject myCrosshair in crosshairs)
        {
            myCrosshair.SetActive(false);
        }
        currentHealth = maxHealth;
        EquipItem(0);
        LoadSettings();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        paused = false;
        animating = false;
    }


    void DebugIt(string SuccessfulDebugMessage)
    {
        Debug.LogWarning(SuccessfulDebugMessage);

    }

    public void GainHealth(float healthToGain)
    {
        if(currentHealth != maxHealth)
        {
            //adding the health if our health isn't equal to 100.
            currentHealth += healthToGain;
        }

        
       if(currentHealth > 100f)
        {
            //if our health is higher than 100... then there's a problem. so we're gonna patch that.
            currentHealth = 100f;
        }
    }

    private void LoadSettings()
    {

        //mouse sensetivity settings
        if (PlayerPrefs.HasKey("Mouse"))
        {
            mouseSensitivity = PlayerPrefs.GetFloat("Mouse");
        }
        else
        {
            mouseSensitivity = 1;

        }

        //post processing settings
        if (PlayerPrefs.HasKey("Post Processing"))
        {
            if (PlayerPrefs.GetInt("Post Processing") == 0)
            {
                if (postProcessing != null)
                {
                    //no post processing wanted
                    postProcessing.SetActive(false);
                }
                
            }
            else if (PlayerPrefs.GetInt("Post Processing") == 1)
            {
                if (postProcessing == null)
                {
                    //post processing wanted
                    postProcessing.SetActive(true);
                }
            }
        }





        //fov camera stuff
        if (PlayerPrefs.HasKey("FOV"))
        {
            myCamera.fieldOfView = PlayerPrefs.GetInt("FOV");

        } else
        {
            myCamera.fieldOfView = 60;
        }

        //FPS Counter settings
        if (PlayerPrefs.HasKey("FPS Counter"))
        {
            if (PlayerPrefs.GetInt("FPS Counter") == 0)
            {
                //no FPS Counter wanted
                Destroy(fpsCounter);

            }
            else if (PlayerPrefs.GetInt("FPS Counter") == 1)
            {
                //FPS Counter wanted
                //just ignore, do NOT return as that will end the function.

            }
        }

        if(myWeaponSways != null)
        {
            foreach (WeaponSway weaponSway in myWeaponSways)
            {
                weaponSway.sensitivity = mouseSensitivity;
            }
        }

        if (myWeaponSways != null)
        {
            foreach (TargetShooter shooter in mySingleShotguns)
            {
                shooter.RefreshCamera((int) myCamera.fieldOfView);
            }
        }

    }



    void Update()
    {
        timer += Time.deltaTime;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (paused)
                return;

            items[itemIndex].Use();
        }
            Look();
            Move();
            Jump();

        PauseMenuCheck();

 
        if(!isAiming)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (Input.GetKeyDown((i + 1).ToString()))
                {
                    EquipItem(i);
                    break;
                }
            }


            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
            {
                if (itemIndex >= items.Length - 1)
                {
                    EquipItem(0);
                }
                else
                {
                    EquipItem(itemIndex + 1);
                }
            }
            else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
            {
                if (itemIndex <= 0)
                {
                    EquipItem(items.Length - 1);
                }
                else
                {
                    EquipItem(itemIndex - 1);
                }
            }
        }      



        if(transform.position.y < -10f) //die if you fall outta the world
        {
            Die();
        }


        if(currentHealth <= 0)
        {
            Die();
        }


        healthbarImage.fillAmount = currentHealth / maxHealth;
        float healthToDisplay = currentHealth / maxHealth * 100;
        healthText.text = healthToDisplay.ToString() + "%";

    }



    void Look()
    {
        if (paused)
            return;

        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
    }

    void Jump()
    {
        if (paused)
            return;

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(transform.up * jumpForce);
        }
    }

    public void Resume()
    {
        StartCoroutine(CloseMenu());
    }

    public void LeaveMatch()
    {
        SceneManager.LoadScene(0);
    }

    void EquipItem(int _index)
    {
        if (_index == previousItemIndex)
            return;

        itemIndex = _index;

        items[itemIndex].itemGameObject.SetActive(true);

        if (previousItemIndex != -1)
        {
            items[previousItemIndex].itemGameObject.SetActive(false);
        }

        previousItemIndex = itemIndex;
        globalIndex = itemIndex;

    }




    void PauseMenuCheck()
    {
        if (Input.GetKey(KeyCode.Escape) ||Input.GetKey(KeyCode.Alpha0)||Input.GetKey(KeyCode.Keypad0))
        {
            if (!animating)
            {
                if (paused)
                {
                    StartCoroutine(CloseMenu());
                }
                else if (!paused)
                {
                    StartCoroutine(OpenMenu());
                }
            }
        }
    }



    IEnumerator OpenMenu()
    {
        animating = true;
        pauseMenuAnimator.Play("PauseMenuOpen", 0, 0f);
        yield return new WaitForSeconds(0.30f);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        animating = false;
        paused = true;
    }

    IEnumerator CloseMenu()
    {
        animating = true;
        pauseMenuAnimator.Play("PauseMenuClose", 0, 0f);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        yield return new WaitForSeconds(0.30f);
        animating = false;
        paused = false;
    }




    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }



    void FixedUpdate()
    {

        if (paused)
            return;


        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

    public void TakeDamage(float damage)
    {
        globalDamage = damage;

        currentHealth = currentHealth - damage;
        healthbarImage.fillAmount = currentHealth / maxHealth;
        float healthToDisplay = currentHealth / maxHealth * 100;
        healthText.text = healthToDisplay.ToString() + "%";

        hitAnimator.Play("hit", 0, 0f);


        if (currentHealth <= 0)
        {
            Die();
        }
    }



    void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}

