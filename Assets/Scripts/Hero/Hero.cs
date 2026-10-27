using UnityEngine;
using UnityEngine.InputSystem;


[SelectionBase]
public class Hero : MonoBehaviour
{
    // Позволяет обращаться к герою из других скриптов
    public static Hero Instance { get; private set; }

    // Скорость движения героя
    private float MoveSpeed = 5f;

    // Rigidbody2D нужен для движения
    private Rigidbody2D rb;

    // Герой сейчас двигается?
    public bool IsRun { get; private set; } = false;

    // Нужно ли запустить удар?
    public bool AttackPressed { get; private set; } = false;

    // Герой сейчас атакует?
    public bool IsAttacking { get; private set; } = false;


    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        Attack();
    }


    private void FixedUpdate()
    {
        Move();
    }


    // Движение героя
    private void Move()
    {
        // Во время атаки герой не двигается
        if (IsAttacking)
        {
            IsRun = false;
            return;
        }

        Vector2 InputVector = Vector2.zero;


        // W — вверх
        if (Keyboard.current.wKey.isPressed)
        {
            InputVector.y = 1f;
        }

        // S — вниз
        if (Keyboard.current.sKey.isPressed)
        {
            InputVector.y = -1f;
        }

        // A — влево
        if (Keyboard.current.aKey.isPressed)
        {
            InputVector.x = -1f;
        }

        // D — вправо
        if (Keyboard.current.dKey.isPressed)
        {
            InputVector.x = 1f;
        }


        // Убираем более быструю скорость по диагонали
        InputVector = InputVector.normalized;

        // Проверяем, двигается ли герой
        IsRun = InputVector != Vector2.zero;


        // Двигаем героя
        rb.MovePosition(
            rb.position + InputVector * (MoveSpeed * Time.fixedDeltaTime)
        );
    }


    // Атака героя
    private void Attack()
    {
        // Сбрасываем команду атаки
        AttackPressed = false;

        // Если герой уже атакует, новую атаку не запускаем
        if (IsAttacking)
        {
            return;
        }

        // Левая кнопка мыши запускает атаку
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            AttackPressed = true;
            IsAttacking = true;
        }
    }


    // Вызывается после окончания атаки
    public void EndAttack()
    {
        IsAttacking = false;
    }
}