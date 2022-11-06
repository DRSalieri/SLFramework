using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SLFramework.Controller
{
    public class Controller3DBase : MonoBehaviour
    {
        #region Enum

        public enum PlayerPosture
        {
            Crouch,
            Stand,
        };

        public enum LocomotionState
        {
            Idle,
            Walk,
            Run
        };

        #endregion

        [Header("Components")]
        [SerializeField] private Transform playerTransform;                 // �����Transform
        [SerializeField] private Animator animator;                         // ����������
        [SerializeField] private Transform cameraTransform;                 // �������Transfomr
        [SerializeField] private CharacterController characterController;   // cc

        [Header("Input")]
        [SerializeField] private Vector2 moveInput;
        [SerializeField] private bool isRunning;
        [SerializeField] private bool isCrouch;
        
        [Header("Movement Settings")]
        [SerializeField] private float crouchSpeed = 1.5f;  // �����ٶ�
        [SerializeField] private float walkSpeed = 2.5f;    // �����ٶ�
        [SerializeField] private float runSpeed = 5.5f;     // �����ٶ�
        [SerializeField] private float gravity = -15f;      // ����

        [Header("Animator Settings")]
        [SerializeField] private float crouchThreshold = 0f;
        [SerializeField] private float standThreshold = 1f;
        
        [Space]
        [Header("Enum States")]
        public PlayerPosture playerPosture = PlayerPosture.Stand;
        public LocomotionState locomotionState = LocomotionState.Idle;

        [Header("Runtime Variables")]
        public Vector3 playerMovement = Vector3.zero;
        


        #region Hash

        private int postureHash;
        private int moveSpeedHash;
        private int turnSpeedHash;
        private int verticalSpeedHash;
        private int leftFeetHash;

        #endregion

        #region MonoBehaviour Function

        private void Start()
        {
            // Components������ʼ��
            playerTransform = transform;
            animator = GetComponent<Animator>();                        
            cameraTransform = Camera.main.transform;
            characterController = GetComponent<CharacterController>();

            // Hash��ʼ��
            postureHash = Animator.StringToHash("Hand_Free_Status");
            moveSpeedHash = Animator.StringToHash("MoveSpeed");
            turnSpeedHash = Animator.StringToHash("TurnSpeed");

            // ָ������
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            SwitchPlayerStates();       // ����Enum����
            CalculateInputDirection();  // �������������µĽ�ɫ�ƶ�
            SetupAnimator();            // ���ý�ɫ����
        }

        #endregion

        #region Input System

        // ��ȡ�ƶ�����
        public void GetMoveInput(InputAction.CallbackContext ctx)
        {
            moveInput = ctx.ReadValue<Vector2>();
        }
        // ��ȡ���ܼ��Ƿ�����
        public void GetRunInput(InputAction.CallbackContext ctx)
        {
            isRunning = ctx.ReadValueAsButton();
        }
        // ��ȡ�׼��Ƿ�����
        public void GetCrouchInput(InputAction.CallbackContext ctx)
        {
            isCrouch = ctx.ReadValueAsButton();
        }

        #endregion

        #region Movement Handler Function

        /// <summary>
        /// ���½�ɫ״̬��Enum������
        /// </summary>
        void SwitchPlayerStates()
        {
            // ����PlayerPosture
            if(isCrouch)
            {
                playerPosture = PlayerPosture.Crouch;
            }
            else
            {
                playerPosture = PlayerPosture.Stand;
            }

            // ����LocomotionState
            if(moveInput.magnitude == 0)
            {
                locomotionState = LocomotionState.Idle;
            }
            else if(!isRunning)
            {
                locomotionState = LocomotionState.Walk;
            }
            else
            {
                locomotionState = LocomotionState.Run;
            }
        }

        /// <summary>
        /// �������뷽����playerMovement��
        /// </summary>
        void CalculateInputDirection()
        {
            // �����ǰ����xOzƽ���ϵ�ͶӰ
            Vector3 camForwardProjection = new Vector3(
                cameraTransform.forward.x,
                0, 
                cameraTransform.forward.z).normalized;

            // ��������������������
            playerMovement =
                camForwardProjection * moveInput.y 
                + cameraTransform.right * moveInput.x;

            // ת������ϵ
            playerMovement =
                playerTransform.InverseTransformVector(playerMovement);
        }

        /// <summary>
        /// ���ý�ɫ����
        /// </summary>
        void SetupAnimator()
        {
            if(playerPosture == PlayerPosture.Stand)
            {
                animator.SetFloat(postureHash, standThreshold, 0.1f, Time.deltaTime);
                switch (locomotionState)
                {
                    case LocomotionState.Idle:
                        animator.SetFloat(moveSpeedHash, 0, 0.1f, Time.deltaTime);
                        break;
                    case LocomotionState.Walk:
                        animator.SetFloat(moveSpeedHash, playerMovement.magnitude * walkSpeed, 0.1f, Time.deltaTime);
                        break;
                    case LocomotionState.Run:
                        animator.SetFloat(moveSpeedHash, playerMovement.magnitude * runSpeed, 0.1f, Time.deltaTime);
                        break;
                }
            }
            else if (playerPosture == PlayerPosture.Crouch)
            {
                animator.SetFloat(postureHash, crouchThreshold, 0.1f, Time.deltaTime);
                switch (locomotionState)
                {
                    case LocomotionState.Idle:
                        animator.SetFloat(moveSpeedHash, 0, 0.1f, Time.deltaTime);
                        break;
                    default:
                        animator.SetFloat(moveSpeedHash, playerMovement.magnitude * crouchSpeed, 0.1f, Time.deltaTime);
                        break;
                }
            }

            // ����ת���ٶ�
            float rad = Mathf.Atan2(playerMovement.x, playerMovement.z);
            animator.SetFloat(turnSpeedHash, rad, 0.1f, Time.deltaTime);
            // ת���ٶ�������Ϊ���ת��
            playerTransform.Rotate(0f, rad * 180 * Time.deltaTime, 0f);
            
        }
        #endregion
    }
}
