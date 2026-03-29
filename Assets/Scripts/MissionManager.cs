using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Required for scene transitions [cite: 2025-09-03]

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance;

    [Header("Boat Capacity")]
    [SerializeField] private int humansOnBoat = 0;
    [SerializeField] private int maxBoatCapacity = 3;

    [Header("Total Progress")]
    [SerializeField] private int totalRescued = 0;
    [SerializeField] private int targetToWin = 15; // The number needed to change scenes [cite: 2025-09-03]

    [Header("Scene Management")]
    [SerializeField] private string nextSceneName; // Type the name of your next level here [cite: 2025-09-03]

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI boatCountText;
    [SerializeField] private TextMeshProUGUI totalRescuedText;
    [SerializeField] private string mainMenuSceneName = "mainMenu"; // Match your scene name exactly [cite: 2026-03-30]

    void Update()
    {
        // Check if the Escape key is pressed [cite: 2026-03-30]
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToMainMenu();
        }
    }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateUI();
    }

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

    public void DeliverToSafeZone()
    {
        if (humansOnBoat > 0)
        {
            totalRescued += humansOnBoat;
            humansOnBoat = 0;
            UpdateUI();

            Debug.Log("SafeZone: Humans delivered! Total Rescued: " + totalRescued);

            // Check if win condition is met [cite: 2025-09-03]
            if (totalRescued >= targetToWin)
            {
                ChangeScene();
            }
        }
    }

    private void ChangeScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName); 
        }
        else
        {
            Debug.LogError("Next Scene Name is missing in MissionManager Inspector!"); 
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

    public void CompleteWaterDelivery(GameObject human, GameObject waterIcon)
    {
        human.tag = "Human";

        if (waterIcon != null)
        {
            waterIcon.SetActive(false);
        }

        Debug.Log("Water Delivered! Human is now ready for rescue.");
    }
    public void ReturnToMainMenu()
    {
        Debug.Log("Escaped Level 1. Returning to Menu..."); 
        
        // Ensure the scene is in Build Settings [cite: 2026-03-30]
        if (!string.IsNullOrEmpty(mainMenuSceneName))
        {
            SceneManager.LoadScene(mainMenuSceneName); 
        }
        else
        {
            Debug.LogError("Main Menu scene name is not set!"); 
        }
    }
}
