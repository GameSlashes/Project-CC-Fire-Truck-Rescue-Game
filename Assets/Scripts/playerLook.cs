using Invector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class playerLook : MonoBehaviour
{
    public GameObject playerObject;
    public float stoppingDistance = 20f;
    public bool distance1;
    public float distanceThreshold = 100f;
    public controllerDetection getPlayer;

    void Update()
    {
        if (playerObject == null && getPlayer.controller != null)
        {
            playerObject = getPlayer.controller;
        }
        if (playerObject == null) return;


        float distance = Vector3.Distance(transform.position, playerObject.transform.position);
        if (distance > distanceThreshold)
        {
            return;
        }

        Vector3 directionToPlayer = playerObject.transform.position - transform.position;

        float distanceToPlayerSqr = directionToPlayer.sqrMagnitude;
        float stoppingDistanceSqr = stoppingDistance * stoppingDistance;

        if (distanceToPlayerSqr > stoppingDistanceSqr)
        {
            directionToPlayer.y = 0f;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

            transform.rotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
            distance1 = false;
        }
        else
        {
            distance1 = true;
        }
    }
}
