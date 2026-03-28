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
        if (target != null)
        {
            _offset = transform.position - target.position;
        }

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

        // Follow the current target while maintaining the isometric offset [cite: 2025-09-03]
        Vector3 desiredPosition = target.position + _offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    }

    // Call this to swap between boat and bot [cite: 2025-09-03]
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void HandleZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollInput) > 0.01f)
        {
            float newSize = cam.orthographicSize - (scrollInput * zoomSpeed);
            cam.orthographicSize = Mathf.Clamp(newSize, minSize, maxSize);
        }
    }
}