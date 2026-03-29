using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; // Required for changing scenes [cite: 2026-03-30]

public class WinScreenManager : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private GameObject winPanel; // Drag the 'Panel' from your Hierarchy here
    [SerializeField] private float delayTime = 4f;

    [Header("Scene Settings")]
    [SerializeField] private string mainMenuName = "mainMenu"; // The exact name of your menu scene [cite: 2026-03-30]

    private bool _canExit = false;

    void Start()
    {
        // Make sure the panel is hidden when the scene first loads [cite: 2026-03-30]
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }

        StartCoroutine(ShowWinPanel());
    }

    void Update()
    {
        // Only listen for Enter after the panel has appeared [cite: 2026-03-30]
        if (_canExit)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                SceneManager.LoadScene(mainMenuName); 
            }
        }
    }

    IEnumerator ShowWinPanel()
    {
        // Wait for exactly 4 seconds [cite: 2026-03-30]
        yield return new WaitForSeconds(delayTime);

        if (winPanel != null)
        {
            winPanel.SetActive(true);
            _canExit = true;
            Debug.Log("Win Panel Visible. Press Enter to exit."); 
        }
    }
}