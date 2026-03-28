using UnityEngine;

public class PlayerSwitchManager : MonoBehaviour
{
    [Header("Vehicle Scripts")]
    [SerializeField] private BoatController boatScript;
    [SerializeField] private BotController botScript;

    [Header("Bot References")]
    [SerializeField] private Rigidbody botRigidbody;
    [SerializeField] private Transform botDockPosition; // Assign "BotPosition" here

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
        // R to swap control [cite: 2025-09-03]
        if (Input.GetKeyDown(KeyCode.R))
        {
            _isControllingBoat = !_isControllingBoat;
            UpdateControlState();
        }

        // Q to recall the bot to the boat [cite: 2025-09-03]
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RecallBot();
        }
    }

    void UpdateControlState()
    {
        boatScript.enabled = _isControllingBoat;
        botScript.enabled = !_isControllingBoat;

        // Kinematic handles whether the bot is free or attached [cite: 2025-09-03]
        botRigidbody.isKinematic = _isControllingBoat;

        UpdateCameraTargets();
    }

    void RecallBot()
    {
        // 1. Force control back to the boat [cite: 2025-09-03]
        _isControllingBoat = true;
        UpdateControlState();

        // 2. Snap the bot to the empty parent "BotPosition"
        if (botDockPosition != null)
        {
            // Reset local position and rotation to align with the parent [cite: 2025-09-03]
            botRigidbody.transform.localPosition = Vector3.zero;
            botRigidbody.transform.localRotation = Quaternion.identity;

            Debug.Log("Bot Recalled to Dock"); 
        }
    }

    void UpdateCameraTargets()
    {
        Transform currentTarget = _isControllingBoat ? boatScript.transform : botScript.transform;

        if (leftCamScript != null) leftCamScript.SetTarget(currentTarget);
        if (rightCamScript != null) rightCamScript.SetTarget(currentTarget);
    }
}