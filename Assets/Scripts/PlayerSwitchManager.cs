using UnityEngine;

public class PlayerSwitchManager : MonoBehaviour
{
    [Header("Vehicle Scripts")]
    [SerializeField] private BoatController boatScript;
    [SerializeField] private BotController botScript;

    [Header("Bot References")]
    [SerializeField] private Rigidbody botRigidbody;
    [SerializeField] private Transform botDockPosition;

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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RecallBot();
        }
    }

    void UpdateControlState()
    {
        boatScript.enabled = _isControllingBoat;
        botScript.enabled = !_isControllingBoat;

        botRigidbody.isKinematic = _isControllingBoat;

        UpdateCameraTargets();
    }

    void RecallBot()
    {
        _isControllingBoat = true;
        UpdateControlState();

        if (botDockPosition != null)
        {
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