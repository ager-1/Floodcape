using UnityEngine;
using TMPro;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance;

    [Header("Boat Capacity")]
    [SerializeField] private int humansOnBoat = 0;
    [SerializeField] private int maxBoatCapacity = 3;

    [Header("Total Progress")]
    [SerializeField] private int totalRescued = 0;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI boatCountText; // Shows "On Boat: 0/3"
    [SerializeField] private TextMeshProUGUI totalRescuedText; // Shows "Total Rescued: 0"

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateUI();
    }

    // Called by the Bot when dropping a human onto the boat [cite: 2025-09-03]
    public bool TryAddHumanToBoat()
    {
        if (humansOnBoat < maxBoatCapacity)
        {
            humansOnBoat++;
            UpdateUI();
            return true;
        }
        return false;
    }

    // Called by the Boat when entering the SafeZone [cite: 2025-09-03]
    public void DeliverToSafeZone()
    {
        if (humansOnBoat > 0)
        {
            // Add the current boat load to the total score [cite: 2025-09-03]
            totalRescued += humansOnBoat;

            // Reset the boat's current load to zero [cite: 2025-09-03]
            humansOnBoat = 0;

            UpdateUI();
            Debug.Log("SafeZone: Humans delivered! Total Rescued: " + totalRescued); 
        }
    }

    void UpdateUI()
    {
        if (boatCountText != null)
        {
            boatCountText.text = "Humans On Boat: " + humansOnBoat + "/" + maxBoatCapacity; 
        }

        if (totalRescuedText != null)
        {
            totalRescuedText.text = "Total Humans Rescued: " + totalRescued; 
        }
    }
}