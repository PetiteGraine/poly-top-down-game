using UnityEngine;
using UnityEngine.AI;

public class FallGuy : MonoBehaviour
{
    [SerializeField] private Lever_actif lever;
    private void OnTriggerStay(Collider other)
    {
        
        Debug.Log($"Enemy will fallen! : {!lever.isActif}");
        if (other.CompareTag("Enemy") && !lever.isActif)
        {
            Debug.Log("Enemy has fallen!");
            if (other.isTrigger) return;
            GameObject enemy = other.gameObject;
            enemy.AddComponent<Rigidbody>();
            enemy.GetComponent<NavMeshAgent>().enabled = false;
        }
    }
}
