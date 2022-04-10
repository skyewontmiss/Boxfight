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
    public bool animating;
    public PlayerGroundCheck playerGroundCheck;



    [SerializeField] float sprintSpeed, walkSpeed, jumpForce, smoothTime;
    [SerializeField] [HideInInspector] public float mouseSensitivity;

    [SerializeField] Item[] items;
    [SerializeField] TargetShooter[] mySingleShotguns;
    [SerializeField] WeaponSway[] myWeaponSways;

    int itemIndex;
    [SerializeField] [HideInInspector] public int globalIndex;

    [SerializeField] public bool isAiming, paused;
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
        Time.timeScale = 1;
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

    void CreateUV()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector2[] UVs = new Vector2[mesh.vertices.Length];
        // Front
        UVs[0] = new Vector2(0.0f, 0.0f);
        UVs[1] = new Vector2(0.333f, 0.0f);
        UVs[2] = new Vector2(0.0f, 0.333f);
        UVs[3] = new Vector2(0.333f, 0.333f);
        // Top
        UVs[4] = new Vector2(0.334f, 0.333f);
        UVs[5] = new Vector2(0.666f, 0.333f);
        UVs[8] = new Vector2(0.334f, 0.0f);
        UVs[9] = new Vector2(0.666f, 0.0f);
        // Back
        UVs[6] = new Vector2(1.0f, 0.0f);
        UVs[7] = new Vector2(0.667f, 0.0f);
        UVs[10] = new Vector2(1.0f, 0.333f);
        UVs[11] = new Vector2(0.667f, 0.333f);
        // Bottom
        UVs[12] = new Vector2(0.0f, 0.334f);
        UVs[13] = new Vector2(0.0f, 0.666f);
        UVs[14] = new Vector2(0.333f, 0.666f);
        UVs[15] = new Vector2(0.333f, 0.334f);
        // Left
        UVs[16] = new Vector2(0.334f, 0.334f);
        UVs[17] = new Vector2(0.334f, 0.666f);
        UVs[18] = new Vector2(0.666f, 0.666f);
        UVs[19] = new Vector2(0.666f, 0.334f);
        // Right        
        UVs[20] = new Vector2(0.667f, 0.334f);
        UVs[21] = new Vector2(0.667f, 0.666f);
        UVs[22] = new Vector2(1.0f, 0.666f);
        UVs[23] = new Vector2(1.0f, 0.334f);
        mesh.uv = UVs;
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
            mouseSensitivity = PlayerPrefs.GetInt("Mouse");
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
            if (mySingleShotguns != null)
            {
                foreach (TargetShooter shooter in mySingleShotguns)
                {
                    shooter.RefreshCamera((int)myCamera.fieldOfView);
                }
            }

        } else
        {
            myCamera.fieldOfView = 80;
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
        Time.timeScale = 1;
        StartCoroutine(CloseMenu());
    }

    public void LeaveMatch()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadSceney("Menu Scene"));
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
        if (Input.GetKeyDown(KeyCode.Escape) ||Input.GetKey(KeyCode.Alpha0)||Input.GetKey(KeyCode.Keypad0))
        {
            if (!animating)
            {
                if (paused)
                {
                    Time.timeScale = 0f;
                    StartCoroutine(CloseMenu());
                }
                else if (!paused)
                {
                    Time.timeScale = 1f;
                    StartCoroutine(OpenMenu());
                }
            }
        }
    }

    public void LoadScene(string Scene)
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadSceney(Scene));
    }

    IEnumerator LoadSceney(string datScene)
    {
        TransitionManager.instance.Close();
        yield return new WaitForSeconds(0.75f);
        SceneManager.LoadScene(datScene);
    }


    IEnumerator OpenMenu()
    {
        animating = true;
        pauseMenuAnimator.Play("PauseMenuOpen", 0, 0f);
        yield return new WaitForSeconds(0.667f);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        animating = false;
        paused = true;
        Time.timeScale = 0f;
    }

    IEnumerator CloseMenu()
    {
        animating = true;
        pauseMenuAnimator.Play("PauseMenuClose", 0, 0f);
        yield return new WaitForSeconds(2f);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        animating = false;
        paused = false;
        Time.timeScale = 1f;
        
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
    
    public void Die()
    {
        Time.timeScale = 1;
        StartCoroutine(LoadSceneyy());
    }

    IEnumerator LoadSceneyy()
    {
        TransitionManager.instance.Close();
        yield return new WaitForSeconds(0.75f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel(string levelToLoad)
    {
        StartCoroutine(LoadSceney(levelToLoad)); 
    }

}