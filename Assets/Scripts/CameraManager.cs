using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject leftCameraPivot;
    [SerializeField] private GameObject rightCameraPivot;

    private bool _isLeftActive = true;

    void Start()
    {
        UpdateCameraStates();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _isLeftActive = !_isLeftActive;
            UpdateCameraStates();
        }
    }

    void UpdateCameraStates()
    {
        if (leftCameraPivot != null && rightCameraPivot != null)
        {
            leftCameraPivot.SetActive(_isLeftActive);
            rightCameraPivot.SetActive(!_isLeftActive);
        }
    }
}