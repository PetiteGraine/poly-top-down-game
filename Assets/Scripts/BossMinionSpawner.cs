using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BossMinionSpawner : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;              // Player
    [SerializeField] private float detectRange = 15f;       // distance (à plat)
    [SerializeField] private float reachableSampleRadius = 2f;

    [Header("Spawn")]
    [SerializeField] private GameObject minionPrefab;
    [SerializeField] private float spawnInterval = 2.5f;
    [SerializeField] private float spawnDistanceInFront = 2.0f;
    [SerializeField] private float spawnHeightOffset = 0.0f;

    [Header("Performance / Stability")]
    [SerializeField] private float reachableCheckInterval = 0.5f; // pas à chaque frame

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

        bool inRange = FlatDistance(transform.position, target.position) <= detectRange;

        // On évite de recalculer le path à chaque frame
        if (Time.time >= nextReachableCheckTime)
        {
            reachableCached = TargetReachable(target, agent, reachableSampleRadius);
            nextReachableCheckTime = Time.time + reachableCheckInterval;
        }

        bool shouldSpawn = inRange && reachableCached;
        Debug.Log($"Spawner: inRange={inRange}, reachable={reachableCached}, shouldSpawn={shouldSpawn}");
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
        while (true)
        {
            SpawnMinionInFrontOfTarget();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnMinionInFrontOfTarget()
    {
        if (minionPrefab == null || target == null) return;

        Vector3 forwardFlat = target.forward;
        forwardFlat.y = 0f;
        forwardFlat = forwardFlat.sqrMagnitude > 0.0001f ? forwardFlat.normalized : Vector3.forward;

        Vector3 spawnPos = target.position + forwardFlat * spawnDistanceInFront;
        spawnPos.y += spawnHeightOffset;

        // Optionnel : snap au NavMesh pour éviter les spawns dans le vide
        if (NavMesh.SamplePosition(spawnPos, out var hit, 2f, NavMesh.AllAreas))
            spawnPos = hit.position;

        Quaternion rot = Quaternion.LookRotation((target.position - spawnPos).WithY(0f), Vector3.up);

        GameObject minion = Instantiate(minionPrefab, spawnPos, rot);

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
}

static class VecExt
{
    public static Vector3 WithY(this Vector3 v, float y)
    {
        v.y = y;
        return v;
    }
}
