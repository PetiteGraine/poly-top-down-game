using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    public Transform target;

    [Header("Rotation")]
    public float mouseSensitivity = 3f;
    public float minPitch = 45f;   // limite basse (pas sous le sol)
    public float maxPitch = 85f;   // quasi top-down

    [Header("Zoom")]
    public float zoomSpeed = 5f;
    public float minZoom = 5f;
    public float maxZoom = 20f;

    private float yaw;
    private float pitch;
    private float currentZoom;

    private Transform cam;

    void Start()
    {
        cam = GetComponentInChildren<Camera>().transform;

        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
        currentZoom = -cam.localPosition.z;
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;

            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            currentZoom -= scroll * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        }

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        transform.position = target.position;

        cam.localPosition = new Vector3(0f, 0f, -currentZoom);
    }
}
