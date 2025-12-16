using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private int _damageAmount = 10;
    [SerializeField] private float _lifetime = 2f;

    private void Start()
    {
        Destroy(gameObject, _lifetime);
    }

    public void SetDamageAmount(int amount)
    {
        _damageAmount = amount;
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("Enemy"))
    //     {
    //         EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
    //         if (enemyHealth != null)
    //         {
    //             enemyHealth.TakeDamage(_damageAmount);
    //         }
    //     }
    // }
}
