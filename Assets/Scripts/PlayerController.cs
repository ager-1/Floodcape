using UnityEngine;

public class BoatController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float turnSpeed = 180f;

    private float _moveInput;
    private float _turnInput;

    void Update()
    {
        // W moves backward, S moves forward
        _moveInput = -Input.GetAxisRaw("Vertical");

        // A rotates Left (negative Y), D rotates Right (positive Y)
        _turnInput = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        ApplyRotation();
        ApplyMovement();
    }

    void ApplyRotation()
    {
        // Calculate the turn amount for this physics frame
        float rotationAmount = _turnInput * turnSpeed * Time.fixedDeltaTime;

        // Create a rotation around the Y axis
        Quaternion deltaRotation = Quaternion.Euler(0, rotationAmount, 0);

        // Apply the rotation to the Rigidbody
        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    void ApplyMovement()
    {
        if (Mathf.Abs(_moveInput) > 0.1f)
        {
            // Move along the boat's own forward axis
            Vector3 moveDirection = transform.forward * _moveInput;

            Vector3 movement = moveDirection * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);
        }
    }
}