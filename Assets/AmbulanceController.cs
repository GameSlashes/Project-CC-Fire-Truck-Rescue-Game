using UnityEngine;

public class AmbulanceController : MonoBehaviour
{
    [Header("Ambulance Animators")]
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private Animator helperAnimator;
    [SerializeField] private Animator stretcherAnimator;

    [Header("Patients")]
    [SerializeField] private GameObject patientActive;
    [SerializeField] private GameObject patientInactive;
    [SerializeField] private bool callitOnce = true;

    /// <summary>
    /// Enables all animators and activates the active patient GameObject while deactivating the inactive patient GameObject.
    /// </summary>
    public void ActivateAmbulanceComponents()
    {
        EnableAnimator(doorAnimator);
        EnableAnimator(helperAnimator);
        EnableAnimator(stretcherAnimator);
        SwitchPatient();
        if (callitOnce)
        {
            var missionManager = MissionManager.Instance;
            if (missionManager != null)
            {
                var currentMission = missionManager.Missions[missionManager.currentMissionIndex];
                if (currentMission != null && currentMission.IsRescueMan)
                {
                    currentMission.rescueManCounter++;
                    missionManager.GameElements.rescueMan.text = currentMission.rescueManCounter.ToString();
                }
                else
                {
                    Debug.LogWarning("Current mission is null or not a rescue mission.");
                }
            }
            else
            {
                Debug.LogWarning("MissionManager instance is null.");
            }
            gameObject.SetActive(false);
            callitOnce = false;
        }

    }

    /// <summary>
    /// Enables the specified animator if it is not null.
    /// </summary>
    /// <param name="animator">Animator to enable.</param>
    private void EnableAnimator(Animator animator)
    {
        if (animator != null)
        {
            animator.enabled = true;
        }
        else
        {
            Debug.LogWarning("Animator reference is missing.");
        }
    }

    /// <summary>
    /// Activates the active patient GameObject and deactivates the inactive patient GameObject if they are not null.
    /// </summary>
    private void SwitchPatient()
    {
        if (patientActive != null)
        {
            patientActive.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Active patient GameObject reference is missing.");
        }

        if (patientInactive != null)
        {
            patientInactive.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Inactive patient GameObject reference is missing.");
        }
    }
}
