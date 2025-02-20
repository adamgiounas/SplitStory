using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // ✅ Singleton for easy access
    private int collectiblesCount = 0;
    public TextMeshProUGUI collectiblesText; // ✅ Reference to UI Text

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCollectible(int amount)
    {
        collectiblesCount += amount;
        Debug.Log("Collected! Total: " + collectiblesCount);

        if (collectiblesText != null)
        {
            collectiblesText.text = "Coins: " + collectiblesCount;
        }
    }
}
