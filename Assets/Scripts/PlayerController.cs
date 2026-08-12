using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Attack")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.6f;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float attackCooldown = 0.3f;

    private Camera mainCamera;
    private Rigidbody2D body;
    private Vector2 moveInput;
    private float nextAttackTime;
    private bool inputEnabled = true;

    private void Awake()
    {
        mainCamera = Camera.main;
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!inputEnabled)
        {
            return;
        }

        ReadMovementInput();
        RotateTowardMouse();
        ReadAttackInput();
    }

    private void FixedUpdate()
    {
        body.MovePosition(body.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    private void ReadMovementInput()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput = moveInput.normalized;
    }

    private void RotateTowardMouse()
    {
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDirection = mouseWorldPosition - transform.position;

        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        body.rotation = angle;
    }

    private void ReadAttackInput()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        if (Time.time < nextAttackTime)
        {
            return;
        }

        nextAttackTime = Time.time + attackCooldown;
        Attack();
    }

    private void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = hit.GetComponent<EnemyHealth>();

                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(attackDamage);
                }
            }
        }

        Debug.Log("Attack");
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void SetInputEnabled(bool isEnabled)
    {
        inputEnabled = isEnabled;

        if (!inputEnabled)
        {
            moveInput = Vector2.zero;
        }
    }

    public void IncreaseMoveSpeed(float amount)
    {
        moveSpeed += amount;
        Debug.Log($"Move Speed: {moveSpeed}");
    }

    public void IncreaseAttackDamage(int amount)
    {
        attackDamage += amount;
        Debug.Log($"Attack Damage: {attackDamage}");
    }
}
