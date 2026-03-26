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

    private float _moveInput;
    private float _turnInput;
    private float _startY;
    private bool _isDead = false;

    void Start()
    {
        _startY = transform.position.y;
        UpdateUI();

        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }

    void Update()
    {
        if (_isDead)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                RestartGame();
            }
            return;
        }

        _moveInput = Input.GetAxisRaw("Vertical");
        _turnInput = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        if (_isDead) return;

        ApplyRotation();
        ApplyMovement();
    }

    void ApplyRotation()
    {
        float rotationAmount = _turnInput * turnSpeed * Time.fixedDeltaTime;
        Quaternion deltaRotation = Quaternion.Euler(0, 0, rotationAmount);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    void ApplyMovement()
    {
        Vector3 moveDirection = transform.up * _moveInput;
        Vector3 targetPosition = rb.position + (moveDirection * moveSpeed * Time.fixedDeltaTime);
        float wobble = Mathf.Sin(Time.time * wobbleSpeed) * wobbleAmount;
        targetPosition.y = _startY + wobble;

        rb.MovePosition(targetPosition);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isDead) return;

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            TakeDamage(10);
        }
    }

    void TakeDamage(int amount)
    {
        durability -= amount;
        durability = Mathf.Clamp(durability, 0, 100);
        UpdateUI();

        if (durability <= 0)
        {
            GameOver();
        }
    }

    void UpdateUI()
    {
        if (durabilityText != null)
        {
            durabilityText.text = "Durability: " + durability;
        }
    }

    void GameOver()
    {
        _isDead = true;
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}