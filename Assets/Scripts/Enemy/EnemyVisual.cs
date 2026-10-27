using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    private Animator animator;

    public bool IsAttacking { get; private set; }

    private bool damageGiven;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (!IsAttacking)
        {
            return;
        }

        AnimatorStateInfo stateInfo =
            animator.GetCurrentAnimatorStateInfo(0);

        // Наносим урон один раз за атаку
        if (!damageGiven &&
            stateInfo.IsName("Attack") &&
            stateInfo.normalizedTime >= 0.5f)
        {
            GiveDamage();
            damageGiven = true;
        }

        // Атака закончилась
        if (stateInfo.IsName("Attack") &&
            stateInfo.normalizedTime >= 1f)
        {
            IsAttacking = false;
            damageGiven = false;

            animator.SetBool("IsAttacking", false);
        }
    }

    public void Attack()
    {
        if (IsAttacking)
        {
            return;
        }

        IsAttacking = true;
        damageGiven = false;

        animator.SetBool("IsAttacking", true);

        // Запускаем каждую атаку с начала
        animator.Play("Attack", 0, 0f);
    }

    private void GiveDamage()
    {
        if (Hero.Instance == null)
        {
            return;
        }

        float distanceToHero = Vector3.Distance(
            transform.position,
            Hero.Instance.transform.position
        );

        if (distanceToHero <= 1.5f)
        {
            PlayerHealth playerHealth =
                Hero.Instance.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage();
            }
        }
    }
}