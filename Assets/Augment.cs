using UnityEngine;

public class Augment : MonoBehaviour
{
    [SerializeField] private int _healthBonus = 5;
    [SerializeField] private int _attackDamageBonus = 1;
    [SerializeField] private float _attackSpeedBonus = 1;
    [SerializeField] private float _moveSpeedBonus = 5;
    [SerializeField] private PlayerStats _playerStats;

    private void Start()
    {
        _playerStats = FindFirstObjectByType<PlayerStats>();
    }

    public int GetHealthBonus()
    {
        return _healthBonus;
    }

    public int GetAttackDamageBonus()
    {
        return _attackDamageBonus;
    }

    public float GetAttackSpeedBonus()
    {
        return _attackSpeedBonus;
    }

    public float GetMoveSpeedBonus()
    {
        return _moveSpeedBonus;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerStats.EquipAugment(this);
        }
    }
}
