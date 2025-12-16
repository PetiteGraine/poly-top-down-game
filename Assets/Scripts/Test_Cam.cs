using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleWASDMove_NewInput : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        float x = 0f;
        float z = 0f;

        if (kb.aKey.isPressed) z -= 1f;
        if (kb.dKey.isPressed) z += 1f;
        if (kb.wKey.isPressed) x -= 1f;
        if (kb.sKey.isPressed) x += 1f;

        Vector3 dir = new Vector3(x, 0f, z).normalized;
        transform.position += dir * speed * Time.deltaTime;
    }
}
