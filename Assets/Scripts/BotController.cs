using UnityEngine;

public class BotController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float turnSpeed = 150f;
    [SerializeField] private float verticalSpeed = 5f;

    [Header("Rescue Settings")]
    [SerializeField] private Transform holdPoint; // Empty object under Bot where human hangs
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

        // Press G to grab human
        if (Input.GetKeyDown(KeyCode.G) && _carriedHuman == null)
        {
            TryGrabHuman();
        }
    }

    void TryGrabHuman()
    {
        // We use a larger OverlapBox to catch flat sprites more easily [cite: 2025-09-03]
        // The 5f, 5f, 5f creates a generous hit zone around the bot [cite: 2025-09-03]
        Collider[] hits = Physics.OverlapBox(transform.position, new Vector3(5f, 5f, 5f));

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Human"))
            {
                _carriedHuman = hit.gameObject;
                _carriedHuman.transform.SetParent(holdPoint);

                // Snap the human to the bot's hanging point [cite: 2025-09-03]
                _carriedHuman.transform.localPosition = Vector3.zero;

                // Critical: Disable the human's collider so it doesn't 
                // hit the bot's own collider while being carried [cite: 2025-09-03]
                if (_carriedHuman.TryGetComponent<BoxCollider>(out BoxCollider hCol))
                    hCol.enabled = false;

                Debug.Log("Human Grabbed!"); 
            break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If bot touches the boat while carrying someone, drop them into the boat
        if (other.CompareTag("Player") && _carriedHuman != null)
        {
            BoatController boat = other.GetComponent<BoatController>();
            if (boat != null && boat.AddHuman())
            {
                Destroy(_carriedHuman); // Remove the physical human as they are now "in" the boat
                _carriedHuman = null;
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
    private void OnCollisionEnter(Collision collision)
    {
        // If we are carrying someone and we bump into the Boat [cite: 2025-09-03]
        if (_carriedHuman != null && collision.gameObject.CompareTag("Player"))
        {
            // Try to find the BoatController on the object we hit [cite: 2025-09-03]
            BoatController boat = collision.gameObject.GetComponent<BoatController>();

            if (boat != null)
            {
                // Try to add the human to the boat's internal counter [cite: 2025-09-03]
                bool success = boat.AddHuman();

                if (success)
                {
                    // If the boat had room, delete the 2D sprite from the bot [cite: 2025-09-03]
                    Destroy(_carriedHuman);
                    _carriedHuman = null;
                    Debug.Log("Human delivered to boat!"); 
            }
                else
                {
                    Debug.Log("Boat is full! Go to SafeZone first.");
            }
            }
        }
    }
}