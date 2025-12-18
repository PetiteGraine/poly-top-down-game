using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private int _maxHealth = 5;
    private int _currentHealth;
    [SerializeField] private int _attackDamage = 1;
    [SerializeField] private float _attackSpeed = 1;
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField] private PlayerWeapon _playerWeapon;
    [SerializeField] private List<Augment> _equippedAugments = new List<Augment>();

    private void Start()
    {
        _currentHealth = _maxHealth;
        _healthText.text = $"HP: {_currentHealth}";
        if (_playerWeapon != null)
        {
            _playerWeapon = GetComponentInChildren<PlayerWeapon>();
        }
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
        Debug.Log("Player has died.");
    }

    public int GetAttackDamage()
    {
        WeaponStats weaponStats = _playerWeapon.GetCurrentWeaponStats();
        int weaponDamage = _playerWeapon != null ? weaponStats.GetDamage() : 0;
        int augmentsDamage = 0;
        foreach (Augment augment in _equippedAugments)
        {
            augmentsDamage += augment.GetAttackDamageBonus();
        }

        return _attackDamage + weaponDamage + augmentsDamage;
    }

    public float GetAttackSpeed()
    {
        WeaponStats weaponStats = _playerWeapon.GetCurrentWeaponStats();
        float augmentsAS = 0;
        foreach (Augment augment in _equippedAugments)
        {
            augmentsAS += augment.GetAttackSpeedBonus();
        }

        return _playerWeapon != null ? weaponStats.GetAttackSpeed() + augmentsAS : _attackSpeed + augmentsAS;
    }

    public float GetMoveSpeed()
    {
        float augmentsMS = 0;
        foreach (Augment augment in _equippedAugments)
        {
            augmentsMS += augment.GetMoveSpeedBonus();
        }


        return _moveSpeed + augmentsMS;
    }

    public int GetCurrentHealth()
    {
        int augmentsHealth = 0;
        foreach (Augment augment in _equippedAugments)
        {
            augmentsHealth += augment.GetHealthBonus();
        }

        return _currentHealth + augmentsHealth;
    }

    public void EquipAugment(Augment augment)
    {
        _equippedAugments.Add(augment);
    }
}
