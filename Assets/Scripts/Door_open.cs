using UnityEngine;
using UnityEngine.InputSystem;

public class DoorSideOpen : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Key key = Key.E;

    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float speed = 180f;

    [SerializeField] private Collider Collider;

    private bool inRange;
    private bool isOpen;

    private float closedY;
    private float targetY;

    private void Awake()
    {
        closedY = transform.localEulerAngles.y;
        targetY = closedY;
    }

    private void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        // touche E (new input system)
        if (inRange && kb[key].wasPressedThisFrame)
        {
            Vector3 localPlayerPos = transform.InverseTransformPoint(player.position);
            float sideSign = (localPlayerPos.z >= 0f) ? 1f : -1f;
            isOpen = !isOpen;
            targetY = isOpen ? closedY + sideSign * openAngle : closedY;
        }

        float y = Mathf.MoveTowardsAngle(transform.localEulerAngles.y, targetY, speed * Time.deltaTime);
        Vector3 e = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(e.x, y, e.z);
        
        float angleDelta = Mathf.Abs(Mathf.DeltaAngle(y, targetY));

        Collider.enabled = angleDelta < 0.5f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) inRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) inRange = false;
    }
}
