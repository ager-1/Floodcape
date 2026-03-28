using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BoatController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float turnSpeed = 180f;

    [Header("Wobble Settings")]
    [SerializeField] private float wobbleAmount = 0.2f;
    [SerializeField] private float wobbleSpeed = 2f;

    [Header("Durability Settings")]
    [SerializeField] private int durability = 100;
    [SerializeField] private TextMeshProUGUI durabilityText;
    [SerializeField] private GameObject gameOverUI;

    [Header("Crate Delivery Settings")]
    [SerializeField] private GameObject[] deliveryCrates; 
    private int _currentCrateIndex = 2; 

    private float _moveInput;
    private float _turnInput;
    private float _startY;
    private bool _isDead = false;

    void Start()
    {
        _startY = transform.position.y;
        UpdateUI();

        ResetCrates();

        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }

    void Update()
    {
        if (_isDead)
        {
            if (Input.GetKeyDown(KeyCode.Return)) RestartGame();
            return;
        }

        _moveInput = Input.GetAxisRaw("Vertical");
        _turnInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.T))
        {
            DeliverCrate();
        }
    }

    void FixedUpdate()
    {
        if (_isDead) return;
        ApplyRotation();
        ApplyMovement();
    }

    void DeliverCrate()
    {
        if (_currentCrateIndex >= 0)
        {
            deliveryCrates[_currentCrateIndex].SetActive(false);
            _currentCrateIndex--;
            Debug.Log("Crate Delivered!");
        }
        else
        {
            Debug.Log("No crates left to deliver!");
        }
    }

    void ResetCrates()
    {
        foreach (GameObject crate in deliveryCrates)
        {
            crate.SetActive(true);
        }
        _currentCrateIndex = deliveryCrates.Length - 1;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CrateSpawn"))
        {
            ResetCrates();
            Debug.Log("Crates Restocked!");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_isDead && collision.gameObject.CompareTag("Obstacle"))
        {
            TakeDamage(10);
        }
    }
    void ApplyRotation()
    {
        float rotationAmount = _turnInput * turnSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, 0, rotationAmount));
    }

    void ApplyMovement()
    {
        Vector3 targetPosition = rb.position + (transform.up * _moveInput * moveSpeed * Time.fixedDeltaTime);
        targetPosition.y = _startY + (Mathf.Sin(Time.time * wobbleSpeed) * wobbleAmount);
        rb.MovePosition(targetPosition);
    }

    void TakeDamage(int amount)
    {
        durability = Mathf.Clamp(durability - amount, 0, 100);
        UpdateUI();
        if (durability <= 0) GameOver();
    }

    void UpdateUI() { if (durabilityText != null) durabilityText.text = "Durability: " + durability; }

    void GameOver() { _isDead = true; if (gameOverUI != null) gameOverUI.SetActive(true); rb.linearVelocity = Vector3.zero; }

    void RestartGame() { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
}