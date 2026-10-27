using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private Animator animator;

    public bool IsDying { get; private set; }

    private int deathCount;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void TakeDamage()
    {
        if (IsDying)
        {
            return;
        }

        // Урон теперь просто фиксируется.
        // Смерть контролирует EnemyAI по таймеру.
    }

    public void StartDeath()
    {
        if (IsDying)
        {
            return;
        }

        IsDying = true;
        deathCount = 0;

        animator.SetTrigger("Death");
    }

    private void Update()
    {
        if (!IsDying)
        {
            return;
        }

        AnimatorStateInfo stateInfo =
            animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Death") &&
            stateInfo.normalizedTime >= 1f)
        {
            deathCount++;

            if (deathCount >= 3)
            {
                Destroy(gameObject);
                return;
            }

            animator.Play("Death", 0, 0f);
        }
    }
}