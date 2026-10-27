using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    // Максимальное здоровье
    [SerializeField] private int MaxHealth = 100;

    // Текущее здоровье
    private int CurrentHealth;

    // Полоска здоровья
    private RectTransform HealthBar;

    // Начальная ширина полоски
    private float MaxBarWidth;

    private void Awake()
    {
        // В начале здоровье максимальное
        CurrentHealth = MaxHealth;

        // Находим Fill
        Transform HealthBarObject = transform.Find("HealthBar/Border/Background/Fill");

        // Получаем Rect Transform
        if (HealthBarObject != null)
        {
            HealthBar = HealthBarObject.GetComponent<RectTransform>();
        }

        // Запоминаем начальную ширину
        if (HealthBar != null)
        {
            MaxBarWidth = HealthBar.rect.width;
        }
    }

    // Получаем урон
    public void TakeDamage(int damage)
    {
        // Уменьшаем здоровье
        CurrentHealth -= damage;

        // Не даём здоровью уйти ниже нуля
        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }

        // Меняем ширину полоски
        if (HealthBar != null)
        {
            float HealthPercent = (float)CurrentHealth / MaxHealth;

            HealthBar.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal,
                MaxBarWidth * HealthPercent
            );
        }

        // Проверяем, закончилось ли здоровье
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    // NPC умирает
    private void Die()
    {
        Destroy(gameObject);
    }
}