using UnityEngine;
using TMPro;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private int _maxHealth = 5;
    private int _currentHealth;
    [SerializeField] private int _attackDamage = 1;
    [SerializeField] private float _attackSpeed = 1;
    [SerializeField] private float _moveSpeed = 5;


    private void Start()
    {
        _currentHealth = _maxHealth;
        _healthText.text = $"HP: {_currentHealth}";
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _healthText.text = $"HP: {_currentHealth}";
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
}
