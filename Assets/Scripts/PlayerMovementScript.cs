using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float acceleration = 15f;
    [SerializeField] private float jumpForce = 3f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private CharacterController controller;

    public Vector3 currentVelocity { get; private set; }
    public float currentSpeed { get; private set;}


    private bool jumpRequested;

    void Update()
    {
        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
            jumpRequested = true;

        ApplyMovement();
        ApplyJump();
    }


    void ApplyMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 motion = transform.forward * z + transform.right * x;
        motion.y =  0f;
        motion.Normalize();

        if (motion.sqrMagnitude >= 0.01f)
        {
            currentVelocity = Vector3.MoveTowards(currentVelocity, motion * maxSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, acceleration * Time.deltaTime);
        }

        float verticalVelocity = Physics.gravity.y * 20f * Time.deltaTime;

        Vector3 fullVelocity = new Vector3(currentVelocity.x, verticalVelocity, currentVelocity.z);

        controller.Move(fullVelocity * Time.deltaTime);
    }

    void ApplyJump()
    {
        if (!jumpRequested) return;

        controller.Move(Vector3.up * jumpForce * Time.deltaTime);

        Debug.Log("Jumping with force: " + jumpForce);
        
        jumpRequested = false;
    }

    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }
}