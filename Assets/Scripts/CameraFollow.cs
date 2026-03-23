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

        // Automatically find the camera component if not assigned
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

        Vector3 desiredPosition = target.position + _offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    }

    void HandleZoom()
    {
        // Get the scroll wheel input (usually returns -0.1 to 0.1)
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scrollInput) > 0.01f)
        {
            // Subtracting the input makes scrolling 'Up' zoom in (smaller size)
            float newSize = cam.orthographicSize - (scrollInput * zoomSpeed);

            // Lock the value between 19 and 23
            cam.orthographicSize = Mathf.Clamp(newSize, minSize, maxSize);
        }
    }
}