using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SlimeAI_Simple : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator anim;

    [Header("Targets")]
    [SerializeField] private Transform target;
    [SerializeField] private Transform center;

    [Header("Ranges")]
    [SerializeField] private float chaseRange = 6f;

    [Header("Animator Params")]
    [SerializeField] private string pIdle = "Idle";     // bool
    [SerializeField] private string pMove = "Move";     // bool
    [SerializeField] private string pAttack = "Attack"; // trigger ou bool

    [Header("Tuning")]
    [SerializeField] private float repathInterval = 0.2f;
    [SerializeField] private float attackCooldown = 1.2f;

    private float repathTimer;
    private bool canAttack = true;
    private bool targetInAttackTrigger = false;

    void Awake()
    {
        if (!agent) agent = GetComponent<NavMeshAgent>();
        if (!anim) anim = GetComponent<Animator>();
        if (!center) center = transform;
    }

    void Update()
    {

        float dist = FlatDistance(transform.position, target.position);

        // Chase ou retour centre
        if (dist <= chaseRange && TargetReachable())
        {
            repathTimer -= Time.deltaTime;
            if (targetInAttackTrigger && canAttack) { 
                repathTimer = repathInterval;
                StartCoroutine(DoAttack());

            }
            else if (repathTimer <= 0f)
            {
                repathTimer = repathInterval;
                agent.SetDestination(target.position);
            }
        }
        else
        {
            GoCenter();
        }

        UpdateAnimFromVelocity();
    }

    void GoCenter()
    {
        repathTimer -= Time.deltaTime;
        if (repathTimer <= 0f)
        {
            repathTimer = repathInterval;
            agent.SetDestination(center.position);
        }
    }

    bool TargetReachable()
    {
        if (!NavMesh.SamplePosition(target.position, out var hit, 2f, NavMesh.AllAreas))
            return false;

        var path = new NavMeshPath();
        agent.CalculatePath(hit.position, path);
        return path.status == NavMeshPathStatus.PathComplete;
    }

    IEnumerator DoAttack()
    {
        canAttack = false;

        anim.SetBool(pIdle, false);
        anim.SetBool(pAttack, true);
        yield return null;
        anim.SetBool(pAttack, false);
        anim.SetBool(pIdle, true);

        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }

    void UpdateAnimFromVelocity()
    {
        Vector3 v = agent.velocity; v.y = 0f;
        bool moving = v.sqrMagnitude > 0f;

        anim.SetBool(pIdle, !moving);
        anim.SetBool(pMove, moving);

    }

    static float FlatDistance(Vector3 a, Vector3 b)
    {
        a.y = 0; b.y = 0;
        return Vector3.Distance(a, b);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            targetInAttackTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            targetInAttackTrigger = false;
    }
}
