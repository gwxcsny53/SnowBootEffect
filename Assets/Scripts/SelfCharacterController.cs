using UnityEngine;
using UnityEngine.InputSystem;

public class SelfCharacterController : MonoBehaviour
{
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
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * walkSpeed * Time.deltaTime);

        // 统一用 moveInput 判断是否在移动（避免与移动逻辑不同步）
        IsMoving = moveInput != Vector2.zero;
    }

}
