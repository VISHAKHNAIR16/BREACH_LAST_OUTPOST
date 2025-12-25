using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ZombieAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform target;        // bunker / attack point
    [SerializeField] private Animator animator;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 1.0f;
    [SerializeField] private float runSpeed = 2.5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Attack")]
    [SerializeField] private float attackDistance = 1.6f;
    [SerializeField] private float attackInterval = 1.2f;
    [SerializeField] private int damagePerAttack = 5;

    // Animator parameter names
    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
    private static readonly int IsAttackingHash = Animator.StringToHash("IsAttacking");

    private float _currentMoveSpeed;
    private bool _isDead;
    private bool _isAttacking;
    private bool _isAngry;
    private float _nextAttackTime;

    private BunkerHealth bunkerHealth;



    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();



    }

    private void Start()
    {
        _currentMoveSpeed = walkSpeed;

        if (animator != null)
        {
            animator.SetBool(IsMovingHash, true);
            animator.SetBool(IsAttackingHash, false);
        }

        if (target != null)
            bunkerHealth = target.GetComponent<BunkerHealth>();


    }

    private void Update()
    {
        if (_isDead || target == null) return;

        Vector3 toTarget = target.position - transform.position;
        toTarget.y = 0f;
        float distance = toTarget.magnitude;

        if (distance > attackDistance)
        {
            MoveTowardsTarget(toTarget);
        }
        else
        {
            StartAttack();
            HandleAttackDamage();
        }
    }

    private void MoveTowardsTarget(Vector3 direction)
    {
        _isAttacking = false;

        if (direction.sqrMagnitude > 0.0001f)
        {
            direction.Normalize();
            transform.position += direction * _currentMoveSpeed * Time.deltaTime;

            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

        if (animator != null)
        {
            animator.SetBool(IsMovingHash, true);
            animator.SetBool(IsAttackingHash, false);
        }
    }

    private void StartAttack()
    {
        if (_isAttacking) return;

        _isAttacking = true;
        if (animator != null)
        {
            animator.SetBool(IsMovingHash, false);
            animator.SetBool(IsAttackingHash, true);
        }
    }


    /// <summary>
    /// Called by ZombieHealth on first hit to make zombie run / angry.
    /// </summary>
    public void OnHit()
    {
        if (_isDead || _isAngry) return;

        _isAngry = true;
        _currentMoveSpeed = runSpeed;

    }

    private void HandleAttackDamage()
    {
        if (bunkerHealth == null) return;

        if (Time.time >= _nextAttackTime)
        {
            _nextAttackTime = Time.time + attackInterval;
            bunkerHealth.TakeDamage(damagePerAttack);
        }
    }


    public void OnDeath()
    {
        _isDead = true;
        if (animator != null)
        {
            animator.SetBool(IsMovingHash, false);
            animator.SetBool(IsAttackingHash, false);
        }
    }

    public Transform Target
    {
        get => target;
        set
        {
            target = value;

            if (target != null)
                bunkerHealth = target.GetComponent<BunkerHealth>();
            else
                bunkerHealth = null;
        }
    }

}
