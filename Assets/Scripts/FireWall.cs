
using UnityEngine;
using System.Collections;

public class FireWall : MonoBehaviour
{
    public Transform teleportPoint;
    public float respawnDelay = 1f;

    private bool playerDied = false;

    void OnTriggerEnter(Collider other)
    {
        PlayerMovement player =
            other.GetComponentInParent<PlayerMovement>();

        if (player == null || playerDied)
            return;

        if (teleportPoint == null)
        {
            Debug.LogError("Assign TeleportPoint in the Inspector!");
            return;
        }

        playerDied = true;

        StartCoroutine(KillAndRespawn(player));
    }

    IEnumerator KillAndRespawn(PlayerMovement player)
    {
        // Stop the Player
        Rigidbody rb = player.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Disable the Player temporarily
        player.gameObject.SetActive(false);

        // Wait before respawning
        yield return new WaitForSeconds(respawnDelay);

        // Teleport back to the respawn point
        player.transform.position = teleportPoint.position;

        // Show the Player again
        player.gameObject.SetActive(true);

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        playerDied = false;
    }
}
