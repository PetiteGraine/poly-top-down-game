using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 5;
    private int _currentHealth;
    [SerializeField] private int _attackDamage = 1;
    [SerializeField] private float _attackSpeed = 1;
    [SerializeField] private float _moveSpeed = 5;

    private BossCombatTeleport _bossCombat; // Reference to the BossCombatTeleport script

    private void Awake() // Reference to the BossCombatTeleport script
    {
        _bossCombat = GetComponent<BossCombatTeleport>();
    }

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        Debug.Log($"Enemy took {damage} damage, current health: {_currentHealth}");
        if (_bossCombat != null)    // Reference to the BossCombatTeleport script
            _bossCombat.OnBossDamaged();

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy has died.");

        Destroy(gameObject);
    }



    public int GetAttackDamage()
    {
        return _attackDamage;
    }

    public float GetAttackSpeed()
    {
        return _attackSpeed;
    }

    public float GetMoveSpeed()
    {
        return _moveSpeed;
    }

    public int GetCurrentHealth()
    {
        return _currentHealth;
    }

    public int GetMaxHealth()
    {
        return _maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(GetAttackDamage());
            }
        }
    }

}
