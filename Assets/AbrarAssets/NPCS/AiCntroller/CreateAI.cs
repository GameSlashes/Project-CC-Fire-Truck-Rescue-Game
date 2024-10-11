using UnityEngine;
using UnityEngine.AI;

public class CreateAI : MonoBehaviour
{
    public static CreateAI Instance;

    [Header("AI Settings")]
    public int maxHumans = 8; // Max number of AI humans allowed
    public GameObject[] humansPrefabs; // Array of human prefab types
    [HideInInspector]
    public int currentHumans = 0; // Current count of instantiated AI humans

    [Header("NavMesh Settings")]
    public int navMeshAreaMask = NavMesh.AllAreas; // Default to all areas, can be customized in Inspector
    [HideInInspector]
    public Transform player; // Reference to player, can be used for proximity checks

    private void OnEnable()
    {
        Instance = this; // Singleton pattern
    }

    /// <summary>
    /// Spawns AI human on a random walkable position in the NavMesh.
    /// </summary>
    /// <param name="AIHuman">The AI human prefab to instantiate.</param>
    void CreateAIHuman(GameObject AIHuman)
    {
        // Get random position within a radius of 200 units from the current object's position
        Vector3 randomDirection = Random.insideUnitSphere * 200;
        randomDirection += transform.position;

        NavMeshHit closestHit;
        // Use NavMesh.SamplePosition to find a valid point on the NavMesh within the specified mask
        if (NavMesh.SamplePosition(randomDirection, out closestHit, 200f, navMeshAreaMask))
        {
            // Check if the area around the spawn point is clear (no player nearby, for example)
            Collider[] colliders = Physics.OverlapSphere(closestHit.position, 25.0f);
            bool canCreateHuman = true;

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Player")) // Avoid instantiating too close to the player
                {
                    canCreateHuman = false;
                    break;
                }
            }

            // Instantiate the AI human if the position is valid and under the max limit
            if (canCreateHuman && currentHumans < maxHumans)
            {
                currentHumans++;
                Instantiate(AIHuman, closestHit.position, Quaternion.identity);
            }
        }
    }

    private void Update()
    {
        // Continuously create AI humans if below the maximum and there are prefabs to use
        if (humansPrefabs.Length > 0 && currentHumans < maxHumans)
        {
            CreateAIHuman(humansPrefabs[Random.Range(0, humansPrefabs.Length)]);
        }
    }
}
