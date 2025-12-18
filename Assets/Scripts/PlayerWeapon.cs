using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private GameObject[] _weapons;
    [SerializeField] private int _currentWeaponIndex;
    [SerializeField] private PlayerStats _playerStats;

    private void Start()
    {
        SwitchWeapon(_currentWeaponIndex);
        if (_playerStats == null)
        {
            _playerStats = GetComponent<PlayerStats>();
        }
    }

    public bool isRangedWeapon()
    {
        if (_weapons.Length == 0) return false;
        WeaponStats weaponComponent = _weapons[_currentWeaponIndex].GetComponent<WeaponStats>();
        if (weaponComponent != null)
        {
            return weaponComponent.IsRanged();
        }
        return false;
    }

    public void SwitchWeapon(int newWeaponIndex)
    {
        if (_weapons.Length == 0) return;
        if (newWeaponIndex == _currentWeaponIndex) return;
        if (_weapons[_currentWeaponIndex] != null)
            _weapons[_currentWeaponIndex].SetActive(false);

        if (newWeaponIndex < 0 || newWeaponIndex >= _weapons.Length) return;

        _weapons[newWeaponIndex].SetActive(true);
        _currentWeaponIndex = newWeaponIndex;
        _playerStats.UpdateCanvas();
    }

    public int GetCurrentWeaponIndex()
    {
        return _currentWeaponIndex;
    }

    public WeaponStats GetCurrentWeaponStats()
    {
        if (_weapons.Length == 0) return null;
        return _weapons[_currentWeaponIndex].GetComponent<WeaponStats>();
    }
}
