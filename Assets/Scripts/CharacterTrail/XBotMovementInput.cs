using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("移动设置")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float gravity = -9.81f;
    
    [Header("组件引用")]
    private CharacterController characterController;
    private Animator animator;
    
    // 输入变量
    private float horizontal;
    private float vertical;
    private Vector3 moveDirection;
    private Vector3 velocity;
    
    // 动画参数名称
    private readonly string IDLE_ANIM = "Idle";
    private readonly string RUNNING_ANIM = "Running";
    
    void Start()
    {
        // 延迟一帧初始化，避免序列化问题
        StartCoroutine(InitializeComponents());
    }
    
    System.Collections.IEnumerator InitializeComponents()
    {
        yield return null; // 等待一帧
        
        // 获取组件
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        
        // 检查必要组件
        if (characterController == null)
        {
            Debug.LogError("PlayerMovement: 缺少Character Controller组件！");
        }
        
        if (animator == null)
        {
            Debug.LogError("PlayerMovement: 缺少Animator组件！");
        }
    }
    
    void Update()
    {
        // 获取输入
        GetInput();
        
        // 处理移动
        HandleMovement();
        
        // 处理重力
        HandleGravity();
        
        // 应用移动
        ApplyMovement();
        
        // 处理旋转
        HandleRotation();
        
        // 处理动画
        HandleAnimation();
    }
    
    /// <summary>
    /// 获取玩家输入
    /// </summary>
    void GetInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        
        // 计算移动方向（世界坐标系）
        moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
    }
    
    /// <summary>
    /// 处理玩家移动
    /// </summary>
    void HandleMovement()
    {
        // 如果没有输入，清零移动方向
        if (moveDirection.magnitude < 0.1f)
        {
            moveDirection = Vector3.zero;
        }
    }
    
    /// <summary>
    /// 处理重力
    /// </summary>
    void HandleGravity()
    {
        if (characterController == null) return;
        
        // 如果角色在地面上，重置垂直速度
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // 小的负值确保角色贴地
        }
        
        // 应用重力
        velocity.y += gravity * Time.deltaTime;
    }
    
    /// <summary>
    /// 应用移动到Character Controller
    /// </summary>
    void ApplyMovement()
    {
        if (characterController == null) return;
        
        // 计算最终移动向量
        Vector3 finalMovement = moveDirection * moveSpeed * Time.deltaTime;
        finalMovement.y = velocity.y * Time.deltaTime;
        
        // 使用Character Controller移动
        characterController.Move(finalMovement);
    }
    
    /// <summary>
    /// 处理玩家旋转
    /// </summary>
    void HandleRotation()
    {
        if (moveDirection != Vector3.zero)
        {
            // 计算目标旋转
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            
            // 平滑旋转
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 
                rotationSpeed * Time.deltaTime);
        }
    }
    
    /// <summary>
    /// 处理动画状态
    /// </summary>
    void HandleAnimation()
    {
        if (animator == null) return;
        
        // 判断是否在移动
        bool isMoving = moveDirection.magnitude > 0.1f;
        
        if (isMoving)
        {
            // 播放跑步动画
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(RUNNING_ANIM))
            {
                animator.Play(RUNNING_ANIM);
            }
        }
        else
        {
            // 播放待机动画
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(IDLE_ANIM))
            {
                animator.Play(IDLE_ANIM);
            }
        }
    }
    
    /// <summary>
    /// 设置移动速度
    /// </summary>
    /// <param name="speed">新的移动速度</param>
    public void SetMoveSpeed(float speed)
    {
        moveSpeed = Mathf.Max(0f, speed);
    }
    
    /// <summary>
    /// 设置旋转速度
    /// </summary>
    /// <param name="speed">新的旋转速度</param>
    public void SetRotationSpeed(float speed)
    {
        rotationSpeed = Mathf.Max(0f, speed);
    }
    
    /// <summary>
    /// 设置重力值
    /// </summary>
    /// <param name="gravityValue">新的重力值</param>
    public void SetGravity(float gravityValue)
    {
        gravity = gravityValue;
    }
    
    /// <summary>
    /// 获取当前移动状态
    /// </summary>
    /// <returns>是否正在移动</returns>
    public bool IsMoving()
    {
        return moveDirection.magnitude > 0.1f;
    }
    
    /// <summary>
    /// 检查是否在地面上
    /// </summary>
    /// <returns>是否在地面</returns>
    public bool IsGrounded()
    {
        return characterController != null && characterController.isGrounded;
    }
    
    /// <summary>
    /// 强制播放指定动画
    /// </summary>
    /// <param name="animationName">动画名称</param>
    public void PlayAnimation(string animationName)
    {
        if (animator != null)
        {
            animator.Play(animationName);
        }
    }
    
    /// <summary>
    /// 添加外力（比如击退效果）
    /// </summary>
    /// <param name="force">力的方向和大小</param>
    public void AddForce(Vector3 force)
    {
        velocity += force;
    }
}