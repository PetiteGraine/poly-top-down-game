using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject _rangedAttackPrefab;
    [SerializeField] private GameObject _meleeAttackPrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _fireSpeed = 3f;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private PlayerWeapon _playerWeapon;
    [SerializeField] private PlayerMovement _playerMovement;

    private float AttackCooldown => 1f / _playerStats.GetAttackSpeed();
    private float _attackCoaldownTimer = 0f;


    private void Start()
    {
        if (_playerStats == null)
        {
            _playerStats = GetComponent<PlayerStats>();
        }
        if (_playerMovement == null)
        {
            _playerMovement = GetComponent<PlayerMovement>();
        }
        if (_playerWeapon == null)
        {
            _playerWeapon = GetComponent<PlayerWeapon>();
        }
    }

    private void Update()
    {
        if (_attackCoaldownTimer > 0f)
        {
            _attackCoaldownTimer -= Time.deltaTime;
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (_attackCoaldownTimer > 0f)
            return;

        Shoot();
        _playerMovement.AttackAnimation();

        _attackCoaldownTimer = AttackCooldown;
    }


    private void Shoot()
    {
        GameObject spawnedAttack;
        if (_playerWeapon.isRangedWeapon() == false)
        {
            spawnedAttack = Instantiate(_meleeAttackPrefab, _firePoint.position, _firePoint.rotation);
            spawnedAttack.GetComponent<Hitbox>().SetDamageAmount(_playerStats.GetAttackDamage());
            return;
        }
        spawnedAttack = Instantiate(_rangedAttackPrefab, _firePoint.position, _firePoint.rotation);
        spawnedAttack.GetComponent<Hitbox>().SetDamageAmount(_playerStats.GetAttackDamage());
        if (spawnedAttack.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.linearVelocity = _firePoint.forward * _fireSpeed;
        }
    }
}
