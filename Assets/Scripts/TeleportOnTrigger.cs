using UnityEngine;

public class TeleportOnTrigger : MonoBehaviour
{
    [Header("Teleport")]
    [SerializeField] private Transform destination;   // point précis
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (destination == null)
        {
            Debug.LogWarning("[TeleportOnTrigger] Destination not set.");
            return;
        }

        Transform player = other.transform;

        // Si ton player a un CharacterController, on le désactive 1 frame pour éviter les bugs
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc) cc.enabled = false;

        player.position = destination.position;
        player.rotation = destination.rotation; // optionnel, si tu veux orienter le player

        if (cc) cc.enabled = true;
    }
}
