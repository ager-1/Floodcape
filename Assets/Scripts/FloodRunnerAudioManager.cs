using UnityEngine;
using System.Collections.Generic;

public class FloodRunnerAudioManager : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip boatClip;
    public AudioClip popClip;
    public AudioClip obstacleHitClip;
    public AudioClip botSoundClip;

    [Header("Volume Controls")]
    [Range(0f, 1f)] public float boatVolume = 0.5f;
    [Range(0f, 1f)] public float popVolume = 0.7f;
    [Range(0f, 1f)] public float obstacleHitVolume = 0.8f;
    [Range(0f, 1f)] public float botSoundVolume = 0.6f;

    [Header("Bot Dialogue Settings")]
    public List<GameObject> botDialogueImages;

    private AudioSource sfxSource;
    private AudioSource boatEngineSource;
    private int lastActiveIndex = -1;

    void Awake()
    {
        // Source for one-off sound effects
        sfxSource = gameObject.AddComponent<AudioSource>();

        // Dedicated source for the looping boat engine
        boatEngineSource = gameObject.AddComponent<AudioSource>();
        boatEngineSource.clip = boatClip;
        boatEngineSource.loop = true;
        boatEngineSource.playOnAwake = false;
    }

    void Update()
    {
        HandleBoatEngine();
        HandlePopInput();
        HandleBotDialogueCheck();

        // Live volume updates (allows you to tweak sliders while the game is running)
        boatEngineSource.volume = boatVolume;
    }

    void HandleBoatEngine()
    {
        bool isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                        Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        if (this.enabled && isMoving)
        {
            if (!boatEngineSource.isPlaying)
                boatEngineSource.Play();
        }
        else
        {
            if (boatEngineSource.isPlaying)
                boatEngineSource.Stop();
        }
    }

    void HandlePopInput()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            sfxSource.PlayOneShot(popClip, popVolume);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            sfxSource.PlayOneShot(obstacleHitClip, obstacleHitVolume);
        }
    }

    void HandleBotDialogueCheck()
    {
        bool anyDialogueActive = false;

        for (int i = 0; i < botDialogueImages.Count; i++)
        {
            if (botDialogueImages[i] != null && botDialogueImages[i].activeInHierarchy)
            {
                anyDialogueActive = true;
                if (i != lastActiveIndex)
                {
                    sfxSource.PlayOneShot(botSoundClip, botSoundVolume);
                    lastActiveIndex = i;
                }
                break;
            }
        }

        if (!anyDialogueActive)
        {
            lastActiveIndex = -1;
        }
    }
}