using UnityEngine;
using TMPro;

public class WaterDelivery : MonoBehaviour
{
    [SerializeField] private GameObject waterIcon;
    [SerializeField] private float deliveryRadius = 15f; // Large enough to reach the street from the balcony [cite: 2026-03-30]
    [SerializeField] private TextMeshProUGUI promptText;

    private bool _playerInRange = false;

    void Update()
    {
        CheckProximity();

        if (_playerInRange && gameObject.CompareTag("WaterNeeded"))
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                DeliverWater();
            }
        }
    }

    void CheckProximity()
    {
        // Search for the boat (tagged 'Player') within the radius [cite: 2026-03-30]
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, deliveryRadius);
        bool foundBoat = false;

        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Player"))
            {
                foundBoat = true;
                break;
            }
        }

        // Toggle the UI prompt based on boat proximity [cite: 2026-03-30]
        if (foundBoat != _playerInRange)
        {
            _playerInRange = foundBoat;
            if (promptText != null) promptText.gameObject.SetActive(_playerInRange);
        }
    }

    private void DeliverWater()
    {
        BoatController boat = FindFirstObjectByType<BoatController>();

        if (boat != null)
        {
            boat.SendMessage("DeliverCrate");
            MissionManager.Instance.CompleteWaterDelivery(gameObject, waterIcon);

            if (promptText != null) promptText.gameObject.SetActive(false);

            // Disable this script so it stops checking proximity after delivery [cite: 2026-03-30]
            this.enabled = false;
        }
    }

    // Visualize the delivery range in the Editor [cite: 2026-03-30]
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, deliveryRadius);
    }
}