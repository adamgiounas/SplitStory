using UnityEngine;
using System.Collections;

public class Switch : MonoBehaviour
{
    public GameObject door; // ✅ Assign the door to open/close
    public Vector3 openPosition; // ✅ Where the door moves when opened
    private Vector3 closedPosition; // ✅ Where the door stays when closed
    private bool isPressed = false;

    private void Start()
    {
        closedPosition = door.transform.position; // ✅ Store the closed position
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Pushable"))
        {
            OpenDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Pushable"))
        {
            CloseDoor();
        }
    }

    private void OpenDoor()
    {
        if (!isPressed)
        {
            isPressed = true;
            StopAllCoroutines(); // ✅ Prevent multiple animations
            StartCoroutine(MoveDoor(door.transform.position, openPosition, 1f)); // ✅ Smoothly move door
        }
    }

    private void CloseDoor()
    {
        if (isPressed)
        {
            isPressed = false;
            StopAllCoroutines();
            StartCoroutine(MoveDoor(door.transform.position, closedPosition, 1f));
        }
    }

    private IEnumerator MoveDoor(Vector3 start, Vector3 end, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            door.transform.position = Vector3.Lerp(start, end, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        door.transform.position = end;
    }
}
