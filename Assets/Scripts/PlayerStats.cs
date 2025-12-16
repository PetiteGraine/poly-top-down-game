using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 5;
    private int _currentHealth;
    [SerializeField] private int _attackDamage = 1;
    [SerializeField] private int _attackSpeed = 1;
    [SerializeField] private int _moveSpeed = 5;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died.");
    }

    public int GetAttackDamage()
    {
        return _attackDamage;
    }

    public int GetAttackSpeed()
    {
        return _attackSpeed;
    }

    public int GetMoveSpeed()
    {
        return _moveSpeed;
    }

    public int GetCurrentHealth()
    {
        return _currentHealth;
    }
}
