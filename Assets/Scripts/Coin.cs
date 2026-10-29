
using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    public Transform teleportPoint;

    public float floatHeight = 0.5f;
    public float floatSpeed = 2f;
    public float rotationSpeed = 60f;

    private Vector3 originalPosition;
    private Vector3 startPosition;
    private bool collected = false;

    private Renderer[] coinRenderers;
    private Collider[] coinColliders;

    void Start()
    {
        originalPosition = transform.position;
        startPosition = originalPosition;

        coinRenderers = GetComponentsInChildren<Renderer>();
        coinColliders = GetComponentsInChildren<Collider>();
    }

    void Update()
    {
        if (collected) return;

        // Float up and down
        float newY = startPosition.y +
            Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        transform.position = new Vector3(
            startPosition.x,
            newY,
            startPosition.z
        );

        // Spin slowly
        transform.Rotate(
            0f,
            rotationSpeed * Time.deltaTime,
            0f
        );
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player" && !collected)
        {
            collected = true;

            // Teleport the Player
            if (teleportPoint != null)
            {
                other.transform.position = teleportPoint.position;
            }

            // Hide the coin
            foreach (Renderer r in coinRenderers)
            {
                r.enabled = false;
            }

            foreach (Collider c in coinColliders)
            {
                c.enabled = false;
            }

            // Start the respawn timer
            StartCoroutine(RespawnCoin());
        }
    }

    IEnumerator RespawnCoin()
    {
        yield return new WaitForSeconds(3f);

        // Return to the original position
        transform.position = originalPosition;
        startPosition = originalPosition;

        // Show the coin again
        foreach (Renderer r in coinRenderers)
        {
            r.enabled = true;
        }

        foreach (Collider c in coinColliders)
        {
            c.enabled = true;
        }

        collected = false;
    }
}
