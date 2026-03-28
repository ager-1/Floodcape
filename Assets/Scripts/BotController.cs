using UnityEngine;

public class BotController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float turnSpeed = 150f;
    [SerializeField] private float verticalSpeed = 5f;

    [Header("Height Limits")]
    [SerializeField] private float minHeight = 2f;
    [SerializeField] private float maxHeight = 68f;

    private float _moveInput;
    private float _turnInput;
    private float _verticalInput;

    void Update()
    {
        _moveInput = -Input.GetAxisRaw("Vertical"); // W/S
        _turnInput = Input.GetAxisRaw("Horizontal"); // A/D

        // Space to go up, Left Shift to go down [cite: 2025-09-03]
        _verticalInput = 0;
        if (Input.GetKey(KeyCode.Space)) _verticalInput = 1;
        else if (Input.GetKey(KeyCode.LeftShift)) _verticalInput = -1;
    }

    void FixedUpdate()
    {
        ApplyRotation();
        ApplyMovement();
    }

    void ApplyRotation()
    {
        float rotationAmount = _turnInput * turnSpeed * Time.fixedDeltaTime;
        // Adjusting Euler axis to match your bot's horizontal spin [cite: 2025-09-03]
        Quaternion deltaRotation = Quaternion.Euler(0, 0, rotationAmount);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    void ApplyMovement()
    {
        // Using transform.up for horizontal movement as requested for your model [cite: 2025-09-03]
        Vector3 horizontalDir = transform.up * _moveInput * moveSpeed * Time.fixedDeltaTime;
        float verticalMove = _verticalInput * verticalSpeed * Time.fixedDeltaTime;

        Vector3 targetPosition = rb.position + horizontalDir + (Vector3.up * verticalMove);

        // Clamping height between 2 and 68 [cite: 2025-09-03]
        targetPosition.y = Mathf.Clamp(targetPosition.y, minHeight, maxHeight);

        rb.MovePosition(targetPosition);
    }
}