using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    [SerializeField] private int _damage = 5;
    [SerializeField] private float _attackSpeed = 1.0f;
    [SerializeField] private bool _isRanged = false;

    public int GetDamage()
    {
        return _damage;
    }

    public float GetAttackSpeed()
    {
        return _attackSpeed;
    }

    public bool IsRanged()
    {
        return _isRanged;
    }
}
