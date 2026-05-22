using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float acceleration = 15f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private CharacterController controller;

    [Header("Physics Parameters")]
    [SerializeField] private float gravityScale = 3f;
    public Vector3 currentVelocity { get; private set; }
    public float verticalVelocity = 0f;
    public float currentSpeed { get; private set;}
    public bool isGrounded => controller.isGrounded;


    void Update()
    {
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

        if (isGrounded  && verticalVelocity <= 0.01f)
        {
            verticalVelocity = -3f;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * gravityScale * Time.deltaTime;
        }



        Vector3 fullVelocity = new Vector3(currentVelocity.x, verticalVelocity, currentVelocity.z);

        controller.Move(fullVelocity * Time.deltaTime);
    }

    void ApplyJump()
    {
        if (!isGrounded) return;

        if (!Input.GetButtonDown("Jump")) return;

        verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y * gravityScale);
    }

}