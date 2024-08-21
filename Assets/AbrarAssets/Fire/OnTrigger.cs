using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OnTrigger : MonoBehaviour
{
    [SerializeField] private controllerDetection exitMyPlayer;
    [SerializeField] private MapLine startPoint;
    [SerializeField] private GameObject endPoint;
    [SerializeField] private bool isFun;
    [SerializeField] private int targetObjectIndexToActivate;

    private void OnTriggerEnter(Collider other)
    {
        // Ensure the other object is tagged as Player
        if (other.CompareTag("Player")&& !isFun)
        {
            //Debug.Log("1");
            HandlePlayerExit(other);
        }
    }

    /// <summary>
    /// Handles the player's exit action when they enter the trigger.
    /// </summary>
    /// <param name="playerCollider">The collider of the player object.</param>
    private void HandlePlayerExit(Collider playerCollider)
    {
        if (exitMyPlayer == null || startPoint == null || endPoint == null)
        {
            Debug.LogError("Required components are not assigned.");
            return;
        }
        playerCollider.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        Rigidbody playerRigidbody = playerCollider.gameObject.GetComponent<Rigidbody>();
        if (playerRigidbody != null)
        {
            // Gradually reduce the velocity
            StartCoroutine(GraduallyStopRigidbody(playerRigidbody));
        }

        exitMyPlayer.getOut();
        UpdateMapLine();
        gameObject.SetActive(false);
    }

    private IEnumerator GraduallyStopRigidbody(Rigidbody rb)
    {
        float duration = 0.5f; // Time to gradually stop the movement
        float elapsed = 0f;

        Vector3 initialVelocity = rb.velocity;
        Vector3 initialAngularVelocity = rb.angularVelocity;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            rb.velocity = Vector3.Lerp(initialVelocity, Vector3.zero, t);
            rb.angularVelocity = Vector3.Lerp(initialAngularVelocity, Vector3.zero, t);

            yield return null;
        }

        // Ensure final velocity is exactly zero
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void UpdateMapLine()
    {
        // Set start and end points for the MapLine
        startPoint.startPoint = exitMyPlayer.gameObject;
        //startPoint.endPoint = endPoint;
        MissionManager.Instance.GameElements.mapLine.GetComponent<MapLine>().endPoint = MissionManager.Instance.Missions[MissionManager.Instance.currentMissionIndex].missionDataObjectData[targetObjectIndexToActivate].objTOActivate.gameObject;
        // Activate the end point object
        endPoint.SetActive(true);
    }
    private void OnEnable()
    {
        //startPoint.endPoint = gameObject;
    }
}
