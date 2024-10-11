using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ObjectPositioner : MonoBehaviour
{
    [Header("Spawn Points")]
    public Transform pipeSpawnPoint;
    public Transform fireExtinguisherSpawnPoint;
    public Transform waterGunSpawnPoint;

    [Header("Objects in Hierarchy")]
    public GameObject waterPipe;
    public GameObject waterGun;
    public GameObject fireExtinguisher;
    public GameObject particals;

    private void Start()
    {
        gameObject.GetComponentInParent<RCCP_CarController>().KillEngine();
    }

    public void PositionObjects()
    {
        PositionAndParentObject(waterPipe, pipeSpawnPoint);
        PositionAndParentObject(waterGun, waterGunSpawnPoint);
        PositionAndParentObject(fireExtinguisher, fireExtinguisherSpawnPoint);
        if (particals)
        {
            particals.SetActive(false);
        }
    }

    public void PositionAndParentObject(GameObject obj, Transform spawnPoint)
    {
        if (obj != null && spawnPoint != null)
        {
            obj.transform.position = spawnPoint.position;
            obj.transform.rotation = spawnPoint.rotation;
            obj.transform.SetParent(spawnPoint.parent);
        }
    }
}
