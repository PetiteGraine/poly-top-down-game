using UnityEngine;
using UnityEngine.AI;

public class BossProjectile : MonoBehaviour
{
    [SerializeField] private float lifeTime = 5f;

    [Header("Teleport Zones")]
    private Collider[] teleportZones;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

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
        if (teleportZones == null || teleportZones.Length == 0) return;


        Vector3 dest = TryGetRandomPointInZones();

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc) cc.enabled = false;

        player.position = dest;

        if (cc) cc.enabled = true;
    }

    Vector3 TryGetRandomPointInZones()
    {
        for (int attempt = 0; attempt < 30; attempt++)
        {
            Collider zone = teleportZones[Random.Range(0, teleportZones.Length)];
            if (zone == null) continue;

            Bounds b = zone.bounds;

            Vector3 randomPos = new Vector3(
                Random.Range(b.min.x, b.max.x),
                b.center.y,
                Random.Range(b.min.z, b.max.z)
            );

            if (NavMesh.SamplePosition(randomPos, out var hit, 10f, NavMesh.AllAreas))
                return hit.position;
        }

        return transform.position;
    }
}
