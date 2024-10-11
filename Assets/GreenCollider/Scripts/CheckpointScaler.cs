using UnityEngine;

public class CheckpointScaler : MonoBehaviour
{
    public Transform target;              // The target object to measure distance from
    public float minDistance = 1f;        // Minimum distance to scale from
    public float maxDistance = 10f;       // Maximum distance to scale to
    public float minScale = 0.5f;         // Minimum scale
    public float maxScale = 2f;           // Maximum scale
    public float minYPosition = 0f;       // Minimum Y position
    public float maxYPosition = 5f;       // Maximum Y position
    public float distanceThreshold = 100f; // Distance threshold for when scaling and rotation occur
    public controllerDetection getPlayer; // Script to detect player

    void Update()
    {
        // Ensure target is set, or try to get it from controllerDetection
        if (target == null && getPlayer.controller != null)
        {
            target = getPlayer.controller.transform;
        }

        // Early exit if target is still not set
        if (target == null) return;

        // Calculate the distance between the checkpoint and the target
        float distance = Vector3.Distance(transform.position, target.position);

        // Check if the distance is within the threshold (e.g., 100 meters)
        if (distance > distanceThreshold)
        {
            // Do nothing if the player is too far away
            return;
        }

        // Clamp the distance to be within min and max distance range
        float clampedDistance = Mathf.Clamp(distance, minDistance, maxDistance);

        // Calculate a normalized factor based on the clamped distance
        float distanceFactor = (clampedDistance - minDistance) / (maxDistance - minDistance);

        // Calculate and apply the scale based on the distance factor
        float scaleFactor = Mathf.Lerp(minScale, maxScale, distanceFactor);
        transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

        // Calculate and apply the Y position based on the distance factor
        float positionFactor = Mathf.Lerp(minYPosition, maxYPosition, distanceFactor);
        transform.position = new Vector3(transform.position.x, positionFactor, transform.position.z);

        // Rotate to face the target (player)
        Vector3 directionToTarget = target.position - transform.position;
        directionToTarget.y = 0f; // Ignore the y-axis for a horizontal rotation
        if (directionToTarget.sqrMagnitude > 0.001f) // Prevent errors from very small distances
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // Smooth rotation
        }
    }
}
