using UnityEngine;
using UnityEngine.AI;

public class BossCombatTeleport : MonoBehaviour
{
    [Header("Teleport Zones")]
    [SerializeField] private Collider[] teleportZones;
    [SerializeField] private float teleportCooldown = 1f;

    [Header("Projectile")]
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float projectileSpeed = 12f;
    [SerializeField] private float shootCooldown = 2f;

    private float lastTeleportTime;
    private float lastShootTime;

    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(ShootTick), 1f, 5f);
    }

    private void ShootTick()
    {
        if (Player == null) return;
        TryShootAtPlayer(Player.transform);
    }

    public void OnBossDamaged()
    {
        if (Time.time - lastTeleportTime < teleportCooldown)
            return;

        TeleportBoss();
        lastTeleportTime = Time.time;
    }

    void TeleportBoss()
    {
        Vector3 pos = GetRandomPointInZones();

        if (agent)
            agent.Warp(pos);
        else
            transform.position = pos;
    }

    Vector3 GetRandomPointInZones()
    {
        if (teleportZones == null || teleportZones.Length == 0)
        {
            Debug.LogWarning("[BossCombatTeleport] No teleport zones assigned.");
            return transform.position;
        }

        // On essaie plusieurs fois (zones + points) pour trouver un point valide sur le NavMesh
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

            if (NavMesh.SamplePosition(randomPos, out var hit, 2f, NavMesh.AllAreas))
                return hit.position;
        }

        return transform.position;
    }

    public void TryShootAtPlayer(Transform player)
    {
        if (Time.time - lastShootTime < shootCooldown)
            return;

        lastShootTime = Time.time;

        if (!shootPoint || !projectilePrefab || !player) return;

        Vector3 dir = (player.position - shootPoint.position);
        dir.y = 0f;
        dir = dir.sqrMagnitude > 0.0001f ? dir.normalized : shootPoint.forward;

        GameObject proj = Instantiate(projectilePrefab, shootPoint.position, Quaternion.LookRotation(dir));
        BossProjectile bp = proj.GetComponent<BossProjectile>();
        if (bp != null)
            bp.Init(teleportZones); // teleportZones du boss


        Rigidbody rb = proj.GetComponent<Rigidbody>();
        if (rb)
            rb.linearVelocity = dir * projectileSpeed;
    }
}
