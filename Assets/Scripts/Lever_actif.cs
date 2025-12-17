using UnityEngine;
using UnityEngine.InputSystem;

public class Lever_actif : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Key key = Key.E;

    [SerializeField] private float ActifAngle = 50f;
    [SerializeField] private float speed = 90f;

    [SerializeField] private Collider Collider;

    [SerializeField] private GateLift gate;

    private bool inRange;
    private bool isActif;

    private float DeactifX;
    private float targetX;

    private void Awake()
    {
        DeactifX = transform.localEulerAngles.x;
        targetX = DeactifX;
    }

    private void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        // touche E (new input system)
        if (inRange && kb[key].wasPressedThisFrame)
        {
            Vector3 localPlayerPos = transform.InverseTransformPoint(player.position);
            isActif = !isActif;
            targetX = isActif ? DeactifX + ActifAngle : DeactifX;

            if (gate != null)
                gate.SetOpen(isActif);

        }

        float x = Mathf.MoveTowardsAngle(transform.localEulerAngles.x, targetX, speed * Time.deltaTime);
        Vector3 e = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(x, e.y, e.z);
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
