using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    // Урон меча
    [SerializeField] private int Damage = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ищем здоровье у Enemy
        Health health = collision.GetComponent<Health>();

        // Если нашли здоровье
        if (health != null)
        {
            // Наносим урон
            health.TakeDamage(Damage);
        }
    }
}