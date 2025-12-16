using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private void FollowTarget()
    {
        Vector3 newPosition = this.gameObject.transform.position;
        newPosition.x = _target.position.x;
        newPosition.z = _target.position.z;
        this.gameObject.transform.position = newPosition;
    }

    private void Update()
    {
        FollowTarget();
    }
}
