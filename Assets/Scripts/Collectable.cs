using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int value = 1; // ✅ How much this collectible is worth

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        { // ✅ Checks if a player touched the collectible
            GameManager.Instance.AddCollectible(value); // ✅ Update the score
            Destroy(gameObject); // ✅ Remove the collectible
        }
    }
}
