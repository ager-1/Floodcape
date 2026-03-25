using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 0.125f;

    [Header("Zoom Settings")]
    [SerializeField] private Camera cam;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minSize = 19f;
    [SerializeField] private float maxSize = 23f;

    private Vector3 _offset;

    void Start()
    {
        // Capture the initial distance between camera and boat [cite: 2025-09-03]
        if (target != null)
        {
            _offset = transform.position - target.position;
        }

        // Auto-assign the camera component if left empty in Inspector [cite: 2025-09-03]
        if (cam == null)
        {
            cam = GetComponent<Camera>();
        }
    }

    void Update()
    {
        HandleZoom();
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Maintain the offset so the angle never changes [cite: 2025-09-03]
        Vector3 desiredPosition = target.position + _offset;

        // Smoothly move to the new position to avoid physics jitter [cite: 2025-09-03]
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    }

    void HandleZoom()
    {
        // Get the mouse wheel input [cite: 2025-09-03]
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scrollInput) > 0.01f)
        {
            // Calculate new orthographic size [cite: 2025-09-03]
            float newSize = cam.orthographicSize - (scrollInput * zoomSpeed);

            // Clamp the size between your requested 19 and 23 [cite: 2025-09-03]
            cam.orthographicSize = Mathf.Clamp(newSize, minSize, maxSize);
        }
    }
}