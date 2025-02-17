using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    private DisappearingPlatform parentPlatform;

    private void Start()
    {
        parentPlatform = GetComponentInParent<DisappearingPlatform>(); // ✅ Find the parent platform
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Platform Break Trigger");
        if (other.CompareTag("Player"))
        {
            parentPlatform.StartDisappearing(); // ✅ Tell the parent platform to disappear
        }
    }
}
