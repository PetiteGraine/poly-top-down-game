using UnityEngine;

public class ChangeWeapon : MonoBehaviour
{
    [SerializeField] private PlayerWeapon _playerWeapon;
    [SerializeField] private GameObject _newWeapon;

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
            _playerWeapon.gameObject.SetActive(false);
            if (_newWeapon != null && _playerWeapon != null)
            {
                _playerWeapon.ChangeWeapon(_newWeapon);
            }
        }
    }
}
