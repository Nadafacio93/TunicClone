using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Referencias")]
    public EnemyController enemy;

    private bool canTakeHit = true;

    private void Awake()
    {
        currentHealth = maxHealth;

        if (enemy == null)
        {
            enemy = GetComponent<EnemyController>();
        }
    }

    public void GetHit(int damage)
    {
        if (enemy.isDead) return;
        if (!canTakeHit) return;

        canTakeHit = false;
        currentHealth -= damage;

        enemy.BeginHitReaction();
        Invoke(nameof(ResetHit), 0.5f);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void ResetHit()
    {
        canTakeHit = true;

        if (!enemy.isDead)
        {
            enemy.EndHitReaction();
        }
    }

    private void Die()
    {
        enemy.Die();
    }
}
