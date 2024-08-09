using System.Reflection;
using UnityEngine;

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
        // Ensure all required components and objects are set
        if (exitMyPlayer == null || startPoint == null || endPoint == null)
        {
            Debug.LogError("Required components are not assigned.");
            return;
        }

        // Cache the Rigidbody component and set drag
        Rigidbody playerRigidbody = playerCollider.gameObject.GetComponent<Rigidbody>();
        if (playerRigidbody != null)
        {
            playerRigidbody.drag = 15f;
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
        }

        exitMyPlayer.getOut();



        UpdateMapLine();

        // Deactivate this game object
        gameObject.SetActive(false);
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
