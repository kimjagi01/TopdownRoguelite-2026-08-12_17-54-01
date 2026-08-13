using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Attack")]
    [SerializeField] private Transform attackPoint;

    private Camera mainCamera;
    private Rigidbody2D body;
    private WeaponManager weaponManager;

    private Vector2 moveInput;
    private float nextAttackTime;
    private bool inputEnabled = true;

    private void Awake()
    {
        mainCamera = Camera.main;
        body = GetComponent<Rigidbody2D>();
        weaponManager = GetComponent<WeaponManager>();

        if (body == null)
        {
            Debug.LogError("PlayerController: Rigidbody2D is missing.");
        }

        if (weaponManager == null)
        {
            Debug.LogError("PlayerController: WeaponManager is missing.");
        }
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
        if (body == null)
        {
            return;
        }

        body.MovePosition(
            body.position +
            moveInput * moveSpeed * Time.fixedDeltaTime
        );
    }

    private void ReadMovementInput()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput = moveInput.normalized;
    }

    private void RotateTowardMouse()
    {
        if (mainCamera == null || body == null)
        {
            return;
        }

        Vector3 mouseWorldPosition =
            mainCamera.ScreenToWorldPoint(Input.mousePosition);

        Vector2 lookDirection =
            mouseWorldPosition - transform.position;

        float angle =
            Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        body.rotation = angle;
    }

    private void ReadAttackInput()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        if (weaponManager == null ||
            weaponManager.CurrentWeapon == null)
        {
            Debug.LogWarning("No weapon equipped.");
            return;
        }

        float attackCooldown = GetAttackCooldown();

        if (Time.time < nextAttackTime)
        {
            return;
        }

        nextAttackTime = Time.time + attackCooldown;

        Attack();
    }

    private float GetAttackCooldown()
    {
        float attackSpeed = weaponManager.GetAttackSpeed();

        // 공격속도 능력치 0 ~ 100
        // 낮을수록 느리고 높을수록 빠름
        return Mathf.Lerp(
            0.8f,
            0.1f,
            attackSpeed / 100f
        );
    }

    private void Attack()
    {
        if (attackPoint == null)
        {
            Debug.LogWarning("AttackPoint is not assigned.");
            return;
        }

        int attackDamage =
            weaponManager.GetAttackDamage();

        float attackRange =
            ConvertRangeRatingToDistance(
                weaponManager.GetAttackRange()
            );

        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                attackPoint.position,
                attackRange
            );

        foreach (Collider2D hit in hits)
        {
            if (!hit.CompareTag("Enemy"))
            {
                continue;
            }

            EnemyHealth enemyHealth =
                hit.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
            }
        }

        Debug.Log(
            $"Attack! Damage: {attackDamage}, Range: {attackRange:F2}"
        );
    }

    private float ConvertRangeRatingToDistance(float rangeRating)
    {
        // 무기 사거리 능력치 0 ~ 100
        // 실제 게임 거리 0.5 ~ 1.5
        return Mathf.Lerp(
            0.5f,
            1.5f,
            rangeRating / 100f
        );
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        float attackRange = 0.6f;

        if (weaponManager != null &&
            weaponManager.CurrentWeapon != null)
        {
            attackRange =
                ConvertRangeRatingToDistance(
                    weaponManager.GetAttackRange()
                );
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            attackPoint.position,
            attackRange
        );
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

        Debug.Log(
            $"Move Speed: {moveSpeed}"
        );
    }

    // 현재는 기존 레벨업 시스템과의 호환을 위해 남겨둔다.
    // 나중에는 무기 업그레이드 시스템으로 변경할 예정.
    public void IncreaseAttackDamage(int amount)
    {
        if (weaponManager == null)
        {
            Debug.LogWarning(
                "PlayerController: WeaponManager is missing."
            );

            return;
        }

        weaponManager.IncreaseAttackDamage(amount);
    }
}