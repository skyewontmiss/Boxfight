using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class RigidbodyFirstPersonController_Local : MonoBehaviour
    {

        [SerializeField] Item[] items;
        public static RigidbodyFirstPersonController_Local Instance;

        public int itemIndex;
        public int previousItemIndex = -1;
        public MeshRenderer me;
        public Material[] possibleSkins;

        [Serializable]
        public class MovementSettings
        {
            public float ForwardSpeed = 8.0f;   // Speed when walking forward
            public float BackwardSpeed = 4.0f;  // Speed when walking backwards
            public float StrafeSpeed = 4.0f;    // Speed when walking sideways
            public float RunMultiplier = 2.0f;   // Speed when sprinting
            public KeyCode RunKey = KeyCode.LeftShift;
            public float JumpForce = 30f;
            public AnimationCurve SlopeCurveModifier = new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(90.0f, 0.0f));
            [HideInInspector] public float CurrentTargetSpeed = 8f;

#if !MOBILE_INPUT
            private bool m_Running;
#endif

            public void UpdateDesiredTargetSpeed(Vector2 input)
            {
                if (input == Vector2.zero) return;
                if (input.x > 0 || input.x < 0)
                {
                    //strafe
                    CurrentTargetSpeed = StrafeSpeed;
                }
                if (input.y < 0)
                {
                    //backwards
                    CurrentTargetSpeed = BackwardSpeed;
                }
                if (input.y > 0)
                {
                    //forwards
                    //handled last as if strafing and moving forward at the same time forwards speed should take precedence
                    CurrentTargetSpeed = ForwardSpeed;
                }
#if !MOBILE_INPUT
                if (Input.GetKey(RunKey))
                {
                    CurrentTargetSpeed *= RunMultiplier;
                    m_Running = true;
                }
                else
                {
                    m_Running = false;
                }
#endif
            }

#if !MOBILE_INPUT
            public bool Running
            {
                get { return m_Running; }
            }
#endif
        }



        [Serializable]
        public class AdvancedSettings
        {
            public float groundCheckDistance = 0.01f; // distance for checking if the controller is grounded ( 0.01f seems to work best for this )
            public float stickToGroundHelperDistance = 0.5f; // stops the character
            public float slowDownRate = 20f; // rate at which the controller comes to a stop when there is no input
            public bool airControl; // can the user control the direction that is being moved in the air
            [Tooltip("set it to 0.1 or more if you get stuck in wall")]
            public float shellOffset; //reduce the radius by that ratio to avoid getting stuck in wall (a value of 0.1f is nice)
        }


        Camera cam;
        public MovementSettings movementSettings = new MovementSettings();
        public MouseLook mouseLook = new MouseLook();
        public AdvancedSettings advancedSettings = new AdvancedSettings();


        private Rigidbody m_RigidBody;
        private CapsuleCollider m_Capsule;
        private float m_YRotation;
        private Vector3 m_GroundContactNormal;
        private bool m_Jump, m_PreviouslyGrounded, m_Jumping, m_IsGrounded;
        private bool paused;
        private bool animating;
        public Animator pauseMenuAnimator;
        public bool isAiming;

        public Vector3 Velocity
        {
            get { return m_RigidBody.velocity; }
        }

        public bool Grounded
        {
            get { return m_IsGrounded; }
        }

        public bool Jumping
        {
            get { return m_Jumping; }
        }

        public bool Running
        {
            get
            {
#if !MOBILE_INPUT
                return movementSettings.Running;
#else
	            return false;
#endif
            }
        }
        public GameObject postProcessing;
        public GameObject fpsCounter;

        private void Awake()
        {
            cam = GetComponentInChildren<Camera>();

            EquipItem(0);
            LoadSettings();
            me.material = possibleSkins[Random.Range(0, possibleSkins.Length)];

        }

        private void LoadSettings()
        {
            //mouse sensetivity settings
            if (PlayerPrefs.HasKey("MouseX"))
            {
                MouseLook.XSensitivity = MouseLook.XSensitivity * PlayerPrefs.GetFloat("MouseX");
            }
            else
            {
                MouseLook.XSensitivity = 1;
            }

            if (PlayerPrefs.HasKey("MouseY"))
            {
                MouseLook.YSensitivity = MouseLook.YSensitivity * PlayerPrefs.GetFloat("MouseY");
            }
            else
            {
                MouseLook.YSensitivity = 1;
            }

            //post processing

            //insert code here

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
                cam.fieldOfView = PlayerPrefs.GetInt("FOV");

            }


            //skins
            //photonView.RPC("ChangeSkin", RpcTarget.All, random);#

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

            if (PlayerPrefs.HasKey("FOV"))
            {
                cam.fieldOfView = PlayerPrefs.GetInt("FOV");

            }


        }


        private void Start()
        {
            m_RigidBody = GetComponent<Rigidbody>();
            m_Capsule = GetComponent<CapsuleCollider>();

            mouseLook.Init (transform, cam.transform);

        }

        void PauseMenuCheck()
        {
            if (Input.GetKey(KeyCode.Escape))
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

        public void Resume()
        {
            StartCoroutine(CloseMenu());
        }

        public void LeaveMatch()
        {
            SceneManager.LoadScene(0);
        }



        private void Update()
        {
            RotateView();
            PauseMenuCheck();

            if (CrossPlatformInputManager.GetButtonDown("Jump") && !m_Jump)
            {
                if (paused)
                    return;

                m_Jump = true;
            }


            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (paused)
                    return;

                items[itemIndex].Use();
            }


            if (!isAiming)
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

        }

        void EquipItem(int _index)
        {
            if (paused)
                return;

            if (_index == previousItemIndex)
                return;

            itemIndex = _index;

            items[itemIndex].itemGameObject.SetActive(true);

            if (previousItemIndex != -1)
            {
                items[previousItemIndex].itemGameObject.SetActive(false);
            }

            previousItemIndex = itemIndex;

        }


        private void FixedUpdate()
        {
            if (paused)
                return;

                GroundCheck();
            Vector2 input = GetInput();

            if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon) && (advancedSettings.airControl || m_IsGrounded))
            {
                // always move along the camera forward as it is the direction that it being aimed at
                Vector3 desiredMove = cam.transform.forward*input.y + cam.transform.right*input.x;
                desiredMove = Vector3.ProjectOnPlane(desiredMove, m_GroundContactNormal).normalized;

                desiredMove.x = desiredMove.x*movementSettings.CurrentTargetSpeed;
                desiredMove.z = desiredMove.z*movementSettings.CurrentTargetSpeed;
                desiredMove.y = desiredMove.y*movementSettings.CurrentTargetSpeed;
                if (m_RigidBody.velocity.sqrMagnitude <
                    (movementSettings.CurrentTargetSpeed*movementSettings.CurrentTargetSpeed))
                {
                    m_RigidBody.AddForce(desiredMove*SlopeMultiplier(), ForceMode.Impulse);
                }
            }

            if (m_IsGrounded)
            {
                m_RigidBody.drag = 5f;

                if (m_Jump)
                {
                    m_RigidBody.drag = 0f;
                    m_RigidBody.velocity = new Vector3(m_RigidBody.velocity.x, 0f, m_RigidBody.velocity.z);
                    m_RigidBody.AddForce(new Vector3(0f, movementSettings.JumpForce, 0f), ForceMode.Impulse);
                    m_Jumping = true;
                }

                if (!m_Jumping && Mathf.Abs(input.x) < float.Epsilon && Mathf.Abs(input.y) < float.Epsilon && m_RigidBody.velocity.magnitude < 1f)
                {
                    m_RigidBody.Sleep();
                }
            }
            else
            {
                m_RigidBody.drag = 0f;
                if (m_PreviouslyGrounded && !m_Jumping)
                {
                    StickToGroundHelper();
                }
            }
            m_Jump = false;
        }


        private float SlopeMultiplier()
        {

            float angle = Vector3.Angle(m_GroundContactNormal, Vector3.up);
            return movementSettings.SlopeCurveModifier.Evaluate(angle);
        }


        private void StickToGroundHelper()
        {
            if (paused)
                return;

            RaycastHit hitInfo;
            if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo,
                                   ((m_Capsule.height/2f) - m_Capsule.radius) +
                                   advancedSettings.stickToGroundHelperDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                if (Mathf.Abs(Vector3.Angle(hitInfo.normal, Vector3.up)) < 85f)
                {
                    m_RigidBody.velocity = Vector3.ProjectOnPlane(m_RigidBody.velocity, hitInfo.normal);
                }
            }
        }


        private Vector2 GetInput()
        {


            Vector2 input = new Vector2
                {

            x = CrossPlatformInputManager.GetAxis("Horizontal"),
                    y = CrossPlatformInputManager.GetAxis("Vertical")
                };
			movementSettings.UpdateDesiredTargetSpeed(input);

            return input;
        }


        private void RotateView()
        {
            if (paused)
                return;

            //avoids the mouse looking if the game is effectively paused
            if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

            // get the rotation before it's changed
            float oldYRotation = transform.eulerAngles.y;

            mouseLook.LookRotation (transform, cam.transform);

            if (m_IsGrounded || advancedSettings.airControl)
            {
                // Rotate the rigidbody velocity to match the new direction that the character is looking
                Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
                m_RigidBody.velocity = velRotation*m_RigidBody.velocity;
            }
        }

        /// sphere cast down just beyond the bottom of the capsule to see if the capsule is colliding round the bottom
        private void GroundCheck()
        {
            if (paused)
                return;

            m_PreviouslyGrounded = m_IsGrounded;
            RaycastHit hitInfo;
            if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo,
                                   ((m_Capsule.height/2f) - m_Capsule.radius) + advancedSettings.groundCheckDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                m_IsGrounded = true;
                m_GroundContactNormal = hitInfo.normal;
            }
            else
            {
                m_IsGrounded = false;
                m_GroundContactNormal = Vector3.up;
            }
            if (!m_PreviouslyGrounded && m_IsGrounded && m_Jumping)
            {
                m_Jumping = false;
            }
        }
    }
}
