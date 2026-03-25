using UnityEngine;

public class BoatController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float turnSpeed = 180f;
    [Header("Wobble Settings")]
    [SerializeField] private float wobbleAmount = 0.2f; /
    [SerializeField] private float wobbleSpeed = 2f;   
    private float _moveInput;
    private float _turnInput;
    private float _startY;
    void Start()
    {
        _startY = transform.position.y;
    }
    void Update()
    {
        _moveInput = Input.GetAxisRaw("Vertical");
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
        Quaternion deltaRotation = Quaternion.Euler(0, 0, rotationAmount);

        rb.MoveRotation(rb.rotation * deltaRotation);
    }
    void ApplyMovement()
    {
        Vector3 moveDirection = transform.up * _moveInput;
        Vector3 targetPosition = rb.position + (moveDirection * moveSpeed * Time.fixedDeltaTime);
        float wobble = Mathf.Sin(Time.time * wobbleSpeed) * wobbleAmount;
        targetPosition.y = _startY + wobble;
        rb.MovePosition(targetPosition);
    }
}