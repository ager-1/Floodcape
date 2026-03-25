using UnityEngine;

public class BoatController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float turnSpeed = 180f;

    [Header("Wobble Settings")]
    [SerializeField] private float wobbleAmount = 0.2f; // How high it bobs
    [SerializeField] private float wobbleSpeed = 2f;    // How fast it bobs

    private float _moveInput;
    private float _turnInput;
    private float _startY;

    void Start()
    {
        // Store the initial Y height so the wobble stays centered
        _startY = transform.position.y;
    }

    void Update()
    {
        // W moves backward, S moves forward [cite: 2025-09-03]
        _moveInput = Input.GetAxisRaw("Vertical");

        // A rotates Left, D rotates Right [cite: 2025-09-03]
        _turnInput = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        ApplyRotation();
        ApplyMovement();
    }

    void ApplyRotation()
    {
        float rotationAmount = _turnInput * turnSpeed * Time.fixedDeltaTime;

        // Rotating on the local Z-axis (blue arrow) as requested
        Quaternion deltaRotation = Quaternion.Euler(0, 0, rotationAmount);

        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    void ApplyMovement()
    {
        // Calculate the base movement position
        // Using transform.up as you noted this fixed your -Z scene movement
        Vector3 moveDirection = transform.up * _moveInput;
        Vector3 targetPosition = rb.position + (moveDirection * moveSpeed * Time.fixedDeltaTime);

        // Calculate the wobble using a Sine wave
        // Mathf.Sin returns a value between -1 and 1
        float wobble = Mathf.Sin(Time.time * wobbleSpeed) * wobbleAmount;

        // Apply the wobble to the Y axis
        targetPosition.y = _startY + wobble;

        rb.MovePosition(targetPosition);
    }
}