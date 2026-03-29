using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue Sprites")]
    [SerializeField] private List<GameObject> dialogues; // Drag objects 1-11 here in order

    [Header("Timing Settings")]
    [SerializeField] private float visibleDuration = 10f;
    [SerializeField] private float waitBetweenDialogues = 90f; // 1.5 minutes

    private GameObject _currentActiveDialogue;
    private Coroutine _sequenceCoroutine;

    void Start()
    {
        // Initially hide all dialogues [cite: 2025-09-03]
        foreach (GameObject d in dialogues)
        {
            if (d != null) d.SetActive(false);
        }

        // Start the sequence [cite: 2025-09-03]
        _sequenceCoroutine = StartCoroutine(DialogueSequence());
    }

    void Update()
    {
        // If Enter is pressed, turn off the current dialogue immediately [cite: 2025-09-03]
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (_currentActiveDialogue != null && _currentActiveDialogue.activeSelf)
            {
                _currentActiveDialogue.SetActive(false);
                Debug.Log("Dialogue skipped by player."); 
            }
        }
    }

    IEnumerator DialogueSequence()
    {
        for (int i = 0; i < dialogues.Count; i++)
        {
            if (dialogues[i] == null) continue;

            // Activate the current dialogue [cite: 2025-09-03]
            _currentActiveDialogue = dialogues[i];
            _currentActiveDialogue.SetActive(true);

            // Wait for 7 seconds [cite: 2025-09-03]
            yield return new WaitForSeconds(visibleDuration);

            // Deactivate after 7 seconds [cite: 2025-09-03]
            _currentActiveDialogue.SetActive(false);

            // Wait for 1.5 minutes before the next one starts [cite: 2025-09-03]
            // We only wait if there is a next dialogue in the list [cite: 2025-09-03]
            if (i < dialogues.Count - 1)
            {
                yield return new WaitForSeconds(waitBetweenDialogues);
            }
        }

        Debug.Log("All dialogues completed.");
    }
}