using UnityEngine;
using UnityEngine.InputSystem;

public class HeroVisual : MonoBehaviour
{
    // Управляет анимациями
    private Animator animator;

    // Отвечает за отображение спрайта
    private SpriteRenderer SpriteRenderer;

    // Область попадания меча
    [SerializeField] private PolygonCollider2D SwordHitbox;

    // Запоминаем, что атака уже началась
    private bool attackStarted = false;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        RunAnimation();
        AttackAnimation();
        CheckAttackEnd();
    }


    private void FixedUpdate()
    {
        Flip();
    }


    // Анимация движения
    private void RunAnimation()
    {
        animator.SetBool("IsRun", Hero.Instance.IsRun);
    }


    // Анимация атаки
    private void AttackAnimation()
    {
        if (Hero.Instance.AttackPressed)
        {
            animator.SetTrigger("Attack");
        }
    }


    // Проверяем, закончилась ли атака
    private void CheckAttackEnd()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);


        // Запоминаем, что герой вошёл в анимацию атаки
        if (Hero.Instance.IsAttacking && stateInfo.IsName("Attack"))
        {
            attackStarted = true;
        }


        // Если атака закончилась и началась другая анимация
        if (Hero.Instance.IsAttacking &&
            attackStarted &&
            !stateInfo.IsName("Attack"))
        {
            attackStarted = false;

            // Разрешаем герою снова двигаться
            Hero.Instance.EndAttack();
        }
    }


    // Поворачиваем героя в сторону движения
    private void Flip()
    {
        // Если герой идёт влево
        if (Keyboard.current.aKey.isPressed)
        {
            SpriteRenderer.flipX = true;
        }

        // Если герой идёт вправо
        if (Keyboard.current.dKey.isPressed)
        {
            SpriteRenderer.flipX = false;
        }
    }


    // Включаем область удара меча
    public void EnableSwordHitbox()
    {
        SwordHitbox.enabled = true;
    }


    // Выключаем область удара меча
    public void DisableSwordHitbox()
    {
        SwordHitbox.enabled = false;
    }
}