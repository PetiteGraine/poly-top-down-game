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
    }
}
