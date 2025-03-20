using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform player;  // Assign the player in the inspector

    private Vector3 lastValidPosition;

    void Start()
    {
        lastValidPosition = transform.position;
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Move smoothly towards the player
        Vector3 targetPosition = new Vector3(player.position.x, player.position.y, transform.position.z);

        // Move camera to the new position
        transform.position = targetPosition;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
    
        transform.position = lastValidPosition;

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        lastValidPosition = transform.position;
    }
}
