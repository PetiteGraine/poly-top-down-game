using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject _attackPrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _fireSpeed = 3f;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private PlayerMovement _playerMovement;


    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Shoot();
            _playerMovement.AttackAnimation();
        }
    }

    private void Shoot()
    {
        GameObject spawnedAttack = Instantiate(_attackPrefab, transform.position, transform.rotation);
        spawnedAttack.GetComponent<Hitbox>().SetDamageAmount(_playerStats.GetAttackDamage());
        if (spawnedAttack.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.linearVelocity = _firePoint.forward * _fireSpeed;
        }
    }
}
