using UnityEngine;

public class MoveAndRotateBetweenPoints : MonoBehaviour
{
    [Header("Déplacement")]
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 2f;

    [Header("Rotation")]
    public Vector3 rotationAxis = Vector3.forward;
    public float rotationSpeed = 90f;
    [SerializeField] private float _attackCooldown = 0.2f;
    private float _attackCoaldownTimer = 0f;
    private Transform currentTarget;

    void Start()
    {
        currentTarget = pointA;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            currentTarget.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, currentTarget.position) < 0.01f)
        {
            currentTarget = (currentTarget == pointA) ? pointB : pointA;
        }

        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime, Space.Self);

        if (_attackCoaldownTimer > 0f)
        {
            _attackCoaldownTimer -= Time.deltaTime;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_attackCoaldownTimer > 0f)
            return;
        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(1);
                _attackCoaldownTimer = _attackCooldown;
            }
        }
    }
}
