using UnityEngine;
using System.Collections;

public class DisappearingPlatform : MonoBehaviour
{
    [Header("Platform Settings")]
    public float disappearDelay = 3f; // Time before disappearing
    public float respawnDelay = 3f; // Time before reappearing
    public Color warningColor = Color.red; // ✅ Color before breaking
    public int flickerCount = 3; // ✅ How many times it flickers

    private Collider platformCollider;
    private Renderer platformRenderer;
    private Color originalColor; // ✅ Stores the platform’s original color

    private void Start()
    {
        platformCollider = GetComponent<Collider>();
        platformRenderer = GetComponent<Renderer>();
        originalColor = platformRenderer.material.color; // ✅ Store original color
    }

    public void StartDisappearing()
    {
        StartCoroutine(DisappearPlatform());
    }

    private IEnumerator DisappearPlatform()
    {
        float flickerInterval = disappearDelay / (flickerCount * 2); // ✅ Controls flicker speed

        // ✅ Flicker effect before disappearing
        for (int i = 0; i < flickerCount; i++)
        {
            platformRenderer.material.color = warningColor;
            yield return new WaitForSeconds(flickerInterval);
            platformRenderer.material.color = originalColor;
            yield return new WaitForSeconds(flickerInterval);
        }

        platformCollider.enabled = false; // ✅ Disable collision
        platformRenderer.enabled = false; // ✅ Hide platform

        yield return new WaitForSeconds(respawnDelay); // ✅ Wait before respawning

        platformCollider.enabled = true;
        platformRenderer.enabled = true;
        platformRenderer.material.color = originalColor; // ✅ Restore original color
    }
}
