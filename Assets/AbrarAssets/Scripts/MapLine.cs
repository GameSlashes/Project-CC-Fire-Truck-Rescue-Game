using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapLine : MonoBehaviour
{
    public static MapLine Instance { get; private set; }

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] public GameObject endPoint;
    [SerializeField] public GameObject startPoint;
    [SerializeField] private bool oneTime;
    [SerializeField] private Vector3 position;
    [SerializeField] private float textureSpeed = 0.5f;
    [SerializeField] private int interpolationFactor = 5; // Higher value means smoother line

    [SerializeField] private NavMeshPath navMeshPath;
    private float textureOffset = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (InGameMissionsManager.Instance != null && !InGameMissionsManager.Instance.inGameMission)
        {
            startPoint = GameController.instance.myPlayers;
        }
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        navMeshPath = new NavMeshPath();
    }

    private void FixedUpdate()
    {

        if (endPoint != null && startPoint != null)
        {
            UpdatePath();
        }
        // Ensuring the object stays at the desired y position
        Vector3 currentPosition = transform.position;
        transform.position = new Vector3(currentPosition.x, position.y, currentPosition.z);
    }

    private void UpdatePath()
    {
        if (NavMesh.CalculatePath(startPoint.transform.position, endPoint.transform.position, NavMesh.AllAreas, navMeshPath))
        {
            Vector3[] smoothCorners = InterpolatePath(navMeshPath.corners, interpolationFactor);

            lineRenderer.positionCount = smoothCorners.Length;
            lineRenderer.SetPositions(smoothCorners);

            // Update texture offset for the arrow tiling effect
            textureOffset += textureSpeed * Time.deltaTime;
            lineRenderer.material.mainTextureOffset = new Vector2(textureOffset, 0);
        }
    }

    private Vector3[] InterpolatePath(Vector3[] path, int factor)
    {
        if (path == null || path.Length < 2)
        {
            return path;
        }

        List<Vector3> smoothPath = new List<Vector3>();

        for (int i = 0; i < path.Length - 1; i++)
        {
            smoothPath.Add(path[i]);
            for (int j = 1; j <= factor; j++)
            {
                float t = j / (float)(factor + 1);
                smoothPath.Add(Vector3.Lerp(path[i], path[i + 1], t));
            }
        }
        smoothPath.Add(path[path.Length - 1]);

        return smoothPath.ToArray();
    }
}
