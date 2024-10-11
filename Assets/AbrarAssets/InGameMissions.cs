using UnityEngine;

public class InGameMissions : MonoBehaviour
{
    public controllerCollision CarEnterExitSystem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!TimerScriptAD.instance.isMission)
            {
                TimerScriptAD.instance.isMission = true;
                InGameMissionsManager.Instance.playerTruck = CarEnterExitSystem;
                InGameMissionsManager.Instance.triggerObject = gameObject;
                InGameMissionsManager.Instance.missionPanel.SetActive(true);
            }
            else
            {
                InGameMissionsManager.Instance.alreadyInMission.SetActive(true);
                Invoke("Deactivate", 2f);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!TimerScriptAD.instance.isMission)
            {
                TimerScriptAD.instance.isMission = false;
                InGameMissionsManager.Instance.playerTruck = null;
                InGameMissionsManager.Instance.triggerObject = null;
                InGameMissionsManager.Instance.missionPanel.SetActive(false);
            }
            else
            {
                InGameMissionsManager.Instance.alreadyInMission.SetActive(false);
            }
        }
    }
    public void Deactivate()
    {
        InGameMissionsManager.Instance.alreadyInMission.SetActive(false);
    }
}
