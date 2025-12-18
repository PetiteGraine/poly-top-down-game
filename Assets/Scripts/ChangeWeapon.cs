using UnityEngine;

public class ChangeWeapon : MonoBehaviour
{
    [SerializeField] private PlayerWeapon _playerWeapon;
    [SerializeField] private int _newWeaponIndex;

    private void Start()
    {
        if (_playerWeapon == null)
        {
            _playerWeapon = FindFirstObjectByType<PlayerWeapon>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Changing weapon");
            _playerWeapon.SwitchWeapon(_newWeaponIndex);
        }
    }
}
