using UnityEngine;

public class SelfCharacterController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 7f;

    public float jumpForce = 5f;
    public float gravity = -9.81f;

    private CharacterController controller;

    // private  ;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {

    }

}
