using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ZombieHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 50f;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string deathTriggerName = "Die";     // Trigger
    [SerializeField] private string hitTriggerName = "Hit";       // Optional Trigger
    [SerializeField] private float destroyDelayAfterDeath = 2f;

    private float _currentHealth;
    private bool _isDead;

    private ZombieAI _zombieAI;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        _zombieAI = GetComponent<ZombieAI>();
    }

    private void Start()
    {
        _currentHealth = maxHealth;
    }

    /// <summary>
    /// Apply damage to this zombie.
    /// </summary>
    public void TakeDamage(float damage)
    {
        if (_isDead) return;

        _currentHealth -= damage;
        if (_currentHealth <= 0f)
        {
            Die();
            return;
        }

        // Optional hit reaction
        if (!string.IsNullOrEmpty(hitTriggerName) && animator != null)
        {
            animator.ResetTrigger(hitTriggerName);
            animator.SetTrigger(hitTriggerName);
        }

        // Inform AI so it can become "angry" and run
        if (_zombieAI != null)
        {
            _zombieAI.OnHit();
        }
    }

    private void Die()
    {
        _isDead = true;

        if (_zombieAI != null)
            _zombieAI.OnDeath();

        if (animator != null && !string.IsNullOrEmpty(deathTriggerName))
        {
            animator.ResetTrigger(deathTriggerName);
            animator.SetTrigger(deathTriggerName);
        }

        Destroy(gameObject, destroyDelayAfterDeath);
    }

    public bool IsDead => _isDead;
}
