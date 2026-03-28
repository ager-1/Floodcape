using UnityEngine;

public class PlayerSwitchManager : MonoBehaviour
{
    [Header("Vehicle Scripts")]
    [SerializeField] private BoatController boatScript;
    [SerializeField] private BotController botScript;

    [Header("Rigidbodies")]
    [SerializeField] private Rigidbody botRigidbody;

    [Header("Camera Pivots")]
    [SerializeField] private CameraFollow leftCamScript;
    [SerializeField] private CameraFollow rightCamScript;

    private bool _isControllingBoat = true;

    void Start()
    {
        UpdateControlState();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            _isControllingBoat = !_isControllingBoat;
            UpdateControlState();
        }
    }

    void UpdateControlState()
    {
        // Toggle movement scripts [cite: 2025-09-03]
        boatScript.enabled = _isControllingBoat;
        botScript.enabled = !_isControllingBoat;

        // Handle Bot docking/kinematic state [cite: 2025-09-03]
        botRigidbody.isKinematic = _isControllingBoat;

        // Update both cameras to follow the current active transform [cite: 2025-09-03]
        Transform currentTarget = _isControllingBoat ? boatScript.transform : botScript.transform;

        if (leftCamScript != null) leftCamScript.SetTarget(currentTarget);
        if (rightCamScript != null) rightCamScript.SetTarget(currentTarget);
    }
}