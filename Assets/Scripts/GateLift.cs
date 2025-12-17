using UnityEngine;

public class GateLift : MonoBehaviour
{
    [Header("Positions")]
    [SerializeField] private Transform closedPoint; // position basse
    [SerializeField] private Transform openPoint;   // position haute

    [Header("Move")]
    [SerializeField] private float speed = 2f;

    private bool isOpen;
    private Transform target;

    private void Awake()
    {
        if (closedPoint != null)
            transform.position = closedPoint.position;

        target = closedPoint;
    }

    private void Update()
    {
        if (target == null) return;
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    public void SetOpen(bool open)
    {
        isOpen = open;
        target = isOpen ? openPoint : closedPoint;
    }

    public void Toggle()
    {
        SetOpen(!isOpen);
    }
}
