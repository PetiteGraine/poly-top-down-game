using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    [SerializeField] private int _maxHealth = 5;
    private int _currentHealth;
    [SerializeField] private int _attackDamage = 1;
    [SerializeField] private float _attackSpeed = 1;
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField] private PlayerWeapon _playerWeapon;
    [SerializeField] private PlayerAugment _playerAugment;
    [SerializeField] private TextMeshProUGUI _HPText;
    [SerializeField] private TextMeshProUGUI _DMGText;
    [SerializeField] private TextMeshProUGUI _ASText;
    [SerializeField] private TextMeshProUGUI _MSText;
    [SerializeField] private GameObject _gameOverScreen;

    private void Start()
    {
        _currentHealth = _maxHealth;
        UpdateCanvas();
        if (_playerAugment != null)
        {
            _playerAugment = GetComponent<PlayerAugment>();
        }
        if (_playerWeapon != null)
        {
            _playerWeapon = GetComponentInChildren<PlayerWeapon>();
        }
    }

    public void UpdateCanvas()
    {
        _HPText.text = $"HP: {GetCurrentHealth()}";
        _DMGText.text = $"DMG: {GetAttackDamage()}";
        _ASText.text = $"AS: {GetAttackSpeed():F2}";
        _MSText.text = $"MS: {GetMoveSpeed():F2}";
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _HPText.text = $"HP: {_currentHealth}";
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        _gameOverScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public int GetAttackDamage()
    {
        List<Augment> _equippedAugments = _playerAugment.GetEquippedAugments();

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
        List<Augment> _equippedAugments = _playerAugment.GetEquippedAugments();

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
        List<Augment> _equippedAugments = _playerAugment.GetEquippedAugments();

        float augmentsMS = 0;
        foreach (Augment augment in _equippedAugments)
        {
            augmentsMS += augment.GetMoveSpeedBonus();
        }


        return _moveSpeed + augmentsMS;
    }

    public int GetCurrentHealth()
    {
        return _currentHealth;
    }

    public void EquipAugment(Augment augment)
    {
        _currentHealth += augment.GetHealthBonus();
        UpdateCanvas();
    }
}
