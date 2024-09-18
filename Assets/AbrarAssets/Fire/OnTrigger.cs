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
        playerCollider.GetComponent<RCCP_CarController>().handbrakeInput_P = 0;
        playerCollider.GetComponent<RCCP_CarController>().throttleInput_P = 0;

        exitMyPlayer.getOut();
        UpdateMapLine();
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
