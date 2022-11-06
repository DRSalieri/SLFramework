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
        [SerializeField] private Transform playerTransform;                 // 自身的Transform
        [SerializeField] private Animator animator;                         // 动画控制器
        [SerializeField] private Transform cameraTransform;                 // 摄像机的Transfomr
        [SerializeField] private CharacterController characterController;   // cc

        [Header("Input")]
        [SerializeField] private Vector2 moveInput;
        [SerializeField] private bool isRunning;
        [SerializeField] private bool isCrouch;
        
        [Header("Movement Settings")]
        [SerializeField] private float crouchSpeed = 1.5f;  // 蹲走速度
        [SerializeField] private float walkSpeed = 2.5f;    // 行走速度
        [SerializeField] private float runSpeed = 5.5f;     // 奔跑速度
        [SerializeField] private float gravity = -15f;      // 重力

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
            // Components变量初始化
            playerTransform = transform;
            animator = GetComponent<Animator>();                        
            cameraTransform = Camera.main.transform;
            characterController = GetComponent<CharacterController>();

            // Hash初始化
            postureHash = Animator.StringToHash("Hand_Free_Status");
            moveSpeedHash = Animator.StringToHash("MoveSpeed");
            turnSpeedHash = Animator.StringToHash("TurnSpeed");

            // 指针消隐
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            SwitchPlayerStates();       // 更新Enum变量
            CalculateInputDirection();  // 计算输入所导致的角色移动
            SetupAnimator();            // 设置角色动画
        }

        #endregion

        #region Input System

        // 获取移动输入
        public void GetMoveInput(InputAction.CallbackContext ctx)
        {
            moveInput = ctx.ReadValue<Vector2>();
        }
        // 获取奔跑键是否输入
        public void GetRunInput(InputAction.CallbackContext ctx)
        {
            isRunning = ctx.ReadValueAsButton();
        }
        // 获取蹲键是否输入
        public void GetCrouchInput(InputAction.CallbackContext ctx)
        {
            isCrouch = ctx.ReadValueAsButton();
        }

        #endregion

        #region Movement Handler Function

        /// <summary>
        /// 更新角色状态（Enum变量）
        /// </summary>
        void SwitchPlayerStates()
        {
            // 更新PlayerPosture
            if(isCrouch)
            {
                playerPosture = PlayerPosture.Crouch;
            }
            else
            {
                playerPosture = PlayerPosture.Stand;
            }

            // 更新LocomotionState
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
        /// 计算输入方向（算playerMovement）
        /// </summary>
        void CalculateInputDirection()
        {
            // 相机的前方在xOz平面上的投影
            Vector3 camForwardProjection = new Vector3(
                cameraTransform.forward.x,
                0, 
                cameraTransform.forward.z).normalized;

            // 获得相对于相机方向的输入
            playerMovement =
                camForwardProjection * moveInput.y 
                + cameraTransform.right * moveInput.x;

            // 转换坐标系
            playerMovement =
                playerTransform.InverseTransformVector(playerMovement);
        }

        /// <summary>
        /// 设置角色动画
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

            // 设置转向速度
            float rad = Mathf.Atan2(playerMovement.x, playerMovement.z);
            animator.SetFloat(turnSpeedHash, rad, 0.1f, Time.deltaTime);
            // 转向速度慢，人为添加转向
            playerTransform.Rotate(0f, rad * 180 * Time.deltaTime, 0f);
            
        }
        #endregion
    }
}
