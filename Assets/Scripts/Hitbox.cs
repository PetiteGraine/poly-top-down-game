using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private int _damageAmount = 10;
    [SerializeField] private float _lifetime = 2f;

    private void Start()
    {
        Destroy(gameObject, _lifetime);
    }

    public void SetDamageAmount(int amount)
    {
        _damageAmount = amount;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(_damageAmount);
            }
            Destroy(gameObject);
        }
        else

            if (other.CompareTag("Enemy"))
            {
                EnemyStats enemyStats = other.GetComponent<EnemyStats>();
                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(_damageAmount);
                }
                Destroy(gameObject);
            }
    }
}
