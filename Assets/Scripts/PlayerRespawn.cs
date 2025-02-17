using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Respawn Settings")]
    public Transform respawnPoint; // ✅ Where to respawn the player
    public float fallThreshold = -20f; // ✅ Y-position where the player respawns

    private CharacterController characterController; // ✅ Reference to CharacterController

    private void Start()
    {
        characterController = GetComponent<CharacterController>(); // ✅ Get CharacterController
    }

    private void Update()
    {
        if (transform.position.y < fallThreshold)
        {
            RespawnPlayer();
        }
    }

    private void RespawnPlayer()
    {
        Debug.Log("Player fell! Respawning...");

        // ✅ Reset CharacterController state before teleporting
        characterController.enabled = false;
        transform.position = respawnPoint.position; // ✅ Move player to respawn point
        transform.rotation = respawnPoint.rotation;
        characterController.enabled = true;
    }
}
