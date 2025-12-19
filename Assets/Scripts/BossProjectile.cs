using UnityEngine;
using UnityEngine.AI;

public class BossProjectile : MonoBehaviour
{
    [SerializeField] private float lifeTime = 5f;

    [Header("Teleport Zones")]
    [SerializeField] private Collider[] teleportZones; // liste de zones
    [SerializeField] private int maxTries = 20;
    [SerializeField] private float navMeshSampleRadius = 2f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Optionnel mais pratique : le boss peut passer la liste au projectile
    public void Init(Collider[] zones)
    {
        teleportZones = zones;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        TeleportPlayer(other.transform);
        Destroy(gameObject);
    }

    void TeleportPlayer(Transform player)
    {
        if (teleportZones == null || teleportZones.Length == 0)
        {
            return;
        }

        if (!TryGetRandomPointInZones(out Vector3 dest))
            return;

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc) cc.enabled = false;

        player.position = dest;

        if (cc) cc.enabled = true;
    }

    bool TryGetRandomPointInZones(out Vector3 result)
    {
        for (int attempt = 0; attempt < maxTries; attempt++)
        {
            Collider zone = teleportZones[Random.Range(0, teleportZones.Length)];
            if (zone == null) continue;

            Bounds b = zone.bounds;

            Vector3 randomPos = new Vector3(
                Random.Range(b.min.x, b.max.x),
                b.center.y,
                Random.Range(b.min.z, b.max.z)
            );

            if (NavMesh.SamplePosition(randomPos, out var hit, navMeshSampleRadius, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }

        }

        result = Vector3.zero;
        return false;
    }
}
