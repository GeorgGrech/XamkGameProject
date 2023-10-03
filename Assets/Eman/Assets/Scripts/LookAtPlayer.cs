using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public Transform playerTransform;

    private void Start()
    {
        // Assuming the player is the main camera in a 2D game
        playerTransform = Camera.main.transform;
    }

    void Update()
    {
        // Make the text look at the player (camera)
        Vector3 direction = playerTransform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f; // Subtract 90 degrees to align correctly
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
