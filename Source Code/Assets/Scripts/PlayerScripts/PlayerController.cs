using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerController : MonoBehaviourPunCallbacks, IDamageable
{
    [SerializeField] GameObject[] doNotShowOnOtherClients, crosshairs;
    [SerializeField] Image healthbarImage;
    [SerializeField] TMP_Text healthText;
    [SerializeField] GameObject cameraHolder;
    [SerializeField] Animator pauseMenuAnimator, textChatMenuAnimator, hitAnimator;
    GameObject postProcessing;
    [SerializeField] Camera myCamera;
    bool animating, isChatting, isThirdPerson;



    [SerializeField] float sprintSpeed, walkSpeed, jumpForce, smoothTime;
    [SerializeField] [HideInInspector] public int mouseSensitivity;
    TMP_Text chatContents;

    [SerializeField] Item[] items;
    [SerializeField] SingleShotGun[] mySingleShotguns;
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

    PhotonView PV;
    PhotonView playerManagerPV;

    PlayerManager playerManager;

    [HideInInspector] [SerializeField] public string killer;
    [HideInInspector] [SerializeField] public float globalDamage;
    public TMP_InputField ChatInput;
    public int _maximumMessages = 8;
    private float buildDelay = 0f;
    float timer;
    public GameObject TeamsCube;

    public Material[] Textures;

    public GameObject fpsCounter;
     

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
        postProcessing = GameObject.FindGameObjectWithTag("PostProcessingObject");
        chatContents = GameObject.FindGameObjectWithTag("ChatObject").GetComponent<TMP_Text>();

        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
        playerManagerPV = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PhotonView>();


    }




    void Start()
    {
        if (PV.IsMine)
        {
            foreach(GameObject myCrosshair in crosshairs)
            {
                  myCrosshair.SetActive(false);
            }
            currentHealth = maxHealth;
            isChatting = false;
            EquipItem(0);
            LoadSettings();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            paused = false;
            animating = false;
            isThirdPerson = true;
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
            foreach (GameObject GODestroy in doNotShowOnOtherClients)
            {
                Destroy(GODestroy);
            }
        }


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
        //loading username
        if (PlayerPrefs.HasKey("Username"))
        {
            PhotonNetwork.NickName = PlayerPrefs.GetString("Username");
            PlayerPrefs.SetString("Username", PhotonNetwork.NickName);
            PlayerPrefs.Save();
        }
        else
        {
            PhotonNetwork.NickName = PhotonNetwork.NickName = "Player " + UnityEngine.Random.Range(0, 9999).ToString("0000");
            PlayerPrefs.SetString("Username", PhotonNetwork.NickName);
            PlayerPrefs.Save();
        }

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

        }


        //skins


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


        if(PlayerPrefs.HasKey("Skin"))
        {
            int skin = PlayerPrefs.GetInt("Skin");
            photonView.RPC("ChangeSkin", RpcTarget.All, skin);

        } else
        {
            
        }

        if (PlayerPrefs.HasKey("FirstMatch"))
        {
            if (PlayerPrefs.GetInt("FirstMatch") == 0)
            {
                //skip over
            } 
        } else
        {
            AchievementManager.instance.AchievementGet("First Match");
        }

    }
    //chat code

    public void SendChat(string msg, bool isSystemMessage)
    {

        if(isSystemMessage == true)
        {
            string NewMessage = "System: " + msg;
            playerManagerPV.RPC("RPC_AddNewMessage", RpcTarget.All, NewMessage);

        } else
        {
            string NewMessage = PhotonNetwork.NickName + ": " + msg;
            playerManagerPV.RPC("RPC_AddNewMessage", RpcTarget.All, NewMessage);
        }


    }



    public void SubmitChat()
    {
        string blankCheck = ChatInput.text;
        blankCheck = Regex.Replace(blankCheck, @"\s", "");

        if(blankCheck == "")
        {
            ChatInput.ActivateInputField();
            ChatInput.text = "";
            return;
        }

        SendChat(ChatInput.text, false);
        ChatInput.DeactivateInputField();
        ChatInput.text = "";
    }


    IEnumerator OpenChat()
    {
        animating = true;
        textChatMenuAnimator.Play("ChatMenuOpen", 0, 0f);
        yield return new WaitForSeconds(0.20f);
        ChatInput.ActivateInputField();
        isChatting = true;
        animating = false;
    }

    IEnumerator CloseChat()
    {
        animating = true;
        textChatMenuAnimator.Play("ChatMenuClose", 0, 0f);
        yield return new WaitForSeconds(0.20f);
        SubmitChat();
        ChatInput.DeactivateInputField();
        isChatting = false;
        animating = false;
    }



    void Update()
    {
        timer += Time.deltaTime;
        //this code runs first since we dont want this to run on locals only


        if (!PV.IsMine)
            return;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (paused)
                return;

            items[itemIndex].Use();
        }

        if(!isChatting)
        {
            Look();
            Move();
            Jump();
        }

        PauseMenuCheck();
        ChatMenuCheck();

 
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
            FallOuttaWorldDie();
        }


        if(currentHealth <= 0)
        {
            killer = "World";
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
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
        RoomManager.instance.OnDestroy();
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

        if (PV.IsMine)
        {
            Hashtable gunIndex = new Hashtable();
            gunIndex.Add("Item Index", itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(gunIndex);

            foreach (GameObject myCrosshair in crosshairs)
            {
                myCrosshair.SetActive(false);
            }

            crosshairs[_index].SetActive(true);
        }

    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(!PV.IsMine && targetPlayer == PV.Owner)
        {
            EquipItem((int)changedProps["Item Index"]);
        }

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



    void ChatMenuCheck()
    {
        if(PlayerPrefs.GetInt("Text Chat") == 1)
        {
            if(!paused)
            {
                if (Input.GetKey(KeyCode.Return))
                {
                    if (!animating)
                    {
                        if (isChatting)
                        {
                            StartCoroutine(CloseChat());

                        }
                        else if (!isChatting)
                        {
                            StartCoroutine(OpenChat());
                        }
                    }
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
        if (!PV.IsMine)
            return;

        if (paused)
            return;


        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

    public void TakeDamage(float damage)
    {
        globalDamage = damage;
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    void ChangeSkin(int texture)
    {
        gameObject.GetComponent<MeshRenderer>().material = Textures[texture];
        gameObject.GetComponent<PlayerController>().UV(gameObject.GetComponent<MeshFilter>().mesh);
    }

    void UV(Mesh mesh)
    {
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



    [PunRPC]
    void RPC_TakeDamage(float damage, PhotonMessageInfo msgInfo)
    {
        if (!PV.IsMine)
            return;

        killer = msgInfo.Sender.NickName;
        PlayerPrefs.SetString("Killer", killer);
        PlayerPrefs.Save();

        currentHealth = currentHealth - damage;
        healthbarImage.fillAmount = currentHealth / maxHealth;
        float healthToDisplay = currentHealth / maxHealth * 100;
        healthText.text = healthToDisplay.ToString() + "%";

        hitAnimator.Play("hit", 0, 0f);


        if(currentHealth <= 0)
        {
            SendChat(killer + " killed " + PhotonNetwork.NickName + "!", true);
            Die();
        }
    }

    void Die()
    {
        playerManager.Die();
    }

    void FallOuttaWorldDie()
    {
        playerManager.MapFallingDeath();
    }

}

