using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private GameObject _weaponsParent;
    [SerializeField] private GameObject _currentWeapon;

    private void Start()
    {
        if (_weaponsParent == null)
        {
            _weaponsParent = transform.Find("Weapon").gameObject;
        }

        if (_currentWeapon == null && _weaponsParent != null)
        {
            foreach (Transform child in _weaponsParent.transform)
            {
                if (child == null) continue;
                WeaponStats weaponStats = child.GetComponent<WeaponStats>();
                if (weaponStats != null)
                {
                    _currentWeapon = weaponStats.gameObject;
                    break;
                }
            }
        }
    }

    public void SetCurrentWeapon(GameObject newWeapon)
    {
        _currentWeapon = newWeapon;
    }

    public GameObject GetCurrentWeapon()
    {
        return _currentWeapon;
    }

    public WeaponStats GetCurrentWeaponStats()
    {
        if (_currentWeapon != null)
        {
            return _currentWeapon.GetComponent<WeaponStats>();
        }
        return null;
    }

    public void ChangeWeapon(GameObject newWeapon)
    {
        _currentWeapon = newWeapon;
    }
}
