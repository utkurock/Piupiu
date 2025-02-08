using System.Collections;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour, IDamageable
{
    public Action OnPlayerHit;

    [SerializeField] private float _jumpForce = 7f;
    [SerializeField] private float _jumpInterval = 4f;
    [SerializeField] private float _changeDirectionInterval = 3f;
    [SerializeField] private float _knockbackThrust = 25f;

    private Rigidbody2D _rigidBody;
    private Movement _movement;
    private ColorChanger _colorChanger;
    private Knockback _knockback;
    private Flash _flash;
    private Health _health;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _movement = GetComponent<Movement>();
        _colorChanger = GetComponent<ColorChanger>();
        _knockback = GetComponent<Knockback>();
        _flash = GetComponent<Flash>();
        _health = GetComponent<Health>();
    }

    private void Start() {
        StartCoroutine(ChangeDirectionRoutine());
        StartCoroutine(RandomJumpRoutine());
    }

    private void OnEnable() {
        OnPlayerHit += HandlePlayerHit;
        OnPlayerHit += AudioManager.Instance.Enemy_OnPlayerHit;
    }

    private void OnDisable() {
        OnPlayerHit -= HandlePlayerHit;
        OnPlayerHit -= AudioManager.Instance.Enemy_OnPlayerHit;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
        Movement playerMovement = playerController?.GetComponent<Movement>();

        if (playerController && playerMovement.CanMove) 
        {
            OnPlayerHit?.Invoke();
        }
    }

    public void Init(Color color) {
        _colorChanger.SetDefaultColor(color);
    }

     public void Die()
    {
       
        // Düşmanı yok et
        Destroy(gameObject);
    }   

    private void HandlePlayerHit() {
        Knockback playerKnockback = PlayerController.Instance.GetComponent<Knockback>();
        playerKnockback.GetKnockedBack(transform.position, _knockbackThrust);

        Health playerHealth = PlayerController.Instance.GetComponent<Health>();
        playerHealth.TakeDamage(1);

        Flash playerFlash = PlayerController.Instance.GetComponent<Flash>();
        playerFlash.StartFlash();
    }

    private IEnumerator ChangeDirectionRoutine()
    {
        while (true)
        {
            float _currentDirection = UnityEngine.Random.Range(0, 2) * 2 - 1; // 1 or -1
            _movement.SetCurrentDirection(_currentDirection);
            yield return new WaitForSeconds(_changeDirectionInterval);
        }
    }

    private IEnumerator RandomJumpRoutine() 
    {
        while (true)
        {
            yield return new WaitForSeconds(_jumpInterval);
            float randomDirection = UnityEngine.Random.Range(-1, 1);
            Vector2 jumpDirection = new Vector2(randomDirection, 1f).normalized;
            _rigidBody.AddForce(jumpDirection * _jumpForce, ForceMode2D.Impulse);
        }
    }

  public void TakeDamage(Vector2 damageDirection, int damageAmount, float knockBackThrust)
{
    _health.TakeDamage(damageAmount);
    _knockback.GetKnockedBack(damageDirection, knockBackThrust);
}


    public void TakeHit()
    {
        _flash.StartFlash();
    }
}
