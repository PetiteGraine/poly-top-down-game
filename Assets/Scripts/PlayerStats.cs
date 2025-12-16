using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 5;
    private int _currentHealth;
    [SerializeField] private int _attackDamage = 1;
    [SerializeField] private float _attackSpeed = 1;
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField] private PlayerWeapon _playerWeapon;

    private void Start()
    {
        _currentHealth = _maxHealth;
        if (_playerWeapon != null)
        {
            _playerWeapon = GetComponentInChildren<PlayerWeapon>();
        }
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
        WeaponStats weaponStats = _playerWeapon.GetCurrentWeaponStats();
        int weaponDamage = _playerWeapon != null ? weaponStats.GetDamage() : 0;
        return _attackDamage + weaponDamage;
    }

    public float GetAttackSpeed()
    {
        WeaponStats weaponStats = _playerWeapon.GetCurrentWeaponStats();
        return _playerWeapon != null ? weaponStats.GetAttackSpeed() : _attackSpeed;
    }

    public float GetMoveSpeed()
    {
        return _moveSpeed;
    }

    public int GetCurrentHealth()
    {
        return _currentHealth;
    }
}
