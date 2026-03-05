using UnityEngine;
using UnityEngine.InputSystem;

public class SelfCharacterController : MonoBehaviour
{
    [Tooltip("角色转向的平滑速度")]
    public float rotationSpeed = 10f; // 建议加上这个变量控制转身快慢
    public float walkSpeed = 5f;
    public float runSpeed = 7f;

    public float jumpForce = 5f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector2 moveInput;

    public bool IsMoving { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            controller = gameObject.AddComponent<CharacterController>();
        }

        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            playerInput = gameObject.AddComponent<PlayerInput>();
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        // 使用 Input System 回调提供的 moveInput 进行移动
        // Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        // controller.Move(move * walkSpeed * Time.deltaTime);

        // // 统一用 moveInput 判断是否在移动（避免与移动逻辑不同步）
        // IsMoving = moveInput != Vector2.zero;

        // 1. 将输入直接作为世界坐标系下的方向 (忽略角色当前的朝向)
        // x 控制世界 X 轴 (左右)，y 控制世界 Z 轴 (前后)
        // Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        // // 如果玩家有输入（方向向量的长度大于 0）
        // if (direction.magnitude >= 0.1f)
        // {
        //     // 2. 【核心转向逻辑】计算目标朝向
        //     // Quaternion.LookRotation 会计算出“看向指定方向”所需的旋转角度
        //     Quaternion targetRotation = Quaternion.LookRotation(direction);

        //     // 3. 【平滑旋转】不要让角色瞬间转过去，使用 Slerp (球面线性插值) 做平滑过渡
        //     transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        //     // 4. 移动角色 (直接向目标方向移动)
        //     controller.Move(direction * walkSpeed * Time.deltaTime);
        // }

        // // 统一用 moveInput 判断是否在移动
        // IsMoving = moveInput != Vector2.zero;



        if (moveInput == Vector2.zero)
        {
            IsMoving = false;
            return;
        }

        IsMoving = true;

        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = (camForward * moveInput.y + camRight * moveInput.x).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        controller.Move(moveDirection * walkSpeed * Time.deltaTime);

    }

}
