using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof (CharacterController))]
    [RequireComponent(typeof (AudioSource))]
    public class FirstPersonController : MonoBehaviour
    {
        [SerializeField] private bool m_IsWalking;
        [SerializeField] private bool m_IsCrouched;
        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_RunSpeed;
        [SerializeField] private float m_CrouchSpeed;
        [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
        [SerializeField] private float m_JumpSpeed;
        [SerializeField] private float m_StickToGroundForce;
        [SerializeField] private float m_GravityMultiplier;
        [SerializeField] private MouseLook m_MouseLook;
        [SerializeField] private bool m_UseFovKick;
        [SerializeField] private FOVKick m_FovKick = new FOVKick();
        [SerializeField] private bool m_UseHeadBob;
        [SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();
        [SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob();
        [SerializeField] private float m_StepInterval;
        [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
        [SerializeField] private AudioClip[] m_FootstepSoundsCrouch;
        [SerializeField] private AudioClip[] m_FootstepSoundsWalk;
        [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
        [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.
        [SerializeField] private AudioSource OutOfBreath;           // the sound played when character touches back on ground.

        private Camera m_Camera;
        private bool m_Jump;
        private float m_YRotation;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        private bool m_Jumping;
        private AudioSource m_AudioSource;

        private Slider sprintBarSlider;
        private GameObject sprintBarObject;
        private float sprintBar, startRunSpeed;
        public bool hiding, sprinting, isGOD;
        public BoxCollider headChecker;
        public AudioMixerGroup audioMixer;

        // Use this for initialization
        private void Start()
        {
            hiding = false;
            sprinting = false;
            startRunSpeed = m_RunSpeed;
            
            m_CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_FovKick.Setup(m_Camera);
            m_HeadBob.Setup(m_Camera, m_StepInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle/2f;
            m_Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
			m_MouseLook.Init(transform , m_Camera.transform);

            sprintBarObject = GameObject.FindWithTag("SprintCharge");
            sprintBarSlider = sprintBarObject.GetComponent<Slider>();
            sprintBar = 300f;

            m_AudioSource.outputAudioMixerGroup = audioMixer;
        }

        // Update is called once per frame
        private void Update()
        {
            if (Time.timeScale != 0) {
                RotateView();
                // the jump state needs to read here to make sure it is not missed
                if (!m_Jump)
                {
                    m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
                }

                if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
                {
                    StartCoroutine(m_JumpBob.DoBobCycle());
                    PlayLandingSound();
                    m_MoveDir.y = 0f;
                    m_Jumping = false;
                }
                if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
                {
                    m_MoveDir.y = 0f;
                }

                m_PreviouslyGrounded = m_CharacterController.isGrounded;

                if (sprintBar == 300f) {
                    sprintBarObject.SetActive(false);
                } else {
                    sprintBarObject.SetActive(true);
                    sprintBarSlider.value = sprintBar/300f;
                }
            } else {
                sprintBarObject.SetActive(false);
            }
        }


        private void PlayLandingSound()
        {
            m_AudioSource.clip = m_LandSound;
            m_AudioSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }


        private void FixedUpdate()
        {
            float speed;
            GetInput(out speed);

            if (!m_CharacterController.enabled) m_CharacterController.enabled = true;
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward*m_Input.y + transform.right*m_Input.x;

            sprinting = !m_IsWalking;
            if (!m_IsWalking && (sprintBar > 0 || isGOD)) {
                if (desiredMove.magnitude > 0) sprintBar -= 1f;
                m_RunSpeed = startRunSpeed;
            } else if (!m_IsWalking) {
                sprinting = false;
                sprintBar = 0f;
                if(m_RunSpeed != m_WalkSpeed) {
                    m_RunSpeed = m_WalkSpeed;
                    speed = m_WalkSpeed;
                    OutOfBreath.Play();
                }
            } else if (sprintBar < 300f) {
                if (desiredMove.magnitude == 0) sprintBar += .9f;
                sprintBar += .45f;
            } else {
                sprintBar = 300f;
            }

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                               m_CharacterController.height/2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDir.x = desiredMove.x*speed;
            m_MoveDir.z = desiredMove.z*speed;


            if (m_CharacterController.isGrounded)
            {
                m_MoveDir.y = -m_StickToGroundForce;

                if (m_Jump)
                {
                    m_MoveDir.y = m_JumpSpeed;
                    PlayJumpSound();
                    m_Jump = false;
                    m_Jumping = true;
                }
            }
            else
            {
                m_MoveDir += Physics.gravity*m_GravityMultiplier*Time.fixedDeltaTime;
            }
            m_CollisionFlags = m_CharacterController.Move(m_MoveDir*Time.fixedDeltaTime);

            ProgressStepCycle(speed);
            UpdateCameraPosition(speed);

            m_MouseLook.UpdateCursorLock();
        }


        private void PlayJumpSound()
        {
            m_AudioSource.clip = m_JumpSound;
            m_AudioSource.Play();
        }


        private void ProgressStepCycle(float speed)
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed*(m_IsWalking ? 1f : m_RunstepLenghten)))*
                             Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio(speed);
        }


        private void PlayFootStepAudio(float speed)
        {
            if (!m_CharacterController.isGrounded)
            {
                return;
            }
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            if (speed < 4f) {
                int n = Random.Range(1, m_FootstepSoundsCrouch.Length);
                m_AudioSource.clip = m_FootstepSoundsCrouch[n];
                m_AudioSource.PlayOneShot(m_AudioSource.clip);
                // move picked sound to index 0 so it's not picked next time
                m_FootstepSoundsCrouch[n] = m_FootstepSoundsCrouch[0];
                m_FootstepSoundsCrouch[0] = m_AudioSource.clip;
            } else if (speed < 8f) {
                int n = Random.Range(1, m_FootstepSoundsWalk.Length);
                m_AudioSource.clip = m_FootstepSoundsWalk[n];
                m_AudioSource.PlayOneShot(m_AudioSource.clip);
                // move picked sound to index 0 so it's not picked next time
                m_FootstepSoundsWalk[n] = m_FootstepSoundsWalk[0];
                m_FootstepSoundsWalk[0] = m_AudioSource.clip;
            } else {
                int n = Random.Range(1, m_FootstepSounds.Length);
                m_AudioSource.clip = m_FootstepSounds[n];
                m_AudioSource.PlayOneShot(m_AudioSource.clip);
                // move picked sound to index 0 so it's not picked next time
                m_FootstepSounds[n] = m_FootstepSounds[0];
                m_FootstepSounds[0] = m_AudioSource.clip;
            }
        }


        private void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;
            if (!m_UseHeadBob)
            {
                return;
            }
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
            {
                m_Camera.transform.localPosition =
                    m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                      (speed*(m_IsWalking ? 1f : m_RunstepLenghten)));
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
            }
            else
            {
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
            }

            if ((m_IsCrouched && m_IsWalking) || hiding) {
                newCameraPosition.y -= .5f;
                if (hiding && newCameraPosition.y > .45f) newCameraPosition.y = .45f;
            }
            m_Camera.transform.localPosition = newCameraPosition;
        }


        private void GetInput(out float speed)
        {
            // Read input
            float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            float vertical = CrossPlatformInputManager.GetAxis("Vertical");

            bool waswalking = m_IsWalking;

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            m_IsWalking = !Input.GetKey(KeyCode.LeftShift);

            if (Application.platform == RuntimePlatform.WebGLPlayer) {
                m_IsCrouched = Input.GetKey(KeyCode.C); // WEBGL CROUCHING ONLY
            } else {
                m_IsCrouched = Input.GetKey(KeyCode.LeftControl) || 
                Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.C);
            }
#endif
            // set the desired speed to be walking or running
            if ((m_IsCrouched && m_IsWalking) || hiding) {
                speed = m_CrouchSpeed;
                if (m_CharacterController.height != 1f) {
                    m_CharacterController.height = 1f;

                    headChecker.enabled = true;
                    
                    Vector3 newPositon;
                    newPositon = transform.localPosition;
                    newPositon.y -= .5f;
                    transform.localPosition = newPositon;
                    m_CharacterController.enabled = false;
                }
                
            } else {
                headChecker.enabled = false;
                speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
                if (m_CharacterController.height != 2f) {
                    m_CharacterController.height = 2f;

                    hiding = false;
                    Vector3 newPositon;
                    newPositon = transform.localPosition;
                    newPositon.y += .5f;
                    transform.localPosition = newPositon;
                    m_CharacterController.enabled = false;
                }
            }
            
            m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
            }
        }

        private void RotateView()
        {
            m_MouseLook.LookRotation (transform, m_Camera.transform);
        }


        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(m_CharacterController.velocity*0.1f, hit.point, ForceMode.Impulse);
        }
    }
}
