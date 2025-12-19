using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BossMinionSpawner : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;              
    [SerializeField] private float detectRange = 15f;       
    [SerializeField] private float reachableSampleRadius = 2f;

    [Header("Spawn")]
    [SerializeField] private GameObject minionPrefab;
    [SerializeField] private float spawnDistanceInFront = 2.0f;


    [Header("Performance / Stability")]
    [SerializeField] private float reachableCheckInterval = 0.5f;

    [Header("Spawn Limits")]
    [SerializeField] private int maxAliveMinions = 3;
    [SerializeField] private int maxTotalMinions = 5;
    [SerializeField] private float forcedSpawnInterval = 10f;

    private int aliveMinions = 0;
    private int totalSpawned = 0;


    private NavMeshAgent agent;
    private Coroutine spawnRoutine;

    private float nextReachableCheckTime;
    private bool reachableCached;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (target == null || agent == null)
        {
            StopSpawning();
            return;
        }
        if (totalSpawned >= maxTotalMinions)
        {
            StopSpawning();
            return;
        }


        bool inRange = FlatDistance(transform.position, target.position) <= detectRange;

        // On évite de recalculer le path à chaque frame
        if (Time.time >= nextReachableCheckTime)
        {
            reachableCached = TargetReachable(target, agent, reachableSampleRadius);
            nextReachableCheckTime = Time.time + reachableCheckInterval;
        }

        bool shouldSpawn = inRange && reachableCached;

        if (shouldSpawn) StartSpawning();
        else StopSpawning();
    }

    void StartSpawning()
    {
        if (spawnRoutine == null)
            spawnRoutine = StartCoroutine(SpawnLoop());
    }

    void StopSpawning()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
    }

    IEnumerator SpawnLoop()
    {
        while (aliveMinions < maxAliveMinions && totalSpawned < maxTotalMinions)
        {
            SpawnMinionInFrontOfTarget();
            yield return new WaitForSeconds(forcedSpawnInterval);
        }
        spawnRoutine = null;
    }



    void SpawnMinionInFrontOfTarget()
    {
        if (minionPrefab == null || target == null) return;

        Vector3 forwardFlat = target.forward;
        forwardFlat.y = 0f;
        forwardFlat = forwardFlat.sqrMagnitude > 0.0001f ? forwardFlat.normalized : Vector3.forward;

        Vector3 spawnPos = target.position + forwardFlat * spawnDistanceInFront;

        // Optionnel : snap au NavMesh pour éviter les spawns dans le vide
        if (NavMesh.SamplePosition(spawnPos, out var hit, 2f, NavMesh.AllAreas))
            spawnPos = hit.position;

        Quaternion rot = Quaternion.LookRotation((target.position - spawnPos).WithY(0f), Vector3.up);

        GameObject minion = Instantiate(minionPrefab, spawnPos, rot);

        aliveMinions++;
        totalSpawned++;

        var stats = minion.GetComponent<EnemyStats>();
        if (stats != null)
            stats.Init(this);


        // Donner la target (player) au minion
        var ai = minion.GetComponent<SlimeAI_Simple>();
        if (ai != null)
            ai.SetTarget(target);

    }

    static float FlatDistance(Vector3 a, Vector3 b)
    {
        a.y = 0; b.y = 0;
        return Vector3.Distance(a, b);
    }

    static bool TargetReachable(Transform target, NavMeshAgent agent, float sampleRadius)
    {
        if (target == null || agent == null) return false;

        if (!NavMesh.SamplePosition(target.position, out var hit, sampleRadius, NavMesh.AllAreas))
            return false;

        var path = new NavMeshPath();
        agent.CalculatePath(hit.position, path);
        return path.status == NavMeshPathStatus.PathComplete;
    }

    public void NotifyMinionDestroyed()
    {
        aliveMinions = Mathf.Max(0, aliveMinions - 1);
    }

}

static class VecExt
{
    public static Vector3 WithY(this Vector3 v, float y)
    {
        v.y = y;
        return v;
    }
}
