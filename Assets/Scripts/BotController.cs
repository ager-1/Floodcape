using UnityEngine;

public class BotController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float turnSpeed = 150f;
    [SerializeField] private float verticalSpeed = 5f;

    [Header("Rescue Settings")]
    [SerializeField] private Transform holdPoint;
    private GameObject _carriedHuman;

    private float _moveInput, _turnInput, _verticalInput;
    private float _minH = 2f, _maxH = 68f;

    void Update()
    {
        _moveInput = -Input.GetAxisRaw("Vertical");
        _turnInput = Input.GetAxisRaw("Horizontal");

        _verticalInput = 0;
        if (Input.GetKey(KeyCode.Space)) _verticalInput = 1;
        else if (Input.GetKey(KeyCode.LeftShift)) _verticalInput = -1;

        if (Input.GetKeyDown(KeyCode.G) && _carriedHuman == null)
        {
            TryGrabHuman();
        }

        // Check for delivery every frame if we are carrying someone
        // This bypasses the need for physical collisions [cite: 2025-09-03]
        if (_carriedHuman != null)
        {
            CheckForBoatDelivery();
        }
    }

    void TryGrabHuman()
    {
        Collider[] hits = Physics.OverlapBox(transform.position, new Vector3(5f, 5f, 5f));

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Human"))
            {
                _carriedHuman = hit.gameObject;
                _carriedHuman.transform.SetParent(holdPoint);
                _carriedHuman.transform.localPosition = Vector3.zero;

                if (_carriedHuman.TryGetComponent<BoxCollider>(out BoxCollider hCol))
                    hCol.enabled = false;

                Debug.Log("Human Grabbed!");
                break;
            }
        }
    }

    void CheckForBoatDelivery()
    {
        Collider[] closeObjects = Physics.OverlapBox(transform.position, new Vector3(4f, 4f, 4f));

        foreach (var hit in closeObjects)
        {
            if (hit.CompareTag("Player"))
            {
                // FIX: Changed TryAddHuman() to TryAddHumanToBoat() to match MissionManager [cite: 2025-09-03]
                if (MissionManager.Instance != null && MissionManager.Instance.TryAddHumanToBoat())
                {
                    Destroy(_carriedHuman);
                    _carriedHuman = null;
                    Debug.Log("Human delivered to MissionManager!");
                    break;
                }
            }
        }
    }

    void FixedUpdate()
    {
        ApplyRotation();
        ApplyMovement();
    }

    void ApplyRotation()
    {
        float rotationAmount = _turnInput * turnSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, 0, rotationAmount));
    }

    void ApplyMovement()
    {
        Vector3 horizontalDir = transform.up * _moveInput * moveSpeed * Time.fixedDeltaTime;
        float verticalMove = _verticalInput * verticalSpeed * Time.fixedDeltaTime;
        Vector3 targetPos = rb.position + horizontalDir + (Vector3.up * verticalMove);
        targetPos.y = Mathf.Clamp(targetPos.y, _minH, _maxH);
        rb.MovePosition(targetPos);
    }

    // Removed OnTriggerEnter and OnCollisionEnter as CheckForBoatDelivery handles it now
}