using UnityEngine;

public class CheckpointScaler : MonoBehaviour
{
    public Transform target; // The target object to measure distance from
    public float minDistance = 1f; // Minimum distance to scale from
    public float maxDistance = 10f; // Maximum distance to scale to
    public float minScale = 0.5f; // Minimum scale
    public float maxScale = 2f; // Maximum scale
    public float minYPosition = 0f; // Minimum Y position
    public float maxYPosition = 5f; // Maximum Y position

    void Update()
    {
        if (target == null) return;

        // Calculate the distance between the checkpoint and the target
        float distance = Vector3.Distance(transform.position, target.position);

        // Calculate the scale factor based on the distance
        float scaleFactor = Mathf.Lerp(minScale, maxScale, (distance - minDistance) / (maxDistance - minDistance));
        scaleFactor = Mathf.Clamp(scaleFactor, minScale, maxScale);

        // Apply the scale factor to the checkpoint
        transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

        // Calculate the position factor based on the distance
        float positionFactor = Mathf.Lerp(minYPosition, maxYPosition, (distance - minDistance) / (maxDistance - minDistance));
        positionFactor = Mathf.Clamp(positionFactor, minYPosition, maxYPosition);

        // Apply the position factor to the checkpoint
        transform.position = new Vector3(transform.position.x, positionFactor, transform.position.z);
    }
}
